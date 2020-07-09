<#
   Date: 07/04/2019
   Purpose: Post Build Script of Dynamo
#>
$ErrorActionPreference = "Stop"

try {
	docker container stop $env:DOCKER_CONTAINER
	docker container rm $env:DOCKER_CONTAINER
}
catch {	
	Invoke-Expression -Command "$env:WORKSPACE\cicdscripts\RestartDockerDesktop.ps1"
	Write-Host $error[0]
	throw $LASTEXITCODE
}
finally {
	docker container prune -f
}
