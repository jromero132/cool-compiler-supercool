all : clean restore build publish

clean:
	dotnet clean SuperCOOL/SuperCOOL.csproj

restore:
	dotnet restore SuperCOOL/SuperCOOL.csproj

build: 
	dotnet build SuperCOOL/SuperCOOL.csproj

publish:
	dotnet publish SuperCOOL/SuperCOOL.csproj -c Release -o bin/

run:
	cd SuperCOOL; dotnet run 