# change to runtime
FROM mcr.microsoft.com/dotnet/core/runtime:2.1 AS base
WORKDIR /app

FROM mcr.microsoft.com/dotnet/core/sdk:2.1 AS build
WORKDIR /src
COPY src/SuperCOOL/SuperCOOL.csproj src/SuperCOOL/
RUN dotnet restore src/SuperCOOL/SuperCOOL.csproj
COPY . .
WORKDIR /src/src/SuperCOOL
RUN dotnet build SuperCOOL.csproj -c Release -o /app

FROM build AS publish
RUN dotnet publish SuperCOOL.csproj -c Release -o /app --no-restore

FROM base AS final
WORKDIR /app
COPY --from=publish /app .
ENTRYPOINT ["bash"]
