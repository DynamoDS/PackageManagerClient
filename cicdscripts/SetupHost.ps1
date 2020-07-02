<#
   Purpose: Setup with the applications needed for the CICD pipeline
#>
$ErrorActionPreference = "Stop"

#Getting directory names to install the tools
$nugetSubDir = ($env:NUGETTOOL -split "\\")[0]

$nugetDir = "$env:WORKSPACE\$env:COMMON_TOOLS_DIR\$nugetSubDir"

#Creation of the tools directories
New-Item -Path $nugetDir  -ItemType Directory

#Download of latest version of Nuget
Invoke-WebRequest -Uri "https://dist.nuget.org/win-x86-commandline/latest/nuget.exe" -OutFile "$nugetDir\nuget.exe"