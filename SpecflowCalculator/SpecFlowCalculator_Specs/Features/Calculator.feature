Feature: Calculator
![Calculator](https://specflow.org/wp-content/uploads/2020/09/calculator.png)
Simple calculator for adding **two** numbers

Link to a feature: [Calculator](SpecFlowCalculator_Specs/Features/Calculator.feature)
***Further read***: **[Learn more about how to generate Living Documentation](https://docs.specflow.org/projects/specflow-livingdoc/en/latest/LivingDocGenerator/Generating-Documentation.html)**

@AdditionScenario
Scenario: Add two numbers
	Given the first number is 50
	And the second number is 70
	When the two numbers are added
	Then the result should be 120

@SubtractionScenario
Scenario:  Subtract two numbers
	Given the first number is 70
	And the second number is 50
	When the two numbers are subtracted
	Then the result should be 20

@MultiplicationScenario
Scenario: Multiply two numbers
	Given the first number is 80
	And the second number is 2
	When the two numbers are multipied
	Then the result should be 160

@DivisionScenario
Scenario: Divid two numbers
	Given the first number is 80
	And the second number is 20
	When the two numbers are divided
	Then the result should be 4