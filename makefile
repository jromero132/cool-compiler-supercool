all : clean restore build

clean:
	dotnet clean src/SuperCOOL/SuperCOOL.csproj

restore:
	dotnet restore src/SuperCOOL/SuperCOOL.csproj

build: 
	dotnet build src/SuperCOOL/SuperCOOL.csproj

publish:
	dotnet publish src/SuperCOOL/SuperCOOL.csproj -c Release -o bin/

tests:
	dotnet test tests/SuperCOOL.Tests

run:
	cd src/SuperCOOL; dotnet run 