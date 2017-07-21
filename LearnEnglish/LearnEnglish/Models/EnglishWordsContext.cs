using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity;
using System.Configuration;
using System.Data.SqlClient;

namespace LearnEnglish.Models
{
    public class EnglishWordsContext : DbContext
    {
        public DbSet<EnglishWords> EngWords { get; set; }



        public bool AddNewWord(EnglishWords u)
        {
            string CS = ConfigurationManager.ConnectionStrings["EnglishWordsContext"].ConnectionString;

            using (SqlConnection con = new SqlConnection(CS))
            {
                SqlCommand cmd = new SqlCommand("AddNewWord", con);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;

                SqlParameter param_first_name = new SqlParameter();
                param_first_name.ParameterName = "@name";
                param_first_name.Value = u.name;
                cmd.Parameters.Add(param_first_name);

                SqlParameter param_last_name = new SqlParameter();
                param_last_name.ParameterName = "@armTranslation";
                param_last_name.Value = u.armTranslation;
                cmd.Parameters.Add(param_last_name);

                con.Open();
                cmd.ExecuteNonQuery();
            }

            return true;
        }


    }


    
}