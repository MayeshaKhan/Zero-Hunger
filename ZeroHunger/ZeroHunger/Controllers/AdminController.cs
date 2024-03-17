using System;
using System.Collections.Generic;
using System.Data.Entity.Migrations;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ZeroHunger.DTOs;
using ZeroHunger.EF;

namespace ZeroHunger.Controllers
{
    public class AdminController : Controller
    {
        // GET: Admin
        public ActionResult Index()
        {
            return View();

        }
        public ActionResult EmployeeList()
        {
            var db = new ZeroHungerEntities1();
            var data = db.Employees.ToList();
            return View(Convert(data));

        }

        [HttpPost]
        public ActionResult Login(LoginDTO obj)
        {
            if (ModelState.IsValid)
            {
                var db = new ZeroHungerEntities1();
                var user = (from u in db.Admins
                            where
                                u.Username.Equals(obj.Username) &&
                                u.Password.Equals(obj.Password)
                            select u).SingleOrDefault();
                if (user != null)
                {
                    Session["user"] = user.Username;
                    Session["id"] = user.Id;
                    Session["type"] = "admin";

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
        [HttpGet]
        public ActionResult Login()
        {
            return View();
        }
        public ActionResult Logout()
        {
            Session.Clear();
            return RedirectToAction("Login");
        }
        //CRUD employee
        //add
        [HttpGet]
        public ActionResult AddEmployee()
        {
            return View();
        }
        public ActionResult AddEmployee(EmployeeDTO e)
        {
            if (ModelState.IsValid)
            {
                var db = new ZeroHungerEntities1();


                var employee = Convert(e);

                employee.Admin_Id = (int)Session["id"];


                db.Employees.Add(employee);
                db.SaveChanges();
                TempData["msg"] = "New employee is added successfully";
                return RedirectToAction("EmployeeList");
            }

            return View(e);
        }
        //edit
        [HttpGet]
        public ActionResult EditEmployee(int id)
        {
            var db = new ZeroHungerEntities1();
            var emp = db.Employees.Find(id);

            return View(Convert((emp)));
        }
        [HttpPost]
        public ActionResult EditEmployee(EmployeeDTO e)
        {
            if (ModelState.IsValid)
            {
                var db = new ZeroHungerEntities1();
                var exobj = db.Employees.Find(e.Id);
                exobj.Name = e.Name;
                exobj.Username = e.Username;
                exobj.Password = e.Password;
                exobj.Phone = e.Phone;


                exobj.Admin_Id = (int)Session["id"];

                db.SaveChanges();
                TempData["msg"] = "Information of employee of ID " + e.Id.ToString() + " is edited successfully";
                return RedirectToAction("EmployeeList");
            }
            return View();
        }
        //delete

        [HttpGet]
        public ActionResult DeleteEmployee(int id)
        {

            var db = new ZeroHungerEntities1();

            var emp = db.Employees.Find(id);
            return View(Convert((emp)));
            // return View();
        }
        [HttpPost]
        public ActionResult DeleteEmployee(EmployeeDTO e)
        {
            var db = new ZeroHungerEntities1();
            db.Employees.Remove(db.Employees.Find(e.Id));
            db.SaveChanges();
            TempData["msg"] = "Employee of ID " + e.Id + " has been deleted";
            return RedirectToAction("Employeelist");
        }
        [HttpGet]
        public ActionResult RequestList()
        {
            var db = new ZeroHungerEntities1();
            return View(db.Requests.ToList());
        }

        //
        [HttpGet]
        public ActionResult RestaurantDetails(int id)
        {
            var db = new ZeroHungerEntities1();
            return View(db.Restaurants.Find(id));
        }
       
        [HttpGet]
        public ActionResult RequestDetails(int id)
        {
            var db = new ZeroHungerEntities1();
            ViewBag.empList = db.Employees.ToList();
            return View(db.Requests.Find(id));
        }
        
        [HttpPost]
        public ActionResult RequestDetails(int id, AssignEmployeeDTO obj)
        {
            var db = new ZeroHungerEntities1();
            if (ModelState.IsValid)
            {
                Employee emp = db.Employees.Find(obj.Emp_Id);
                if (emp == null)
                {
                    TempData["msg"] = "Employee id does not exist";
                }
                else
                {
                    Request rq = db.Requests.Find(id);
                    rq.Emp_Id = obj.Emp_Id;
                    rq.Admin_Id = (int)Session["id"];
                    db.Requests.AddOrUpdate(rq);
                    db.SaveChanges();
                    TempData["msg"] = emp.Username + " has been assigned for request id " + id;
                    return RedirectToAction("requestlist");
                }
            }
            return View(db.Requests.Find(id));
        }



        public static List<EmployeeDTO> Convert(List<Employee> data)
        {
            var list = new List<EmployeeDTO>();
            foreach (var item in data)
            {
                list.Add(Convert(item));
            }
            return list;
        }
        public static EmployeeDTO Convert(Employee e)
        {
            return new EmployeeDTO()
            {
                Id = e.Id,
                Name = e.Name,
                Username = e.Username,
                Password = e.Password,
                Phone = e.Phone,


            };
        }
        public static Employee Convert(EmployeeDTO e)
        {
            return new Employee()
            {
                Id = e.Id,
                Name = e.Name,
                Username = e.Username,
                Password = e.Password,
                Phone = e.Phone,
                Admin_Id = e.Admin_Id




            };
        }
    }
}