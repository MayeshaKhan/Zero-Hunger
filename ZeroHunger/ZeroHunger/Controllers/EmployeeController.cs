using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ZeroHunger.DTOs;
using ZeroHunger.EF;
using System.Data.Entity.Migrations;
using System.Data.Entity.Core.Common.CommandTrees;
using System.Data.Entity.Core.Metadata.Edm;
namespace ZeroHunger.Controllers
{
    public class EmployeeController : Controller
    {
        // GET: Employee
        public ActionResult Index()
        {
            return View();
        }
        [HttpGet]
        public ActionResult Login()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Login(LoginDTO obj)
        {
            if (ModelState.IsValid)
            {
                var db = new ZeroHungerEntities1();
                var user = (from u in db.Employees
                            where
                                u.Username.Equals(obj.Username) &&
                                u.Password.Equals(obj.Password)
                            select u).SingleOrDefault();
                if (user != null)
                {
                    Session["user"] = user.Username;
                    Session["id"] = user.Id;
                    Session["type"] = "employee";

                    TempData["msg"] = "Successfully login";
                    return RedirectToAction("Index");
                }
                else
                {
                    TempData["msg"] = "Invalid credential";
                }
            }
            return View(obj);
        }

        public ActionResult Logout()
        {
            Session.Clear();
            return RedirectToAction("login");
        }
        [HttpGet]
        public ActionResult CollectRequest(int id)
        {
            var db = new ZeroHungerEntities1();
            Request rq = db.Requests.Find(id);
            if (rq == null)
            {
                TempData["msg"] = "Three is no request with id " + id.ToString();
            }
            else
            {
                rq.Status = "collected";
                db.Requests.AddOrUpdate(rq);
                TempData["msg"] = "Request of id " + id.ToString() + " is set as collected";
                db.SaveChanges();
            }
            return RedirectToAction("RequestList");
        }

      
        [HttpGet]
        public ActionResult RequestList()
        {
            var db = new ZeroHungerEntities1();
            return View(db.Employees.Find((int)Session["id"]).Requests.ToList());
        }
      
        [HttpGet]
        public ActionResult RequestDetails(int id)
        {
            var db = new ZeroHungerEntities1();
            return View(db.Requests.Find(id));
        }
   
        [HttpGet]
        public ActionResult RestaurantDetails(int id)
        {
            var db = new ZeroHungerEntities1();
            return View(db.Restaurants.Find(id));
        }


    }
}