using LearnEnglish.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Windows.Forms;
using System.Web.Mvc;

namespace LearnEnglish.Controllers
{
    public class HomeController : Controller
    {

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

            EnglishWordsContext EWC = new EnglishWordsContext();
            List<EnglishWords> words = EWC.EngWords.ToList();

            return View(words);
        }


        [HttpGet]
        public ActionResult AddWord()
        {
            return View();
        }


        private void MsgBox(string sMessage)
        {
            string msg = "&lt;script language=\"javascript\"&gt;";
            msg += "alert('" + sMessage + "');";
            msg += "&lt;/script>";
            Response.Write(msg);
        }


        [HttpPost]
        public ActionResult AddWord(System.Web.Mvc.FormCollection FC)
        {
            EnglishWords newWord = new EnglishWords();
            newWord.name = FC["name"];
            newWord.armTranslation = FC["armTranslation"];

            if (newWord.name != null && newWord.name != "" &&
               newWord.armTranslation != null && newWord.armTranslation != "")
            {
             
                EnglishWordsContext EWC = new EnglishWordsContext();
                EWC.AddNewWord(newWord);
                MsgBox("Record added successfuly");             // NEED TO FIX
               

            } else
            {
                MsgBox("Error: Try again");                     // NEED TO FIX

            }


            return RedirectToAction("AddWord");
        }




        [HttpGet]
        public ActionResult QuizGetWords()
        {
            EnglishWordsContext EWC = new EnglishWordsContext();
            List<int> allWordsID = new List<int>();

            EWC.EngWords.ToList().ForEach(x => allWordsID.Add(x.id));
            allWordsID.Shuffle();

            TempData["idList"] = allWordsID;

            return View();
        }


        [HttpGet]
        public ActionResult Quiz()
        {
            List<int> words = (List<int>)TempData["idList"];

            EnglishWordsContext EWC = new EnglishWordsContext();

            
            EnglishWords word = EWC.EngWords.SingleOrDefault(e => e.id == words[0]);
            words.RemoveAt(0);
            

            return View();
        }

    }
}