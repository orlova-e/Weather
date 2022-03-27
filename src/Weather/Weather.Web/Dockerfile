FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build-env
WORKDIR /app

COPY *.sln .
COPY Domain/*.csproj ./Domain/
COPY Infrastructure/*.csproj ./Infrastructure/
COPY Services/*.csproj ./Services/
COPY Web/*.csproj ./Web/

RUN dotnet restore

COPY Domain/. ./Domain/
COPY Infrastructure/. ./Infrastructure/
COPY Services/. ./Services/
COPY Web/. ./Web/

WORKDIR /app/Web
RUN dotnet publish -c Release -o out

FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS runtime
WORKDIR /app

COPY --from=build-env /app/Web/out ./
ENTRYPOINT [ "dotnet", "Web.dll" ]
