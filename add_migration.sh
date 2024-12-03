#!/bin/bash

if [ -z "$1" ]; then
    echo "Error: Migration name not provided."
    usage
fi

MIGRATION_NAME=$1

PROJECT_PATH="Infrastructure/Infrastructure.csproj"
STARTUP_PROJECT_PATH="API/API.csproj"

echo "Running: dotnet ef migrations add $MIGRATION_NAME --project $PROJECT_PATH --startup-project $STARTUP_PROJECT_PATH"

dotnet ef migrations add "$MIGRATION_NAME" --project "$PROJECT_PATH" --startup-project "$STARTUP_PROJECT_PATH"

