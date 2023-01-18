### Create projects
```sh
dotnet new sln healthify
dotnet new webapi -o webapi
dotnet new xunit -o test

dotnet sln healthify.sln add webapi/webapi.csproj
dotnet sln healthify.sln add test/test.csproj

dotnet add ./test/test.csproj reference ./webapi/webapi.csproj
```

### Add test dependencies
```sh
cd test
dotnet add package Moq
dotnet add package FluentAssertions
dotnet add package Bogus
```

### Add webapi dependencies
```sh
cd webapi
dotnet add package JsonFlatFileDataStore
```

### Run tests
```sh
dotnet test
```

### Run webapi
```sh
Control + F5
```