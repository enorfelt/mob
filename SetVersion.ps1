$filePath = $args[0] # path to the file i.e. 'C:\Users\ben\Code\csproj powershell\MySmallLibrary.csproj'
$newVersion = $args[1] # the build version, from VSTS build i.e. "1.1.20170323.1"

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