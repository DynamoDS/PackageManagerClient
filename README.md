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
