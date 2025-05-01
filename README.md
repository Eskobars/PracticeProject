# Vue + ASP.NET Core Football.api & JSONPlaceholder project

This project is a full-stack application using Vite (Vue 3) for the frontend and ASP.NET Core Web API for the backend. It consumes an external Soccer API and the JSONPlaceholder posts API.

## Tech Stack

- Frontend Vue 3 (Vite)
- Backend ASP.NET Core 7 Web API
- HTTP Clients `HttpClientFactory` via `AddHttpClient`
- Caching In-memory cache (`IMemoryCache`)
- API Documentation Swagger (OpenAPI)
- Debugging Chrome debugger (VS Code) and Visual Studio launch support


## Prerequisites

- [.NET 7 SDK](https://dotnet.microsoft.com/en-us/download/dotnet/7.0)
- [Node.js + npm](https://nodejs.org/)
- Vite installed locally via `npm install`
- [Visual Studio 2022+](https://visualstudio.microsoft.com/) with ASP.NET & Web Development workload
- OR use [VS Code](https://code.visualstudio.com/) + Chrome for debugging

## Notes
    - The external Soccer API may be rate-limited — a 3s delay is added between requests to different leagues.
    - Trying to update a self-created post fails due to JSONPlaceholder limitations. 
    - May require a page refresh upon starting.

## Running the App

### 1. Backend (ASP.NET Core)

- Open the solution in Visual Studio (`VueApp1.sln`).
- Build and run the backend project (`VueApp1.Server`).
- Alternatively, you can run the backend manually from the command line:

  ```bash
  dotnet run --project VueApp1.Server
  
Swagger UI is available at:
https://localhost:7033/swagger/index.html


### 2. Frontend (Vue with Vite)

In the `VueApp1.Client` directory:

npm install

npm run dev

Frontend will run at:

https://localhost:5173

## Debugging in Visual Studio

    Set VueApp1.Server as the startup project.

    Press F5 to launch with debugger.

    The frontend (Vite) should start but you can run it separately — start it via npm run dev.

    You may need to refresh the page at first start.


## API Key Configuration

    You can find an api key by inserting the one I have sent via e-mail or by logging into https://dashboard.api-football.com/login/.

    Open appsettings.json in VueApp1.Server.

    Insert your key into this part of the code.
```bash
{
  "ApiSettings": {
    "ApiKey": "your_api_key_here"
  }
}
```


## Project Structure

```bash
VueApp1
├── VueApp1.Client         # Vite-powered Vue frontend
├── VueApp1.Server         # ASP.NET Core Web API
│   ├── Controllers
│   ├── Services
│   ├── Models
│   ├── Interfaces
│   └── appsettings*.json  # API Key
```

## API Features

    Fetch and filter today's soccer fixtures (/api/soccer/today)

    CRUD for Posts (/api/posts)

  Soccer API
  
      Fetch today's soccer fixtures:
      GET /api/soccer/today
  
  Posts API (CRUD)
  
      Get all posts:
      GET /api/posts
  
      Get post by ID:
      GET /api/posts/{id}
  
      Create a new post:
      POST /api/posts
  
      Update a post:
      PUT /api/posts/{id}
  
      Patch a post (partially update):
      PATCH /api/posts/{id}
  
      Delete a post:
      DELETE /api/posts/{id}



