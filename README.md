# Detect Similar IT Support Tickets Before Submission

Our project is an IT support bot that provides a cost-effective alternative to the traditional IT ticketing system. It is designed to handle common IT support requests, such as password resets, software installations, and troubleshooting, without the need for human intervention. The AI model will categorize, prioritize, and automatically create an IT ticket, reducing the manual steps in the process.

Technical Requirements
Store ticket information in the existing SQLite database.
Use SQLite Full-Text Search (FTS5) to quickly locate related ticket descriptions.
Compare the new ticket description against existing tickets.
Rank results based on relevance.
Integrate the search functionality with the existing EF Core application.
Return the most relevant matching tickets.
Business Value
Reduces duplicate IT requests.
Helps IT staff avoid solving the same issue multiple times.
Allows users to find existing solutions faster.
Improves organization and reporting of recurring technical problems.

## Projects

- `CheaperThanAi.sln` — Visual Studio solution
- `src/CheaperThanAi.Server` — ASP.NET Core host and API
- `src/CheaperThanAi.Client` — Blazor WebAssembly UI
- `src/CheaperThanAi.Shared` — Shared request/response models

## Run

```bash
dotnet run --project src/CheaperThanAi.Server
```

Then open the launched browser URL (HTTPS on `https://localhost:7170` by default).

Submit a request from the home page. The stub API responds with:

> We're still working on this functionality.

## AI Models

If you want to run the AI model locally, you can use the Ollama Chat Client. The model expects you to have Ollama installed and running. You can download it from [Ollama's official website](https://ollama.com/).

The default model is llama3.1. If you would like to pull and run a different model, you just need to update the appsettings.json file.

To get llama3.1, you need to have Ollama installed. Then run the following command in your terminal:
```bash
ollama pull llama3.1
```

This should take a while to download. Once it is done, you can cofirm it is installed by running:
```bash
ollama list
```
