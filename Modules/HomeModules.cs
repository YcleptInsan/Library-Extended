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
      Get["/catalog"]= _ => {
        Dictionary<object, string> newDictionary = new Dictionary()
        List<Books> allBooks = Books.GetAll();
        Patrons newPatrons = Patrons.Find(Request.Form["user-name"]);
        newDictionary.Add(allBooks, "allBooks");
        newDictionary.Add(newPatrons, "user");
        // Have a user error message on other page if user incorrectly entered a name
      }
    }
  }
}
