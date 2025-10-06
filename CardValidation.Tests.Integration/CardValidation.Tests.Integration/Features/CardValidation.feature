Feature: Validate Credit Card
  As a QA engineer
  I want to validate credit card numbers via the API
  So that I can ensure the correct payment system is returned

  Scenario: Valid Visa card number
    Given I have a credit card number "4111111111111111"
    When I send a POST request to "/CardValidation/card/credit/validate"
    Then the response status code should be 200
    And the response should contain "Visa"

  Scenario: Invalid card number
    Given I have a credit card number ""
    When I send a POST request to "/CardValidation/card/credit/validate"
    Then the response status code should be 400