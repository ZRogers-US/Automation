Feature: Simplebooks customer name update


Scenario: Update customers name on current orders
	Given User has orders
	When Book orders CustomerName is Zachary
	Then update the orders to Zak

