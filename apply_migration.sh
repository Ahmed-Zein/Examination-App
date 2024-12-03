#!/bin/bash


PROJECT_PATH="Infrastructure/Infrastructure.csproj"
STARTUP_PROJECT_PATH="API/API.csproj"

echo "Running: dotnet ef database update --project "$PROJECT_PATH" --startup-project "$STARTUP_PROJECT_PATH""

dotnet ef database update --project "$PROJECT_PATH" --startup-project "$STARTUP_PROJECT_PATH"
