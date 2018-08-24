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

> Why code something yourself, when someone else has done it already? 

A quick search on Github reveals [someone already did this](https://github.com/JasonGT/NorthwindTraders). So
with thanks to [Jason](https://github.com/JasonGT) at SSW, I've copied the entity and database 
code sections we need.

I might want to put Blazor components in the same library as the entities, do I create `NorthwindBlazor.Entities` using 
`dotnet new blazorlib -o NorthwindBlazor.Entities` at the command line, and add the 
project into the solution. Still waiting on [Item Templates](https://github.com/aspnet/Blazor/issues/23)
on the **Add** dialog here.

The `NorthwindDbContext` class needs to exist in a separate DLL, which I've created as 
.NET Core 2.1 library. I could have used a .NET Standard 2.0 project, but since this will only 
ever be used in the server, and we don't want it on the client, that's okay.

### Test











