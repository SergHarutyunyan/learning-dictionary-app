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
            EngWordManager.idList.Clear();
            return View();
        }

        [HttpGet]
        public ActionResult GetWords()
        {           
           return View(EngWordModelManager.EngWords);
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


        [HttpGet]
        public ActionResult PrepareForQuiz()
        {

            ViewBag.NotLearnedWordsCount = EngWordModelManager.getNotLearnedWordsCount();
            EngWordModelManager.makeIsLearnedToFalse();
            EngWordManager.idList.Clear();
            //correctAnswers = 0;            

            return View();
        }

        [HttpPost]
        public ActionResult PrepareForQuiz(System.Web.Mvc.FormCollection inputValues)
        {
            EngWordModelManager.CreateUser(
                inputValues["FirstName"],
                Convert.ToInt32(inputValues["Age"]),
                Convert.ToInt32(inputValues["NumberOfWords"])
                ,0
                );

            if (EngWordModelManager.getIdList(Convert.ToInt32(inputValues["NumberOfWords"])) != 0)
                return RedirectToAction("QuizStart");
            else
                return RedirectToAction("PrepareForQuiz");
        }

        [HttpGet]
        public ActionResult QuizStart()
        {
            if (EngWordManager.idList.Count != 0) {
                EngWord EnglishWordsModelObject = EngWordModelManager.getNextWord();
                //ViewBag.passedEnglishWordModelObject = EnglishWordsModelObject;                

                ViewData["total"] = EngWordModelManager.getUser().getNumberOfWords();
                ViewData["correct"] = EngWordModelManager.getUser().getCorrectAnswers();

                EngWordManager.idList.RemoveAt(0);

                return View(EnglishWordsModelObject);
            } 

            return View("QuizOver" , EngWordModelManager.getUser());
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
                    EngWordModelManager.getUser().incrementCorrectAnswers();
                } 
            }

            return RedirectToAction("QuizStart");
        }


    }
}