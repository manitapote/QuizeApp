using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using modelTest.Models;
using modelTest.ViewModel;
using System.Collections;
using modelTest.Controllers;

namespace modelTest.Controllers
{
    public class adminController : Controller
    {
        public personContext pContext = new personContext();
        questionGRE gre = new questionGRE();        
        questionSAT sat = new questionSAT();
        methods m = new methods();
        question q = new question();

        //get all questions
        public ActionResult Index()                           
        {
            List<questionGRE> gree = pContext.questionsGRE.ToList();
            List<questionSAT> satt = pContext.questionsSAT.ToList();
            QsnGreSat qsngresat = new QsnGreSat();
            qsngresat.questionGRE = gree;
            qsngresat.questionSAT = satt;
            return View(qsngresat);
        }


        //set session variable(in which is)
        [HttpPost]
        public ActionResult Add_temp(String v)           
        {           
            Session["admin_choice"] = Convert.ToString(v);
            return Json(v, JsonRequestBehavior.AllowGet);

        }

        [HttpGet]
        public ActionResult Add()
        {            
            return View();
        }

        [HttpPost]
        public ActionResult Add(FormCollection formCollection)
        {
            string tt = Convert.ToString(Session["admin_choice"]);
            if (tt == "GRE")
            {
                gre = m.AddItemGRE(formCollection, "add");              //adding form data to questionGRE object
                pContext.questionsGRE.Add(gre);              
            }
            else
            {
                sat = m.AddItemSAT(formCollection,"add");
                pContext.questionsSAT.Add(sat);                 
            }
            pContext.SaveChanges();                                   //save changes in database
            gre = null;
            sat = null;
            return RedirectToAction("Index", "admin");
        }

        //Delete data from database
        public ActionResult Delete(int id)
        {
            bool v = false;
            int k = id;
            q = m.GetItem(id, Convert.ToString(Session["admin_choice"]));   //get item form table on the basis of admin_choice
            if (Convert.ToString(Session["admin_choice"]) == "GRE")
            {
                var itemToRemove = (questionGRE)q;
                pContext.questionsGRE.Attach(itemToRemove);
                if (itemToRemove != null)
                {
                    //remove the item
                    pContext.questionsGRE.Remove(itemToRemove);
                    pContext.SaveChanges();
                    v = true;
                }
            }
            else
            {
                var itemToRemove = (questionSAT)q;
                pContext.questionsSAT.Attach(itemToRemove);
                if (itemToRemove != null)
                {
                    pContext.questionsSAT.Remove(itemToRemove);
                    pContext.SaveChanges();
                    v = true;
                }
            }
            return Json(v, JsonRequestBehavior.AllowGet);
        }//

        public ActionResult Edit(int id)
        {
            QsnGreSat_single itemToEdit = new QsnGreSat_single();
             q= m.GetItem(id, Convert.ToString(Session["admin_choice"]));   //retrieving question with id on basis of admin choice
            if (Convert.ToString(Session["admin_choice"]) == "GRE")
            {
                itemToEdit.questionGRE = (questionGRE)q;
                itemToEdit.questionSAT = null;                
            }
            else
            {
                itemToEdit.questionSAT = (questionSAT)q;
                itemToEdit.questionGRE = null;               
            }
            return View(itemToEdit);
        }//

        [HttpPost]
        public ActionResult Edit(FormCollection formCollection)
        {
            string admin_choice = Convert.ToString(Session["admin_choice"]);            
            if (admin_choice == "GRE")  //update in GRE table
            {
                gre = m.AddItemGRE(formCollection, "eedit");
                pContext.questionsGRE.Attach(gre);
                pContext.Entry(gre).State = EntityState.Modified;                
            }
            else  //update in SAT table
            {
                sat = m.AddItemSAT(formCollection,"eedit");                //assign form data in questionSAT object                
                pContext.questionsSAT.Attach(sat);
                pContext.Entry(sat).State = EntityState.Modified;         //  modify data    
            }
            pContext.SaveChanges();
            gre = null;
            sat = null;            
            return RedirectToAction("Index");
        }//

        [HttpPost]
        public ActionResult AdminLogin(string usrNm, string pswrd)
        {            
            Session["adminLoggedIn"] = false;               //setting admin login session to false initially
            if((usrNm=="manita")&&(pswrd=="manita"))        //checking user name and password of admin
            {
                Session["adminLoggedIn"] = true;

            }
            return Json(new { ad= Convert.ToBoolean(Session["adminLoggedIn"]) }, JsonRequestBehavior.AllowGet);
        }

         public  ActionResult AdminSignOut()
        {
            Session["adminLoggedIn"] = false;               //set admin logged in session false when signed out
            return RedirectToAction("Index", "person");
        }

    }

   

}
