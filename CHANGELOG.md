### 3.0.1
* switch pipeline.yml scripts to use internal shared scripts
### version 3.0
* update RestSharp to 108.0.1, this comes with many API breaking changes, some affecting us, see below:
* removal of basic auth - our API no longer makes sense with RestSharp immutable clients.
* switch from newtonsoft.json to system.text.json
* add unit test project
* drop all targets below net6.
* OAuth1 is no longer supported, we no longer sign requests.
* Request base classes are modified to remove support for signing requests.
### version 2.5
* added user/votes request route
* Add net6.0 as a target framework
### version 2.4
* IOAuth2AccessTokenProvider
### version 2.2
* updated Newtonsoft.Json to v13.0.1
* Added changelog.md
* Added changelog.md
