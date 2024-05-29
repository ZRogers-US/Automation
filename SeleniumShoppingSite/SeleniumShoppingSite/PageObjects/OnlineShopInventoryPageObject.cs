using NUnit.Framework;
using OpenQA.Selenium;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SeleniumShoppingSite.PageObjects
{
    public class OnlineShopInventoryPageObject
    {
        private string _inventoryUrl = "https://www.saucedemo.com/inventory.html";
        public int ItemIndex { get; set; } = 0;
        public string ItemName { get; set; } = "";

        private readonly IWebDriver _driver;

        public OnlineShopInventoryPageObject(IWebDriver driver)
        {
            _driver = driver;
            _driver.Url = _inventoryUrl;
        }

        public IList<IWebElement> InventoryItems
        {
            get
            {
                return _driver.FindElements(By.ClassName("inventory_item"));
            }
        } 

        public void StoreItemName(string itemName)
        {
            itemName = itemName.ToLower();
            itemName = itemName.Replace(" ", "-");
            this.ItemName = itemName;
        }

        public void StoreItemIndex(int index)
        {
            this.ItemIndex = index;
        }

        public (int,string) FindItem(string itemName)
        {
            IList<IWebElement> inventory = this.InventoryItems;
            (int, string) results = (0, string.Empty);
            for (int i = 0; i < inventory.Count; i++)
            {
                var item = inventory[i];
                var inventoryItemName = item.FindElement(By.ClassName("inventory_item_name"));
                if (inventoryItemName.Text == itemName)
                {
                    results = (i, itemName);
                    return results;
                }
            }
            Assert.Fail("Item not found");
            return results;
        }

        public string FindPrice ()
        {

            IList<IWebElement> inventory = this.InventoryItems;
            IWebElement priceElement = inventory[this.ItemIndex].FindElement(By.ClassName("inventory_item_price"));
            string priceString = priceElement.Text;
            return priceString;
        }

        public void CheckPrice(string itemPrice, int maxPrice)
        {
            string[] priceStringArray = itemPrice.Split("$");
            float priceValue = float.Parse(priceStringArray[1]);
            if (priceValue > maxPrice)
            {
                Assert.Fail("Price too high");
            }
        }

        public IWebElement GetCartButton(int itemIndex, string itemName)
        {

            IList<IWebElement> inventory = this.InventoryItems;
            IWebElement addToCartButton = inventory[itemIndex].FindElement(By.Id($"add-to-cart-{itemName}"));
            return addToCartButton;
        }
    }
}
