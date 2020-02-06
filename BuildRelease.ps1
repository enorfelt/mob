$version = $args[0]

$currentLocation = Get-Location

if ($version -ne "") {
  .\SetVersion.ps1 "$($currentLocation)/src/MobSwitcher.Cli/MobSwitcher.Cli.csproj" $version
  .\SetVersion.ps1 "$($currentLocation)/src/MobSwitcher.Core/MobSwitcher.Core.csproj" $version
  .\SetVersion.ps1 "$($currentLocation)/src/MobSwitcher.Windows/MobSwitcher.Windows.csproj" $version
  .\SetVersion.ps1 "$($currentLocation)/src/MobSwitcher.Installer/MobSwitcher-Installer.wxs" $version
  .\SetVersion.ps1 "$($currentLocation)/pkg/MobSwitcher/mobswitcher.nuspec" $version
  .\SetVersion.ps1 "$($currentLocation)/pkg/MobSwitcher/tools/VERIFICATION.txt" $version
}
Invoke-Expression -Command "dotnet restore"
Invoke-Expression -Command "dotnet build --no-restore"
Invoke-Expression -Command "dotnet test --no-restore"
Invoke-Expression -Command "dotnet publish --no-restore -c Release -o .\src\MobSwitcher.Installer .\src\MobSwitcher.Cli\MobSwitcher.Cli.csproj"
Invoke-Expression -Command "candle.exe .\src\MobSwitcher.Installer\MobSwitcher-Installer.wxs -o .\src\MobSwitcher.Installer\MobSwitcher-Installer.wixobj"
Invoke-Expression -Command "light.exe -ext WixUIExtension .\src\MobSwitcher.Installer\MobSwitcher-Installer.wixobj -o .\pkg\MobSwitcher\tools\MobSwitcher-Installer.msi"
Remove-Item -Path ".\pkg\MobSwitcher\tools\MobSwitcher-Installer.wixpdb"
.\SetFileHash.ps1
Invoke-Expression -Command "choco pack .\pkg\MobSwitcher\mobswitcher.nuspec --out .\pkg\MobSwitcher"