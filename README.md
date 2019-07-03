# K2Documentation.Samples.Management.EnvironmentLibrary
This project contains sample code that demonstrates how to use the Environment Library management API to create and edit environment libraries in K2.   

Find more information about this API in the K2 Developers Reference at 
https://help.k2.com/onlinehelp/k2five/DevRef/current/default.htm#OtherAPI/O-EnvLib-AddEdit.htm

## Prerequisites
The sample code has the following dependencies: 
* .NET Assemblies (both assemblies are included with K2 client-side tools install and are also included in the project's References folder)
  * SourceCode.EnvironmentLibrary.Management
  * SourceCode.EnvironmentSettings.Client
  * SourceCode.Hosting.Client.BaseAPI

## Getting started
* Use these code snippets to learn how to programatically interact with environment libraries in K2.  
* Note that this projects may compile, but may not run as-is. You will need to edit the code snippets to work in your environment. 
* Fetch or Pull the appropriate branch of this project for your version of K2. 
* The Master branch is considered the latest, up-to-date version of the sample project. Older versions will be branched. For example, there may be a branch called K2-Five-5.0 that is specific to K2 Five version 5.0. There may be another branch called K2-Five-5.1 that is specific to K2 Five version 5.3. Assume that the master branch is configured for the latest release version of K2 Five. 
* The Visual Studio project contains a folder called "References" where you can find the referenced .NET assemblies or other uncommon assemblies. By default, try to reference these assemblies from your own environment for consistency, but we provide the referenced assemblies as a convenience in case you are not able to locate or use the referenced assemblies in your environment. 
* The Visual Studio project contains a folder called "Resources". This folder contains addiitonal resources that may be required to use the same code, such as K2 deployment packages, sample files, SQL scripts and so on. 
   
## License
This project is licensed under the MIT license, which can be found in LICENSE.

## Notes
 * The sample code is provided as-is without warranty.
 * These sample code projects are not supported by K2 product support. 
 * The sample code is not necessarily comprehensive for all operations, features or functionality. 
 * We only accept code contributions that are compatible with the MIT license (essentially, MIT and Public Domain).
