<#
Date: 07/02/2020
Purpose: To build Greg inside a windows docker container
#>
$ErrorActionPreference = "Stop"

try {
    docker exec $env:DOCKER_CONTAINER powershell -command "$env:DOCKER_WORKSPACE\cicdscripts\ProcessNugetPackage.ps1" -Workspace $env:DOCKER_WORKSPACE -NugetPath $env:DOCKER_WORKSPACE\$env:COMMON_TOOLS_DIR\$env:NUGETTOOL -ApiKey $env:APIKEY

    if($LASTEXITCODE -ne 0) {
		throw "Package/Publish of the nuget package failed"
	}
}
catch {

    if($error[0].Exception.Message -eq $errorMessage){
        Invoke-Expression -Command "$env:WORKSPACE\cicdscripts\PostDeploy.ps1"
    }
    else {
        Invoke-Expression -Command "$env:WORKSPACE\cicdscripts\RestartDockerDesktop.ps1"
    }

    Write-Host $error[0]
	throw $LASTEXITCODE
}
