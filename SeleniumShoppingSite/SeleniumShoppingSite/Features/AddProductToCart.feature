Feature: AddProductToCart
this is the description for add product to cart

Scenario: Selecting an item under $30
  Given An item name of Sauce Labs Backpack
  When the price is below 30
  Then Add to cart