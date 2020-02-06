ECHO off
SET version=%1

IF /I "%version%" NEQ "" (
  Powershell.exe -executionpolicy remotesigned -File SetVersion.ps1 "./src/MobSwitcher.Cli/MobSwitcher.Cli.csproj" %version%
  Powershell.exe -executionpolicy remotesigned -File SetVersion.ps1 "./src/MobSwitcher.Core/MobSwitcher.Core.csproj" %version%
  Powershell.exe -executionpolicy remotesigned -File SetVersion.ps1 "./src/MobSwitcher.Windows/MobSwitcher.Windows.csproj" %version%
  Powershell.exe -executionpolicy remotesigned -File SetVersion.ps1 "./src/MobSwitcher.Installer/MobSwitcher-Installer.wxs" %version%
  Powershell.exe -executionpolicy remotesigned -File SetVersion.ps1 "./pkg/MobSwitcher/mobswitcher.nuspec" %version%
)
dotnet restore
dotnet build --no-restore
dotnet test --no-restore
dotnet publish --no-restore -c Release -o .\src\MobSwitcher.Installer .\src\MobSwitcher.Cli\MobSwitcher.Cli.csproj
candle.exe .\src\MobSwitcher.Installer\MobSwitcher-Installer.wxs -o .\src\MobSwitcher.Installer\MobSwitcher-Installer.wixobj
light.exe -ext WixUIExtension .\src\MobSwitcher.Installer\MobSwitcher-Installer.wixobj -o .\pkg\MobSwitcher\tools\MobSwitcher-Installer.msi
REM rm .\pkg\MobSwitcher\MobSwitcher-Installer.wixpdb
checksum.exe .\pkg\MobSwitcher\tools\MobSwitcher-Installer.msi -t=sha256