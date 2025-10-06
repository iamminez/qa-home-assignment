Feature: Validate Credit Card
  As a QA engineer
  I want to validate credit card numbers via the API
  So that I can ensure the correct payment system is returned

Scenario: Full card details is valid
    Given I have the owner "Jane Doe"
    Given I have a credit card number "4111111111111111"
    Given I have the CVV "123"
    Given I have the date "09/29"
    When I send a POST request to "/CardValidation/card/credit/validate"
    Then the response status code should be 200

Scenario: Visa card number is valid
    Given I have the owner "Jane Doe"
    Given I have a credit card number "4111111111111111"
    Given I have the CVV "123"
    Given I have the date "09/29"
    When I send a POST request to "/CardValidation/card/credit/validate"
    Then the response status code should be 200
    And the response should contain "Visa"

Scenario: MasterCard card number is valid
    Given I have the owner "Jane Doe"
    Given I have a credit card number "5500000000000004"
    Given I have the CVV "123"
    Given I have the date "09/29"
    When I send a POST request to "/CardValidation/card/credit/validate"
    Then the response status code should be 200
    And the response should contain "MasterCard"

Scenario: AmericanExpress card number is valid
    Given I have the owner "Jane Doe"
    Given I have a credit card number "340000000000009"
    Given I have the CVV "123"
    Given I have the date "09/29"
    When I send a POST request to "/CardValidation/card/credit/validate"
    Then the response status code should be 200
    And the response should contain "AmericanExpress"

Scenario: Owner should be required
    Given I have a credit card number "340000000000009"
    Given I have the CVV "123"
    Given I have the date "09/29"
    When I send a POST request to "/CardValidation/card/credit/validate"
    Then the response status code should be 400
    And the response should contain validation error "Owner is required" for field "Owner"
 
Scenario: Owner name with numbers is invalid
    Given I have the owner "Jane 123"
    Given I have a credit card number "340000000000009"
    Given I have the CVV "123"
    Given I have the date "09/29"    
    When I send a POST request to "/CardValidation/card/credit/validate"
    Then the response status code should be 400
    And the response should contain validation error "Wrong owner" for field "Owner"

Scenario: Owner name with special characters is invalid
    Given I have the owner "Jane¤#&"( J(/&"
    Given I have a credit card number "340000000000009"
    Given I have the CVV "123"
    Given I have the date "09/29"    
    When I send a POST request to "/CardValidation/card/credit/validate"
    Then the response status code should be 400
    And the response should contain validation error "Wrong owner" for field "Owner"

Scenario: Owner name with alphanumeric characters is invalid
    Given I have the owner "Jan3 D03"
    Given I have a credit card number "340000000000009"
    Given I have the CVV "123"
    Given I have the date "09/29"    
    When I send a POST request to "/CardValidation/card/credit/validate"
    Then the response status code should be 400
    And the response should contain validation error "Wrong owner" for field "Owner"

Scenario: Credit card number should be required
    Given I have the owner "Jane Doe"
    Given I have the CVV "123"
    Given I have the date "09/29"
    When I send a POST request to "/CardValidation/card/credit/validate"
    Then the response status code should be 400
    And the response should contain validation error "Number is required" for field "Number"

Scenario: Credit card number with letters is invalid
    Given I have the owner "Jane Doe"
    Given I have a credit card number "340000000000abc"
    Given I have the CVV "123"
    Given I have the date "09/29"    
    When I send a POST request to "/CardValidation/card/credit/validate"
    Then the response status code should be 400
    And the response should contain validation error "Wrong number" for field "Number"

Scenario: Credit card number with special characters is invalid
    Given I have the owner "Jane Doe"
    Given I have a credit card number "&4000000000000!"
    Given I have the CVV "123"
    Given I have the date "09/29"    
    When I send a POST request to "/CardValidation/card/credit/validate"
    Then the response status code should be 400
    And the response should contain validation error "Wrong number" for field "Number"

Scenario: CVV should be required
  Given I have the owner "John Doe"
  And I have a credit card number "4111111111111111"
  And I have the date "09/29"
  When I send a POST request to "/CardValidation/card/credit/validate"
  Then the response status code should be 400
  And the response should contain validation error "Cvv is required" for field "Cvv"

Scenario: CVV with letters is invalid
    Given I have the owner "Jane Doe"
    Given I have a credit card number "340000000000009"
    Given I have the CVV "abc"
    Given I have the date "09/29"    
    When I send a POST request to "/CardValidation/card/credit/validate"
    Then the response status code should be 400
    And the response should contain validation error "Wrong cvv" for field "Cvv"

Scenario: CVV with special characters is invalid
    Given I have the owner "Jane Doe"
    Given I have a credit card number "340000000000009"
    Given I have the CVV "abc!"
    Given I have the date "09/29"    
    When I send a POST request to "/CardValidation/card/credit/validate"
    Then the response status code should be 400
    And the response should contain validation error "Wrong cvv" for field "Cvv"

Scenario: Date should be required
  Given I have the owner "John Doe"
  And I have a credit card number "4111111111111111"
  And I have the CVV "123"
  When I send a POST request to "/CardValidation/card/credit/validate"
  Then the response status code should be 400
  And the response should contain validation error "Date is required" for field "Date"

Scenario: Date with letters is invalid
    Given I have the owner "Jane Doe"
    Given I have a credit card number "340000000000009"
    Given I have the CVV "123"
    Given I have the date "ab/29"    
    When I send a POST request to "/CardValidation/card/credit/validate"
    Then the response status code should be 400
    And the response should contain validation error "Wrong date" for field "Date"

Scenario: Date with special characters is invalid
    Given I have the owner "Jane Doe"
    Given I have a credit card number "340000000000009"
    Given I have the CVV "123"
    Given I have the date "0!/29"    
    When I send a POST request to "/CardValidation/card/credit/validate"
    Then the response status code should be 400
    And the response should contain validation error "Wrong date" for field "Date"

Scenario: Date should not be expired
  Given I have the owner "John Doe"
  And I have a credit card number "4111111111111111"
  And I have the date "09/24"
  And I have the CVV "123"
  When I send a POST request to "/CardValidation/card/credit/validate"
  Then the response status code should be 400
  And the response should contain validation error "Wrong date" for field "Date"
