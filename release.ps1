git submodule update --init --recursive

$releasePropertiesPath = "./release.xml"

$releaseProperties = [xml](Get-Content $releasePropertiesPath)

$previousVersion = $releaseProperties.previousVersion
$previousVersionValues = $previousVersion.Split('.')
$previousVersionMajor = $previousVersionValues[0]
$previousVersionMinor = $previousVersionValues[1]
$previousVersionPatch = $previousVersionValues[2]

Write-Host "Previous version: $($previousVersion)"

$clientPath = (Resolve-Path .\).Path

cd test\specs\client

$currentSpecsVerion = git describe --tags --abbrev=0

$currentSpecsVerionValues = $currentSpecsVerion.Split('.')
$currentSpecsVerionMajor = $currentSpecsVerionValues[0].Trim("v")
$currentSpecsVerionMinor = $currentSpecsVerionValues[1]

Write-Host "Current specs verion: $($currentSpecsVerion)"

cd $clientPath

$newVersionPatch = ""
if ($previousVersionMajor -eq $currentSpecsVerionMajor -and $previousVersionMinor -eq $currentSpecsVerionMinor)
{
	$newVersionPatch = [string]([convert]::ToInt32($previousVersionPatch, 10) + 1)
}
else
{
	$newVersionPatch = "0"
}

$newVersion = "$($currentSpecsVerionMajor).$($currentSpecsVerionMinor).$($newVersionPatch)"

$answer = Read-Host "Going to release version $($newVersion). Proceed? [y/n]"
if ($answer -eq "y")	
{
	$releaseProperties.previousVersion = $newVersion;
	$releaseProperties.Save((Resolve-Path $releasePropertiesPath).Path)
	
	git stage $releasePropertiesPath
	git commit -m"Update release version to $($newVersion)"
	git tag -a "v$($newVersion)" -m "Release $($newVersion)"
	git push origin "v$($newVersion)"
	
	Write-Host "Pushed tag to Git origin. It will now trigger the deployment pipeline."
}
