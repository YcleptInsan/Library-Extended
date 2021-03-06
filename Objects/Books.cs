using System;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace Library.Objects
{
  public class Books
  {
    private int _id;
    private string _title;

  public Books(string Title, int id = 0)
  {
    _id = id;
    _title = Title;
  }
  public int GetId()
  {
    return _id;
  }
  public string GetTitle()
  {
    return _title;
  }

  public void SetTitle(string Title)
  {
    _title = Title;
  }


  public override bool Equals(System.Object otherBooks)
    {
      if(!(otherBooks is Books))
      {
        return false;
      }
      else
      {
        Books newBooks = (Books) otherBooks;
        bool titleEquality = this.GetTitle() == newBooks.GetTitle();
        bool idEquality = this.GetId() == newBooks.GetId();

        return (titleEquality && idEquality);
      }
    }
    public static List<Books> GetAll()
    {
      SqlConnection conn = DB.Connection();
      conn.Open();

      SqlCommand cmd = new SqlCommand("SELECT * FROM books", conn);
      SqlDataReader rdr = cmd.ExecuteReader();

      List<Books> books = new List<Books>{};
      while(rdr.Read())
      {
        int id = rdr.GetInt32(0);
        string title = rdr.GetString(1);

        Books newBook = new Books(title, id);
        books.Add(newBook);
      }

      if(rdr != null)
      {
        rdr.Close();
      }
      if(conn != null)
      {
        conn.Close();
      }

      return books;
    }
    public void Save()
    {
      SqlConnection conn = DB.Connection();
      conn.Open();

      SqlCommand cmd = new SqlCommand("INSERT INTO books (title) OUTPUT INSERTED.id VALUES (@Title)", conn);

      SqlParameter titleParameter = new SqlParameter("@Title", this.GetTitle());

      cmd.Parameters.Add(titleParameter);

      SqlDataReader rdr = cmd.ExecuteReader();

      while(rdr.Read())
      {
        this._id = rdr.GetInt32(0);
      }

      if(rdr != null)
      {
        rdr.Close();
      }
      if(conn != null)
      {
        conn.Close();
      }

    }
    public static Books Find(int idToFind)
    {
      SqlConnection conn = DB.Connection();
      conn.Open();

      SqlCommand cmd = new SqlCommand("SELECT * FROM books WHERE id = @BooksId", conn);
      SqlParameter idParam = new SqlParameter("@BooksId", idToFind);
      cmd.Parameters.Add(idParam);

      SqlDataReader rdr = cmd.ExecuteReader();

      int id = 0;
      string title = null;

      while(rdr.Read())
      {
        id = rdr.GetInt32(0);
        title = rdr.GetString(1);
      }

      Books foundBooks  = new Books(title, id);

      if(rdr != null)
      {
        rdr.Close();
      }
      if(conn != null)
      {
        conn.Close();
      }

      return foundBooks;
    }

    public static List<Books> Search(string test)
    {
      SqlConnection conn = DB.Connection();
      conn.Open();

      SqlCommand cmd = new SqlCommand("SELECT * FROM books WHERE title LIKE @TitleName", conn);
      SqlParameter idParam = new SqlParameter("@TitleName", test + "%");
      cmd.Parameters.Add(idParam);

      SqlDataReader rdr = cmd.ExecuteReader();

      int id = 0;
      string title = null;

      List<Books> allBooks = new List<Books>{};

      while(rdr.Read())
      {
        id = rdr.GetInt32(0);
        title = rdr.GetString(1);
        Books foundBooks  = new Books(title, id);
        allBooks.Add(foundBooks);
      }

      if(rdr != null)
      {
        rdr.Close();
      }
      if(conn != null)
      {
        conn.Close();
      }

      return allBooks;
    }

    public void Delete()
    {
      SqlConnection conn = DB.Connection();
      conn.Open();

      SqlCommand cmd = new SqlCommand("DELETE FROM books WHERE id = @BookId; DELETE FROM book_authors WHERE book_id = @BookId;", conn);
      SqlParameter idParam = new SqlParameter("@BookId", this.GetId());
      cmd.Parameters.Add(idParam);
      cmd.ExecuteNonQuery();

      if(conn != null)
      {
        conn.Close();
      }
    }
    // public void AddBook(Books newBook)
    // {
    //   SqlConnection conn = DB.Connection();
    //   conn.Open();
    //
    //   SqlCommand cmd = new SqlCommand("INSERT INTO patrons_books (patron_id, book_id) VALUES (@PatronId, @BookId);", conn);
    //
    //   SqlParameter patronParam = new SqlParameter("@PatronId", this.GetId());
    //   SqlParameter bookParam = new SqlParameter("@BookId", newBook.GetId());
    //
    //   cmd.Parameters.Add(patronParam);
    //   cmd.Parameters.Add(bookParam);
    //   cmd.ExecuteNonQuery();
    //
    //   if(conn != null)
    //   {
    //     conn.Close();
    //   }
    // }
    //
    // public List<Books> GetBooks()
    // {
    //   SqlConnection conn = DB.Connection();
    //   conn.Open();
    //
    //   SqlCommand cmd = new SqlCommand("SELECT books.* FROM patrons JOIN patrons_books ON (patrons.id = patrons_books.patron_id) JOIN books ON (books.id = patrons_books.book_id) WHERE patrons.id = @PatronId;", conn);
    //
    //   SqlParameter patronParameter = new SqlParameter("@PatronId", this.GetId());
    //   cmd.Parameters.Add(patronParameter);
    //
    //   List<Books> books = new List<Books>{};
    //   SqlDataReader rdr = cmd.ExecuteReader();
    //
    //   while(rdr.Read())
    //   {
    //     int id = rdr.GetInt32(0);
    //     string title = rdr.GetString(1);
    //     string author = rdr.GetString(2);
    //     DateTime dueDate = rdr.GetDateTime(3);
    //
    //     Books newBook = new Books(title, author,dueDate, id);
    //     books.Add(newBook);
    //   }
    //
    //   if(conn != null)
    //   {
    //     conn.Close();
    //   }
    //   if(rdr != null)
    //   {
    //      rdr.Close();
    //   }
    //
    //   return books;
    // }
    public static void DeleteAll()
    {
      SqlConnection conn = DB.Connection();
      conn.Open();

      SqlCommand cmd = new SqlCommand("DELETE FROM books;", conn);
      cmd.ExecuteNonQuery();

      if(conn != null)
      {
        conn.Close();
      }
    }
  }
}
