using CardValidation.Core.Enums;
using CardValidation.Core.Models;
using CardValidation.Core.Services;
using FluentAssertions;
using Microsoft.VisualStudio.TestPlatform.TestHost;
using Reqnroll;
using System.Net.Http.Json;
using ValidationResult = CardValidation.Core.Models.ValidationResult;
using Microsoft.AspNetCore.Mvc.Testing;

namespace CardValidation.Tests.Integration.StepsDefinition
{
    [Binding]
    public class CardValidationSteps
    {
        // Unit-level validation
        private readonly CardValidationService _service = new();
        private ValidationResult _validationResult = new();
        private string _owner = string.Empty;
        private string _issueDate = string.Empty;
        private string _cvc = string.Empty;
        private string _cardNumber = string.Empty;
        private bool _result;
        private PaymentSystemType _paymentSystemType;
        private Exception? _exception;

        // Integration-level API testing
        private readonly WebApplicationFactory<Program> _factory;
        private readonly ScenarioContext _scenarioContext;
        private HttpResponseMessage _response = default!;
        private CreditCard _creditCard = default!;

        public CardValidationSteps(WebApplicationFactory<Program> factory, ScenarioContext scenarioContext)
        {
            _factory = factory;
            _scenarioContext = scenarioContext;
        }

        // Unit test steps
        [Given("the card owner is {string}")]
        public void GivenTheCardOwnerIs(string owner) => _owner = owner;

        [Given("the issue date is {string}")]
        public void GivenTheIssueDateIs(string issueDate) => _issueDate = issueDate;

        [Given("the CVC is {string}")]
        public void GivenTheCVCIs(string cvc) => _cvc = cvc;

        [Given("the card number is {string}")]
        public void GivenTheCardNumberIs(string cardNumber) => _cardNumber = cardNumber;

        [When("I validate the owner")]
        public void WhenIValidateOwner() => _result = _service.ValidateOwner(_owner);

        [When("I validate the issue date")]
        public void WhenIValidateIssueDate() => _result = _service.ValidateIssueDate(_issueDate);

        [When("I validate the CVC")]
        public void WhenIValidateCvc() => _result = _service.ValidateCvc(_cvc);

        [When("I validate the card number")]
        public void WhenIValidateCardNumber() => _result = _service.ValidateNumber(_cardNumber);

        [When("I validate the full card")]
        public void WhenIValidateTheFullCard()
        {
            var cardDto = new CardDto
            {
                Owner = _owner,
                Number = _cardNumber,
                IssueDate = _issueDate,
                Cvc = _cvc
            };

            _validationResult = _service.ValidateCard(cardDto);
        }

        [Then("the result should be valid")]
        public void ThenTheResultShouldBeValid() => _result.Should().BeTrue();

        [Then("the result should be invalid")]
        public void ThenTheResultShouldBeInvalid() => _validationResult.IsValid.Should().BeFalse();

        [Then("the errors should contain:")]
        public void ThenTheErrorsShouldContain(Table table)
        {
            foreach (var row in table.Rows)
            {
                var expectedError = row[0];
                _validationResult.Errors.Should().Contain(expectedError);
            }
        }

        [When("I get the payment system type")]
        public void WhenIGetThePaymentSystemType()
        {
            try
            {
                _paymentSystemType = _service.GetPaymentSystemType(_cardNumber);
            }
            catch (Exception ex)
            {
                _exception = ex;
            }
        }

        [Then("the result should be (true|false)")]
        public void ThenTheResultShouldBe(bool expected) => _result.Should().Be(expected);

        [Then("the payment system should be {string}")]
        public void ThenThePaymentSystemShouldBe(string expected)
        {
            Enum.TryParse(expected, out PaymentSystemType expectedType).Should().BeTrue();
            _paymentSystemType.Should().Be(expectedType);
        }

        [Then("an exception should be thrown")]
        public void ThenAnExceptionShouldBeThrown() => _exception.Should().BeOfType<NotImplementedException>();

        // Integration test steps
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
            var actualStatusCode = (int)_response.StatusCode;
            actualStatusCode.Should().Be(expectedStatusCode);
        }

        [Then(@"the response should contain ""(.*)""")]
        public async Task ThenTheResponseShouldContain(string expectedContent)
        {
            var content = await _response.Content.ReadAsStringAsync();
            content.Should().Contain(expectedContent);
        }
    }
}