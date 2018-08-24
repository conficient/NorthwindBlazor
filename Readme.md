# BlazorNorthwind Sample Application

The purpose of this repo is to test a "real-world" database-linked
[Blazor](https://blazor.net/) application. This is going to be a fairly
common scenario and it act as a learning tool for me, and also identify
some pain points which I can feed back to the Blazor team.

## Part 1 - Proper Preparation Prevents Poor Performance

The aim is to have a Blazor web application with data from a SQL database, 
that allows us to browse and edit the data. This is a very common scenario 
and one that Blazor should be able to handle. One item that we won't address
in this app is security - it's going to be wide open initially, but we
might look to add this on later.

### Aims

We want to test the following:

 * database access
 * re-use of models, shared code and validation
 * Component use/strategies
 * State management
 * REST API 
 * Forms (including validation)
 * Grids/tables
 * Navigation
 * Search

 ### Requirements

 A quick list of the stuff we're using as a reference:
 * [Visual Studio 2017 (version 15.8)](https://visualstudio.microsoft.com/)
 * [Blazor 0.5.1](https://github.com/aspnet/Blazor/releases/tag/0.5.1)
 * [.Net Core 2.1 (version 2.1.400) SDK](https://www.microsoft.com/net/download)
 


 ### SQL Setup

I'm going to use the [Northwind](https://docs.microsoft.com/en-us/dotnet/framework/data/adonet/sql/linq/downloading-sample-databases)
database for this experiment. There are more complex demo databases such as
AdventureWorks but these are more about SQL Feature demonstration that a useful basic
database such as Northwind, which only has 13 tables.

For development I've restored Northwind onto a SQL 2014 Developer Edition server using 
the restore SQL script from the site above. I've set up a single user called _Blazor_,
(password also _Blazor_) and given it full access to all the tables in the database. 

That's okay for a test like this, don't do this on a real application.

## Blazor Application

Right, time to get coding! I boot up Visual Studio 2017 version 15.8, and select 
*New ASP.NET Core Web Application*, and then select the *Blazor (ASP.NET Core hosted)*
application type. I could make life easier if I picked the server-side Blazor since
it would mean I don't have to create an API, but I have performance concerns about
this mode of running Blazor, and anyway I want to emulate how most people will run
an app like this, using a REST API. Read the [Getting Started Guide](https://blazor.net/docs/get-started.html)
if you're new to Blazor.

### Database Access Framework

Since this project is using Blazor, we need a framework that will work with .NET Standard. 
The obvious choice is Microsoft's [EFCore](https://docs.microsoft.com/en-us/ef/core/) but 
I am also considering [Dapper](https://stackexchange.github.io/Dapper/). We'll start
with EFCore for now.

In previous a previous test when I'd used EF Core it worked, but you have to separate 
the table classes from the `DataContext` since the context conflicts with Mono's 
webassembly implementation. You don't need the datacontext in the Blazor app anyway 
as it cannot access the database from the browser context.

#### Code Generation

Since we have a database already we can use EF's database-first approach and [generate the
classes and the EF datacontext](https://docs.microsoft.com/en-us/ef/core/get-started/aspnetcore/existing-db). 

But wait! We need to remember [Sanderson's First Rule of Software Development](https://www.youtube.com/watch?v=JU-6pAxqAa4) 

> Let someone else do this for you 

A quick search on Github reveals [someone already did this](https://github.com/JasonGT/NorthwindTraders). So
with thanks to [Jason](https://github.com/JasonGT) at SSW, I've copied the entity and database 
code sections we need.

#### Entities

I might want to put Blazor components in the same library as the entities, so I create 
the library **NorthwindBlazor.Entities** using 
`dotnet new blazorlib -o NorthwindBlazor.Entities` at the command line, and add the 
project into the solution (still waiting on [Project and Item Templates](https://github.com/aspnet/Blazor/issues/23)!).

This contains some example code which I delete since we don't need it, and copy the Entity 
classes from Jason's GitHub repo.

#### DbContext

We cannot put the `DbContext` class in the same DLL as the libraries as it causes a 
conflict with Mono-webassembly. So the context goes in **NorthwindBlazor.Database** 
which I've created as _.NET Core 2.1_ library. I could have used a .NET Standard 2.0 
library, but since this will only ever be used in the server, and we don't want it 
on the client, that's okay.

### Test the Database

I created a test class **NorthwindBlazor.Database.Tests** (using MSTestv2 and .NET Core) 
to check that the database connectivity is working correctly. Initially I used the code
```
   var customers = await db.Customers.ToListAsync();
```
However, the code only used the `CustomerId` and `CompanyName` properties, so requesting
the full object when you don't need it is bad practice. I changed this to
```
    var customers = await (from c in db.Customers 
                           select new { c.CustomerId, c.CompanyName}).ToListAsync();
```

This is better (and faster).

However, it occurs to me that this view of the table is one I'm going to want in 
the application. If I wrote the WebAPI this way, it would return an anonymous type. 
When I want to consume this in Blazor, I'm going to need to create a type for it anyway
so I can deserialize the JSON into .NET objects. So I added a folder `CustomerModels` 
in the Entities class and a class `CompanyNameOnly.cs`, then changed the test to
```
var customers = await 
        (from c in db.Customers
         orderby c.CompanyName
         select new Entities.CustomerModels.CompanyNameOnly(c.CustomerId, c.CompanyName)
         ).ToListAsync();
```
That's a better approach.

## Connecting up the Database in the Server

So next step is wire up the database in the server, add an API method and then access this method from Blazor.

### Server

We rename the `SampleDataController.cs` that gets created by default to `CustomerController.cs`, and
change the method to `CompanyNames`. This returns the same list of `CompanyNameOnly` objects as the test.

I added a `Common.cs` class to create the context in, as it will be required in several controllers.  
I have referenced the various database libraries to make this happen.

### Blazor App

First I added **NorthwindBlazor.Entities** to the references of the app. Then I renamed `FetchData.cshtml` 
to `Customers.cshtml`, and amended the code to make the new API call.

Initially I messed up and put the wrong URL in the `GetJSONasync` call, and page simply stuck on "Loading". 
I was able diagnose the cause as the browser's console logs the errors being generated by Blazor.

Having fixed the URL, it works and we now have a list of customers displayed in a table.

#### The 1990s Called..

At this point we're not much futher on that we would have been if the app was pure HTML and written twenty
years ago. Okay it's Bootstrap 4 and making Asynchronous REST calls. 

But each time the app user goes to the /Customers page it will make a database call and get the 
customer list. But this is Blazor and its an [SPA](https://en.wikipedia.org/wiki/Single-page_application) 
framework - we need to re-think how the app will work.

A list of customers might be something that's fairly static, or might change a lot depending on the
nature of the application. We also have a list of Products and Categories. The Categories are probably
not going to change all that much, so perhaps we can store those on the client and re-use the data
and this will save database and API calls.



