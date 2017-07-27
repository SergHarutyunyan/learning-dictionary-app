using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Windows.Forms;
using System.Web.Mvc;
using System.Data.SqlClient;
using System.Configuration;
using LearnEnglish.DataAccess;
using LearnEnglish.BusinessObjects;
using System.Data.Entity;

namespace LearnEnglish.Controllers
{
    public class HomeController : Controller
    {

        EngWordManager EngWordModelManager = new EngWordManager();


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
           return View(EngWordModelManager.allWords);
        }


        [HttpGet]
        public ActionResult AddWord()
        {
            return View();
        }


        [HttpPost]
        public ActionResult AddWord(EngWord newWord) {

            EngWordModelManager.AddWord(newWord);         
            return RedirectToAction("AddWord");
        }

        [HttpGet]
        public ActionResult DeleteWord()
        {

            return View();
        }


        [HttpPost]
        public ActionResult DeleteWord(EngWord word)
        {

            EngWordModelManager.DeleteWord(word);
            return RedirectToAction("DeleteWord");
        }


        static List<int> idList = new List<int>();
        static int totalRecords;
        static int correctAnswers = 0;

        private int getNotLearnedWordsCount()
        {
            using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["EngWordManager"].ConnectionString))
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

            EngWordModelManager.makeIsLearnedToFalse();
            EngWordModelManager.EngWords.ToList().ForEach(x => { if (x.isLearned == false) { idList.Add(x.id); } });
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
                EngWord EnglishWordsModelObject = EngWordModelManager.EngWords.SingleOrDefault(em => em.id == idList.FirstOrDefault());
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
                using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["EngWordManager"].ConnectionString))
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