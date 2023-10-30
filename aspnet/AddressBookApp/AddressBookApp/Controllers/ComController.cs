using AddressBookApp.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace AddressBookApp.Controllers
{
    public class ComController : Controller
    {
        protected AddressBookInfoEntities db = new AddressBookInfoEntities();


        // GET: Com
        protected virtual ActionResult Index()
        {
            return View();
        }
    }
}