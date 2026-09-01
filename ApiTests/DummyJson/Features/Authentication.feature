Feature: Authentication
    As a QA engineer
    I want to verify the DummyJSON authentication flow
    So that I can confirm users can log in successfully

@smoke
Scenario: Successful login with valid credentials
    Given I have valid DummyJSON credentials
    When I log in
    Then I should receive a valid access token