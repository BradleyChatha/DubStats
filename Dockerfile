# Build Frontend
FROM node:alpine AS node-env

COPY . /app/
WORKDIR /app/frontend
RUN npm install
RUN npm run build

# Build Backend
FROM mcr.microsoft.com/dotnet/core/sdk:3.0 AS build-env

WORKDIR /app/backend
COPY --from=node-env /app/backend/wwwroot/client /app/backend/wwwroot/client
RUN dotnet restore
RUN dotnet publish -c Release -o out

# Build runtime image
FROM mcr.microsoft.com/dotnet/core/aspnet:3.1.2-alpine3.11
WORKDIR /app/
COPY --from=build-env /app/out .
ENTRYPOINT ["dotnet", "backend.dll"]
