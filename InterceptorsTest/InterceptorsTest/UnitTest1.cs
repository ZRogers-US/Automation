using Newtonsoft.Json.Linq;
using RestSharp;

namespace InterceptorsTest
{
    [TestClass]
    public class UnitTest1
    {

        private string _baseRickUrl = "https://rickandmortyapi.com/api";
        private string _baseBookUrl = "https://simple-books-api.glitch.me";
        private string _token = "5a2eac996bb065d25bf6e0dbec8c9200adc5ccc0d7c72dc86592e10bfd12fc23";
        private int _characterId = 1;

        

        //Test method using old interception to change the Uri before sending the request then incrementing
        //the characterId after the request has completed
        [TestMethod]
        public async Task GetCharacterData_InterceptRequest_CheckStatusCode_ShouldBeOkay()
        {
            RestClientOptions options = new RestClientOptions(_baseRickUrl);
            RestClient client = new RestClient(options);
            RestRequest request = new RestRequest("/character/");
            request.OnBeforeRequest = (preRequest) =>
            {
                preRequest.RequestUri = new Uri(preRequest.RequestUri.ToString() + _characterId);
                return ValueTask.CompletedTask;
            };
            request.OnAfterRequest = (postRequest) =>
            {
                _characterId++;
                return ValueTask.CompletedTask;
            };
            request.OnBeforeDeserialization = (preDeserialization) =>
            {
                Console.WriteLine("Before Deserialization Content is of type: " + preDeserialization.Content.GetType());
            };
            RestResponse<RickAndMortyCharacter> response = await client.ExecuteAsync<RickAndMortyCharacter>(request);

            RickAndMortyCharacter? character = response.Data;
            
            Assert.AreEqual(response.StatusCode.ToString(), "OK");
            Assert.AreEqual(_characterId, 2);
        }


        //Test method using old interception to add an authorisation header before sending the request
        [TestMethod]
        public async Task PostBookOrder_InterceptClient_CheckStatusCode_ShouldBeOkay()
        {
            var bookOrderBody = new
            {
                bookId = 6,
                customerName = "Zak"
            };
            RestClientOptions options = new RestClientOptions(_baseBookUrl);
            RestClient client = new RestClient(options);
            RestRequest request = new RestRequest("/orders", Method.Post);
            request.OnBeforeRequest = (http) =>
            {
                http.Headers.Add("Authorization", "Bearer " + _token);
                return ValueTask.CompletedTask;
            };
            request.AddJsonBody(bookOrderBody);
          
            RestResponse response = await client.ExecuteAsync(request);

            JObject content = JObject.Parse(response.Content);
            Console.WriteLine(response.Content);

            Assert.AreEqual((int)response.StatusCode, 201);
        }


    }

}



