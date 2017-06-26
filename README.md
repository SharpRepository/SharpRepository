What is SharpRepository?
--------------------------------

SharpRepository is a generic repository written in C# which includes support for various relational, 
document and object databases including Entity Framework, RavenDB, MongoDb and Db4o. SharpRepository includes Xml and
InMemory repository implementations as well. SharpRepository offers built-in caching options for AppFabric, 
Memcached and standard System.Runtime.Caching. SharpRepository also supports Specifications, FetchStrategies, 
Batches and Traits. 


How do I get started?
--------------------------------
Check out the [getting started guide](https://github.com/SharpRepository/SharpRepository/wiki/Getting-started). When you're done there, review the SharpRepository.Samples, SharpRepository.Tests.Integration and SharpRepository.Tests 
project for additional sample usage and implementation details.

Important EntityFramework Notice
--------------------------------
Please use SharpRepository.EfRepository moving forward instead of Ef5Repository.  Both will work with EF5 and EF6 but the naming was confusing once EF6 was released so we basically renamed the package.  All updates will be made to EfRepository moving forward.

Compatibility Issues
--------------------------------
- Project targets .NET Framework 4.5.2
- CouchDB Repository is not compatible with CouchDB 2.0.0 (removed temporary views support)

Running tests
--------------------------------
Install:
- CouchDB 1.6
- SQL Server Compact
- MongoDB

The fastest way is use nunit3 console: you can get console here https://github.com/nunit/nunit-console/releases
After that run tests with: 
nunit3-console -v "SharpRepository.Samples\bin\Debug\SharpRepository.Samples.dll" "SharpRepository.Tests\bin\Debug\SharpRepository.Tests.dll" "SharpRepository.Tests.Integration\bin\Debug\SharpRepository.Tests.Integration.dll"


Have Questions?
--------------------------------
Please use the google group for SharpRepository:
* sharprepository@googlegroups.com
* https://groups.google.com/d/forum/sharprepository


