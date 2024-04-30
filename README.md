[![Nuget](https://img.shields.io/nuget/v/Greg?logo=nuget)](https://www.nuget.org/packages/Greg/)

[![PackageManagerClient-VS2022Build](https://github.com/DynamoDS/PackageManagerClient/actions/workflows/PackageManagerClientMSbuild.yml/badge.svg)](https://github.com/DynamoDS/PackageManagerClient/actions/workflows/PackageManagerClientMSbuild.yml) 


# Dynamo Package Manager Client

This repo is the package manager client for the Dynamo Package Manager. Package Manager Client is designed to cover all of the capabilities of the Dynamo Package Manager.

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

    Build the `PackageManagerClient\src\GregClient.sln` solution

- From command line:

    ```bat
    nuget restore src\GregClient.sln
    msbuild src\GregClient.sln
    ```

#### How to increment the assembly version:

Locate the [GregClient.csproj](https://github.com/DynamoDS/PackageManagerClient/blob/master/src/GregClient/GregClient.csproj) and change the `Version` property.
___

The Dynamo Package Manager and the Dynamo Package Manager Client both comply with the [Autodesk Privacy Policy](https://www.autodesk.com/company/legal-notices-trademarks/privacy-statement).
