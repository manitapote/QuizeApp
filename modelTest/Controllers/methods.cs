using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Web;
using modelTest.Models;
using System.Web.Mvc;
using System.Web.SessionState;
using modelTest.Controllers;
using System.Web.Routing;
//using System.Web.Optimization;

namespace modelTest.Controllers
{
    public class methods : Controller
    {
        public void InitializeController(RequestContext context)
        {
            base.Initialize(context);
        }

        personContext pContext = new personContext();
        questionGRE gre = new questionGRE();
        questionSAT sat = new questionSAT();
        question q = new question();


        public string Encrypt(string str)
        {
            string EncrptKey = "2013;[pnuLIT)WebCodeExpert";
            byte[] byKey = { };
            byte[] IV = { 18, 52, 86, 120, 144, 171, 205, 239 };
            byKey = System.Text.Encoding.UTF8.GetBytes(EncrptKey.Substring(0, 8));
            DESCryptoServiceProvider des = new DESCryptoServiceProvider();
            byte[] inputByteArray = Encoding.UTF8.GetBytes(str);
            MemoryStream ms = new MemoryStream();
            CryptoStream cs = new CryptoStream(ms, des.CreateEncryptor(byKey, IV), CryptoStreamMode.Write);
            cs.Write(inputByteArray, 0, inputByteArray.Length);
            cs.FlushFinalBlock();
            return Convert.ToBase64String(ms.ToArray());
        }


        public string Decrypt(string str)
        {
            str = str.Replace(" ", "+");
            string DecryptKey = "2013;[pnuLIT)WebCodeExpert";
            byte[] byKey = { };
            byte[] IV = { 18, 52, 86, 120, 144, 171, 205, 239 };
            byte[] inputByteArray = new byte[str.Length];

            byKey = System.Text.Encoding.UTF8.GetBytes(DecryptKey.Substring(0, 8));
            DESCryptoServiceProvider des = new DESCryptoServiceProvider();
            inputByteArray = Convert.FromBase64String(str.Replace(" ", "+"));
            MemoryStream ms = new MemoryStream();
            CryptoStream cs = new CryptoStream(ms, des.CreateDecryptor(byKey, IV), CryptoStreamMode.Write);
            cs.Write(inputByteArray, 0, inputByteArray.Length);
            cs.FlushFinalBlock();
            System.Text.Encoding encoding = System.Text.Encoding.UTF8;
            return encoding.GetString(ms.ToArray());
        }

        public question GetItem(int id, string test)
        {
            if(test=="GRE")
                q= pContext.questionsGRE.SingleOrDefault(x => x.QsnID == id);
            else
                q = pContext.questionsSAT.SingleOrDefault(x => x.QsnID == id);
            return (q);
        }
        public questionGRE AddItemGRE(FormCollection formCollection, string page)
        {
            string correctAns = formCollection["CorrectAs"];
            gre.Qsn = formCollection["Qsn"];
            gre.OptionA = formCollection["OptionA"];
            gre.OptionB = formCollection["OptionB"];
            gre.OptionC = formCollection["OptionC"];
            gre.OptionD = formCollection["OptionD"];
            if (page == "add")
            {
                switch (correctAns)
                {
                    case "OptionA":
                        gre.CorrectAs = formCollection["OptionA"];
                        break;
                    case "OptionB":
                        gre.CorrectAs = formCollection["OptionB"];
                        break;
                    case "OptionC":
                        gre.CorrectAs = formCollection["OptionC"];
                        break;
                    case "OptionD":
                        gre.CorrectAs = formCollection["OptionD"];
                        break;

                }
            }
            if(page =="eedit")
                gre.CorrectAs = correctAns;            
            gre.Detail = formCollection["Detail"];
            gre.QsnID = Convert.ToInt32(formCollection["QsnID"]);
            return gre;
        }
        public questionSAT AddItemSAT(FormCollection formCollection,string page)
        {
            sat.Qsn = formCollection["Qsn"];
            sat.OptionA = formCollection["OptionA"];
            sat.OptionB = formCollection["OptionB"];
            sat.OptionC = formCollection["OptionC"];
            sat.OptionD = formCollection["OptionD"];
           string correctAns = formCollection["CorrectAs"];
           if (page == "add")
           {
               switch (correctAns)
               {
                   case "OptionA":
                       sat.CorrectAs = formCollection["OptionA"];
                       break;
                   case "OptionB":
                       sat.CorrectAs = formCollection["OptionB"];
                       break;
                   case "OptionC":
                       sat.CorrectAs = formCollection["OptionC"];
                       break;
                   case "OptionD":
                       sat.CorrectAs = formCollection["OptionD"];
                       break;
                   case "OptionE":
                       sat.CorrectAs= formCollection["OptionE"];
                       break;

               }
           }
           if (page == "eedit")
               sat.CorrectAs = correctAns;  
            sat.Detail = formCollection["Detail"];
            sat.OptionE = formCollection["OptionE"];
            sat.QsnID = Convert.ToInt32(formCollection["QsnID"]);
            return sat;
        }
        
        public string GetCorrectAns(int qid,string test)
        {
            string correctAns = "";                                              
            if (test!= null){
                if((test == "GRE")||(test=="0"))
                {
                    correctAns = pContext.questionsGRE.Where(u => u.QsnID == qid).Select(u => u.CorrectAs).SingleOrDefault();
                }
                else
                {
                    correctAns = pContext.questionsSAT.Where(u => u.QsnID == qid).Select(u => u.CorrectAs).SingleOrDefault();
                }
            }
            return correctAns;
        }
                
        public question RandomRow(string t)
        {
            if ((t=="GRE")||(t=="0"))
            {
                q = (from c in pContext.questionsGRE.OrderBy(y => Guid.NewGuid()).Take(1) select c).SingleOrDefault();
            }
            else
                q = (from c in pContext.questionsSAT.OrderBy(y => Guid.NewGuid()).Take(1) select c).SingleOrDefault();
            return q;
        }
    }
}