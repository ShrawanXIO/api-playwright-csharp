Feature: Product CRUD
    As a QA engineer
    I want to verify DummyJSON's product endpoints
    So that I can confirm create, read, update, and delete all work correctly

@smoke
Scenario: Retrieve all products
    When I request all products
    Then I should receive a list of products

@regression
Scenario: Retrieve a single product by id
    When I request the product with id 1
    Then I should receive that product

@regression
Scenario: Create a new product
    Given I have a new product titled "Test Product"
    When I create the product
    Then the created product should have a title of "Test Product"

@regression
Scenario Outline: Update a product's title
    Given I want to update product 1 with the title "<title>"
    When I update the product
    Then the updated product should have the title "<title>"

    Examples:
      | title                 |
      | Updated Product Title |
      | Another Updated Title |

@regression
Scenario: Delete a product
    When I delete the product with id 1
    Then the product should be marked as deleted