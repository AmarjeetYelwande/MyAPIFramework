Feature: Template feature for testing rest API

#Install Node.js on your machine to get npm command
#For running local Json server on your machine issue following command
#npm install -g json-server
#create json file with some data and issue following command to run file on server
#json-server --watch db.json
#Do not forget to change endpoints.json in Data/Endpoints folder for your endpoint

 @LocalJsonServerAPITestGet
Scenario Outline: TestSimpleLocalJsonServerAPIGet
	Given I have the endpoint for resource <application>
	And I set api request method to <apimethod>
	And I have set <authorization> for the request with header parameters header1 And header2
	And I have parameters for constructing Endpoint resource1 And resource2
	When I send request
	Then The response status code should be <statuscode> with standard description
	And The response should be received in <maxresponsetime> milliseconds
	And The Response body should contain expected data for <api> call

	Examples:
		| test          | application              | authorization | apimethod | contenttype                    | statuscode | maxresponsetime | api           |
		| testsimpleapi | testlocaljsongetandelete | No            | GET       | application/json;charset=utf-8 | 200        | 5000            | testlocaljson |

