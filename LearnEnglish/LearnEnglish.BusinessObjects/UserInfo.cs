using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LearnEnglish.BusinessObjects
{
    public class UserInfo
    {
        private static UserInfo obj = new UserInfo();

        private UserInfo() { }

        private string FirstName;
        private int? Age;
        private int? NumberOfWords;
        private int? CorrectAnswers;

        public static void setUser(string name, int age, int number, int correct)
        {
            obj.FirstName = name;
            obj.Age = age;
            obj.NumberOfWords = number;
            obj.CorrectAnswers = correct;          
        }

        public static UserInfo retUser()
        {
            return obj;
        }

        public static void clearUser()
        {
            obj.FirstName = null;
            obj.Age = null;
            obj.NumberOfWords = null;
            obj.CorrectAnswers = null;
        }

        public string getUserName() { return obj.FirstName; }
        public int? getAge() { return obj.Age; }
        public int? getNumberOfWords() { return obj.NumberOfWords; }
        public int? getCorrectAnswers() { return obj.CorrectAnswers; }


        public void incrementCorrectAnswers() { obj.CorrectAnswers++; }
    }
}
