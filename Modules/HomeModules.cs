using System;
using System.Collections.Generic;
using Nancy;

namespace Library.Objects
{
  public class HomeModule : NancyModule
  {
    public HomeModule()
    {
      Get["/"] = _ => {
        return View["index.cshtml"];
      };
      Post["/"] = _ => {
        Patrons newPatron = new Patrons(Request.Form["new-user"]);
        newPatron.Save();
        return View["index.cshtml"];
      };
      Post["/catalog"]= _ => {
        Dictionary<string, object> newDictionary = new Dictionary<string, object>();
        List<Books> allBooks = Books.GetAll();
        // Patrons test = Patrons.login("Hunter");
        Patrons newPatrons = Patrons.login(Request.Form["user-name"]);
        newDictionary.Add("allBooks", allBooks);
        newDictionary.Add("user", newPatrons);
        // Have a user error message on other page if user incorrectly entered a name
        return View["catalog.cshtml", newDictionary];
      };
    }
  }
}
