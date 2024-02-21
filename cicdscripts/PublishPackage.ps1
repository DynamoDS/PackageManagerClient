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

		#deploy already built package.
		$assemblyPath = "src\GregClient\bin\release"
		$nupkgFile = Get-ChildItem $assemblyPath\*.nupkg -Depth 1
		dotnet nuget push $nupkgFile --api-key $env:API_KEY --source $env:NUGET_PUBLISH_SOURCE

		if($LASTEXITCODE -ne 0) {
			throw "The package-generation process failed"
		}
	}
	catch {
		Write-Host $error[0]
		throw $LASTEXITCODE
	}
}
else
{
	Write-Host("Deployment is skipped because this is not a release branch")
}