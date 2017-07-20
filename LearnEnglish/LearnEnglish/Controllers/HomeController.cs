using LearnEnglish.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
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

            EnglishWordsContext EWC = new EnglishWordsContext();
            List<EnglishWords> words = EWC.EngWords.ToList();

            return View(words);
        }
    }
}