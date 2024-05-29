#equiverlent of setting a tag on each scenario
@FeatureTag 
Feature: Example Feature

This is extra information about the feature
#A group of given steps that run before each scenario and after any before hooks
Background: 
 Given I am logged in as admin

Scenario Outline: green bottles
  Given there are <start> green bottles
  When <fall> green bottles fall
  Then I should have <left> green bottles left

  Examples:
    | start | fall | left |
    |    12 |   5  |    7 |
    |    20 |   5  |   15 |


Scenario: example doc string
  Given  to post to blog
  """
  This is the text to be posted to the blog
  """
  
Scenario: example data table
  Given the following users are registered
  | username | email                    |
  | Superman | savingtheworld@gmail.com |
  |  Hulk    |   MrStrong@gmail.com     |