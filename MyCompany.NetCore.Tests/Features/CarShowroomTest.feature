Feature: Showroom Service API Test

 @ShowroomAPI
Scenario Outline: CartypeGetAPI
	Given I have the endpoint for resource <application>
	And I set api request method to <apimethod>
	And I have set <authorization> for the request with header parameters header1 And header2
	And I have parameters for constructing Endpoint resource1 And resource2
	When I send request
	Then The response status code should be <statuscode> with standard description
	And The response should be received in <maxresponsetime> milliseconds
	
	Examples:
		| test                      | application | authorization | apimethod | contenttype                     | statuscode | maxresponsetime |
		| HappyPathSaloon           | saloon      | No            | GET       |  application/json;charset=utf-8 | 200        | 200             |
		| HappyPathSUV              | suv         | No            | GET       |  application/json;charset=utf-8 | 200        | 200             |
		| HappyPathHatchback        | hatchback   | No            | GET       |  application/json;charset=utf-8 | 200        | 200             |
		| WrongCarTypeText          | suvallsmall | No            | GET       |  application/json;charset=utf-8 | 200        | 200             | 
		| NegativeEmptyCarType      | empty       | No            | GET       |  application/json;charset=utf-8 | 404        | 200             | 
		| NegativeWrongCarType      | mini        | No            | GET       |  application/json;charset=utf-8 | 404        | 200             |