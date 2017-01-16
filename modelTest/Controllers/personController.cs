using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using modelTest.Models;
using System.Data.SqlClient;
using System.Web.SessionState;
using modelTest.Controllers;
using modelTest.ViewModel;

namespace modelTest.Controllers
{
    public class personController : Controller
    {
        //Global variables
        personContext pContext = new personContext();
        methods m = new methods();                              
        QsnGreSat_single q = new QsnGreSat_single();
        question qsn = new question();

        [HttpGet]
        public ActionResult Index()    //Get sample question in Index page
        {            
            qsn=m.RandomRow(Convert.ToString(Session["QsnID"]));
            if (Convert.ToInt32(Session["QsnID"]) < 1)
            {
                //Random question from GRE table
                q.questionGRE = (questionGRE)qsn;
            }
            else
            {
                //Randowm question from SAT table
                q.questionSAT = (questionSAT)qsn;
            }
            return View(q);
           
        }

        [HttpPost]
        public ActionResult Index(int id, string option )   //id= question id ,option= option choosen by user
        {
            Object[] arr = new Object[4];

            //Retrieving correct ans
            string t = m.GetCorrectAns(id, Convert.ToString(Session["QsnID"]));
            if (Convert.ToInt32(Session["QsnID"]) < 1)         
            {
               
                //Retrieving detail of question
                arr[2] = pContext.questionsGRE.Where(u => u.QsnID == id).Select(u => u.Detail).SingleOrDefault();
            }
            else
            {
                arr[2] = pContext.questionsSAT.Where(u => u.QsnID == id).Select(u => u.Detail).SingleOrDefault();
            }
            Session["QsnID"] = Convert.ToInt32(Session["QsnID"]) + 1;    //incrementing session id to allow only two questions in Index page
            arr[1] = Convert.ToInt32(Session["QsnID"]);
            if (option == t)                                            //checking user option matches correct ans
                arr[0] = true;                                         
            else
                arr[0] = false;
            arr[3] = t;
            return Json(arr, JsonRequestBehavior.AllowGet);            //returning jason data to ajax call
        }
        public ActionResult Details()
        {
            List<person> prsn = pContext.personss.ToList();          //retrieving list of person
            return View(prsn);
        }

        [HttpGet]
        //Registration for new user
        public ActionResult SignUp()                               
        {
            return View();
        }

        [HttpPost]
        public ActionResult SignUp(string firstname, string lastname, string gender, string email, string password, string confirm_password, string test)
        {
           if (password == confirm_password)                   //checking if password matches to confirm password entered
                {
                    try
                    {
                        using (pContext)
                        {
                            //retrieving email form existing database if new user entered email exists
                            var chkemail = (from s in pContext.personss where s.Email == email select s).FirstOrDefault();
                            if (chkemail == null)                   //checking if any exist
                            {
                                person p = new person();
                                p.FirstName = firstname;
                                p.LastName = lastname;
                                p.Gender = gender;
                                p.Email = email;
                                p.passwrd = m.Encrypt(password);
                                p.TotalPoint = 0;
                                p.Test = test;
                                pContext.personss.Add(p);
                                pContext.SaveChanges();
                                int userId = pContext.personss.Where(u => u.Email == email).Select(u => u.UserID).SingleOrDefault();
                                Session["userId"] = userId;           //instantiating session variables
                                Session["userName"] = firstname;
                                if (Session["userId"] != null)
                                    Session["test"] = test;
                                return RedirectToAction("Welcome");
                            }
                            else
                            {
                                Response.Write("Email Already Exit");
                                return View();
                            }
                        }
                    }
                    catch (Exception e)
                    {
                        Response.Write("Catch Block" + e);
                        return View();
                    }
                }
                else
                {
                    Response.Write("Password and Confirm password doesnot match");
                    return View();
                }
            }
            
        
        public ActionResult Welcome()
        {                     
            if (Session["userId"] != null)                                          //checking user logged in or not
            {
                var id = Convert.ToInt32(Session["userId"]);
                var user = pContext.personss.SingleOrDefault(x => x.UserID == id); //retrieving particular user detail
                return View(user);                                                 
            }
            else
                return RedirectToAction("SignUp");
        }

        public ActionResult WelcomeQuestion_Retrieve()                            //ajax to retrieve question
        {            
            Object[] obj = new Object[2];           
            if (Session["test"] != null)                                         //checking user assigned to test   
            {
                obj[0] = m.RandomRow(Convert.ToString(Session["test"]));         //getting random question
            }
            obj[1] = Convert.ToString(Session["test"]);
            return Json(obj, JsonRequestBehavior.AllowGet);
        }

        //check correct answer
        //calculate total point
        //save total point to database
        public ActionResult Check(string value, string qid)                     
        {
            Object[] arr = new Object[2];               
            int userId = Convert.ToInt32(Session["userId"]);

            //access previous total point
            int totalPoint = pContext.personss.Where(u => u.UserID == userId).Select(u => u.TotalPoint).SingleOrDefault();

            //retrieving correct
            string correctAns = m.GetCorrectAns(Convert.ToInt32(qid), Convert.ToString(Session["test"]));
            if (correctAns == value)
            {
                arr[0] = true;
                totalPoint= totalPoint + 10;
            }
            else
                arr[0] = false;
            arr[1] = totalPoint;
            var user = new person() { UserID = userId, TotalPoint = totalPoint };
            pContext.personss.Attach(user);
            pContext.Entry(user).Property(x => x.TotalPoint).IsModified = true;
            pContext.SaveChanges();
            return Json(arr, JsonRequestBehavior.AllowGet);     //arr[0]=user answer correct or not, arr[1]=total point
        }


        [HttpGet]
        public ActionResult SignIn()
        {
            return View();
        }

        [HttpPost]
        public ActionResult SignIn(string email, string password)
        {           
                try
                {
                    using (pContext)
                    {
                        password = m.Encrypt(password);                         //encrypt password

                        //retrieving user
                        var query = (from p in pContext.personss
                                     where ((p.Email == email) && (p.passwrd == password))
                                     select p).SingleOrDefault();
                        if (query != null)
                        {
                            person pr = query;

                            //setting session variables
                            Session["userId"] = pr.UserID;              
                            Session["userName"] = pr.FirstName;
                            Session.Add("test", pr.Test);
                            return RedirectToAction("Welcome");
                        }
                        else
                        {
                            Response.Write("Email doesnot exist");
                            return View();
                        }
                    }
                }
                catch (Exception e)
                {
                    Response.Write("Error!!!  " + e);
                    return View();
                }           
         }

        public ActionResult SignOut()
        {
            //setting all session variables to null
            Session["userName"] = null;
            Session["userId"]=null;
            Session["test"]=null;
            Session["QsnID"] = 0;
            return RedirectToAction("Index");
        }
    }
}



