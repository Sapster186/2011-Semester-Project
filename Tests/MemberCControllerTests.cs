using System;
using System.Reflection;
using System.Windows.Forms;
using _2011__Semester_Project.Boundary;
using _2011__Semester_Project.Controller;
using _2011__Semester_Project.Entity;

// Tests use the real built application. Staff roles are simulated here only,
// because the shared database is not supplied. No database rows are changed.
internal static class MemberCControllerTests
{
    private static int checks;

    private static void Check(bool condition, string name)
    {
        if (!condition) throw new Exception("FAIL: " + name);
        checks++;
        Console.WriteLine("PASS: " + name);
    }

    private static void SetSession(string role, string username)
    {
        typeof(LoginController).GetProperty("CurrentUserRole").GetSetMethod(true).Invoke(null, new object[] { role });
        typeof(LoginController).GetProperty("CurrentUsername").GetSetMethod(true).Invoke(null, new object[] { username });
    }

    private static T Field<T>(object instance, string name)
    {
        return (T)instance.GetType().GetField(name, BindingFlags.NonPublic | BindingFlags.Instance).GetValue(instance);
    }

    private static void LoadForm(Form form, string method)
    {
        form.GetType().GetMethod(method, BindingFlags.NonPublic | BindingFlags.Instance)
            .Invoke(form, new object[] { null, EventArgs.Empty });
    }

    private static void Denied(Action action, string name)
    {
        bool denied = false;
        try { action(); }
        catch (UnauthorizedAccessException) { denied = true; }
        Check(denied, name);
    }

    private static void TestDeniedActions(AdminController admin, string role)
    {
        string accessDenied = "Access denied. Only Administrators may maintain reference data.";
        Check(admin.CheckAdminAccess() == accessDenied, role + " denied admin access");
        Check(admin.AddInterestQuery(1, "Question", 1) == accessDenied, role + " denied adding questions");
        Check(admin.SaveInterestQuery(new InterestQuery(1, 1, "Question", 1, true)) == accessDenied,
            role + " denied editing questions");
        Check(admin.DeactivateInterestQuery(1) == accessDenied, role + " denied deactivating questions");
        Check(admin.SaveFundingOption(new FundOption { FundName = "Test funding", FundType = "Bursary",
            CloseDate = DateTime.Today.AddMonths(1), IsActive = true }) == accessDenied, role + " denied saving funding");
        Check(admin.DeactivateFundingOption(1) == accessDenied, role + " denied deactivating funding");
        Denied(delegate { admin.GetDegreeProgrammes(); }, role + " denied degree reads");
        Denied(delegate { admin.GetDegreeRequirements(); }, role + " denied requirement reads");
        Denied(delegate { admin.GetFundingOptions(); }, role + " denied funding reads");
        Denied(delegate { admin.GetFundingTypes(); }, role + " denied funding type reads");
        Denied(delegate { admin.GetInterestQueries(); }, role + " denied question reads");
        Denied(delegate { admin.GetInterestGroups(); }, role + " denied interest group reads");
    }

    private static void TestMenu(string role, string menuName)
    {
        SetSession(role, role == "Learner" ? null : "test-staff");
        using (MainWindow main = new MainWindow())
        {
            LoadForm(main, "MainWindow_Load");
            Check(main.IsMdiContainer, role + " main window uses MDI");
            foreach (string name in new string[] { "mnuLearner", "mnuAdvisor", "mnuAdmin" })
                Check(Field<ToolStripMenuItem>(main, name).Available == (name == menuName), role + " menu " + name);
            Check(!Field<ToolStripMenuItem>(main, "mnuMaintainReferenceData").Available,
                role + " maintenance entered through dashboard");
            Check(!Field<ToolStripMenuItem>(main, "mnuReports").Available,
                role + " reports entered through dashboard");

            // Plain forms exercise the real MDI helper without opening another member's workflow.
            MethodInfo openChild = typeof(MainWindow).GetMethod("OpenChild", BindingFlags.NonPublic | BindingFlags.Instance);
            Form child = new Form();
            openChild.Invoke(main, new object[] { child });
            Check(child.MdiParent == main, role + " child belongs to the MDI main window");
            Form duplicate = new Form();
            openChild.Invoke(main, new object[] { duplicate });
            Check(duplicate.IsDisposed, role + " duplicate child disposed");
            Check(main.MdiChildren.Length == 1 && main.MdiChildren[0] == child, role + " existing child reused");
        }
    }

