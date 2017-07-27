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

        public List<EngWord> allWords { get { return EngWords.ToList(); } } 



        /******************            ********************/

        
        public bool AddWord(EngWord word)
        {          
            try  
            {           
                SqlCommand command = new SqlCommand("AddNewWord", connection);
                command.CommandType = System.Data.CommandType.StoredProcedure;

                SqlParameter firstParameter = new SqlParameter();
                firstParameter.ParameterName = "@name";
                firstParameter.Value = word.name;
                command.Parameters.Add(firstParameter);

                SqlParameter secondParameter = new SqlParameter();
                secondParameter.ParameterName = "@armTranslation";
                secondParameter.Value = word.armTranslation;
                command.Parameters.Add(secondParameter);

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


        public bool AddNewWord(EngWord word)
        {
            if (word.name != null && word.armTranslation != null)
            {
                try
                {
                    EngWords.Add(word);
                    SaveChanges();

                }
                catch (System.Data.Entity.Infrastructure.DbUpdateException e)
                {
                    return false;
                }
            } else
            {
                // return FILL ALL FIELDS
                return false;
            }

            return true;

        }


        public bool makeIsLearnedToFalse()
        {
          
            try
            {
                SqlCommand cmd = new SqlCommand("UpdateIsLearned", connection);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;

                connection.Open();
                cmd.ExecuteNonQuery();
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



    }
}