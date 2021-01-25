<#
Date: 07/02/2020
Purpose: To build Greg inside a windows docker container
#>
$ErrorActionPreference = "Stop"

$errorMessage = "The build was not successful, check for errors"
$dockerImage = "artifactory.dev.adskengineer.net/dynamo/desktop/buildtools/2019:1.1.1"

#Clear Nuget Cache	
&"$env:WORKSPACE\$env:COMMON_TOOLS_DIR\$env:NUGETTOOL" locals all -clear

try {
    # Stop container
    docker stop $env:DOCKER_CONTAINER

    # Remove the previous container so that we start with a fresh container
    docker rm $env:DOCKER_CONTAINER

    # One time pull of the docker image
    docker pull $dockerImage

    # Creating the container
    docker run -m 8GB -d -t --mount type=bind,source=$env:WORKSPACE,target=$env:DOCKER_WORKSPACE --name $env:DOCKER_CONTAINER $dockerImage

    # Restore Nuget packages Greg solution
    docker exec $env:DOCKER_CONTAINER $env:DOCKER_WORKSPACE\$env:COMMON_TOOLS_DIR\$env:NUGETTOOL restore $env:DOCKER_WORKSPACE\src\GregClient.sln
    
    # Build Greg solution
    docker exec $env:DOCKER_CONTAINER msbuild -restore $env:DOCKER_WORKSPACE\src\GregClient.sln

    if($LASTEXITCODE -ne 0) {
        throw $errorMessage
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