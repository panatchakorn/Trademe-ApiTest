
Feature: SearchUsedCars
	In order to view used cars details with number plate, kilometres, body and seats
	As a user
	I want to be able to search any existing used car listing

	# I add the authentication here as I don't want to keep authenticating with trademe for every scenario.
Scenario: (1) Caller must be authenticated when calling used car search api
	Given caller wants to perform used car search api request
	When caller authenticate with Trademe, access token is generated
	

	# I'm unsure why Trademe is unable to process the search_string Query String parameter per their api reference.
	# https://api.tmsandbox.co.nz/v1/Search/General.json?search_string=BMW
	# Response shows "The server encountered an error processing the request."
	# I've also tried category Query string parameter which I have retrieved the number from https://api.tmsandbox.co.nz/v1/Categories.json without success.
	# I've tried making the same request from Fiddler and get the same response.
	
	# The api call works fine when it does not have Query String parameter.
@UsedCarSearch
Scenario: (2) User can search used car from homepage
	Given user has already authenticated
	When user searches <SearchKeyword> in cars category from "/v1/Search/General.json"
	Then the response is OK
	And the result contains used car listing based on keyword
	Examples: 
	| SearchKeyword |
	| BMW           |

	# View individual car listing works fine.
@UsedCarSearch
Scenario: (3) User can select specific car listing from search results to see full details
	Given user has already search for used car
	When user selects a to view specific car <ListingId> from "/v1/Listings/"
	Then the response is OK
	And the page displays selected used cars listing details <ListingId>, <NumberPlate> , <Kilometres> , <Body> , <Seats>
	Examples:
	| ListingId  | NumberPlate | Kilometres	| Body      | Seats  |
	| 2149242890 | EDD369      | 163,794km  | Sedan     | 5		 |
	| 2149242667 | KEL336      | 44,191km   | Hatchback | 		 |

	# Encoutered the same issue where Response shows "The server encountered an error processing the request."
	# It happens to both call with query parameter and without. Unsure of the reason.
	# - https://api.tmsandbox.co.nz/v1/Search/Motors/Used.json 
	# - https://api.tmsandbox.co.nz/v1/Search/Motors/Used.json?search_string=BMW
@UsedCarSearch
Scenario: ( 4) User can view used car listing from Trademe Motors page default results 
	Given user has navigated to Trademe Motors page
	When user select to retrieve used car listing without entering a keyword from "/v1/Search/Motors/Used.json"
	Then the response is OK
	And the page displays all used car listing details