using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ZeroHunger.DTOs;
using ZeroHunger.EF;
using System.Data.Entity.Migrations;



namespace ZeroHunger.Controllers
{
    public class RestaurantController : Controller
    {
        // GET: Restaurant
        public ActionResult Index()
        {
            var db = new ZeroHungerEntities1();
            return View(db.Restaurants.Find((int)Session["id"]).Requests.ToList());

        }
        [HttpGet]
        public ActionResult SignUp()
        {
            return View();
        }
        [HttpPost]
        public ActionResult SignUp(SignUpDTO s)
        {
            if (ModelState.IsValid)
            {
                var db = new ZeroHungerEntities1();
                db.Restaurants.Add(convert(s));
                db.SaveChanges();
                TempData["msg"] = "Successfully signed up";
                return RedirectToAction("Login");
            }
            return View(s);
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
                var user = (from u in db.Restaurants
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
            return RedirectToAction("Login");
        }
        //adding Food
        [HttpGet]
        public ActionResult AddFood()
        {
            return View();
        }
        [HttpPost]
        public ActionResult AddFood(FoodDTO f)
        {
            if (ModelState.IsValid)
            {
                List<FoodDTO> FoodList = null;
                if (Session["foodlist"] == null)
                {
                    FoodList = new List<FoodDTO>();
                }
                else
                {
                    FoodList = (List<FoodDTO>)Session["foodlist"];
                }
                FoodList.Add(f);
                Session["foodlist"] = FoodList;
                TempData["msg"] = "New food is added to request(total food " + FoodList.Count + ")";
                return RedirectToAction("RequestedFood");
            }
            return View(f);
        }

        [HttpGet]
        public ActionResult RequestedFood()
        {
            return View((List<FoodDTO>)Session["foodlist"]);
        }
        [HttpGet]
        public ActionResult Proceed()
        {
            if (Session["foodlist"] == null)
            {
                TempData["msg"] = "The food list is empty!";
                return RedirectToAction("RequestedFood");
            }
            return View();
        }

        [HttpPost]
        public ActionResult Proceed(ProcessRequestDTO pr)
        {
            if (Session["foodlist"] == null)
            {
                TempData["msg"] = "The food list is empty!";
                return RedirectToAction("RequestedFood");
            }
            if (ModelState.IsValid)
            {
                var db = new ZeroHungerEntities1();
                Request rq = new Request()
                {
                    Status = "Processing",
                    OrderTime = DateTime.Now,
                    Expiry = pr.Expiry,
                    Res_Id = (int)Session["id"]
                };
                db.Requests.Add(rq);
                foreach (var item in (List<FoodDTO>)Session["foodlist"])
                {
                    Food fd = new Food()
                    {
                        Type = item.Type,
                        Quantity = item.Quantity,
                        Request_Id = rq.Id
                    };
                    db.Foods.Add(fd);
                    rq.Quantity += item.Quantity;
                }
                db.SaveChanges();
                Session["foodlist"] = null;
                TempData["msg"] = "Successfully added an request";
                return RedirectToAction("Index");
            }
            return View(pr);
        }
        
        [HttpGet]
        public ActionResult RequestDetails(int id)
        {
            var db = new ZeroHungerEntities1();
            return View(db.Requests.Find(id));
        }
        

        Restaurant convert(SignUpDTO s)
        {
            return new Restaurant()
            {
                Name = s.Name,
                Email = s.Email,
                Phone = s.Phone,
                Address = s.Address,
                Username = s.Username,
                Password = s.Password
            };
        }

    }
}