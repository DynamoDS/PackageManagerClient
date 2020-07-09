<#
Date: 07/02/2020
Purpose: To create and publish the Greg nuget package
#>
[CmdletBinding()]
param (
    [Parameter(Mandatory)]
    [string]
    $Workspace,
    [Parameter(Mandatory)]
    [string]
	$NugetPath,
	[Parameter(Mandatory)]
    [string]
    $ApiKey
)

$ErrorActionPreference = "Stop"

$assemblyPath = "$Workspace\bin\Debug"
$nuspecPath = "$Workspace\nuspec"

$dllVersion = [System.Diagnostics.FileVersionInfo]::GetVersionInfo("$assemblyPath\Greg.dll").FileVersion

try {

	& "$NugetPath" pack -basePath $assemblyPath -version $dllVersion -OutputDirectory $nuspecPath $nuspecPath\GregClient.nuspec

	$nupkgFile = Get-ChildItem $nuspecPath\*.nupkg -Depth 1

	& "$NugetPath" push $nupkgFile.FullName -ApiKey $ApiKey -Source nuget.org
}
catch {	
	Write-Host $error[0]
	throw $LASTEXITCODE
}