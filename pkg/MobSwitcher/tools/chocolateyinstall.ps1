
$ErrorActionPreference = 'Stop';
$toolsDir = "$(Split-Path -parent $MyInvocation.MyCommand.Definition)"
$fileLocation = Join-Path $toolsDir 'MobSwitcher-Installer.msi'

function Show-InstalledToast {
  $ErrorActionPreference = "SilentlyContinue"

  $notificationTitle = "MobSwitcher installed!"

  [Windows.UI.Notifications.ToastNotificationManager, Windows.UI.Notifications, ContentType = WindowsRuntime] > $null
  $template = [Windows.UI.Notifications.ToastNotificationManager]::GetTemplateContent([Windows.UI.Notifications.ToastTemplateType]::ToastText01)

  #Convert to .NET type for XML manipuration
  $toastXml = [xml] $template.GetXml()
  $toastXml.GetElementsByTagName("text").AppendChild($toastXml.CreateTextNode($notificationTitle)) > $null

  #Convert back to WinRT type
  $xml = New-Object Windows.Data.Xml.Dom.XmlDocument
  $xml.LoadXml($toastXml.OuterXml)

  $toast = [Windows.UI.Notifications.ToastNotification]::new($xml)
  $toast.Tag = "mob-timer"
  $toast.Group = "mobswitcher"
  $toast.ExpirationTime = [DateTimeOffset]::Now.AddMinutes(5)
  $toast.SuppressPopup = $true

  $notifier = [Windows.UI.Notifications.ToastNotificationManager]::CreateToastNotifier("{fb4831aa-8a45-4828-ba0e-b840bcfb395a}!MobSwitcher")
  $notifier.Show($toast);
}

$packageArgs = @{
  packageName    = $env:ChocolateyPackageName
  unzipLocation  = $toolsDir
  fileType       = 'MSI'
  file           = $fileLocation
  softwareName   = 'MobSwitcher*'
  checksum       = 'A3ADA51A52B529593B94754BD03BEB732037C25FDF696368B619AA9044F749BE'
  checksumType   = 'sha256'
  silentArgs     = "/qn /norestart /l*v `"$($env:TEMP)\$($packageName).$($env:chocolateyPackageVersion).MsiInstall.log`""
  validExitCodes = @(0, 3010, 1641)
}

Install-ChocolateyInstallPackage @packageArgs
$env:Path = [System.Environment]::GetEnvironmentVariable("Path", "Machine") + ";" + [System.Environment]::GetEnvironmentVariable("Path", "User")
Show-InstalledToast










    








