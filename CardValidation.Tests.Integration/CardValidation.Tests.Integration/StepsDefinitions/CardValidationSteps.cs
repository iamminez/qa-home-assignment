using CardValidation.ViewModels;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc.Testing;
using Reqnroll;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;

namespace CardValidation.Tests.Integration.StepsDefinitions
{
    [Binding]
    public class CardValidationSteps
    {
        private readonly WebApplicationFactory<Program> _factory;
        private readonly ScenarioContext _scenarioContext;
        private HttpResponseMessage _response = default!;
        private CreditCard _creditCard = default!;

        public CardValidationSteps(WebApplicationFactory<Program> factory, ScenarioContext scenarioContext)
        {
            _factory = factory;
            _scenarioContext = scenarioContext;
        }

        [Given(@"I have a credit card number ""(.*)""")]
        public void GivenIHaveACreditCardNumber(string number)
        {
            _creditCard = new CreditCard { Number = number };
        }

        [When(@"I send a POST request to ""(.*)""")]
        public async Task WhenISendAPOSTRequestTo(string endpoint)
        {
            var client = _factory.CreateClient();
            _response = await client.PostAsJsonAsync(endpoint, _creditCard);
            _scenarioContext["Response"] = _response;
        }

        [Then(@"the response status code should be (.*)")]
        public void ThenTheResponseStatusCodeShouldBe(int expectedStatusCode)
        {
            ((int)_response.StatusCode).Should().Be(expectedStatusCode);
        }

        [Then(@"the response should contain ""(.*)""")]
        public async Task ThenTheResponseShouldContain(string expectedContent)
        {
            var content = await _response.Content.ReadAsStringAsync();
            content.Should().Contain(expectedContent);
        }
    }
}