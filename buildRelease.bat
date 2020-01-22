dotnet restore
dotnet build --no-restore
dotnet test --no-restore
dotnet publish --no-restore -c Release -o .\src\MobSwitcher.Installer .\src\MobSwitcher.Cli\MobSwitcher.Cli.csproj
candle.exe .\src\MobSwitcher.Installer\MobSwitcher-Installer.wxs -o .\src\MobSwitcher.Installer\MobSwitcher-Installer.wixobj
light.exe -ext WixUIExtension .\src\MobSwitcher.Installer\MobSwitcher-Installer.wixobj -o .\pkg\MobSwitcher\tools\MobSwitcher-Installer.msi
checksum.exe .\pkg\MobSwitcher\tools\MobSwitcher-Installer.msi -t=sha256