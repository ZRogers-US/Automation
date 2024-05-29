using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using RestSharp;
using System.Threading.Tasks;
using System.Text.Json;
using System.Collections;
using System.Collections.Generic;
using Newtonsoft.Json.Linq;
using RestSharp.Authenticators.OAuth2;
using FluentAssertions;

namespace HowToRestSharp
{
    [TestClass]
    public class UnitTest1
    {
        private string _baseStarWarsUrl = "https://swapi.dev/api/";
        private string _baseToDoUrl = "https://api.vschool.io/zak__rogers/";
        private string _baseBooksUrl = "https://simple-books-api.glitch.me/";
        private RestClient _restClient = new RestClient();
        private RestRequest _request = new RestRequest();
        private RestResponse _response = new RestResponse();
        private string _bookAuth = "cbc9f9c9398f3fa8fe1f6fd27fa538e9235bf23a1ef1c1abe6996a051bb02288";
        private string _tempOrderNumber = "pS56CSDOIOXYOFZsFF2x3";

        // Test method to get a book back from Simple books api using a book Id the asserting that it matches the title provided
        [TestMethod]
        public void TestGetMethod()
        {
            var options = new RestClientOptions(_baseBooksUrl);
            var client = new RestClient(options);
            var request = new RestRequest("books/3", Method.Get);
            var response = client.Get(request);

            var content = JObject.Parse(response.Content); //Parsing thejson responce into a JObject so can access the data
            if(content.ContainsKey("name")) // checking if the JObject has a key called name
            {
                Assert.AreEqual("The Vanishing Half", content.GetValue("name"));
            }
        }

        //Test method to get a planet back from the starwars api and the checking if it contains a key that only a planet would have
        [TestMethod]
        public void GetStarwarsPlanetTestIsPlanet()
        {
            RestClientOptions options = new RestClientOptions(_baseStarWarsUrl);
            _restClient = new RestClient(options);
            _request = new RestRequest("planets/5",Method.Get);
            _response = _restClient.Get(_request);

            JObject responceContent = JObject.Parse(_response.Content);
            Assert.IsTrue(responceContent.ContainsKey("orbital_period"));
            Console.WriteLine(responceContent);
        }

        //Test method to get all the planets from the starwars api then checking that the data received is an array and that the first two responces are two different planets
        [TestMethod]
        public void GetPlanetList()
        {
            RestClientOptions options = new RestClientOptions(_baseStarWarsUrl);
            _restClient = new RestClient(options);
            _request = new RestRequest("planets", Method.Get);
            _response = _restClient.Get(_request);
            JObject responceContent = JObject.Parse(_response.Content);

            Assert.IsInstanceOfType(responceContent.GetValue("results"), typeof(JArray)); //asserting that the results are a JArray 
            Assert.AreNotEqual(responceContent.GetValue("results")[0], responceContent.GetValue("results")[1]);
            Console.WriteLine(responceContent.GetValue("results")[2]["name"]);
        }

        // Test Method for getting a random person from the starwars api using a random number. then asserting that the data has a height and demostrating the AreEqual and AreNotEqual asserts
        [TestMethod]
        public void GetRandomPerson()
        {
            Random rand = new Random();
            int randomId = rand.Next(100);

            RestClientOptions options = new RestClientOptions(_baseStarWarsUrl);
            _restClient = new RestClient(options);
            _request = new RestRequest($"people/{randomId}", Method.Get);
            _response = _restClient.Get(_request);
            JObject responceContent = JObject.Parse(_response.Content);

            Assert.IsTrue(responceContent.ContainsKey("height"));
            Assert.AreEqual(responceContent.GetValue("height"), responceContent.GetValue("height"));
            Assert.AreNotEqual(responceContent.GetValue("height"), responceContent.GetValue("width"));
        }

        //Test method to PUT a todo item to the vschool todo list API and asserting that the request was sucessfull
        [TestMethod]
        public void PutToDoItemTestIsSucessful()
        {
            RestClientOptions options = new RestClientOptions(_baseToDoUrl);
            _restClient = new RestClient(options);
            _request = new RestRequest("todo/6605989178fe452c114b6c57", Method.Put);
            
            var body = new //creating an anonymous type to contain the json data to be added.
            {
                title = "This is required",
                description = "This has been updated",
                price = 0,
                imgUrl = "http://path-to-url.jpg",
                completed = false
            };
            
            _request.AddJsonBody(body); // adding the body data using AddJsonBody so the request know the data type is json without setting manually
            _response = _restClient.Put(_request);

            Assert.IsTrue(_response.IsSuccessful);
            Console.WriteLine(_response.StatusCode);
        }

        //Test method to DELETE a todo item from the vschool todo list api and confirm it was sucesful
        [TestMethod]
        public void DeleteToDoItemTestIsSucessful()
        {
            RestClientOptions options = new RestClientOptions(_baseToDoUrl);
            _restClient = new RestClient(options);
            _request = new RestRequest("todo/6616a54d78fe452c114b7936", Method.Delete);
            _response = _restClient.Delete(_request);

            Assert.IsTrue(_response.IsSuccessful);
            Console.WriteLine(_response.Content);
        }

