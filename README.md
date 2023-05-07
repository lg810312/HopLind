# HopLind
OpenAI SDK an WebAPI for OpenAI and Azure OpenAI Service

The SDK has been created using .NET and is capable of supporting both OpenAI and Azure OpenAI Service. At this time, the SDK is specifically targeting GPT and includes wrapped APIs for embedding and completion. As GPT-3.5 is more powerful and easier to use than older models for completion API, only GPT-3.5 is currently supported. However, GPT-4 is planned to be supported in the future roadmap. These APIs have been designed to allow for effortless switching between the two SaaS providers without any need for coding modifications.

The WebAPI and Blazor Server projects are both based on the SDK and call the OpenAI API at the backend depending on the API configuration in appsettings.json.

***API configuration example:***

**OpenAI**
```json
  "OpenAI": {
    "APIType": "openai",
    "APIBase": "https://api.openai.com/v1",
    "APIKey": "sk-qwertyuiopasdfghjklzxcvbnm",
    "APIVersion": ""
  }
```

**Azure**
```json
  "OpenAI": {
    "APIType": "azure",
    "APIBase": "https://YOUR-SERVICE.openai.azure.com/",
    "APIKey": "YOUR API KEY",
    "APIVersion": "2023-03-15-preview"
  }
```
Azure OpenAI API does not support specifying model and must deploy model before use, to seamless switch between OpenAI and Azure, the deployment name should be as same as its model name.

You may deploy either WebAPI or Blazor Server depending on your requirements. You can also use them as templates or utilize the SDK to build applications from scratch.

**Note:** The SDK, WebAPI, and Blazor projects are currently under development and in alpha testing. As a result, there may be breaking changes.
