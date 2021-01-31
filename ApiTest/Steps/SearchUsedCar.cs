using ApiTest.Actions;
using NUnit.Framework;
using System;
using System.Net;
using TechTalk.SpecFlow;

namespace ApiTest.Steps
{
    [Binding]
    public sealed class SearchUsedCar
    {
       
        [Given(@"caller wants to perform used car search api request")]
        public void GivenCallerWantsToPerformUsedCarSearchApiRequest()
        {
            // Nothing to be done for this step
        }

        [When(@"caller authenticate with Trademe, access token is generated")]
        public void WhenCallerAuthenticateWithTradeAccessTokenIsGenerated()
        {
            TrademeLogin.Authenticate();
        }

        [Given(@"user has already authenticated")]
        public void GivenUserHasAlreadyAuthenticated()
        {
            // Nothing to be done for this step
        }

        [When(@"user searches (.*) in cars category from ""(.*)""")]
        public void WhenUserSearchesInCarsCategoryFrom(string keyword, string apiResource)
        {
            BaseApiOAuth1Test.GeneralSearch(keyword, apiResource);
        }

        [Then(@"the response is OK")]
        public void ThenTheResponseIsOK()
        {
            HttpStatusCode status = HttpStatusCode.OK;
            Assert.That(BaseApiOAuth1Test.CheckStatusCode(status), Is.True);
        }


        [Then(@"the result contains used car listing based on keyword")]
        public void ThenTheResultContainsUsedCarListingBasedOnKeyword()
        {
            // To do when able to get the keyword search working
        }

        [Given(@"user has already search for used car")]
        public void GivenUserHasAlreadySearchForUsedCar()
        {
            // Nothing to be done for this step
            
        }

        [When(@"user selects a to view specific car (.*) from ""(.*)""")]
        public void WhenUserSelectsAToViewSpecificCarFrom(Int64 listingId, string apiResource)
        {
            BaseApiOAuth1Test.ViewListingDetails(listingId, apiResource);
        }

        [Then(@"the page displays selected used cars listing details (.*), (.*) , (.*) , (.*) , (.*)")]
        public void ThenThePageDisplaysSelectedUsedCarsListingDetails(Int64 listingId, string numberPlate, string kilometres, string body, string seats)
        {
            Assert.That(BaseApiOAuth1Test.CheckCarDetails(listingId, numberPlate, kilometres, body, seats), Is.True);
        }


        [Given(@"user has navigated to Trademe Motors page")]
        public void GivenUserHasNavigatedToTrademeMotorsPage()
        {
            // Nothing to be done for this step
        }

        [When(@"user select to retrieve used car listing without entering a keyword from ""(.*)""")]
        public void WhenUserSelectToRetrieveUsedCarListingWithoutEnteringAKeywordFrom(string apiResource)
        {
            BaseApiOAuth1Test.AllUsedCarSearch(apiResource);
        }

        [Then(@"the page displays all used car listing details")]
        public void ThenThePageDisplaysAllUsedCarListingDetails()
        {
            // To be done when able to get the successful OK response
        }

    }
}
