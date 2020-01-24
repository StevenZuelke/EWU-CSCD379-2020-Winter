[![Build Status](https://dev.azure.com/Stevezuelkepsn/CSCD379-2020-Winter/_apis/build/status/StevenZuelke.EWU-CSCD379-2020-Winter%20(1)?branchName=master)](https://dev.azure.com/Stevezuelkepsn/CSCD379-2020-Winter/_build/latest?definitionId=2&branchName=master)
# Assignment

The purpose of this assignment is to learn the following:

<<<<<<< HEAD
- Leveraging code analysis
- Activating nullability
- Implementing continuous integration
- Basic unit testing
- Git basics including rebase

## Instructions

1. Turn on nullability in the **SecretSanta.API** project.
2. Configure nullability in **SecretSanta.API** project to be conditional based Nullability not already being set (pending Thursday lecture).
3. Add the following classes to the **SecretSanta.Business** project with the following the associated properties:
   - **`Gift`**
     - `int Id`e
 as read-only
     - `string Title`
     - `string Description`
     - `string Url`
     - `User User`

   - **`User`**
     - `int Id` as read-only
     - `string FirstName`
     - `string LastName`
     - <*a collection of*> `Gifts`

4. Add non-default constructors for `Gift` and `User`.
5. Add unit tests to all projects except **SecretSanta.Web** and fully unit test the new classes in **SecretSanta.Business**(pending Thursday lecture).
6. Refactor nullability setting into solution level **props** and *targets** files (pending Thursday lecture).
7. Add the following list of code analysis assemblies and appropriately handle all warnings: `IntelliTectAnalyzer.dll`,`Microsoft.NetCore.Analyzers.dll`,`Microsoft.CodeQuality.Analyzers.dll`,`Microsoft.NetCore.Analyzers.dll`,`Microsoft.NetFramework.Analyzers.dll`.  Refactor out a solution level global suppression file for disabling code analysis warnings across all projects.
8. Configure Azure DevOps build for continuous integration to compile and run all unit tests# CSCD-379-2020-Winter
9. If any updates occur in **Assignment1** prior to your PR, rebase onto **Assignment1**.  Finally, and just prior to submitting your PR, rebase from master (which has additional commits added after **Assessment1** was created such as the addition of the cSpell.json in commit #7b106a6).

## Extra Credit

The following are options for extra credit (you don't need to do them all):

- Use reflection to test all properties on all classes in **SecretSanta.Business**.
- Configure tests to run in parallel both locally and in Azure DevOps.
=======
- Create a data layer using Entity Framework (EF)
- Learn the basics of the EF convention and fluent API ways to configure a database using Code First
- Override save methods to implement fingerprinting of records
- Implement mocking to stub out pieces that haven't been implemented yet
- Unit testing database functionality

## Instructions

1. Add the following classes to the **SecretSanta.Data** project with the following associated properties:
   - **`EntityBase`**
     - `int Id`
   - **`FingerPrintEntityBase`** inherit from **`EntityBase`**
     - `string CreatedBy`
     - `DateTime CreatedOn`
     - `string ModifiedBy`
     - `DateTime ModifiedOn`
   - **`Group`** inherit from **`FingerPrintEntityBase`**
     - `string Name`
   - Create a class that will hold the many to many relationship between User and Group
     - Add a collection on both User and Group that references this class
2. Modify the following classes in the **SecretSanta.Data** project:
   - **`Gift`**
     - Remove the Id property and inherit from FingerPrintEntityBase
   - **`User`**
     - Remove the Id property and inherit from FingerPrintEntityBase
     - Make `Gifts` read/write
     - Create a nullable Santa property that contains a User as someone's Santa
3. Remove non-default constructors for `Gift` and `User`.
4. In the **SecretSanta.Data** project
   - Create the **`ApplicationDbContext`** class
     - Create two constructors
        - One only takes the DbContextOptions and passes it on to the base class
        - One that takes the DbContextOptions and an IHttpContextAccessor, pass the DbContextOptions to the base class and save off the IHttpContextAccessor in a class property
     - Add DbSet properties for User, Gift, and Group classes
     - Override OnModelCreating method and use the fluent API to configure the many-to-many pieces that are needed for setting up the database properly
     - Override both SaveChanges methods and apply the fingerprinting logic
5. Update unit tests in the **`SecretSanta.Data.Tests`** project to test actual database logic instead of just constructor logic
   - Mock up IHttpContextAccessor to verify that fingerprint information is working correctly (using Moq library)


## Extra Credit

The following are options for extra credit:
- Enable console logging that will log all database commands
>>>>>>> upstream/Assignment2
