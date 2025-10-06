using System.Net.Http.Json;
using CardValidation.ViewModels;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc.Testing;
using Reqnroll;

namespace CardValidation.Tests.Integration.StepsDefinitions
{
    [Binding]
    public class CardValidationSteps
    {
        private readonly WebApplicationFactory<Program> _factory;
        private readonly ScenarioContext _scenarioContext;
        private HttpResponseMessage _response = default!;
        private readonly CreditCard _creditCard = new(); // Initialize once


        public CardValidationSteps(WebApplicationFactory<Program> factory, ScenarioContext scenarioContext)
        {
            _factory = factory;
            _scenarioContext = scenarioContext;
        }


        [Given(@"I have the owner ""(.*)""")]
        public void GivenIHaveTheOwnerName(string owner)
        {
            _creditCard.Owner = owner;
        }

        [Given(@"I have a credit card number ""(.*)""")]
        public void GivenIHaveACreditCardNumber(string number)
        {
            _creditCard.Number = number;
        }

        [Given(@"I have the CVV ""(.*)""")]
        public void GivenIHaveTheCVC(string cvv)
        {
            _creditCard.Cvv = cvv;
        }

        [Given(@"I have the date ""(.*)""")]
        public void GivenIHaveTheDate(string date)
        {
            _creditCard.Date = date;
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

        [Then(@"the response should contain validation error ""(.*)"" for field ""(.*)""")]
        public async Task ThenTheResponseShouldContainValidationError(string expectedError, string field)
        {
            var content = await _response.Content.ReadAsStringAsync();
            var errors = System.Text.Json.JsonSerializer.Deserialize<Dictionary<string, string[]>>(content);

            errors.Should().ContainKey(field);
            errors[field].Should().Contain(expectedError);
        }
    }
}