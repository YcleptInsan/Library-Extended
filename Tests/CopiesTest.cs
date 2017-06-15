using Xunit;
using System;
using System.Collections.Generic;

namespace Library.Objects
{
  [Collection("Library")]
  public class CopiesTest : IDisposable
  {
    public CopiesTest()
    {
      DBConfiguration.ConnectionString = "Data Source=(localdb)\\mssqllocaldb;Initial Catalog=library_test;Integrated Security=SSPI;";
    }

    [Fact]
    public void Copies_GetAll_DatabaseEmpty()
    {
      List<Copies> newCopies = Copies.GetAll();
      List<Copies> testCopies = new List<Copies>{};

      Assert.Equal(newCopies, testCopies);
    }

    [Fact]
    public void Copies_Save_SaveCopiesToDatabase()
    {
      Copies newCopy = new Copies(10, 1);
      newCopy.Save();

      List<Copies> controlList = Copies.GetAll();
      List<Copies> testList = new List<Copies>{newCopy};

      Assert.Equal(controlList, testList);
    }

    [Fact]
    public void Copies_AddBook_AddCopiesToPatrons()
    {
      Copies newCopies = new Copies(1);
      newCopies.Save();
      Books newBook = new Books("Of mice and men", new DateTime(2017, 05, 06), 1);
      newBook.Save();

      Patrons newPatrons1 = new Patrons("Jerry", 1);
      newPatrons1.Save();
      Patrons newPatrons2 = new Patrons("Jerry", 2);
      newPatrons2.Save();

      newCopies.AddPatrons(newPatrons1);
      newCopies.AddPatrons(newPatrons2);

      List<Patrons> testList = newCopies.GetPatronCopies();
      List<Patrons> controlList = new List<Patrons>{newPatrons1, newPatrons2};

      Assert.Equal(controlList, testList);
    }

    [Fact]
    public void Copies_Delete_DeleteSingleCopy()
    {
      Patrons newPatrons = new Patrons("David", 1);
      newPatrons.Save();
      Books newBook = new Books("Of mice and men", new DateTime(2017, 05, 06), 1);
      newBook.Save();
      Copies newCopies = new Copies(1);
      newCopies.Save();

      newCopies.AddPatrons(newPatrons);

      newCopies.Delete();

      List<Copies> testCopies = Copies.GetAll();
      List<Copies> controlCopies = new List<Copies>{};

      Assert.Equal(controlCopies, testCopies);
    }

    [Fact]
    public void Copies_Search_SearchAllBooksForBook()
    {

      Books newBook = new Books("Of mice and men", new DateTime(2017, 05, 06), 1);
      newBook.Save();

      Books testBook = new Books("Of another world", new DateTime(2017, 05, 06), 1);
      testBook.Save();
      Books faceBook = new Books("Of dice", new DateTime(2017, 05, 06), 1);
      faceBook.Save();
      Books anotherBook = new Books("A whole new world", new DateTime(2017, 05, 06), 1);
      anotherBook.Save();

      List<Books> controlBooks = new List<Books>{newBook, testBook, faceBook};

      List<Books> searchBooks = Books.Search("of");

      Assert.Equal(controlBooks, searchBooks);
    }

    public void Dispose()
    {
      Copies.DeleteAll();
      Patrons.DeleteAll();
      Books.DeleteAll();
    }
  }
}
