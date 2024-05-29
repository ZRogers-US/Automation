using Microsoft.VisualStudio.TestTools.UnitTesting;
using RestSharp;
using System;
using Newtonsoft.Json.Linq;
using System.Threading.Tasks;
using RestSharp.Authenticators.OAuth2;
using System.Net.Http;
using Newtonsoft.Json;
using System.Threading;
using System.Xml.Linq;
using RestSharp.Serializers.CsvHelper;
using System.IO;
using RestSharp.Serializers;
using System.Linq;
using System.Collections.Generic;
using FluentAssertions;
using FluentAssertions.Execution;

namespace HowToRestSharp
{

    [TestClass]
    public class AsyncUnitTest
    {
        private string _baseStarWarsUrl = "https://swapi.dev/api";
        private string _baseToDoUrl = "https://api.vschool.io/ZacharyRogers";
        private string _baseBookUrl = "https://simple-books-api.glitch.me";
        private string _baseCatUrl = "https://cat-fact.herokuapp.com";
        private string _baseMockTargetUrl = "https://mocktarget.apigee.net";
        private string _baseBeeceptorEcho = "https://echo.free.beeceptor.com";
        private string _baseRickUrl = "https://rickandmortyapi.com/api";
        private string _baseLocationsUrl = "https://api.scaleserp.com/locations";

        private RestClient _client = new RestClient();
        private RestRequest _request = new RestRequest();
        private RestResponse _response = new RestResponse();

        //test method to get a list of planets asynconously fromt he starwars api and asserting that the responce content is not null and the firstplanet in the list is tatooine
        [TestMethod]
        public async Task GetListOfPlanetsAsync()
        {
            RestClientOptions options = new RestClientOptions(_baseStarWarsUrl);
            _client = new RestClient(options);
            _request = new RestRequest("/planets", Method.Get);
            _response = await _client.ExecuteAsync<RestResponse>(_request);

            var content = JObject.Parse(_response.Content);

            string firstPlanetName = content.GetValue("results")[0]["name"].ToString();

            Assert.IsNotNull(content);
            Assert.AreEqual("Tatooine", firstPlanetName);
        }

        //test method to get a planet using a starwarsplanet class to parse the json data into 
        [TestMethod]
        public async Task GetAPlanetAsyncUsingPlanetClass()
        {
            RestClientOptions options = new RestClientOptions(_baseStarWarsUrl);
            _client = new RestClient(options);
            _request = new RestRequest("/planets/1", Method.Get);
            _response = await _client.ExecuteAsync(_request);
            var content = JsonConvert.DeserializeObject<StarwarsPlanet>(_response.Content);
            string firstPlanetName = content.Name;
            Console.WriteLine(firstPlanetName);
            //Assert.IsNotNull(content);
            //Assert.AreEqual("Tatooine", firstPlanetName);
            using (new AssertionScope())
            {
                content.Should().NotBeNull()
                            .And.BeOfType<StarwarsPlanet>();
                firstPlanetName.Should().StartWith("Tat")
                                    .And.EndWith("ine")
                                    .And.NotBeEmpty();
            }
        }

        //test method for getting person from the starwars api asynconosly
        [TestMethod]
        public async Task GetSinglePersonAsync()
        {
            RestClientOptions options = new RestClientOptions(_baseStarWarsUrl);
            _client = new RestClient(options);
            _request = new RestRequest("/people/3"); // when the request http method is not provided will default to get
            _response = await _client.ExecuteAsync(_request);

            Assert.IsTrue(_response.IsSuccessful);
            Console.WriteLine(_response.Content);
            var content = JObject.Parse(_response.Content);
            Assert.IsTrue(content.ContainsKey("name"));
        }

