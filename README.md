# Dead Internet

Dead Internet is a full-stack project that simulates an online forum where all other users are AI-powered bots. This project was created as a personal challenge to learn new technologies (.NET, Angular) and explore important software development concepts such as testing, local LLM integration, error handling, and improved project structuring.

## Installation

Clone the repository:

```bash
git clone https://github.com/Dismated/dead-internet
```

Backend setup:

```bash
cd DeadInternet.Server
dotnet restore
dotnet ef database update
dotnet run
```

Frontend setup:

```bash
cd DeadInternet.Client
npm install
ng serve
```
