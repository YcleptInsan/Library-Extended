using System;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace Library.Objects
{
  public class Authors
  {
    private int _id;
    private string _name;

    public Authors(string name, int id = 0)
    {
      _name = name;
      _id = id;
    }
    public int GetId()
    {
      return _id;
    }
    public void SetId(int newId)
    {
      _id = newId;
    }
    public string GetName()
    {
      return _name;
    }
    public void SetName(string newName)
    {
      _name = newName;
    }
    public override bool Equals(System.Object otherAuthors)
    {
      if(!(otherAuthors is Authors))
      {
        return false;
      }
      else
      {
        Authors newAuthors = (Authors) otherAuthors;
        bool nameEquality = this.GetName() == newAuthors.GetName();
        bool idEquality = this.GetId() == newAuthors.GetId();
        return (nameEquality && idEquality);
      }
    }
    public static List<Authors> GetAll()
    {
      SqlConnection conn = DB.Connection();
      conn.Open();

      SqlCommand cmd = new SqlCommand("SELECT * FROM authors", conn);
      SqlDataReader rdr = cmd.ExecuteReader();

      List<Authors> authors = new List<Authors>{};
      while(rdr.Read())
      {
        int id = rdr.GetInt32(0);
        string testAuthors = rdr.GetString(1);



        Authors newAuthor = new Authors(testAuthors, id);
        authors.Add(newAuthor);
      }

      if(rdr != null)
      {
        rdr.Close();
      }
      if(conn != null)
      {
        conn.Close();
      }

      return authors;
    }
    public void Save()
    {
      SqlConnection conn = DB.Connection();
      conn.Open();

      SqlCommand cmd = new SqlCommand("INSERT INTO authors (name) OUTPUT INSERTED.id VALUES (@authors)", conn);

      SqlParameter titleParameter = new SqlParameter("@authors", this.GetId());

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
    public void AddBooks(Books newBooks)
    {
      SqlConnection conn = DB.Connection();
      conn.Open();

      SqlCommand cmd = new SqlCommand("INSERT INTO book_authors (book_id, authors_id) OUTPUT INSERTED.id VALUES (@BooksId, @CopiesId);", conn);

      SqlParameter booksIdParam = new SqlParameter("@BooksId", newBooks.GetId());
      SqlParameter authorsIdParam = new SqlParameter("@CopiesId", this.GetId());

      cmd.Parameters.Add(booksIdParam);
      cmd.Parameters.Add(authorsIdParam);
      cmd.ExecuteNonQuery();

      if(conn != null)
      {
        conn.Close();
      }
    }

    public List<Books> GetAuthorsBooks()
    {
      SqlConnection conn = DB.Connection();
      conn.Open();

      SqlCommand cmd = new SqlCommand("SELECT books.* FROM authors JOIN book_authors ON (authors.id = book_authors.authors_id) JOIN books ON (books.id = book_authors.book_id) WHERE authors.id = @AuthorId;", conn);

      SqlParameter BookParameter = new SqlParameter("@AuthorId", this.GetId());
      cmd.Parameters.Add(BookParameter);

      List<Books> AllBooks = new List<Books>{};
      SqlDataReader rdr = cmd.ExecuteReader();

      while(rdr.Read())
      {
        int id = rdr.GetInt32(0);
        string title = rdr.GetString(1);
        DateTime dueDate = rdr.GetDateTime(2);

        Books newBooks = new Books(title, dueDate, id);
        AllBooks.Add(newBooks);

      }
      if(conn != null)
      {
        conn.Close();
      }
      if(rdr != null)
      {
        rdr.Close();
      }
      return AllBooks;
    }
    public static void DeleteAll()
    {
      SqlConnection conn = DB.Connection();
      conn.Open();

      SqlCommand cmd = new SqlCommand("DELETE FROM authors;", conn);
      cmd.ExecuteNonQuery();

      if(conn != null)
      {
        conn.Close();
      }
    }
  }
}