        //test method were two seperate requests are made for a random person and then comparing the two responces
        [TestMethod]
        public async Task CompareTwoPeopleAsync()
        {
            Random rand = new Random();
            int randomNumber = rand.Next(0, 25);

            RestClientOptions options = new RestClientOptions(_baseStarWarsUrl);
            _client = new RestClient(options);

            RestRequest firstRequest = new RestRequest("/people/{personId}", Method.Get);
            firstRequest.AddUrlSegment("personId", randomNumber);
            RestResponse firstResponse = await _client.ExecuteAsync(firstRequest);
            var firstContent = JObject.Parse(firstResponse.Content);
            Console.WriteLine(firstContent);

            randomNumber = rand.Next(0, 25);
            RestRequest secondRequest = new RestRequest("/people/{personId}"); //without a method will default to Get
            secondRequest.AddUrlSegment("personId", randomNumber);
            RestResponse secondResponse = await _client.ExecuteAsync(secondRequest);
            var secondContent = JObject.Parse(secondResponse.Content);
            Console.WriteLine(secondContent);

            if (firstResponse.IsSuccessful && secondResponse.IsSuccessful)
            {
                Assert.AreNotEqual(firstContent, secondContent);
            }
        }

        //test method to post three todos and then get the list of todos back to and checking that there are no duplicates of the firt todo item
        [TestMethod]
        public async Task PostAndGetThreeToDosAsync()
        {
            RestClientOptions options = new RestClientOptions(_baseToDoUrl);
            _client = new RestClient(options);

            var firstToDo = new
            {
                title = "This is the first task",
                description = "This is optional 13",
                price = 10, //must be a number
                imgUrl = "http://path-to-url.jpg",
                completed = false //must be boolean                
            };
            var secondToDo = new
            {
                title = "This is the second task",
                description = "This is optional 14",
                price = 30, //must be a number
                imgUrl = "http://path-to-url.jpg",
                completed = false //must be boolean                
            };
            var thirdToDo = new
            {
                title = "This is the third task",
                description = "This is optional 15",
                price = 0, //must be a number
                imgUrl = "http://path-to-url.jpg",
                completed = false //must be boolean                
            };

            var toDoData = new[] { firstToDo, secondToDo, thirdToDo };

            foreach (var toDo in toDoData)
            {
                _request = new RestRequest("/todo", Method.Post);
                _request.AddJsonBody(toDo);
                _response = await _client.ExecuteAsync(_request);
                Assert.IsTrue(_response.IsSuccessful);
            }

            _request = new RestRequest("/todo", Method.Get);
            _response = await _client.ExecuteAsync(_request);
            var content = JArray.Parse(_response.Content);
            Console.WriteLine(content.Count);

            Assert.IsTrue(content.Count == 16);

            //Checking theres no duplicates of the first todo item
            for (int i = 1; i < content.Count; i++)
            {
                Assert.AreNotEqual(content[0], content[i]);
            }
        }

        //test method to get a todo item by its id
        [TestMethod]
        public async Task GetToDoTaskById()
        {
            RestClientOptions options = new RestClientOptions(_baseToDoUrl);
            _request = new RestRequest("/todo/{taskNumber}");
            _request.AddUrlSegment("taskNumber", "6617dcc178fe452c114b7a2c");
            _client = new RestClient(options);
            _response = await _client.ExecuteAsync(_request);
            var content = JObject.Parse(_response.Content);

            Assert.AreEqual(content.GetValue("title"), "This is the third task");
            Assert.AreEqual(content.GetValue("description"), "This is optional 9");
            Console.WriteLine(content);
        }

        //deleting a todo item using its id
        [TestMethod]
        public async Task DeleteToDoItem()
        {
            RestClientOptions options = new RestClientOptions(_baseToDoUrl);
            _client = new RestClient(options);
            _request = new RestRequest("/todo/{taskId}", Method.Delete);
            _request.AddUrlSegment("taskId", "6617dba878fe452c114b7a1d");
            _response = await _client.ExecuteAsync(_request);

            Assert.IsTrue((int)_response.StatusCode == 200);// parsing the status code into an int to compare with a expected http responce code
        }

