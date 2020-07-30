<#
   Purpose: Setup with the applications needed for the CICD pipeline
#>
$ErrorActionPreference = "Stop"

#Getting directory names to install the tools
$nugetSubDir = ($env:NUGETTOOL -split "\\")[0]
$elementsPath = ($env:WORKSPACE -split "\\")

$toolsDir = $elementsPath[0] + '\' + $elementsPath[1] + '\tools'
$nugetDir = "$env:WORKSPACE\$env:COMMON_TOOLS_DIR\$nugetSubDir"

#Creation of the tools directories
New-Item -Path $nugetDir  -ItemType Directory

#Download of latest version of Nuget
Invoke-WebRequest -Uri "https://dist.nuget.org/win-x86-commandline/latest/nuget.exe" -OutFile "$nugetDir\nuget.exe"

try {
   #Instal Python from nuget
   &"$env:WORKSPACE\$env:COMMON_TOOLS_DIR\$env:NUGETTOOL" install python -source nuget.org -ExcludeVersion -OutputDirectory $toolsDir

   if($LASTEXITCODE -ne 0) {
      throw "Install of Python has failed"
   }
}
catch {
   Write-Host $error[0]
   throw $LASTEXITCODE   
}