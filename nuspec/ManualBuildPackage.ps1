<#
Date: 07/02/2020
Purpose: To create the Greg nuget package
#>
[CmdletBinding()]
param (
    [Parameter(Mandatory)]
    [string]
    $RepositoryPath,
    [Parameter(Mandatory)]
    [string]
    $NugetPath,
    [Parameter(Mandatory)]
    [string]
    $OutputPackage,
    [string]
    $Configuration = 'Debug'
)
$ErrorActionPreference = "Stop"

$assemblyPath = "$RepositoryPath\bin\$Configuration"
$nuspecPath = "$RepositoryPath\nuspec"

$dllVersion = [System.Diagnostics.FileVersionInfo]::GetVersionInfo("$assemblyPath\Greg.dll").FileVersion

try {

	& "$NugetPath" pack -basePath $assemblyPath -version $dllVersion -OutputDirectory $OutputPackage $nuspecPath\GregClient.nuspec
}
catch {	
	Write-Host $error[0]
	throw $LASTEXITCODE
}