        //test method to demonstraight the Put method asynciosly 
        [TestMethod]
        public async Task PutToDoUsingId()
        {
            RestClientOptions options = new RestClientOptions(_baseToDoUrl);
            _client = new RestClient(options);
            _request = new RestRequest("/todo/6617dc5978fe452c234b7a20", Method.Put);
            var body = new
            {
                title = "This task was put in by id",
                price = 29,
                imgUrl = "http://path-to-url.jpg",
                completed = true
            };
            _request.AddJsonBody(body);
            _response = await _client.ExecuteAsync(_request);
            var content = _response.Content;
            Console.WriteLine(content);
            Assert.IsTrue(_response.IsSuccessful);
        }

        //test method to demonstraight the post method to post userinfo to request a bearer auth token
        [TestMethod]
        public async Task PostBookAuthRequestAsync()
        {
            var requestBody = new
            {
                clientName = "Zak Rogers",
                clientEmail = "zrogers225@usspeaking.com"
            };
            RestClientOptions options = new RestClientOptions(_baseBookUrl);
            _client = new RestClient(options);
            _request = new RestRequest("/api-clients", Method.Post);
            _request.AddJsonBody(requestBody);
            _response = await _client.ExecuteAsync(_request);

            Console.WriteLine(_response.StatusCode);
            Console.WriteLine(_response.Content);
            Assert.IsTrue(_response.IsSuccessful);
        }

        //test method to demonstraight the post method to post userinfo to request a bearer auth token
        [TestMethod]
        public async Task PostBookAuthRequestSerialisationAsync()
        {
            var requestBody = new
            {
                clientName = "Zak Rogers",
                clientEmail = "zrogers225@usspeaking.com"
            };
            RestClientOptions options = new RestClientOptions(_baseBookUrl);
            _client = new RestClient(options, configureSerialization: s => s.UseJson());
            _request = new RestRequest("/api-clients", Method.Post);
            _request.AddBody(requestBody);
            _response = await _client.ExecuteAsync(_request);

            Console.WriteLine(_response.StatusCode);
            Console.WriteLine(_response.Content);
            Assert.IsTrue(_response.IsSuccessful);
        }

        //test method using the bearer token to post a book order using a cancellation token to cancel the async Task
        [TestMethod]
        public async Task PostBookOrderAsyncWithCancellationToken()
        {
            CancellationTokenSource source = new CancellationTokenSource(); //creating acancellationtoekn source
            CancellationToken cancellationToken = source.Token; // getting the token from the source

            var authenticator = new OAuth2AuthorizationRequestHeaderAuthenticator("9080bc62b7badd48d21d460adc96385f144d3742cfc1c1ce52f06c2fbf67fae0fig", "Bearer"); // remove fig from end to work
            var requestBody = new
            {
                bookId = 6,
                customerName = "Zak"
            };
            RestClientOptions options = new RestClientOptions(_baseBookUrl)
            {
                Authenticator = authenticator // adding the auth header to the options
            };
            _client = new RestClient(options);
            _request = new RestRequest("/orders", Method.Post);
            _request.AddJsonBody(requestBody);
            try
            {
                _response = await _client.ExecuteAsync(_request, cancellationToken); // if no cancellation token is provide will set to default
                bool requestAuthorised = (int)_response.StatusCode != 401; // confirm the request has been authorised
                if (requestAuthorised)
                {
                    Assert.IsTrue(requestAuthorised); // assert to confirm its authorised
                    Console.WriteLine(_response.Content);
                }
                else
                {
                    source.Cancel(); // if its not authorised cancel the async Task
                    cancellationToken.ThrowIfCancellationRequested(); // throw a operation canceled exception

                }
            }
            catch (OperationCanceledException ex) // exceptions used for when a task is canceled.
            {
                Console.WriteLine(ex.Message + " Request was unauthrised");
                Assert.Fail("Request was unauthorised"); //fails an assert with the message that the request was unauthorised
            }
        }

