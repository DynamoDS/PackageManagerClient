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

$assemblyPath = "$Workspace\bin\Release"

try {

	$nupkgFile = Get-ChildItem $assemblyPath\*.nupkg -Depth 1

	& "$NugetPath" push $nupkgFile.FullName -ApiKey $ApiKey -Source nuget.org
}
catch {	
	Write-Host $error[0]
	throw $LASTEXITCODE
}