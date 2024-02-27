[CmdletBinding()]
param (
    [Parameter()]
    [string[]]
    $ReleaseBranches
)

$ErrorActionPreference = "Stop"

$regexBranches = @()
foreach ($branch in $ReleaseBranches -split ",")
{
	$regexBranches +=  "(^" + $branch + "*)"
}
$regex = $regexBranches -join "|"

Write-Host $env:BRANCH_NAME
Write-Host $regex

<#
Regex matching release branches
#>
$reBranch = [regex]$regex

if ($env:BRANCH_NAME -match $reBranch) 
{
	try {
		Set-PSDebug -Trace 2;
		#deploy already built package.
		$assemblyPath = "$env:WORKSPACE\bin\release"
		$nupkgFile = Get-ChildItem $assemblyPath\*.nupkg -Depth 1
		Write-Host $env:NUGET_PUBLISH_SOURCE
		Write-Host $nupkgFile
		Write-Host $env:APIKEY
		dotnet nuget push $nupkgFile --api-key $env:APIKEY --source $env:NUGET_PUBLISH_SOURCE

		if($LASTEXITCODE -ne 0) {
			throw "The package-generation process failed"
		}
	}
	catch {
		Set-PSDebug -Trace 0;
		Write-Host $error[0]
		throw $LASTEXITCODE
	}
}
else
{
	Write-Host("Deployment is skipped because this is not a release branch")
}