        // Test method to Post a array of todo items to the vschool todo api and confirm each was sucessfull
        [TestMethod]
        public void PostToDoItemsTestIsSucessful()
        {
            RestClientOptions options = new RestClientOptions(_baseToDoUrl);
            _restClient = new RestClient(options);
           
            var task1 = new
            {
                title = "Task 46",
                price = 50,
                description = "this is the 46st task to post",
                imgUrl = "http://path-to-url.jpg",
                completed = false
            };
            var task2 = new
            {
                title = "Task 47",
                price = 20,
                description = "this is the 47nd task to post",
                imgUrl = "http://path-to-url.jpg",
                completed = false
            };
            var task3 = new
            {
                title = "Task 48",
                price = 5,
                description = "this is the 48rd task to post",
                imgUrl = "http://path-to-url.jpg",
                completed = false
            };
            var tasks = new[] {task1, task2, task3 };

            RestRequest[] requests = new RestRequest[tasks.Length];
            RestResponse[] responses = new RestResponse[tasks.Length];
            for (int i = 0; i < requests.Length; i++)
            {
                requests[i] = new RestRequest("todo/", Method.Post);
                requests[i].AddJsonBody(tasks[i]);
                responses[i] = _restClient.Post(requests[i]);
                Assert.IsTrue(responses[i].IsSuccessful);
            }
        }

        //Test method to post a user to the simple books api to get an authorisation token
        [TestMethod]
        public void PostBookUser()
        {
            RestClientOptions options = new RestClientOptions(_baseBooksUrl);
            _restClient = new RestClient(options);
            _request = new RestRequest("api-clients/", Method.Post);
            var authData = new
            {
                clientName = "Zak Rogers",
                clientEmail = "zrogers46@usspeaking.com"
            };

            _request.AddJsonBody(authData);
            _response = _restClient.Execute(_request);

            if(_response.IsSuccessful)
            {
                var content = JObject.Parse(_response.Content);
                _bookAuth = content.GetValue("accessToken").ToString();
                Console.WriteLine(_bookAuth);
            }
            else
            {
                Console.WriteLine(_response.StatusCode);
                Assert.Fail("Unable to register user");
            }

           // Console.WriteLine(_response.Content);
        }

        //Test method to post a new book order to the simple books api using an athorisation token from the previous registered user above
        [TestMethod]
        public void PostBookUserOrder()
        {
            //PostBookUser(); //testing perposes
            Console.WriteLine(_bookAuth);
            var authenticator = new OAuth2AuthorizationRequestHeaderAuthenticator(_bookAuth, "Bearer"); //creating a new authenticator header object
            RestClientOptions options = new RestClientOptions(_baseBooksUrl)
            {
                Authenticator = authenticator // including the authenticator header in the request options
            };
            _restClient = new RestClient(options);
            _request = new RestRequest("orders", Method.Post);
            var orderData = new
            {
                bookId = 6,
                customerName = "Zak Rogers"
            };

            _request.AddJsonBody(orderData);
            _response = _restClient.Execute(_request);
            var content = JObject.Parse(_response.Content);
            _tempOrderNumber = content.GetValue("orderId").ToString();
            Console.WriteLine(_tempOrderNumber);
            Assert.IsTrue(_response.IsSuccessful);

        }

        //Test method to get the list of book orders for the signed in userusing the bearer token
        [TestMethod]
        public void GetBookOrders()
        {
            //PostBookUserOrder(); //testing perposes
            OAuth2AuthorizationRequestHeaderAuthenticator authenticator = new OAuth2AuthorizationRequestHeaderAuthenticator(_bookAuth, "Bearer");
            RestClientOptions options = new RestClientOptions(_baseBooksUrl)
            {
                Authenticator = authenticator
            }; 
            _restClient = new RestClient(options);
            _request = new RestRequest("orders", Method.Get);
            _response = _restClient.Execute(_request);
            Console.WriteLine(_response.Content);
            
            _request.Method.Should().BeOneOf(Method.Get, Method.Search)
                                .And.HaveValue(0, "should be Get");
        }

        //TEst method to patch a book order to update the name from zak rogers to zachary rogers
        [TestMethod]
        public void PatchBookTestIsSucessful()
        {
            //GetBookOrders(); //testing perposes
            OAuth2AuthorizationRequestHeaderAuthenticator authenticator = new OAuth2AuthorizationRequestHeaderAuthenticator(_bookAuth, "Bearer");
            RestClientOptions option = new RestClientOptions(_baseBooksUrl)
            {
                Authenticator = authenticator
            };
             
            var body = new
            {
                customerName= "Zachary Rogers",
            };
            _restClient = new RestClient(option);
            _request = new RestRequest($"orders/{{orderId}}", Method.Patch); 
            _request.AddUrlSegment("orderId", _tempOrderNumber); // adding a url segment to the request url to include an order number 
            _request.AddJsonBody(body);
            _response = _restClient.Execute(_request);
            Assert.IsTrue(_response.IsSuccessful);

            //// confirming change
            authenticator = new OAuth2AuthorizationRequestHeaderAuthenticator(_bookAuth, "Bearer");
            RestClientOptions options = new RestClientOptions(_baseBooksUrl)
            {
                Authenticator = authenticator
            };
            _restClient = new RestClient(options);
            _request = new RestRequest($"orders/{{orderId}}", Method.Get); 
            _request.AddUrlSegment("orderId", _tempOrderNumber);
            _response = _restClient.Execute(_request);
            var content = JObject.Parse(_response.Content);
            Assert.AreNotEqual("Zak Rogers", content.GetValue("bookId"));
            Console.WriteLine(content);
        }

    
    }
}
