using LearnEnglish.BusinessObjects;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.Entity;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LearnEnglish.DataAccess
{
    public class EngWordManager : DbContext
    {

        static string engConnectionString = ConfigurationManager.ConnectionStrings["EngWordManager"].ConnectionString;
        SqlConnection connection = new SqlConnection(engConnectionString);

        /*****************  PROPERTIES **********************/

        public DbSet<EngWord> EngWords { get; set; }

        public static List<int> idList = new List<int>();
 
        /****************** Adding Word ********************/

        public object AddWord(EngWord newWord)
        {

            if (string.IsNullOrEmpty(newWord.name) || string.IsNullOrEmpty(newWord.armTranslation))
                return new { result = "Fill all blanks", isRedirect = false  };


            string check = newWord.armTranslation;
            for (int i = 0; i < check.Length; i++)
            {
                if (!Char.IsLetter(check[i]))
                {
                    return new { result = "Incorrect input!!", isRedirect = false };
                }
            }

            try
            {
                EngWords.Add(newWord);
                SaveChanges();            

            }
            catch (System.Data.Entity.Infrastructure.DbUpdateException e)
            {
                return new { result = "Word already exists!!", isRedirect = false };
            }

            return new { result = "Added successfully!!", isRedirect = true };

        }

        /****************** Deleting Word ********************/

        public bool DeleteWordIdFix(EngWord word)
        {
            try
            {
                SqlCommand command = new SqlCommand("DeleteWord", connection);
                command.CommandType = System.Data.CommandType.StoredProcedure;

                SqlParameter firstParameter = new SqlParameter();
                firstParameter.ParameterName = "@name";
                firstParameter.Value = word.name;
                command.Parameters.Add(firstParameter);

                connection.Open();
                command.ExecuteNonQuery();
            }
            catch (SqlException e)
            {
                return false;
            }
            finally
            {
                connection.Close();
            }
            return true;
        }



        public void DeleteWord(EngWord word)
        {
           using (SqlConnection connection = new SqlConnection(engConnectionString)) { 
                SqlCommand command = new SqlCommand("SimpleDeleteWord", connection);
                command.CommandType = System.Data.CommandType.StoredProcedure;


                SqlParameter firstParameter = new SqlParameter();
                firstParameter.ParameterName = "@name";
                firstParameter.Value =word.name;
                command.Parameters.Add(firstParameter);

                connection.Open();
                command.ExecuteNonQuery();
            }
        }

        /****************** IsLearned to False ********************/

        public void makeIsLearnedToFalse()
        {

            using (SqlConnection connection = new SqlConnection(engConnectionString))
            {
                SqlCommand cmd = new SqlCommand("UpdateIsLearned", connection);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;

                connection.Open();
                cmd.ExecuteNonQuery();
            }
        }


        public int getNotLearnedWordsCount()
        {
            try
            {
                SqlCommand cmd = new SqlCommand("Select count(*) from EnglishWords where isLearned = '0' ", connection);
                connection.Open();
                int totalCount = (int)cmd.ExecuteScalar();

                return totalCount;
            }
            catch (SqlException e)
            {
                return -1;
            }
            finally { connection.Close(); }
        }


        public int getIdList(int numberOfRows)
        {
            try
            {
                List<int> temp = new List<int>();


                SqlCommand command = new SqlCommand("getRandomRows", connection);
                command.CommandType = System.Data.CommandType.StoredProcedure;


                SqlParameter firstParameter = new SqlParameter();
                firstParameter.ParameterName = "@numberOfRows";
                firstParameter.Value = numberOfRows;
                command.Parameters.Add(firstParameter);

                connection.Open();
                SqlDataReader data = command.ExecuteReader();
           
                while(data.Read())
                {
                    idList.Add((int)data.GetValue(0));
                }

                return idList.Count;

            }
            catch (SqlException e)
            {
                return 0;
            }
            finally { connection.Close(); }
        }

        public void AddScore(TranslationCheck Check)
        {
            using (SqlConnection connection = new SqlConnection(engConnectionString))
            {
                SqlCommand cmd = new SqlCommand("Update EnglishWords set isLearned = '1' where id = " + Check.TranslationId, connection);
                connection.Open();
                cmd.ExecuteNonQuery();
                getUser().incrementCorrectAnswers();
            }
        }


        public EngWord getNextWord()
        {
            return EngWords.SingleOrDefault(em => em.id == EngWordManager.idList.FirstOrDefault());
        }


        /****************** User ********************/

        public void CreateUser(string name, int age, int number, int correct)
        {
            UserInfo.setUser(name, age, number, correct);
        }

        public void DeleteUser(string name, int age, int number, int correct)
        {
            UserInfo.clearUser();
        }

        public UserInfo getUser()
        {
            return UserInfo.retUser();
        }

    }
}