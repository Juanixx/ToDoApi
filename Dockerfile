# Use the official ASP.NET Core runtime as a base image
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

# Use the official .NET SDK image for building the project
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src
COPY . .
RUN dotnet restore "ToDoApi.csproj"
RUN dotnet build "ToDoApi.csproj" -c Release -o /app/build

# Publish the app to the /app/publish folder
FROM build AS publish
RUN dotnet publish "ToDoApi.csproj" -c Release -o /app/publish

# Create the final image based on the official ASP.NET Core runtime image
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "ToDoApi.dll"]
