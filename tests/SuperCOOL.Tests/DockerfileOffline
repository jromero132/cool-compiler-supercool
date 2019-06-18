# change to runtime
FROM microsoft/dotnet:2.1-sdk AS base
WORKDIR /app

FROM microsoft/dotnet:2.1-sdk AS build
WORKDIR /src
COPY tests/SuperCOOL.Tests/SuperCOOL.Tests.csproj tests/SuperCOOL.Tests/
COPY ["packages", "packages/"]
COPY src/SuperCOOL/SuperCOOL.csproj src/SuperCOOL/
RUN dotnet restore tests/SuperCOOL.Tests/SuperCOOL.Tests.csproj -s packages/ -s /usr/share/dotnet/sdk/NuGetFallbackFolder/
COPY . .
WORKDIR /src/tests/SuperCOOL.Tests
RUN dotnet build SuperCOOL.Tests.csproj -c Release -o /app

FROM build AS publish
RUN dotnet publish SuperCOOL.Tests.csproj -c Release -o /app --no-restore

FROM base AS final
WORKDIR /app
COPY --from=publish /app .
ENTRYPOINT ["bash"]
