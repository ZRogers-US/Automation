Feature: LocationCites

@Cities
Scenario: Get Cities for a country
	Given User is authorised on API
		And A Country Code <country>
	When Type is a City
	Then Add to Lists
	Examples: 
	| country |
	| fr      |
	| ma      |
	| uk      |