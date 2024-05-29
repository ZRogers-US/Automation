using Microsoft.VisualStudio.TestTools.UnitTesting;
using RestSharp;
using RestSharpSpecFlow.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RestSharpSpecFlow.StepDefinitions
{
    [Binding]
    public sealed class RickAndMorty_StepDefinitions
    {
        private RestClient _restClient = new RestClient();
        private RestRequest _restRequest = new RestRequest();
        private string _baseUrlRickAndMorty = "https://rickandmortyapi.com/api";
        private RickAndMortyCharacter? firstCharacter;
        private RickAndMortyCharacter? secondCharacter;
        private bool firstCharacterMatchesSpecies = false;
        private bool secondCharacterMatchesSpecies = false;
        private RickAndMortyCharacter? _character;
        private static List<RickAndMortyCharacter>? _femaleCharacters = new List<RickAndMortyCharacter>();

        [Given("the first character has an ID of (.*)")]
        public async Task GivenTheFirstCharacterIdOf(int firstCharacterId)
        {
            RestClientOptions options = new RestClientOptions(_baseUrlRickAndMorty);
            using (_restClient = new RestClient(options))
            {
                _restRequest = new RestRequest("/character/{id}", Method.Get);
                _restRequest.AddUrlSegment("id", firstCharacterId);
                RestResponse<RickAndMortyCharacter> response = await _restClient.ExecuteAsync<RickAndMortyCharacter>(_restRequest);
                firstCharacter = response.Data;
                if (firstCharacter != null) Console.WriteLine(firstCharacter.Name);
            };
        }

        [Given("the second character has an ID of (.*)")]
        public async Task GivenTheSecondCharacterIdOf(int secondCharacterId)
        {
            RestClientOptions options = new RestClientOptions(_baseUrlRickAndMorty);
            using (_restClient = new RestClient(options))
            {
                _restRequest = new RestRequest("/character/{id}", Method.Get);
                _restRequest.AddUrlSegment("id", secondCharacterId);
                RestResponse<RickAndMortyCharacter> response = await _restClient.ExecuteAsync<RickAndMortyCharacter>(_restRequest);
                secondCharacter = response.Data;
                if (secondCharacter != null) Console.WriteLine(secondCharacter.Name);
            };
        }

        [When("the two characters' species is compared to (.*)")]
        public void WhenCharactersSpeciesIsComparedTo(string species)
        {
            if (firstCharacter != null && secondCharacter != null)
            {
                if (firstCharacter.Species == species) firstCharacterMatchesSpecies = true;
                if (secondCharacter.Species == species) secondCharacterMatchesSpecies = true;
            }
        }

        [Then("the result should be true")]
        public void ThenTheResultShouldBeTrue()
        {
            Assert.IsTrue(firstCharacterMatchesSpecies);
            Assert.IsTrue(secondCharacterMatchesSpecies);
        }

        [Given("A character ID of (.*)")]
        public async Task GivenACharacters(int characterID)
        {
            RestClientOptions options = new RestClientOptions(_baseUrlRickAndMorty);
            using (_restClient = new RestClient(options))
            {
                _restRequest = new RestRequest("/character/{id}", Method.Get);
                _restRequest.AddUrlSegment("id", characterID);
                RestResponse<RickAndMortyCharacter> response = await _restClient.ExecuteAsync<RickAndMortyCharacter>(_restRequest);
                _character = response.Data;
            }
        }

        [When("A character is female")]
        public void CharacterIsFemale()
        {
            if(_character != null)
            {
                if (_character.Gender != "Female")
                {
                    Assert.Fail($"The character, {_character.Name}, is not Female");
                    _character = null;
                }
                else
                {
                    Assert.IsTrue(_character.Gender == "Female");
                }
            }
        }

        [Then("Add to list of female characters")]
        public void AddToList()
        {
            if(_character != null)
            {
                _femaleCharacters.Add(_character);
                Console.WriteLine(_femaleCharacters.Count);
            }
        }

    }
}
