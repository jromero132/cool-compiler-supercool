# change to runtime
FROM mcr.microsoft.com/dotnet/core/runtime:2.1 AS base
WORKDIR /app

FROM mcr.microsoft.com/dotnet/core/sdk:2.1 AS build
RUN apt update && apt install -y spim
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
