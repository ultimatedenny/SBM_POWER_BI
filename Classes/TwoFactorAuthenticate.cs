using SBM_POWER_BI.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Web;
using static SBM_POWER_BI.Classes.MASTERDATA;

namespace SBM_POWER_BI.Classes
{
    public class TwoFactorAuthenticate
    {
        public string CONNECTION = ConfigurationManager.ConnectionStrings["CONNECTION"].ToString();
        public int Timeout = 1000;

        public PP<List<ADD_SYSTEM_TFA>> ADD_SYSTEM_TFA(ADD_SYSTEM_TFA MODEL)
        {
            DataSet DT1 = new DataSet();
            string storedProcedureName = "ADD_SYSTEM_TFA";
            using (SqlConnection connection = new SqlConnection(CONNECTION))
            {
                using (SqlCommand command = new SqlCommand(storedProcedureName, connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Connection.Open();
                    command.Parameters.AddWithValue("@APPLICATION", MODEL.APPLICATION);
                    //command.Parameters.AddWithValue("@SECRET", MODEL.SECRET);
                    SqlDataAdapter data = new SqlDataAdapter(command);
                    data.Fill(DT1);
                    command.CommandTimeout = Timeout;
                    command.Connection.Close();
                }
            }

            var DATA = (from row in DT1.Tables[1].AsEnumerable()
                        select new ADD_SYSTEM_TFA()
                        {
                            ID = row["ID"].ToString(),
                            APPLICATION = row["APPLICATION"].ToString(),
                            SECRET = row["SECRET"].ToString(),
                            IS_REQUIRE = Convert.ToBoolean(row["IS_REQUIRE"]),
                            CREATE_DATE = row["CREATE_DATE"].ToString(),
                            CREATE_USER = row["CREATE_USER"].ToString(),
                            UPDATE_DATE = row["UPDATE_DATE"].ToString(),
                            UPDATE_USER = row["UPDATE_USER"].ToString(),
                        }).ToList();

            return new PP<List<ADD_SYSTEM_TFA>>
            {
                Result = Convert.ToBoolean(DT1.Tables[0].Rows[0]["RESULT"]),
                Message = DT1.Tables[0].Rows[0]["MESSAGE"].ToString(),
                StatusCode = 200,
                Data = DATA
            };
        }
        public PP<List<ADD_USER_TFA>> GET_USER_TFA(ADD_USER_TFA MODEL)
        {
            DataSet DT1 = new DataSet();
            string storedProcedureName = "GET_USER_TFA";
            using (SqlConnection connection = new SqlConnection(CONNECTION))
            {
                using (SqlCommand command = new SqlCommand(storedProcedureName, connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Connection.Open();
                    command.Parameters.AddWithValue("@USERID", MODEL.USERID);
                    command.Parameters.AddWithValue("@EMAIL", MODEL.EMAIL);
                    command.Parameters.AddWithValue("@APPLICATION", MODEL.APPLICATION);
                    //command.Parameters.AddWithValue("@SECRET_URL", MODEL.SECRET_URL);
                    //command.Parameters.AddWithValue("@SECRET_CODE", MODEL.SECRET_CODE);
                    //command.Parameters.AddWithValue("@SECRET_TEXT", MODEL.SECRET_TEXT);
                    SqlDataAdapter data = new SqlDataAdapter(command);
                    data.Fill(DT1);
                    command.CommandTimeout = Timeout;
                    command.Connection.Close();
                }
            }

            var DATA = (from row in DT1.Tables[1].AsEnumerable()
                        select new ADD_USER_TFA()
                        {
                            ID = row["ID"].ToString(),
                            USERID = row["USERID"].ToString(),
                            EMAIL = row["EMAIL"].ToString(),
                            APPLICATION = row["APPLICATION"].ToString(),
                            SECRET_CODE = row["SECRET_CODE"].ToString(),
                            SECRET_TEXT = row["SECRET_TEXT"].ToString(),
                            SECRET_URL = row["SECRET_URL"].ToString(),
                            EXPIRE_DATE = row["EXPIRE_DATE"].ToString(),
                            CREATE_DATE = row["CREATE_DATE"].ToString(),
                            CREATE_USER = row["CREATE_USER"].ToString(),
                            UPDATE_DATE = row["UPDATE_DATE"].ToString(),
                            UPDATE_USER = row["UPDATE_USER"].ToString(),
                        }).ToList();

            return new PP<List<ADD_USER_TFA>>
            {
                Result = Convert.ToBoolean(DT1.Tables[0].Rows[0]["RESULT"]),
                Message = DT1.Tables[0].Rows[0]["MESSAGE"].ToString(),
                StatusCode = 200,
                Data = DATA
            };
        }

        public PP<List<ADD_USER_TFA>> ADD_USER_TFA(ADD_USER_TFA MODEL)
        {
            DataSet DT1 = new DataSet();
            string storedProcedureName = "ADD_USER_TFA";
            using (SqlConnection connection = new SqlConnection(CONNECTION))
            {
                using (SqlCommand command = new SqlCommand(storedProcedureName, connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Connection.Open();
                    command.Parameters.AddWithValue("@USERID", MODEL.USERID);
                    command.Parameters.AddWithValue("@EMAIL", MODEL.EMAIL);
                    command.Parameters.AddWithValue("@APPLICATION", MODEL.APPLICATION);
                    command.Parameters.AddWithValue("@SECRET_URL", MODEL.SECRET_URL);
                    command.Parameters.AddWithValue("@SECRET_CODE", MODEL.SECRET_CODE);
                    command.Parameters.AddWithValue("@SECRET_TEXT", MODEL.SECRET_TEXT);
                    SqlDataAdapter data = new SqlDataAdapter(command);
                    data.Fill(DT1);
                    command.CommandTimeout = Timeout;
                    command.Connection.Close();
                }
            }

            var DATA = (from row in DT1.Tables[1].AsEnumerable()
                        select new ADD_USER_TFA()
                        {
                            ID = row["ID"].ToString(),
                            USERID = row["USERID"].ToString(),
                            EMAIL = row["EMAIL"].ToString(),
                            APPLICATION = row["APPLICATION"].ToString(),
                            SECRET_CODE = row["SECRET_CODE"].ToString(),
                            SECRET_TEXT = row["SECRET_TEXT"].ToString(),
                            SECRET_URL = row["SECRET_URL"].ToString(),
                            EXPIRE_DATE = row["EXPIRE_DATE"].ToString(),
                            CREATE_DATE = row["CREATE_DATE"].ToString(),
                            CREATE_USER = row["CREATE_USER"].ToString(),
                            UPDATE_DATE = row["UPDATE_DATE"].ToString(),
                            UPDATE_USER = row["UPDATE_USER"].ToString(),
                        }).ToList();

            return new PP<List<ADD_USER_TFA>>
            {
                Result = Convert.ToBoolean(DT1.Tables[0].Rows[0]["RESULT"]),
                Message = DT1.Tables[0].Rows[0]["MESSAGE"].ToString(),
                StatusCode = 200,
                Data = DATA
            };
        }
    }
}