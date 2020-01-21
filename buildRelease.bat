dotnet restore
dotnet build --no-restore
dotnet test --no-restore
dotnet publish --no-restore -c Release -o .\src\MobSwitcher.Installer .\src\MobSwitcher.Cli\MobSwitcher.Cli.csproj
checksum.exe .\src\MobSwitcher.Installer\mob.exe -t=sha256