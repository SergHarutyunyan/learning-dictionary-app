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

        EnglishWordsManager EnglishWordsModelManager = new EnglishWordsManager();


        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Start()
        {
            idList.Clear();
            correctAnswers = 0;
            return View();
        }



        [HttpGet]
        public ActionResult GetWords()
        {     
            List<EnglishWords> allWords = EnglishWordsModelManager.EngWords.ToList();
            return View(allWords);
        }


        [HttpGet]
        public ActionResult AddWord()
        {
            return View();
        }


        [HttpPost]
        public ActionResult AddWord(EnglishWords passedEnglishWordParams) {


            if (passedEnglishWordParams.name != null && passedEnglishWordParams.name != "" &&
               passedEnglishWordParams.armTranslation != null && passedEnglishWordParams.armTranslation != "")
            {
                //EWC.AddNewWord(EW);                      
                try
                {
                    EnglishWordsModelManager.EngWords.Add(passedEnglishWordParams);
                    EnglishWordsModelManager.SaveChanges();
                    //ViewData["Response"] = "Successfully registered";
                }
                catch (System.Data.Entity.Infrastructure.DbUpdateException e)
                {
                    ViewData["Response"] = "Error: Record already existing";            //   
                }
            }
            else
                ViewData["Response"] = "Error: Fill blanks";//
            

            return RedirectToAction("AddWord");
        }

        static List<int> idList = new List<int>();
        static int totalRecords;
        static int correctAnswers = 0;

        private int getNotLearnedWordsCount()
        {
            using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["EnglishWordsContext"].ConnectionString))
            {
                SqlCommand cmd = new SqlCommand("Select count(*) from EnglishWords where isLearned == '0' ", con);
                con.Open();
                int totalCount = (int)cmd.ExecuteScalar();

                return totalCount;
            }
        }



        [HttpGet]
        public ActionResult QuizGetWords()
        {
            idList.Clear();
            correctAnswers = 0;

            EnglishWordsModelManager.makeIsLearnedToFalse();
            EnglishWordsModelManager.EngWords.ToList().ForEach(x => { if (x.isLearned == false) { idList.Add(x.id); } });
            totalRecords = getNotLearnedWordsCount();
            idList.Shuffle();

            return View();
        }

        [HttpGet]
        public ActionResult QuizStart()
        {
            ViewBag.totalRecords = totalRecords;
            ViewBag.correctAnswers = correctAnswers;

            if (idList.Count != 0) {
                EnglishWords EnglishWordsModelObject = EnglishWordsModelManager.EngWords.SingleOrDefault(em => em.id == idList.FirstOrDefault());
                ViewBag.passedEnglishWordModelObject = EnglishWordsModelObject;
                
                idList.RemoveAt(0);

                return View(EnglishWordsModelObject);
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
                    correctAnswers++; ;
                } 
            }

            return RedirectToAction("QuizStart");
        }


    }
}