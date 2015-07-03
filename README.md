#GRegClient 

GRegClient is a client for the GReg package manager written in C#.  You can find the source code for GReg [here](https://github.com/pboyer/GReg/).  GRegClient is designed to cover all of the capabilities of GReg.  

#### GRegClient can do the following for you:

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

#### GRegClient doesn't do this for you:

* magically find all of the dependencies your package has (that's up to you)
* provide Autodesk 360 login credentials

#### Steps to build GRegClientNET solution:

1. Navigate to `GRegClientNET\third_party\RestSharp` folder
2. Build `RestSharp.sln` solution (both `Debug` and `Release` configurations)
3. Build `GRegClientNET\src\GregClient.sln` solution

Note: Step 1 and 2 are only done once after cloning the repo for the first time
