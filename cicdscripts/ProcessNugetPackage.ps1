<#
Date: 07/02/2020
Purpose: To create the Greg nuget package
#>
$ErrorActionPreference = "Stop"

$assemblyPath = "$env:WORKSPACE\bin\Debug"
$nuspecPath = "$env:WORKSPACE\nuspec"

$dllVersion = [System.Diagnostics.FileVersionInfo]::GetVersionInfo("$assemblyPath\Greg.dll").FileVersion

try {

	& "$env:WORKSPACE\$env:COMMON_TOOLS_DIR\$env:NUGETTOOL" pack -basePath $assemblyPath -version $dllVersion -OutputDirectory $nuspecPath $nuspecPath\GregClient.nuspec

	$nupkgFile = Get-ChildItem $nuspecPath\*.nupkg -Depth 1

	& "$env:WORKSPACE\$env:COMMON_TOOLS_DIR\$env:NUGETTOOL" push $nupkgFile.FullName -ApiKey $env:APIKEY -Source nuget.org
}
catch {	
	Write-Host $error[0]
	throw $LASTEXITCODE
}