FROM microsoft/dotnet:2.1-sdk AS build-env
WORKDIR /app

COPY Mocabulary.sln ./
COPY EnglishHubRepository/*.csproj ./EnglishHubRepository/
COPY EnglishHubApi/*.csproj ./EnglishHubApi/

RUN dotnet restore

# Copy everything else and build
COPY . ./
RUN dotnet publish -c Release -o out

# Build runtime image
RUN dotnet publish -c Release -o out

ENTRYPOINT ["dotnet", "out/EnglishHubApi.dll"]