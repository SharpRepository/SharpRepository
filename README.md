![sharp repository logo](https://user-images.githubusercontent.com/6349515/28491141-7b600e46-6eeb-11e7-8c4c-d6139479c18e.png)

What is SharpRepository?
--------------------------------

SharpRepository is a generic repository written in C# which includes support for various relational, 
document and object databases including Entity Framework, RavenDB, MongoDb and Db4o. SharpRepository includes Xml and
InMemory repository implementations as well. SharpRepository offers built-in caching options for AppFabric, 
Memcached and standard System.Runtime.Caching. SharpRepository also supports Specifications, FetchStrategies, 
Batches and Traits. 

How do I get started?
--------------------------------
Check out the [getting started guide](https://github.com/SharpRepository/SharpRepository/wiki/Getting-started). When you're done there, review the SharpRepository.Samples, SharpRepository.Samples.MvcCore, SharpRepository.Samples.MVC5, SharpRepository.Tests.Integration and SharpRepository.Tests 
project for additional sample usage and implementation details.

Compatibility Issues
--------------------------------
- All packages support .NET Framework 4.6 and .NET Standard 2.0. A good part of them supports .NET Standard 1.3
- CouchDB Repository is not compatible with CouchDB 2.0.0 (removed temporary views support)

Running tests
--------------------------------

Integration tests uses all implementations. In order to avoid failing tests and long timeouts you have to install:
- CouchDB 1.x (not 2.x)
- SQL Server Compact
- MongoDB

We notice timeouts and long test discovery in VS2017 and timeouts in "dotnet test". 
The best way is use nunit3 console you can get console here https://github.com/nunit/nunit-console/releases and add installation folder in your path
After that from your project folder you can run all tests with: 
```
nunit3-console.exe ".\SharpRepository.Samples\bin\Debug\net461\SharpRepository.Samples.dll" ".\SharpRepository.Tests\bin\Debug\net461\SharpRepository.Tests.dll" ".\SharpRepository.Tests.Integration\bin\Debug\net461\SharpRepository.Tests.Integration.dll"
```


Have Questions?
--------------------------------

* https://github.com/SharpRepository/SharpRepository/issues
* mail to sharprepository@googlegroups.com
* https://groups.google.com/d/forum/sharprepository
* open a question on stackoverflow.com with sharp-repository tag https://stackoverflow.com/questions/tagged/sharp-repository


