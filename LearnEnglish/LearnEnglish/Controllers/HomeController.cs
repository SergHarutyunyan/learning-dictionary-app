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
        public ActionResult SearchWord()
        {
            return View();
        }


        [HttpPost]
        public ActionResult SearchWord(string name)
        {
            var words = EngWordModelManager.EngWords.Where(a => a.name.Contains(name)).ToList();
            if (words.Count <= 0)
            {
                return HttpNotFound();
            }

            return PartialView(words);
        }

        [HttpGet]
        public ActionResult ListAllWords()
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
          
            return Json(EngWordModelManager.AddWord(newWord));             
          
        }



        [HttpGet]
        public ActionResult DeletionBox(int id)
        {
          
            EngWord word = EngWordModelManager.EngWords.Find(id);
            return View(word);
        }



        [HttpPost, ActionName("DeletionBox")]
        public ActionResult Deletion(int id)
        {
            EngWord word = EngWordModelManager.EngWords.Find(id);
            EngWordModelManager.DeleteWord(word);
            return RedirectToAction("ListAllWords");

        }    

        [HttpGet]
        public ActionResult PrepareForQuiz()
        {

            if (EngWordModelManager.getNotLearnedWordsCount() <= 100)
                ViewBag.NotLearnedWordsCount = EngWordModelManager.getNotLearnedWordsCount();
            else
                ViewBag.NotLearnedWordsCount = 100;


            EngWordModelManager.makeIsLearnedToFalse();
            EngWordManager.idList.Clear();        

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

                ViewData["total"] = EngWordModelManager.getUser().getNumberOfWords();
                ViewData["correct"] = EngWordModelManager.getUser().getCorrectAnswers();

                EngWordManager.idList.RemoveAt(0);

                return View(EnglishWordsModelObject);
            } 

            return View("QuizOver" , EngWordModelManager.getUser());
        }

        [HttpPost]
        public ActionResult QuizStart(TranslationCheck Check)
        {

            if(Check.ArmTranslation == Check.FilledTranslation)
            {
                EngWordModelManager.AddScore(Check);
            }

            return RedirectToAction("QuizStart");
        }



    }
}