using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using RestSharp;
using RestSharp.Authenticators.OAuth2;
using RestSharpSpecFlow.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TechTalk.SpecFlow;

namespace RestSharpSpecFlow.StepDefinitions
{

    public class SimpleBookContextInjection
    {
        public List<BookOrder_Model> OrderList { get; set; } = new List<BookOrder_Model>();
        public List<string> BookIDsToUpdate { get; set; } = new List<string>();
    }

    [Binding]
    public class SimpleBooks_StepDefinitions
    {
        private string _baseUrlSimpleBooks = "https://simple-books-api.glitch.me";
        string token = "be8c578d7c6109ba7ea8b13eeaf9bfd05b49a199d5aae81e38f94dc33ac0ecbd";

        readonly SimpleBookContextInjection _contextInjection;
        private ScenarioContext _scenarioContext;

        SimpleBooks_StepDefinitions(SimpleBookContextInjection contextInjection, ScenarioContext scenarioContext)
        {
            _contextInjection = contextInjection;
            _scenarioContext = scenarioContext;
        }

        [Given ("User has orders")]
        public async Task GivenUserHasOrders()
        {
            var auth = new OAuth2AuthorizationRequestHeaderAuthenticator(token, "Bearer");
            RestClientOptions options = new RestClientOptions(_baseUrlSimpleBooks)
            {
                Authenticator = auth
            };
            RestRequest request = new RestRequest("/orders", Method.Get);
            using (RestClient client = new RestClient(options))
            {
                RestResponse response = await client.ExecuteAsync(request);
                var content = JArray.Parse(response.Content);
                for (int i = 0; i < content.Count; i++)
                {
                    BookOrder_Model bookOrder = JsonConvert.DeserializeObject<BookOrder_Model>(content[i].ToString());
                    _contextInjection.OrderList.Add(bookOrder);
                }
                Assert.IsTrue(content.Count > 0);
            }
        }

        [When ("Book orders CustomerName is (.*)")]
        public void WhenOrdersCustomerNameIs(string customerName)
        {
            for (int i = 0; i <_contextInjection.OrderList.Count; i++)
            {
                if (_contextInjection.OrderList[i].CustomerName == customerName) _contextInjection.BookIDsToUpdate.Add(_contextInjection.OrderList[i].Id);
            }
            if(_contextInjection.BookIDsToUpdate.Count == 0)
            {
                Console.WriteLine($"the cunstomer's name is: {_scenarioContext}");
                Assert.Fail("No orders to update");
            }
        }

        [Then("update the orders to (.*)")]
        public async Task ThenUpdateOrdersTo(string newName)
        {
            var auth = new OAuth2AuthorizationRequestHeaderAuthenticator(token, "Bearer");
            RestClientOptions options = new RestClientOptions(_baseUrlSimpleBooks)
            {
                Authenticator = auth
            };
            var body = new
            {
                customerName = newName
            };
            RestClient client = new RestClient(options);
            for (int i =0; i<_contextInjection.BookIDsToUpdate.Count; i++)
            {
                RestRequest request = new RestRequest("/orders/{id}", Method.Patch);
                request.AddUrlSegment("id", _contextInjection.BookIDsToUpdate[i]);
                request.AddJsonBody(body);
                RestResponse response = await client.ExecuteAsync(request);
                Assert.IsTrue((int)response.StatusCode == 204);
            }
           
        }
    }
}
