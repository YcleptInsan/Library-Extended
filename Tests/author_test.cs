using Xunit;
using System;
using System.Collections.Generic;

namespace Library.Objects
{
  [Collection("Library")]
  public class AuthorsTests : IDisposable
  {
    public AuthorsTests()
    {
      DBConfiguration.ConnectionString = "Data Source=(localdb)\\mssqllocaldb;Initial Catalog=library_test;Integrated Security=SSPI;";
    }
    [Fact]
    public void Authors_GetAll_ListOfAuthors()
    {
      Authors newAuthor = new Authors("Name", 1);

      List<Authors> newAuthors = new List<Authors>{newAuthor};

      Assert.Equal(newAuthor, newAuthors[0]);
    }
    // [Fact]
    // public void Authors_Saves_SavesToDataBase()
    // {
    //   Authors newAuthor = new Authors("James", 1);
    //   newAuthor.Save();
    //
    //   Authors thisAuthor = Authors.GetAll()[0];
    //
    //   Assert.Equal(newAuthor, thisAuthor);
    // }
    [Fact]
    public void Authors_AddBook_AddAuthorsToBooks()
    {
      Authors newAuthors = new Authors("James", 1);
      newAuthors.Save();
      Books newBook = new Books("Of mice and men", new DateTime(2017, 05, 06), 1);
      newBook.Save();

      Books newBooks1 = new Books("Jerry", new DateTime(2017, 05, 06), 1);
      newBooks1.Save();
      Books newBooks2 = new Books("Jerry",new DateTime(2017, 05, 06), 2);
      newBooks2.Save();

      newAuthors.AddBooks(newBooks1);
      newAuthors.AddBooks(newBooks2);

      List<Books> testList = newAuthors.GetAuthorsBooks();
      List<Books> controlList = new List<Books>{newBooks1, newBooks2};

      Assert.Equal(controlList, testList);
    }
    public void Dispose()
    {
      Authors.DeleteAll();
      Copies.DeleteAll();
      Patrons.DeleteAll();
      Books.DeleteAll();
    }
  }
}
