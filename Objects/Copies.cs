using System;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace Library.Objects
{
  public class Copies
  {
    private int _id;
    private int _copiesId;
    private DateTime _dueDate;

    public Copies(int copiesId, DateTime DueDate, int id = 0)
    {
      _copiesId = copiesId;
      _id = id;
      _dueDate = DueDate;
    }

    public int GetId()
    {
      return _id;
    }
    public int GetCopiesId()
    {
      return _copiesId;
    }
    public DateTime GetDueDate()
    {
      return _dueDate;
    }
    public DateTime SetDueDate()
    {
      return DateTime.Now.AddDays(14);
    }

    public override bool Equals(System.Object otherCopies)
    {
      if(!(otherCopies is Copies))
      {
        return false;
      }
      else
      {
        Copies newCopies = (Copies) otherCopies;
        bool copiesIdEquality = this.GetCopiesId() == newCopies.GetCopiesId();
        bool idEquality = this.GetId() == newCopies.GetId();
        bool dueDateEquality = this.GetDueDate() == newCopies.GetDueDate();

        return (copiesIdEquality && idEquality && dueDateEquality);
      }
    }

    public void Save()
    {
      SqlConnection conn = DB.Connection();
      conn.Open();

      SqlCommand cmd = new SqlCommand("INSERT INTO copies (copies, dueDate) OUTPUT INSERTED.id VALUES (@copies, @dueDate)", conn);

      SqlParameter titleParameter = new SqlParameter("@copies", this.GetCopiesId());
      SqlParameter DateParameter = new SqlParameter("@dueDate", this.GetDueDate());

      cmd.Parameters.Add(titleParameter);
      cmd.Parameters.Add(DateParameter);

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

    public static List<Copies> GetAll()
    {
      SqlConnection conn = DB.Connection();
      conn.Open();

      SqlCommand cmd = new SqlCommand("SELECT * FROM copies", conn);
      SqlDataReader rdr = cmd.ExecuteReader();

      List<Copies> copies = new List<Copies>{};
      while(rdr.Read())
      {
        int id = rdr.GetInt32(0);
        int testcopies = rdr.GetInt32(1);
        DateTime dueDate = rdr.GetDateTime(2);

        Copies newBook = new Copies(testcopies, dueDate, id);
        copies.Add(newBook);
      }

      if(rdr != null)
      {
        rdr.Close();
      }
      if(conn != null)
      {
        conn.Close();
      }

      return copies;
    }

    public void AddPatrons(Patrons newPatrons)
    {
      SqlConnection conn = DB.Connection();
      conn.Open();

      SqlCommand cmd = new SqlCommand("INSERT INTO patrons_copies (patrons_id, copies_id) OUTPUT INSERTED.id VALUES (@PatronsId, @CopiesId);", conn);

      SqlParameter patronsIdParam = new SqlParameter("@PatronsId", newPatrons.GetId());
      SqlParameter copiesIdParam = new SqlParameter("@CopiesId", this.GetId());

      cmd.Parameters.Add(patronsIdParam);
      cmd.Parameters.Add(copiesIdParam);
      cmd.ExecuteNonQuery();

      if(conn != null)
      {
        conn.Close();
      }
    }

    public List<Patrons> GetPatronCopies()
    {
      SqlConnection conn = DB.Connection();
      conn.Open();

      SqlCommand cmd = new SqlCommand("SELECT patrons.* FROM copies JOIN patrons_copies ON (copies.id = patrons_copies.copies_id) JOIN patrons ON (patrons.id = patrons_copies.patrons_id) WHERE copies.id = @CopiesId;", conn);

      SqlParameter BookParameter = new SqlParameter("@CopiesId", this.GetId());
      cmd.Parameters.Add(BookParameter);

      List<Patrons> AllPatrons = new List<Patrons>{};
      SqlDataReader rdr = cmd.ExecuteReader();

      while(rdr.Read())
      {
        int id = rdr.GetInt32(0);
        string copiesId = rdr.GetString(1);

        Patrons newPatrons = new Patrons(copiesId, id);
        AllPatrons.Add(newPatrons);

      }
      if(conn != null)
      {
        conn.Close();
      }
      if(rdr != null)
      {
        rdr.Close();
      }
      return AllPatrons;
    }

    public void Delete()
    {
      SqlConnection conn = DB.Connection();
      conn.Open();

      SqlCommand cmd = new SqlCommand("DELETE FROM copies WHERE id = @copiesId; DELETE FROM patrons_copies WHERE copies_id = @copiesId;", conn);
      SqlParameter idParam = new SqlParameter("@copiesId", this.GetId());
      cmd.Parameters.Add(idParam);
      cmd.ExecuteNonQuery();

      if(conn != null)
      {
        conn.Close();
      }
    }

    public static void DeleteAll()
    {
      SqlConnection conn = DB.Connection();
      conn.Open();

      SqlCommand cmd = new SqlCommand("DELETE FROM copies;", conn);
      cmd.ExecuteNonQuery();

      if(conn != null)
      {
        conn.Close();
      }
    }
  }
}
