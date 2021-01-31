using ApiTest.Actions;
//using Microsoft.VisualStudio.TestTools.UnitTesting;
using NUnit.Framework;
using TechTalk.SpecFlow;


namespace ApiTest.Steps
{
    [Binding]
    public sealed class GetCharities
    {
        
        [Given(@"user is looking for a specific charity")]
        public void GivenUserIsLookingForASpecificCharity()
        {
            // nothing to be done for this step
        }


        [When(@"user retrive charities list from ""(.*)""")]
        public void WhenUserRetriveCharitiesListFrom(string apiResource)
        {
            BaseApiNoAuthTest.GetCharities(apiResource);
        }
        
        [Then(@"the list contains (.*)")]
        public void ThenTheListContains(string charity)
        {
            Assert.That(BaseApiNoAuthTest.CheckCharities(charity), Is.True);
        }


    }
}