        //test method with try catch error handeling asynconous
        [TestMethod]
        public async Task PostBookOrderWithErrorHandlingAsync()
        {
            var requestBody = new
            {
                bookId = 7,
                customerName = "Zak"
            };
            RestClientOptions option = new RestClientOptions(_baseBookUrl)
            {
                ThrowOnAnyError = true // setting this property to true forces restsharp to throw an exception if any error occurs when making a request or during deseriaisation 
            };
            _client = new RestClient(option);
            _request = new RestRequest("/orders", Method.Post);
            _request.AddJsonBody(requestBody);
            try
            {
                _response = await _client.ExecuteAsync(_request);
            }
            catch (Exception ex)
            {
                Assert.Fail($"Request failed: {ex.Message}");
                throw new HttpRequestException("not authorised");
                //throw ex;
            }

        }

        //Test method using query parameters
        [TestMethod]
        public async Task QueryParametersViaAsync()
        {
            string animalType = "cat";
            int amountOfFacts = 5;
            RestClientOptions options = new RestClientOptions(_baseCatUrl);
            _client = new RestClient(options);
            _request = new RestRequest("/facts/random");
            _request.AddQueryParameter("animal_type", animalType); // AddQueryParameter must be used when using Post http method
            _request.AddQueryParameter("amount", amountOfFacts);
            _response = await _client.ExecuteAsync(_request);

            var content = JArray.Parse(_response.Content); // parseing the data into a Jarray as the responce data is an array of objects

            Console.WriteLine(content);
            Assert.IsTrue(content.Count == amountOfFacts);
        }

        [TestMethod]
        public async Task GetRequestParametersViaAsync()
        {
            string animalType = "cat";
            int amountOfFacts = 5;
            RestClientOptions options = new RestClientOptions(_baseCatUrl);
            _client = new RestClient(options);
            _request = new RestRequest("/facts/random").AddParameter("amount", amountOfFacts); // AddParameter can be used if using the Get http method
            _client.AddDefaultQueryParameter("animal_type", animalType); // adds a default query parameter to the client so each time the client is used it will use these query parameters
            _response = await _client.ExecuteAsync(_request);

            var content = JArray.Parse(_response.Content);

            Console.WriteLine(content);
            //Assert.IsTrue(content.Count == amountOfFacts);
            using (new AssertionScope())
            {
                content.Count.Should().BeLessThan(amountOfFacts + 1)
                                  .And.BePositive()
                                  .And.BeInRange(0, 10)
                                  .And.NotBeInRange(20, 100);
            }
        }

        //test method to get xml data
        [TestMethod]
        public async Task GetXmlDataAsync()
        {
            RestClientOptions options = new RestClientOptions(_baseMockTargetUrl);
            _client = new RestClient(options);
            _request = new RestRequest("/xml");
            _response = await _client.ExecuteAsync(_request);

            XElement content = XElement.Parse(_response.Content);
            //Console.WriteLine(_response.Content);
            //Console.WriteLine(content);
            //Console.WriteLine(content.Element("city"));
            Console.WriteLine(content.Element("city").Value); //getting the value inside the <city> tag

            using(new AssertionScope())
            {
                //Assert.IsTrue(content.Element("city").Value == "San Jose");
                content.Should().HaveElement("city", Exactly.Once());
                content.Element("city").Value.Should().StartWith("S")
                                                  .And.EndWith("e");
            }
        }

        //test method to post xml data 
        [TestMethod]
        public async Task PostXmlDataAsync()
        {
            RestClientOptions options = new RestClientOptions(_baseMockTargetUrl);
            _client = new RestClient(options);
            var body = "<book><title>The Wonderful Wizard of Oz</title><author>L. Frank Baum</author><year>1900</year><price>39.95</price></book>";
            _request = new RestRequest("/echo", Method.Post);
            _request.AddXmlBody(body); // using AddXmlBody adds the body with the content type of xml
            _response = await _client.ExecuteAsync(_request);

            Console.WriteLine(_response.Content);
            Assert.IsTrue(_response.IsSuccessful);

        }


        //test method to post xml data 
        [TestMethod]
        public async Task PostXmlDataSerialisationAsync()
        {
            RestClientOptions options = new RestClientOptions(_baseMockTargetUrl);
            _client = new RestClient(options, configureSerialization: s => s.UseXml());
            var body = "<book><title>The Wonderful Wizard of Oz</title><author>L. Frank Baum</author><year>1900</year><price>39.95</price></book>";
            _request = new RestRequest("/echo", Method.Post);
            _request.AddBody(body);
            _response = await _client.ExecuteAsync(_request);

            Console.WriteLine(_response.Content);
            Assert.IsTrue(_response.IsSuccessful);

        }

