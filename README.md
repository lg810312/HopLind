# HopLind
OpenAI SDK an WebAPI for OpenAI and Azure OpenAI Service

The SDK is developed using .net and supports both OpenAI and Azure OpenAI Service.

The WebAPI and Blazor are both based on the SDK and call the OpenAI API at the backend depending on the API configuration in appsettings.json.

***API configuration example:***
```json
  "OpenAI": {
    "APIType": "openai",
    "APIBase": "https://api.openai.com/v1",
    "APIKey": "sk-qwertyuiopasdfghjklzxcvbnm",
    "APIVersion": ""
  }
```

The SDK, WebAPI, and Blazor projects are currently under development and in alpha testing. As a result, there may be breaking changes.
