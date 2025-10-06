Feature: Card Validation
  Validate card fields and determine payment system type.

  #Scenario: Valid full card input
  #Given the card owner is "John Doe"
  #And the card number is "4111111111111111"
  #And the issue date is "12/30"
  #And the CVC is "123"
  #When I validate the full card
  #Then the result should be valid
  #And the payment system should be "Visa"

  Scenario: Valid card owner
    Given the card owner is "John Doe"
    When I validate the owner
    Then the result should be valid
#
#  Scenario: Invalid card owner
#    Given the card owner is ""
#    When I validate the owner
#    Then the result should be false
#
#  Scenario: Valid issue date
#    Given the issue date is "01/25"
#    When I validate the issue date
#    Then the result should be true
#
#  Scenario: Invalid issue date
#    Given the issue date is "13/99"
#    When I validate the issue date
#    Then the result should be false
#
#  Scenario: Valid CVC
#    Given the CVC is "123"
#    When I validate the CVC
#    Then the result should be true
#
#  Scenario: Invalid CVC
#    Given the CVC is "12"
#    When I validate the CVC
#    Then the result should be false
#
#  Scenario: Valid card number
#    Given the card number is "4111111111111111"
#    When I validate the card number
#    Then the result should be true
#
#  Scenario: Invalid card number
#    Given the card number is "1234567890123456"
#    When I validate the card number
#    Then the result should be false
#
#  Scenario: Get Visa payment system
#    Given the card number is "4111111111111111"
#    When I get the payment system type
#    Then the payment system should be "Visa"
#
#  Scenario: Get MasterCard payment system
#    Given the card number is "5500000000000004"
#    When I get the payment system type
#    Then the payment system should be "MasterCard"
#
#  Scenario: Unknown card type throws exception
#    Given the card number is "0000000000000000"
#    When I get the payment system type
#    Then an exception should be thrown

# CardValidation.Web
  Scenario: Valid Visa card number
    Given I have a credit card number "4111111111111111"
    When I send a POST request to "/CardValidation/card/credit/validate"
    Then the response status code should be 200
    And the response should contain "Visa"

  Scenario: Invalid card number
    Given I have a credit card number ""
    When I send a POST request to "/CardValidation/card/credit/validate"
    Then the response status code should be 400
