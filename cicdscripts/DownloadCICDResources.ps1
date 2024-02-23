<#
   Purpose: Pull of the source from git
#>
$ErrorActionPreference = "Stop"

try {

    #Redirecting GIT output
    $env:GIT_REDIRECT_STDERR = '2>&1'

    if ([string]::IsNullOrWhiteSpace($env:URL_REPO_TOOLS) -or [string]::IsNullOrWhiteSpace($env:BRANCH_TOOLS)) 
    {
        throw "The tool repository parameters had not been set properly"
    }
    else 
    {
        $JUser = "$env:GITADSK_USERNAME"
        $JPassword = [System.Web.HttpUtility]::UrlEncode("$env:GITADSK_PASSWORD")

        $GitUrl = "https://" + "$JUser" + ":" + "$JPassword" + "@$env:URL_REPO_TOOLS"

        git clone -b $env:BRANCH_TOOLS "$GitUrl" "$env:WORKSPACE\$env:COMMON_TOOLS_DIR"

        if($LASTEXITCODE -ne 0)
        {
            throw "The download of the tools failed"
        }
    }
    
}
catch {
    Write-Host $error[0]
	throw $LASTEXITCODE    
}