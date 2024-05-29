Feature: Purchasing stock

Scenario: As a StockApp user i want to purchase a given amount of stock at the latest value so that i can increase the value of my portfolio
	Given that i am a StockApp user
		And my initial portfolio has a value of '<Inital Portfolio Value>'
	When i purchase '<Amount of Stock to Buy>' amount of '<Stock Code>' at the latest value
	Then My portfolio has increased in value '<Inital Portfolio Value>'
	Examples:
	| Inital Portfolio Value | Amount of Stock to Buy | Stock Code |
	| 1500                   |   10                   |     MSFT   |
	| 0                      |   5                    |     SBUX   |
	| 100                    |   3                    |     AMZN   |
	| 25000                  |   8                    |     BA     |