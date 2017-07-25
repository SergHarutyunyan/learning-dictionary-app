using LearnEnglish.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Windows.Forms;
using System.Web.Mvc;
using System.Data.SqlClient;
using System.Configuration;

namespace LearnEnglish.Controllers
{
    public class HomeController : Controller
    {


        EnglishWordsContext EWC = new EnglishWordsContext();


        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Start()
        {

            return View();
        }



        [HttpGet]
        public ActionResult GetWords()
        {

            
            List<EnglishWords> words = EWC.EngWords.ToList();

            return View(words);
        }


        [HttpGet]
        public ActionResult AddWord()
        {
            return View();
        }


        [HttpPost]
        public ActionResult AddWord(EnglishWords EW)
        {;

            if (EW.name != null && EW.name != "" &&
               EW.armTranslation != null && EW.armTranslation != "")
            {             
                //EWC.AddNewWord(EW);                      
                try
                {
                    EWC.EngWords.Add(EW);
                    EWC.SaveChanges();
                   // TempData["notice"] = "Successfully registered";
                }
                catch (System.Data.Entity.Infrastructure.DbUpdateException e)
                {
                    //TempData["notice"] = "Error: Record already existing";
                    MessageBox.Show("Error: Record already existing");      // Need to fix (published version)
                }

                           
            } else
                MessageBox.Show("Error: Try again");


            MessageBox.Show("Record added succesfully");

            return RedirectToAction("AddWord");
        }

        static List<int> allWordsID = new List<int>();
        static int totalRecords;
        static int correctAnswers = 0;

        //private int getTotalWordsCount()
        //{
        //    using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["EnglishWordsContext"].ConnectionString))
        //    {
        //        SqlCommand cmd = new SqlCommand("Select count(*) from EnglishWords", con);
        //        con.Open();
        //        int totalCount = (int)cmd.ExecuteScalar();

        //        return totalCount;

        //    }

        //    return -1;
        //}

        [HttpGet]
        public ActionResult QuizGetWords()
        {
         
            EWC.isLearnedFalse();
            EWC.EngWords.ToList().ForEach(x => { if (x.isLearned == false) { allWordsID.Add(x.id); } });
            totalRecords = allWordsID.Count;
            allWordsID.Shuffle();

            return View();
        }

        [HttpGet]
        public ActionResult QuizStart()
        {
            

            if (allWordsID.Count != 0) {
                EnglishWords EW = EWC.EngWords.SingleOrDefault(e => e.id == allWordsID.FirstOrDefault());
                ViewBag.obj = EW;
                ViewBag.totalRecords = totalRecords;
                ViewBag.correctAnswers = correctAnswers;
                allWordsID.RemoveAt(0);

                return View(EW);
            } else
            {

            }


            return View();
        }

        [HttpPost]
        public ActionResult QuizStart(TranslationCheck TC)
        {

            if(TC.ArmTranslation == TC.FilledTranslation)
            {
                using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["EnglishWordsContext"].ConnectionString))
                {
                    SqlCommand cmd = new SqlCommand("Update EnglishWords set isLearned = '1' where id = " + TC.TranslationId, con);
                    con.Open();
                    cmd.ExecuteNonQuery();
                    correctAnswers++;
                } 
            }

            return RedirectToAction("QuizStart");
        }


    }
}