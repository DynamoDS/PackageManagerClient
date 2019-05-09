param(
[string]$nuget= $(throw "-nuget is required."),
[string]$out=$(throw "-out is required")
)

if(!(Test-Path -Path $nuget\lib\net47\)){
    New-Item -ItemType directory -Path $nuget\lib\net47\
}
if(!(Test-Path -Path $nuget\lib\net45\)){
    New-Item -ItemType directory -Path $nuget\lib\net45\
}


if(!(Test-Path -Path $nuget\content)){
    New-Item -ItemType directory -Path $nuget\content
}

if(!(Test-Path -Path $nuget\content\controllers)){
    New-Item -ItemType directory -Path $nuget\content\controllers
}

if(!(Test-Path -Path $nuget\tools)){
    New-Item -ItemType directory -Path $nuget\tools
}

copy $out\Greg_net45.dll $nuget\lib\net45\
Remove-Item -Path $nuget\lib\net45\Greg.dll -ErrorAction Ignore
Rename-Item -path $nuget\lib\net45\Greg_net45.dll -NewName Greg.dll

copy $out\Greg.dll $nuget\lib\net47\

$dllVersion = [System.Diagnostics.FileVersionInfo]::GetVersionInfo("$out\Greg.dll").FileVersion

nuget pack -version $dllVersion -OutputDirectory $nuget $nuget\GregClient.nuspec 