        //Getting csv responce data
        [TestMethod]
        public async Task GetCSVData()
        {
            RestClientOptions options = new RestClientOptions(_baseBeeceptorEcho);
            _client = new RestClient(options, configureSerialization: s => s.UseCsvHelper());
            _request = new RestRequest("/post", Method.Post);

            var csvData = "Name,Town,Age\r\nZak,Rushden,30\r\n";
            _request.AddBody(csvData, "text/csv");
            _response = await _client.ExecuteAsync(_request);

            var content = JObject.Parse(_response.Content);
            Console.WriteLine(content.GetValue("rawBody"));

        }

        //read csv file then Post
        [TestMethod]
        public async Task PostCSVDataFromFile()
        {
            RestClientOptions options = new RestClientOptions(_baseBeeceptorEcho);
            _client = new RestClient(options, configureSerialization: s => s.UseCsvHelper());
            _request = new RestRequest("/post", Method.Post);

            string text = File.ReadAllText("C:\\Users\\zrogers\\OneDrive - Universally Speaking Ltd\\Documents\\.AutomationTesting\\Trainign evidence\\csv request test.csv");

            _request.AddBody(text, "text/csv");
            _response = await _client.ExecuteAsync(_request);

            var content = JObject.Parse(_response.Content);
            Console.WriteLine(content.GetValue("rawBody"));

        }


        //test method to get a planet and parse straight into the starwarsplanet class
        [TestMethod]
        public async Task GetAsyncPlanetWithClass()
        {
            RestClientOptions options = new RestClientOptions(_baseStarWarsUrl);
            _client = new RestClient(options);
            _request = new RestRequest("/planets/1");

            StarwarsPlanet responsePlanet = await _client.GetAsync<StarwarsPlanet>(_request);
            Console.WriteLine(responsePlanet.Name);
            string firstPlanetName = responsePlanet.Name;

            Assert.IsNotNull(responsePlanet);
            Assert.AreEqual("Tatooine", firstPlanetName);
        }


        //test method to get a planet using executeAsync and a class
        [TestMethod]
        public async Task ExecuteAsyncPlanetWithClass()
        {
            RestClientOptions options = new RestClientOptions(_baseStarWarsUrl);
            _client = new RestClient(options);
            _request = new RestRequest("/planets/1");

            RestResponse<StarwarsPlanet> response = await _client.ExecuteAsync<StarwarsPlanet>(_request);
            StarwarsPlanet planet = response.Data;
            Console.WriteLine(planet.Name);

            string firstPlanetName = planet.Name;

            Assert.IsNotNull(planet);
            Assert.AreEqual("Tatooine", firstPlanetName);
        }

        //test method deserialization Error handleing try to deserialise a todo list into a StarwarsPlanet class
        [TestMethod]
        public async Task DeserializationError()
        {
            RestClientOptions options = new RestClientOptions(_baseToDoUrl)
            {
                FailOnDeserializationError = true
            };
            _client = new RestClient(options);
            _request = new RestRequest("/todo");

            RestResponse<StarwarsPlanet> response = await _client.ExecuteAsync<StarwarsPlanet>(_request);
            StarwarsPlanet planet = response.Data;
            Console.WriteLine(planet);
            Console.WriteLine(response.ResponseStatus);

            Assert.IsTrue(response.ResponseStatus == ResponseStatus.Error);
        }

        //test method deserialization Error handleing throwing an exception
        [TestMethod]
        public async Task DeserializationErrorThrowException()
        {
            RestClientOptions options = new RestClientOptions(_baseToDoUrl)
            {
                ThrowOnDeserializationError = true
            };
            _client = new RestClient(options);
            _request = new RestRequest("/todo");

            try
            {
                RestResponse<StarwarsPlanet> response = await _client.ExecuteAsync<StarwarsPlanet>(_request);
                StarwarsPlanet planet = response.Data;
                Console.WriteLine(planet);
                Console.WriteLine(response.ResponseStatus);

                Assert.IsTrue(response.ResponseStatus == ResponseStatus.Error);
            }
            catch (Exception ex) 
            {
                Assert.Fail(ex.Message);
            }
        }

