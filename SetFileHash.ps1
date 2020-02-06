$newChecksum = Get-Filehash -Path ".\pkg\MobSwitcher\tools\MobSwitcher-Installer.msi"

if ($null -ne $newChecksum) {
  $installScriptPath = ".\pkg\MobSwitcher\tools\chocolateyinstall.ps1"
  ((Get-Content -path $installScriptPath -Raw) -replace "checksum       = '\w{64}'","checksum       = '$($newChecksum.Hash)'") | Set-Content -Path $installScriptPath
  Write-Host "Updated checksum $($newChecksum.Hash) in $($installScriptPath)"

  $verificationFilePath = ".\pkg\MobSwitcher\tools\VERIFICATION.txt"
  ((Get-Content -path $verificationFilePath -Raw) -replace "checksum: \w{64}","checksum: $($newChecksum.Hash)") | Set-Content -Path $verificationFilePath
  Write-Host "Updated checksum $($newChecksum.Hash) in $($verificationFilePath)"
}