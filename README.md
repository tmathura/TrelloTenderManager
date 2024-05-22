# Getting Started

To begin, you’ll need the following from Trello:

* API Key
* Token
* Board ID

### Generating an API Key

To generate an `API Key`, you must first create a Trello Power-Up. Refer to our [Managing Power-Ups documentation](https://developer.atlassian.com/cloud/trello/guides/power-ups/) for details on creating your first Power-Up. Once your Power-Up is created, visit the [Trello Power-Ups admin page](https://trello.com/power-ups/admin), access your Power-Up, navigate to the "API Key" tab, and select "Generate a new API Key".

### Generating a Token

After obtaining your API Key, generate a `Token` on the same page. Click the hyperlinked "Token" next to the API key.

### Obtaining a Board Id

To get your `Board Id`, go to your Trello board, append ".json" to the board's URL, and navigate to it. In the JSON response, search for `"idBoard"` — this is the ID you need.

# Configuring the Development Environment

Ensure you have the following software installed:

* [Visual Studio Community 2022](https://www.visualstudio.com/vs/community/)
* [Git](https://git-scm.com/downloads)

### Setup

1. Install the required software listed above.
2. Clone the repository to your development machine ([guide](https://help.github.com/desktop/guides/contributing/working-with-your-remote-repository-on-github-or-github-enterprise)).

### Running the WebApp

1. Open the ***'TrelloTenderManager.sln'*** solution in Visual Studio 2022.
2. In the ***'TrelloTenderManager.WebApi\appsettings.Development.json'*** file, set the `Trello: ApiKey`, `Trello: Token`, and `Trello: BoardId` with the values obtained in the "Getting Started" section.
3. Right-click the ***'TrelloTenderManager.WebApi'*** project and select ***'Debug -> Start New Instance'***.
4. Ensure the `WebApi:ApiBaseUrl` setting in ***'TrelloTenderManager.WebApp\appsettings.json'*** is set to the Web API URL.
5. Right-click the ***'TrelloTenderManager.WebApp'*** project and select ***'Debug -> Start New Instance'***.
6. To process a CSV file, navigate to the ***'Process CSV'*** page, choose a file, and click ***'Submit'***. This will immediately process the file and create Trello tickets.
7. To queue a CSV file, navigate to the ***'Queue CSV'*** page, choose a file, and click ***'Submit'***. This queues the file and processes it in the background, which is helpful for large files.
8. To view queued files and their statuses, navigate to the ***'Show Queue'*** page.

### Running the WebApi Integration Tests

1. Open the ***'TrelloTenderManager.sln'*** solution in Visual Studio 2022.
2. In the ***'TrelloTenderManager.WebApi\appsettings.Development.json'*** file, set the `Trello: ApiKey`, `Trello: Token`, and `Trello: BoardId` with the values obtained in the "Getting Started" section.
3. Right-click the ***'TrelloTenderManager.WebApi'*** project and select ***'Debug -> Start Without Debugging'***.
4. Ensure the `WebApi:ApiBaseUrl` setting in ***'TrelloTenderManager.WebApi.IntegrationTests\appsettings.json'*** is set to the Web API URL.
5. Run all tests in the ***'TrelloTenderManager.WebApi.IntegrationTests'*** project.