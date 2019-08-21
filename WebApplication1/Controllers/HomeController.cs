using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;

namespace WebApplication1.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }

        public ActionResult DeleteBook(String bookTitle)
        {
            using (var database = new EntityFramework.PublixLibraryEntities())
            {

                var extistingBook = database.PublixLibraries.Where(b => b.Title.ToLower() == bookTitle.ToLower()).FirstOrDefault();
                if (extistingBook != null)
                {
                    database.PublixLibraries.Remove(extistingBook);
                    database.SaveChanges();
                }
            }
            return RedirectToAction("ListBooks", "Home");
        }

        public ActionResult ListBooks(Models.BookModel book)
        {
            
            ViewBag.Message = String.Empty;
            
            
            if (ModelState.IsValid)
            {
                
                using (var database = new EntityFramework.PublixLibraryEntities())
                {
                    var existingBook = database.PublixLibraries.Where(b => b.Title.ToLower() == book.Title.ToLower()).FirstOrDefault();
                    if (existingBook != null)
                    {
                        ViewBag.Message = String.Format("The book {0} by {1} already exists!", existingBook.Title, existingBook.Author);
                    }
                    else
                    {
                        var dbBook = new EntityFramework.PublixLibrary()
                        {
                            Title = book.Title,
                            Author = book.Author,
                            Publisher = book.Publisher,
                            Price = book.Cost
                        };
                        // make sure to add the book to the collection before saving!
                        database.PublixLibraries.Add(dbBook);
                        database.SaveChanges();
                        ViewBag.Message = "Changes saved!";
                    }
                }
            }

            using (var database = new EntityFramework.PublixLibraryEntities())
            {
                var availablePublishers = new List<SelectListItem>();
                foreach (var publisher in database.PublixLibraries.OrderBy(b => b.Publisher).Select(b => b.Publisher).Distinct())
                {
                    var item = new SelectListItem();
                    item.Value = publisher;
                    item.Text = publisher;
                    availablePublishers.Add(item);
                }
                ViewData["Publisher"]= new SelectList(availablePublishers, "Value", "Text");

                ViewData["Library"] = database.PublixLibraries.OrderBy(b => b.Title).ToArray();
            }
            return View("ListBooks", book);
        }
        //
        [HttpGet]
        public ActionResult EditBook(int bookid)
        {
            Models.BookModel model = null;

            using (var database = new EntityFramework.PublixLibraryEntities())
            {
                var dbBook = database.PublixLibraries.Find(bookid);
                if (dbBook != null)
                {
                    model = new Models.BookModel();
                    model.Id = dbBook.Id;
                    model.Title = dbBook.Title;
                    model.Author = dbBook.Author;
                    model.Cost = dbBook.Price;
                    model.Publisher = dbBook.Publisher;
                }
            }
            return View("EditBook", model);

        }
        //Validation
        [HttpPost]
        public ActionResult EditBook(Models.BookModel book)
        {
            using (var database = new EntityFramework.PublixLibraryEntities())
            {
                var dbBook = database.PublixLibraries.Find(book.Id);
                if (dbBook != null)
                {
                    dbBook.Title = book.Title;
                    dbBook.Author = book.Author;
                    dbBook.Price = book.Cost;
                    dbBook.Publisher = book.Publisher;
                    database.SaveChanges();
                }
            }
            return RedirectToAction("ListBooks");
        }
    }
}