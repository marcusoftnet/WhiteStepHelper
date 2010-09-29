using TechTalk.SpecFlow;

namespace Marcusoft.BDD.WhiteStepHelper.Specs.Steps
{
    [Binding]
    public class ApplicationStart : WhiteStepBase
    {
        public ApplicationStart() : base(new AutomationNameResolver()) { }

        [BeforeTestRun]
        public static void BeforeTestRun()
        {
            StartWindowInApplication(TestConfig.ApplicationUnderTestPath, TestConfig.MainWindowName);
        }

        [AfterTestRun]
        public static void AfterTestRun()
        {
            CloseApplicationUnderTest();
        }

    }
}