    [STAThread]
    private static int Main()
    {
        LoginController login = new LoginController();
        AdminController admin = new AdminController();
        try
        {
            login.Logout();
            TestDeniedActions(admin, "Logged-out user");
            Check(login.Login("", "", "Learner"), "TC14: learner enters without credentials or database");
            Check(LoginController.CurrentUsername == null && LoginController.CurrentUserRole == "Learner",
                "TC14: anonymous learner session");
            TestDeniedActions(admin, "Learner");
            SetSession("Career Advisor", "test-advisor");
            TestDeniedActions(admin, "Career Advisor");
            SetSession("Administrator", null);
            Check(!admin.IsAdministrator(), "An admin role without a username is rejected");
            SetSession("Administrator", "test-admin");
            Check(admin.IsAdministrator(), "Administrator session is recognised");

            Check(admin.SaveInterestQuery(null) != null, "Null question rejected");
            Check(admin.AddInterestQuery(0, "Question", 1) != null, "Missing interest group rejected");
            Check(admin.AddInterestQuery(1, " ", 1) != null, "Blank question rejected");
            Check(admin.AddInterestQuery(1, new string('x', 201), 1) != null, "Oversized question rejected");
            foreach (float weight in new float[] { 0, -1, float.NaN, float.PositiveInfinity })
                Check(admin.AddInterestQuery(1, "Question", weight) != null, "Invalid weight rejected: " + weight);
            Check(admin.DeactivateInterestQuery(0) != null, "Invalid question ID rejected");
            Check(admin.SaveFundingOption(null) != null, "Null funding rejected");
            Check(admin.SaveFundingOption(new FundOption { FundName = " " }) != null, "Blank funding name rejected");
            Check(admin.SaveFundingOption(new FundOption { FundName = "Funding", FundType = "" }) != null,
                "Missing funding type rejected");
            Check(admin.DeactivateFundingOption(0) != null, "Invalid funding ID rejected");

            Check(!login.Login("", "", "Career Advisor"), "TC1: advisor cannot bypass credentials");
            Check(LoginController.CurrentUserRole == null, "Failed login clears previous administrator session");
            Check(!login.Login("user", "password", "Unknown"), "Unknown role rejected");
            Check(!login.Login("user", " ", "Administrator"), "Blank administrator password rejected");

            using (UserAuth form = new UserAuth())
            {
                LoadForm(form, "UserAuth_Load");
                ComboBox role = Field<ComboBox>(form, "cmbRole");
                TextBox username = Field<TextBox>(form, "txtUsername");
                TextBox password = Field<TextBox>(form, "txtPassword");
                Button button = Field<Button>(form, "btnLogin");
                Check(!button.Enabled, "Login disabled before selecting a role");
                role.SelectedItem = "Learner";
                Check(button.Enabled && button.Text == "Continue", "TC14: learner Continue enabled");
                Check(!username.Enabled && !password.Enabled, "TC14: credential boxes disabled");
                role.SelectedItem = "Career Advisor";
                Check(!button.Enabled && username.Enabled && password.Enabled, "TC1: advisor requires input");
                username.Text = "advisor1";
                password.Text = "password with spaces ";
                Check(button.Enabled, "TC1: advisor login enabled with credentials");
                username.Clear();
                password.Clear();
                role.SelectedItem = "Administrator";
                Check(!button.Enabled && username.Enabled && password.Enabled, "Administrator requires credentials");
                username.Text = "admin1";
                password.Text = "Admin123!";
                Check(button.Enabled, "Administrator login enabled with credentials");
            }

            TestMenu("Learner", "mnuLearner");
            TestMenu("Career Advisor", "mnuAdvisor");
            TestMenu("Administrator", "mnuAdmin");
            typeof(LearnerController).GetProperty("CurrentLearnerID").GetSetMethod(true).Invoke(null, new object[] { 5 });
            login.Logout();
            Check(LoginController.CurrentUsername == null && LoginController.CurrentUserRole == null,
                "Logout clears username and role");
            Check(LearnerController.CurrentLearnerID == 0, "Logout clears learner session");
            Console.WriteLine("PASSED: " + checks + " checks. Database integration tests remain pending.");
            return 0;
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine(ex);
            return 1;
        }
        finally { login.Logout(); }
    }
}
