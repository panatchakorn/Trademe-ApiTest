using ApiTest.Actions;
using TechTalk.SpecFlow;

namespace ApiTest.Utils
{
    [Binding]
    public sealed class ScenarioHooks
    {
        // For additional details on SpecFlow hooks see http://go.specflow.org/doc-hooks
        
        [BeforeScenario("ApiNoAuthentication")]
        public void BeforeScenario()
        {
            BaseApiNoAuthTest.SetBaseUri();
        }

          
        [BeforeScenario("UsedCarSearch")]
        public void BeforeScenario2()
        {
            // Restore this Trademe authentication, if you want to run specific scenario and not the whole feature.
            //   TrademeLogin.Authenticate(); 
            //

            BaseApiOAuth1Test.SetApiBaseUri();
        }

        [AfterScenario]
        public void AfterScenario()
        {
            // Nothing to be done for this step

        }
    }
}
