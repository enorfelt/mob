dotnet restore
dotnet build --no-restore
dotnet publish --no-restore -c Release -o .\pkg\MobSwitcher\tools .\src\MobSwitcher.Cli\MobSwitcher.Cli.csproj