        //test method to get rick and morts characters including pagination
        [TestMethod]
        public async Task GetCharactersPagination()
        {
            int pageNumber = 3;
            RestClientOptions options = new RestClientOptions(_baseRickUrl);
            _client = new RestClient(options);
            _request = new RestRequest("character/");
            _request.AddQueryParameter("page", pageNumber);
            _response = await _client.ExecuteAsync(_request);
            var content = JObject.Parse(_response.Content);

            Console.WriteLine("Page 3: " + content["results"]);

            string nextRequestUrl = content["info"]["next"].ToString();

            options = new RestClientOptions(nextRequestUrl);
            _client = new RestClient(options);
            _request = new RestRequest();
            _response = await _client.ExecuteAsync(_request);
            content = JObject.Parse(_response.Content);
            
            //Console.WriteLine(content);
            Console.WriteLine("Page 4: " + content["results"]);
        }


        //test method to get locations filters by london, collect all the results in a list and search the next pages untill viewing all the results then assert
        //the number of locations in the list matches that of number of results returned on one page with the limit set to the total number of locations
        [TestMethod]
        public async Task GetLocationsPagination()
        {
            int pageNumber = 1;
            int itemsPerPage = 10;
            string city = "london";
            RestClientOptions options = new RestClientOptions(_baseLocationsUrl);
            _client = new RestClient(options);
            _request = new RestRequest();
            _request.AddQueryParameter("api_key", "demo");
            _request.AddQueryParameter("q", city);
            _request.AddQueryParameter("page", pageNumber);
            _request.AddQueryParameter("limit", itemsPerPage);
            RestResponse<LocationsResults> response = await _client.ExecuteAsync<LocationsResults>(_request);
            LocationsResults results = response.Data;

            int numberOfLocations = results.Locations_Total;
            int numberOfResults = results.Locations_Total_Current_Page;
            List<Location> locations = new List<Location>();

            foreach (Location location in results.Locations)
            {
                locations.Add(location);
            }

            while (numberOfResults != numberOfLocations) 
            {
                pageNumber++;
                options = new RestClientOptions(_baseLocationsUrl);
                _client = new RestClient(options);
                _request = new RestRequest();
                _request.AddQueryParameter("api_key", "demo");
                _request.AddQueryParameter("q", city);
                _request.AddQueryParameter("page", pageNumber);
                _request.AddQueryParameter("limit", itemsPerPage);
                response = await _client.ExecuteAsync<LocationsResults>(_request);
                results = response.Data;

                numberOfResults += results.Locations_Total_Current_Page;
                foreach (Location location in results.Locations)
                {
                    locations.Add(location);
                }
            }

            Assert.AreEqual(numberOfLocations, numberOfResults);

            pageNumber = 1;
            itemsPerPage = numberOfLocations;
            options = new RestClientOptions(_baseLocationsUrl);
            _client = new RestClient(options);
            _request = new RestRequest();
            _request.AddQueryParameter("api_key", "demo");
            _request.AddQueryParameter("q", city);
            _request.AddQueryParameter("page", pageNumber);
            _request.AddQueryParameter("limit", itemsPerPage);
            response = await _client.ExecuteAsync<LocationsResults>(_request);
            results = response.Data;

            Assert.AreEqual(locations.Count, results.Locations.Length);
        }

        /*
        class HeaderInterceptor(string headerName, string headerValue) : Interceptors.Interceptor
        {
            public override ValueTask BeforeHttpRequest(HttpRequestMessage requestMessage, CancellationToken cancellationToken)
            {
                requestMessage.Headers.Add(headerName, headerValue);
                return ValueTask.CompletedTask;
            }
        }*/


    }
}
