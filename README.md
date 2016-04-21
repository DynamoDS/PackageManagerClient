#Package Manager Client

This repo is the package manager client for the Dynamo Package Manager.  You can find the source code for the Dynamo Package Manager [here](https://github.com/pboyer/GReg/).  Package Manager Client is designed to cover all of the capabilities of the Dynamo Package Manager.  

#### The Package Manager Client can do the following for you:

* properly format, compress, and upload a package to the server
* properly format, compress, and upload a new package version to the server
* download a specific package header
* download a specific package body
* determine server connectivity
* upvote or downvote a package
* search across packages
* deprecate a package
* manage maintainers of a package
* manage authentication

#### The Package Manager Client doesn't do this for you:

* magically find all of the dependencies your package has (that's up to you)
* provide Autodesk 360 login credentials

#### Steps to build The Package Manager Client solution:

1. Build `The PackageManagerClient\src\GregClient.sln` solution

