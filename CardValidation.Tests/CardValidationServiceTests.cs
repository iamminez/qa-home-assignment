using Xunit;
using CardValidation.Core.Services;
using CardValidation.Core.Enums;
using CardValidation.Core.Models;
using System;

namespace CardValidation.Core.Tests.Unit
{
    public class CardValidationServiceTests
    {
        private readonly CardValidationService _service = new CardValidationService();

        //Validate Owner
        [Theory] // Parameter
        [InlineData("Mari Tamm")]
        [InlineData("Mari Dilig")]
        public void ValidateOwner_ValidName(string owner)
        {
            var result = _service.ValidateOwner(owner);
            Assert.True(result);
        }

        [Theory]
        [InlineData("Märi Tämm Smith Jr.")]
        public void ValidateOwner_InvalidName(string owner)
        {
            var result = _service.ValidateOwner(owner);
            Assert.False(result);
        }

        [Theory]
        [InlineData("Kad!!!")]
        public void ValidateOwner_NameWithSpecialCharacters(string owner)
        {
            var result = _service.ValidateOwner(owner);
            Assert.False(result);
        }

        [Theory]
        [InlineData("Mari318333")]

        public void ValidateOwner_NameWithNumbers(string owner)
        {
            var result = _service.ValidateOwner(owner);
            Assert.False(result);
        }
        [Theory]
        [InlineData("")]
        public void ValidateOwner_NameEmpty(string owner)
        {
            var result = _service.ValidateOwner(owner);
            Assert.False(result);
        }

        //Validate Issue Date
        [Theory]
        [InlineData("12/2025")] // xxxx format
        public void ValidateIssueDate_ValidFourCharacterFormat(string issueDate)
        {
            var result = _service.ValidateIssueDate(issueDate);
            Assert.True(result);
        }

        [Theory]
        [InlineData("12/30")] // Future date
        public void ValidateIssueDate_FutureDateFormat(string issueDate)
        {
            var result = _service.ValidateIssueDate(issueDate);
            Assert.True(result);
        }

        [Theory]
        [InlineData("01/99")] // Future short year
        public void ValidateIssueDate_FutureShortYear(string issueDate)
        {
            var result = _service.ValidateIssueDate(issueDate);
            Assert.True(result);
        }

        [Theory]
        [InlineData("01/20")] // Past date
        public void ValidateIssueDate_PastDate(string issueDate)
        {
            var result = _service.ValidateIssueDate(issueDate);
            Assert.False(result);
        }

        [Theory]
        [InlineData("13/25")] // Invalid month
        public void ValidateIssueDate_InvalidMonth(string issueDate)
        {
            var result = _service.ValidateIssueDate(issueDate);
            Assert.False(result);
        }

        [Theory]
        [InlineData("12/000")] // Invalid month
        public void ValidateIssueDate_InvalidYear(string issueDate)
        {
            var result = _service.ValidateIssueDate(issueDate);
            Assert.False(result);
        }

        [Theory]
        [InlineData("abc")]   // Invalid char
        public void ValidateIssueDate_InvalidInput(string issueDate)
        {
            var result = _service.ValidateIssueDate(issueDate);
            Assert.False(result);
        }

        //Validate CVC
        [Theory]
        [InlineData("123")]
        public void ValidateCvc_ValidCvc(string cvc)
        {
            var result = _service.ValidateCvc(cvc);
            Assert.True(result);
        }

        [Theory]
        [InlineData("1234")]
        public void ValidateCvc_FourNumbersCvc(string cvc)
        {
            var result = _service.ValidateCvc(cvc);
            Assert.True(result);
        }

        [Theory]
        [InlineData("abcd")]
        public void ValidateCvc_LettersInCvc(string cvc)
        {
            var result = _service.ValidateCvc(cvc);
            Assert.False(result);
        }

        [Theory]
        [InlineData("")]
        public void ValidateCvc_EmptyCvc(string cvc)
        {
            var result = _service.ValidateCvc(cvc);
            Assert.False(result);
        }

        //Validate Card Number
        [Theory]
        [InlineData("4111111111111111")] // Visa
        public void ValidateNumber_VisaCardNumber(string cardNumber)
        {
            var result = _service.ValidateNumber(cardNumber);
            Assert.True(result);
        }

        [Theory]
        [InlineData("5500000000000004")] // MasterCard
        public void ValidateNumber_MastercardCardNumber(string cardNumber)
        {
            var result = _service.ValidateNumber(cardNumber);
            Assert.True(result);
        }

        [Theory]
        [InlineData("340000000000009")]  // AmEx
        public void ValidateNumber_AmExCardNumber(string cardNumber)
        {
            var result = _service.ValidateNumber(cardNumber);
            Assert.True(result);
        }

        [Theory]
        [InlineData("")] // Empty
        public void ValidateNumber_EmptyCardNumber(string cardNumber)
        {
            var result = _service.ValidateNumber(cardNumber);
            Assert.False(result);
        }

        //Validate Payment Card 
        [Theory]
        [InlineData("4111111111111111")] // Visa
        public void GetPaymentSystemType_VisaCard(string cardNumber)
        {
            var result = _service.GetPaymentSystemType(cardNumber);
            Assert.Equal(PaymentSystemType.Visa, result);
        }

        [Theory]
        [InlineData("5500000000000004")] // MasterCard
        public void GetPaymentSystemType_MasterCard(string cardNumber)
        {
            var result = _service.GetPaymentSystemType(cardNumber);
            Assert.Equal(PaymentSystemType.MasterCard, result);
        }

        [Theory]
        [InlineData("340000000000009")] // American Express
        public void GetPaymentSystemType_AmEx(string cardNumber)
        {
            var result = _service.GetPaymentSystemType(cardNumber);
            Assert.Equal(PaymentSystemType.AmericanExpress, result);
        }

        [Fact] //No parameters
        public void GetPaymentSystemType_ThrowException_ForUnknownCard()
        {
            var invalidCard = "1234567890123456";
            Assert.Throws<NotImplementedException>(() => _service.GetPaymentSystemType(invalidCard));
        }
    }
}