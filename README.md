![PackageManagerClient-VS2022Build](https://github.com/DynamoDS/PackageManagerClient/workflows/PackageManagerClient-VS2022Build/badge.svg)
# Dynamo Package Manager Client

This repo is the package manager client for the Dynamo Package Manager.  Package Manager Client is designed to cover all of the capabilities of the Dynamo Package Manager.  

#### The Package Manager Client can do the following for you:

* properly format, compress, and upload a package to the server
* properly format, compress, and upload a new package version to the server
* download a specific package header
* download a specific package body
* determine server connectivity
* upvote or downvote a package, collect user votes
* search across packages
* deprecate a package
* manage maintainers of a package
* manage authentication

#### The Package Manager Client doesn't do this for you:

* magically find all of the dependencies your package has (that's up to you)
* provide Autodesk 360 login credentials

#### Steps to build The Package Manager Client solution:

- From Visual Studio IDE:

    Build `The PackageManagerClient\src\GregClient.sln` solution

- From command line:

    ```bat
    nuget restore src\GregClient.sln
    msbuild src\GregClient.sln
    ```

#### Nuget
The Package Manager Client is available from NuGet [here](https://www.nuget.org/packages/Greg/). 

#### Manual Build of the NugetPackage
- Make sure you have Nuget version 4.9.0 or later.
>**Important Note:** Please avoid to publish a locally built package

##### Steps:

1. Build the solution
2. On the repository directory, execute the following PowerShell script with the following parameters
   ```bat
   pwsh -ExecutionPolicy ByPass -File ".\nuspec\ManualBuildPackage.ps1 [REPOSITORY_PATH] [NUGET_EXE_PATH] [OUTPUT_PATH]"
   ```
   - **REPOSITORY_PATH** Absolute path of the repository (e.g. c:\Workspace)
   - **NUGET_EXE_PATH** Full path of the Nuget CLI executable (e.g. C:\Tools\nuget.exe)
   - **OUTPUT_PATH** Output path of the nuget package generated (e.g. c:\Workspace\output)

___

The Dynamo Package Manager and the Dynamo Package Manager Client both comply with the [Autodesk Privacy Policy](https://www.autodesk.com/company/legal-notices-trademarks/privacy-statement).
