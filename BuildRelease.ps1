function Set-Version($filePath, $newVersion) {
  Write-Host "Starting process of generating new version number for the $($filePath)"

  $splitNumber = $newVersion.Split(".")
  if ( $splitNumber.Count -eq 3 ) {
    $majorNumber = $splitNumber[0]
    $minorNumber = $splitNumber[1]
    $revisionNumber = $splitNumber[2]
	
    # I need to keep my build number under the 65K int limit, hence this hack of a method
    #$myBuildNumber = (Get-Date).Year + ((Get-Date).Month * 31) + (Get-Date).Day
    $myBuildNumber = $majorNumber + "." + $minorNumber + "." + $revisionNumber

    if ($filePath -match "\.csproj$") {
      $xml = New-Object XML
      $xml.Load($filePath)
      $versionNode = $xml.Project.PropertyGroup.Version
      if ($null -eq $versionNode) {
        # If you have a new project and have not changed the version number the Version tag may not exist
        $versionNode = $xml.CreateElement("Version")
        $xml.Project.PropertyGroup.AppendChild($versionNode)
        Write-Host "Version XML tag added to the csproj"
      }
      $xml.Project.PropertyGroup.Version = $myBuildNumber
      $xml.Save($filePath)
    }
    elseif ($filePath -match "\.nuspec") {
      $xml = New-Object XML
      $xml.Load($filePath)
      $versionNode = $xml.package.metadata.version
      if ($null -eq $versionNode) {
        # If you have a new project and have not changed the version number the Version tag may not exist
        $versionNode = $xml.CreateElement("version")
        $xml.package.metadata.AppendChild($versionNode)
        Write-Host "Version XML tag added to the csproj"
      }
      $xml.package.metadata.version = $myBuildNumber
      $xml.Save($filePath)
    }
    elseif ($filePath -match "\.wxs") {
      $xml = New-Object XML
      $xml.Load($filePath)
      $xml.Wix.Product.SetAttribute("Version", $myBuildNumber)
      $xml.Save($filePath)
    }
    elseif ($filePath -match "\.txt") {
      ((Get-Content -path $filePath -Raw) -replace "/\d.\d.\d/", "/$($myBuildNumber)/") | Set-Content -Path $filePath
    }
    Write-Host "Updated "$filePath" and set to version "$myBuildNumber
  }
  else {
    Write-Host "ERROR: Something was wrong with the build number"
  }
}

function Set-FileChecksum() {
  $newChecksum = Get-Filehash -Path ".\pkg\MobSwitcher\tools\MobSwitcher-Installer.msi"

  if ($null -ne $newChecksum) {
    $installScriptPath = ".\pkg\MobSwitcher\tools\chocolateyinstall.ps1"
    ((Get-Content -path $installScriptPath -Raw) -replace "checksum       = '\w{64}'", "checksum       = '$($newChecksum.Hash)'") | Set-Content -Path $installScriptPath
    Write-Host "Updated checksum $($newChecksum.Hash) in $($installScriptPath)"

    $verificationFilePath = ".\pkg\MobSwitcher\tools\VERIFICATION.txt"
    ((Get-Content -path $verificationFilePath -Raw) -replace "checksum: \w{64}", "checksum: $($newChecksum.Hash)") | Set-Content -Path $verificationFilePath
    Write-Host "Updated checksum $($newChecksum.Hash) in $($verificationFilePath)"
  }
}

$version = $args[0]

if ($null -ne $version -and $version -ne "") {
  Write-Host "set version $($version)" -ForegroundColor Cyan
  $currentLocation = Get-Location
  Set-Version "$($currentLocation)/src/MobSwitcher.Cli/MobSwitcher.Cli.csproj" $version
  Set-Version "$($currentLocation)/src/MobSwitcher.Core/MobSwitcher.Core.csproj" $version
  Set-Version "$($currentLocation)/src/MobSwitcher.Windows/MobSwitcher.Windows.csproj" $version
  Set-Version "$($currentLocation)/src/MobSwitcher.Installer/MobSwitcher-Installer.wxs" $version
  Set-Version "$($currentLocation)/pkg/MobSwitcher/mobswitcher.nuspec" $version
  Set-Version "$($currentLocation)/pkg/MobSwitcher/tools/VERIFICATION.txt" $version
}
Write-Host "`ndotnet restore" -ForegroundColor Cyan
dotnet restore
Write-Host "`ndotnet build" -ForegroundColor Cyan
dotnet build --no-restore
Write-Host "`ndotnet test" -ForegroundColor Cyan
dotnet test --no-restore
Write-Host "`ndotnet publish" -ForegroundColor Cyan
dotnet publish --no-restore -c Release -o .\src\MobSwitcher.Installer .\src\MobSwitcher.Cli\MobSwitcher.Cli.csproj
Write-Host "`nwix build" -ForegroundColor Cyan
candle.exe .\src\MobSwitcher.Installer\MobSwitcher-Installer.wxs -o .\src\MobSwitcher.Installer\MobSwitcher-Installer.wixobj
Write-Host "`nwix publish" -ForegroundColor Cyan
light.exe -ext WixUIExtension .\src\MobSwitcher.Installer\MobSwitcher-Installer.wixobj -o .\pkg\MobSwitcher\tools\MobSwitcher-Installer.msi
Write-Host "`nwix remove pdb" -ForegroundColor Cyan
Remove-Item -Path ".\pkg\MobSwitcher\tools\MobSwitcher-Installer.wixpdb"
Write-Host "`nset file checksum" -ForegroundColor Cyan
Set-FileChecksum
Write-Host "`nchoco pack" -ForegroundColor Cyan
choco pack .\pkg\MobSwitcher\mobswitcher.nuspec --out .\pkg\MobSwitcher