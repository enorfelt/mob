dotnet restore
dotnet build --no-restore
dotnet test --no-restore
dotnet publish --no-restore -c Release -o .\pkg\MobSwitcher\tools .\src\MobSwitcher.Cli\MobSwitcher.Cli.csproj
checksum.exe .\pkg\MobSwitcher\tools\mob.exe -t=sha256