Feature: Retrieve a list of Charities	

@ApiNoAuthentication
Scenario: Confirm a charity is in the retrieved list
	Given user is looking for a specific charity
	When user retrive charities list from "/v1/Charities.json"
	Then the list contains <Description>
	Examples:
	| Description                |
	| St John                    |
	| Our People, Our City Fund  |
