Feature: TechStore

@signup
Scenario: Signup to TechStore
	Given A New user wants to sign up 
	  And the username is BilboBagins
	  And the password is TestPassword
	When the user selects the signup button
	Then an alert appears saying Signup Successful 
	

Scenario: signin to TechStore
	Given A returning user wants to sign in
	  And the username is FrodoBagins
	  And the password is TestPassword
	When the user selects the login button
	Then the user logs out