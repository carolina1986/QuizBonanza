# This file works as a blueprint for the container that will run the application.
# It is a text file that contains all the commands a user could call on the command line to assemble an image.
# INSTRUCTIONS -what you want to do   
# FROM - Uses a ready image as a base
# WORKDIR - Sets the project directory
# COPY - Copies files from the host to the container
# RUN - Executes a command
# ENTRYPOINT - Sets the default command to run when the container starts   

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /app

COPY *.csproj ./
RUN dotnet restore
 
COPY . ./
RUN dotnet publish -c Release -o /app/publish
 
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS runtime
WORKDIR /app
COPY --from=build /app/publish .
 
ENTRYPOINT ["dotnet", "QuizApp.dll"]
