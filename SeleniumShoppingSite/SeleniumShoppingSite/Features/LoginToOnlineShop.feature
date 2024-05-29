Feature: LoginToOnlineShop

Scenario: Sucessfull Login to Online Shop
  Given Username is standard_user
    And Password is secret_sauce
  When User selects the login button
  Then New page is loaded
