using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using RestSharp;
using RestSharpSpecFlow.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RestSharpSpecFlow.StepDefinitions
{/*
    public class Locations_ContextInjection
    {
        public string? ResponseContent { get; set; }
        public List<Location> Places { get; set; }
        public List<string>? UkCities { get; set; } 
        public List<string>? FranceCities { get; set; } 
        public List<string>? MoroccoCities { get; set; } 

        public Locations_ContextInjection()
        {
            Places = new List<Location>();
            UkCities = new List<string>();
            FranceCities = new List<string>();
            MoroccoCities = new List<string>();
        }
    }


    [Binding]
    public sealed class Locations_StepDefinitions
    {
        private string _baseUrlLocations = "https://api.scaleserp.com";
        private readonly Locations_ContextInjection _contextInjection;// = new Locations_ContextInjection();

        public Locations_StepDefinitions(Locations_ContextInjection contextInjection) 
        {
            _contextInjection = contextInjection;
        }

        [Given("User is authorised on API")]
        public async Task GivenUserIsAuthorisedOnAPI()
        {
            RestClientOptions options = new RestClientOptions(_baseUrlLocations);
            RestClient client = new RestClient(options);
            RestRequest request = new RestRequest("/locations?api_key=demo&q=london", Method.Get);
            //request.AddQueryParameter("api_key", "demo");
            RestResponse response = await client.ExecuteAsync(request);
            Console.WriteLine(response.StatusCode);
            if(response.StatusCode.ToString() != "OK")
            {
                Assert.Fail("auth failed");
            }
        }

        [Given("A Country Code (.*)")]
        public async Task GivenACountry(string country)
        {
            RestClientOptions options = new RestClientOptions(_baseUrlLocations);
            RestClient client = new RestClient(options);
            RestRequest request = new RestRequest("/locations", Method.Get);
            request.AddQueryParameter("api_key", "demo");
            request.AddQueryParameter("country_code", country);
            request.AddQueryParameter("limit", 100);
            RestResponse response = await client.ExecuteGetAsync(request);
            _contextInjection.ResponseContent = response.Content;
        }

        [When("Type is a City")]
        public void TypeIsACity()
        {
            var content = JsonConvert.DeserializeObject<LocationsResultsModel>(_contextInjection.ResponseContent);

            foreach(var place in content.Locations)
            {
                if(place.Type == "City")
                {
                    _contextInjection.Places.Add(place);
                }
            }
        }

        [Then("Add to Lists")]
        public void ThenAddToLists()
        {
            foreach (var place in _contextInjection.Places)
            {
                switch (place.Country_Code)
                {
                    case "FR":
                        _contextInjection.FranceCities.Add(place.Name);
                        break;
                    case "MA":
                        _contextInjection.MoroccoCities.Add(place.Name);
                        break;
                    case "UK":
                        _contextInjection.UkCities.Add(place.Name);
                        break;
                }
            }
            Console.WriteLine("France: " + _contextInjection.FranceCities.Count);
            Console.WriteLine("UK: " + _contextInjection.UkCities.Count);
            Console.WriteLine("Morocco: " + _contextInjection.MoroccoCities.Count);
        }

    }*/
}
