using SBM_POWER_BI.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.DirectoryServices;
using System.Globalization;
using System.Linq;
using System.Web.UI.WebControls;

namespace SBM_POWER_BI.Classes
{
    public class MASTERDATA
    {
        public string CONNECTION = ConfigurationManager.ConnectionStrings["CONNECTION"].ToString();
        public int Timeout = 1000;
        public List<PBI_REPORT> GET_REPORT()
        {
            DataTable DT1 = new DataTable();
            string storedProcedureName = "GET_REPORT";
            using (SqlConnection connection = new SqlConnection(CONNECTION))
            {
                using (SqlCommand command = new SqlCommand(storedProcedureName, connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Connection.Open();
                    SqlDataAdapter data = new SqlDataAdapter(command);
                    data.Fill(DT1);
                    command.CommandTimeout = Timeout;
                    command.Connection.Close();
                }
            }
            var DATA = (from row in DT1.AsEnumerable()
                        select new PBI_REPORT()
                        {
                            ID = row["ID"].ToString(),
                            REPORT_NAME = row["REPORT_NAME"].ToString(),
                            REPORT_DESC = row["REPORT_DESC"].ToString(),
                            REPORT_URL = row["REPORT_URL"].ToString(),
                            REPORT_DEPT = row["REPORT_DEPT"].ToString(),
                            REPORT_CATEGORY = row["REPORT_CATEGORY"].ToString(),
                            REPORT_VISIBLE = row["REPORT_VISIBLE"].ToString(),
                            TOTAL_VIEW = row["TOTAL_VIEW"].ToString(),
                            TOTAL_PAGE = row["TOTAL_PAGE"].ToString(),
                            CREATE_USER = row["CREATE_USER"].ToString(),
                            CREATE_DATE = row["CREATE_DATE"].ToString(),
                            UPDATE_USER = row["UPDATE_USER"].ToString(),
                            UPDATE_DATE = row["UPDATE_DATE"].ToString(),
                        }).ToList();
            return DATA;
        }
        public PP<RESPONSE> ADD_REPORT(PBI_REPORT MODEL)
        {
            DataTable DT1 = new DataTable();
            string storedProcedureName = "ADD_REPORT";
            using (SqlConnection connection = new SqlConnection(CONNECTION))
            {
                using (SqlCommand command = new SqlCommand(storedProcedureName, connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Connection.Open();
                    command.Parameters.AddWithValue("@REPORT_NAME", MODEL.REPORT_NAME);
                    command.Parameters.AddWithValue("@REPORT_DESC", MODEL.REPORT_DESC);
                    command.Parameters.AddWithValue("@REPORT_URL", MODEL.REPORT_URL);
                    command.Parameters.AddWithValue("@REPORT_URL_EXTERNAL", MODEL.REPORT_URL_EXTERNAL);
                    command.Parameters.AddWithValue("@REPORT_URL_PAGE", (MODEL.REPORT_URL_PAGE ?? ""));
                    command.Parameters.AddWithValue("@REPORT_DEPT", MODEL.REPORT_DEPT);
                    command.Parameters.AddWithValue("@REPORT_CATEGORY", MODEL.REPORT_CATEGORY);
                    command.Parameters.AddWithValue("@USER", COOKIES.GetCookies("Name").ToUpper());
                    SqlDataAdapter data = new SqlDataAdapter(command);
                    data.Fill(DT1);
                    command.CommandTimeout = Timeout;
                    command.Connection.Close();
                }
            }
            return new PP<RESPONSE>
            {
                Result = Convert.ToBoolean(DT1.Rows[0]["RESULT"]),
                Message = DT1.Rows[0]["MESSAGE"].ToString(),
                StatusCode = 200,
                Data = null
            };
        }
        public PP<List<PBI_REPORT>> DETAIL_REPORT(PBI_REPORT MODEL)
        {
            DataSet DT1 = new DataSet();
            string storedProcedureName = "DETAIL_REPORT";
            using (SqlConnection connection = new SqlConnection(CONNECTION))
            {
                using (SqlCommand command = new SqlCommand(storedProcedureName, connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Connection.Open();
                    command.Parameters.AddWithValue("@ID", MODEL.ID);
                    SqlDataAdapter data = new SqlDataAdapter(command);
                    data.Fill(DT1);
                    command.CommandTimeout = Timeout;
                    command.Connection.Close();
                }
            }
            var DATA = (from row in DT1.Tables[1].AsEnumerable()
                        select new PBI_REPORT()
                        {
                            ID = row["ID"].ToString(),
                            REPORT_NAME = row["REPORT_NAME"].ToString(),
                            REPORT_DESC = row["REPORT_DESC"].ToString(),
                            REPORT_URL = row["REPORT_URL"].ToString(),
                            REPORT_URL_EXTERNAL = row["REPORT_URL_EXTERNAL"].ToString(),
                            REPORT_DEPT = row["REPORT_DEPT"].ToString(),
                            REPORT_CATEGORY = row["REPORT_CATEGORY"].ToString(),
                            REPORT_VISIBLE = row["REPORT_VISIBLE"].ToString(),
                            TOTAL_VIEW = row["TOTAL_VIEW"].ToString(),
                            CREATE_USER = row["CREATE_USER"].ToString(),
                            CREATE_DATE = row["CREATE_DATE"].ToString(),
                            UPDATE_USER = row["UPDATE_USER"].ToString(),
                            UPDATE_DATE = row["UPDATE_DATE"].ToString(),
                            SubReports = new List<PBI_REPORT_SUB>()
                        }).ToList();

            var DATA_SUB = (from row in DT1.Tables[2].AsEnumerable()
                            select new PBI_REPORT_SUB()
                            {
                                ID = row["ID"].ToString(),
                                REPORT_PARENT = row["REPORT_PARENT"].ToString(),
                                REPORT_PAGE = row["REPORT_PAGE"].ToString(),
                                CREATE_USER = row["CREATE_USER"].ToString(),
                                CREATE_DATE = row["CREATE_DATE"].ToString(),
                                UPDATE_USER = row["UPDATE_USER"].ToString(),
                                UPDATE_DATE = row["UPDATE_DATE"].ToString(),
                            }).ToList();

            foreach (var reportSub in DATA_SUB)
            {
                var matchingReport = DATA.FirstOrDefault(report => report.ID == reportSub.REPORT_PARENT);
                if (matchingReport != null)
                {
                    matchingReport.SubReports.Add(reportSub);
                }
            }

            var DATA_FINAL = DATA;

            return new PP<List<PBI_REPORT>>
            {
                Result = Convert.ToBoolean(DT1.Tables[0].Rows[0]["RESULT"]),
                Message = DT1.Tables[0].Rows[0]["MESSAGE"].ToString(),
                StatusCode = 200,
                Data = DATA_FINAL
            };
        }
        public PP<RESPONSE> EDIT_REPORT(PBI_REPORT MODEL)
        {
            DataTable DT1 = new DataTable();
            string storedProcedureName = "EDIT_REPORT";
            using (SqlConnection connection = new SqlConnection(CONNECTION))
            {
                using (SqlCommand command = new SqlCommand(storedProcedureName, connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Connection.Open();
                    command.Parameters.AddWithValue("@ID", MODEL.ID);
                    command.Parameters.AddWithValue("@REPORT_NAME", MODEL.REPORT_NAME);
                    command.Parameters.AddWithValue("@REPORT_DESC", MODEL.REPORT_DESC);
                    command.Parameters.AddWithValue("@REPORT_URL", MODEL.REPORT_URL);
                    command.Parameters.AddWithValue("@REPORT_URL_EXTERNAL", MODEL.REPORT_URL_EXTERNAL);
                    command.Parameters.AddWithValue("@REPORT_URL_PAGE", (MODEL.REPORT_URL_PAGE ?? ""));
                    command.Parameters.AddWithValue("@REPORT_DEPT", MODEL.REPORT_DEPT);
                    command.Parameters.AddWithValue("@REPORT_CATEGORY", MODEL.REPORT_CATEGORY);
                    command.Parameters.AddWithValue("@USER", COOKIES.GetCookies("Name").ToUpper());
                    SqlDataAdapter data = new SqlDataAdapter(command);
                    data.Fill(DT1);
                    command.CommandTimeout = Timeout;
                    command.Connection.Close();
                }
            }
            return new PP<RESPONSE>
            {
                Result = Convert.ToBoolean(DT1.Rows[0]["RESULT"]),
                Message = DT1.Rows[0]["MESSAGE"].ToString(),
                StatusCode = 200,
                Data = null
            };

        }
        public PP<RESPONSE> DELETE_REPORT(PBI_REPORT MODEL)
        {
            DataTable DT1 = new DataTable();
            string storedProcedureName = "DELETE_REPORT";
            using (SqlConnection connection = new SqlConnection(CONNECTION))
            {
                using (SqlCommand command = new SqlCommand(storedProcedureName, connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Connection.Open();
                    command.Parameters.AddWithValue("@ID", MODEL.ID);
                    SqlDataAdapter data = new SqlDataAdapter(command);
                    data.Fill(DT1);
                    command.CommandTimeout = Timeout;
                    command.Connection.Close();
                }
            }
            return new PP<RESPONSE>
            {
                Result = Convert.ToBoolean(DT1.Rows[0]["RESULT"]),
                Message = DT1.Rows[0]["MESSAGE"].ToString(),
                StatusCode = 200,
                Data = null
            };

        }
        public List<PBI_CATEGORY> GET_CATEGORY()
        {
            DataTable DT1 = new DataTable();
            string storedProcedureName = "GET_CATEGORY";
            using (SqlConnection connection = new SqlConnection(CONNECTION))
            {
                using (SqlCommand command = new SqlCommand(storedProcedureName, connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Connection.Open();
                    SqlDataAdapter data = new SqlDataAdapter(command);
                    data.Fill(DT1);
                    command.CommandTimeout = Timeout;
                    command.Connection.Close();
                }
            }
            var DATA = (from row in DT1.AsEnumerable()
                        select new PBI_CATEGORY()
                        {
                            ID = row["ID"].ToString(),
                            CATEGORY_NAME = row["CATEGORY_NAME"].ToString(),
                            CATEGORY_DESC = row["CATEGORY_DESC"].ToString(),
                            CREATE_USER = row["CREATE_USER"].ToString(),
                            CREATE_DATE = row["CREATE_DATE"].ToString(),
                            UPDATE_USER = row["UPDATE_USER"].ToString(),
                            UPDATE_DATE = row["UPDATE_DATE"].ToString(),
                        }).ToList();
            return DATA;
        }
        public PP<RESPONSE> ADD_CATEGORY(PBI_CATEGORY MODEL)
        {
            DataTable DT1 = new DataTable();
            string storedProcedureName = "ADD_CATEGORY";
            using (SqlConnection connection = new SqlConnection(CONNECTION))
            {
                using (SqlCommand command = new SqlCommand(storedProcedureName, connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Connection.Open();
                    command.Parameters.AddWithValue("@CATEGORY_NAME", MODEL.CATEGORY_NAME);
                    command.Parameters.AddWithValue("@CATEGORY_DESC", MODEL.CATEGORY_DESC);
                    command.Parameters.AddWithValue("@USER", COOKIES.GetCookies("Name").ToUpper());
                    SqlDataAdapter data = new SqlDataAdapter(command);
                    data.Fill(DT1);
                    command.CommandTimeout = Timeout;
                    command.Connection.Close();
                }
            }
            return new PP<RESPONSE>
            {
                Result = Convert.ToBoolean(DT1.Rows[0]["RESULT"]),
                Message = DT1.Rows[0]["MESSAGE"].ToString(),
                StatusCode = 200,
                Data = null
            };
        }
        public List<PBI_CATEGORY> DETAIL_CATEGORY(PBI_CATEGORY MODEL)
        {
            DataSet DT1 = new DataSet();
            string storedProcedureName = "DETAIL_CATEGORY";
            using (SqlConnection connection = new SqlConnection(CONNECTION))
            {
                using (SqlCommand command = new SqlCommand(storedProcedureName, connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Connection.Open();
                    command.Parameters.AddWithValue("@ID", MODEL.ID);
                    SqlDataAdapter data = new SqlDataAdapter(command);
                    data.Fill(DT1);
                    command.CommandTimeout = Timeout;
                    command.Connection.Close();
                }
            }
            var DATA = (from row in DT1.Tables[1].AsEnumerable()
                        select new PBI_CATEGORY()
                        {
                            ID = row["ID"].ToString(),
                            CATEGORY_NAME = row["CATEGORY_NAME"].ToString(),
                            CATEGORY_DESC = row["CATEGORY_DESC"].ToString(),
                            CREATE_USER = row["CREATE_USER"].ToString(),
                            CREATE_DATE = row["CREATE_DATE"].ToString(),
                            UPDATE_USER = row["UPDATE_USER"].ToString(),
                            UPDATE_DATE = row["UPDATE_DATE"].ToString(),
                        }).ToList();
            return DATA;
        }
        public PP<RESPONSE> EDIT_CATEGORY(PBI_CATEGORY MODEL)
        {
            DataTable DT1 = new DataTable();
            string storedProcedureName = "EDIT_CATEGORY";
            using (SqlConnection connection = new SqlConnection(CONNECTION))
            {
                using (SqlCommand command = new SqlCommand(storedProcedureName, connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Connection.Open();
                    command.Parameters.AddWithValue("@ID", MODEL.ID);
                    command.Parameters.AddWithValue("@CATEGORY_NAME", MODEL.CATEGORY_NAME);
                    command.Parameters.AddWithValue("@CATEGORY_DESC", MODEL.CATEGORY_DESC);
                    command.Parameters.AddWithValue("@USER", COOKIES.GetCookies("Name").ToUpper());
                    SqlDataAdapter data = new SqlDataAdapter(command);
                    data.Fill(DT1);
                    command.CommandTimeout = Timeout;
                    command.Connection.Close();
                }
            }
            return new PP<RESPONSE>
            {
                Result = Convert.ToBoolean(DT1.Rows[0]["RESULT"]),
                Message = DT1.Rows[0]["MESSAGE"].ToString(),
                StatusCode = 200,
                Data = null
            };
        }
        public PP<RESPONSE> DELETE_CATEGORY(PBI_CATEGORY MODEL)
        {
            DataTable DT1 = new DataTable();
            string storedProcedureName = "DELETE_CATEGORY";
            using (SqlConnection connection = new SqlConnection(CONNECTION))
            {
                using (SqlCommand command = new SqlCommand(storedProcedureName, connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Connection.Open();
                    command.Parameters.AddWithValue("@ID", MODEL.ID);
                    SqlDataAdapter data = new SqlDataAdapter(command);
                    data.Fill(DT1);
                    command.CommandTimeout = Timeout;
                    command.Connection.Close();
                }
            }
            return new PP<RESPONSE>
            {
                Result = Convert.ToBoolean(DT1.Rows[0]["RESULT"]),
                Message = DT1.Rows[0]["MESSAGE"].ToString(),
                StatusCode = 200,
                Data = null
            };
        }
        public List<PBI_DEPARTMENT> GET_DEPARTMENT()
        {
            DataTable DT1 = new DataTable();
            string storedProcedureName = "GET_DEPARTMENT";
            using (SqlConnection connection = new SqlConnection(CONNECTION))
            {
                using (SqlCommand command = new SqlCommand(storedProcedureName, connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Connection.Open();
                    SqlDataAdapter data = new SqlDataAdapter(command);
                    data.Fill(DT1);
                    command.CommandTimeout = Timeout;
                    command.Connection.Close();
                }
            }
            var DATA = (from row in DT1.AsEnumerable()
                        select new PBI_DEPARTMENT()
                        {
                            ID = row["ID"].ToString(),
                            DEPARTMENT_ID = row["DEPARTMENT_ID"].ToString(),
                            DEPARTMENT_NAME = row["DEPARTMENT_NAME"].ToString(),
                            DEPARTMENT_DESC = row["DEPARTMENT_DESC"].ToString(),
                            CREATE_USER = row["CREATE_USER"].ToString(),
                            CREATE_DATE = row["CREATE_DATE"].ToString(),
                            UPDATE_USER = row["UPDATE_USER"].ToString(),
                            UPDATE_DATE = row["UPDATE_DATE"].ToString()
                        }).ToList();
            return DATA;
        }
        public PP<RESPONSE> ADD_DEPARTMENT(PBI_DEPARTMENT MODEL)
        {
            DataTable DT1 = new DataTable();
            string storedProcedureName = "ADD_DEPARTMENT";
            using (SqlConnection connection = new SqlConnection(CONNECTION))
            {
                using (SqlCommand command = new SqlCommand(storedProcedureName, connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Connection.Open();
                    command.Parameters.AddWithValue("@DEPARTMENT_ID", MODEL.DEPARTMENT_ID);
                    command.Parameters.AddWithValue("@DEPARTMENT_NAME", MODEL.DEPARTMENT_NAME);
                    command.Parameters.AddWithValue("@DEPARTMENT_DESC", MODEL.DEPARTMENT_DESC);
                    command.Parameters.AddWithValue("@USER", COOKIES.GetCookies("Name").ToUpper());
                    SqlDataAdapter data = new SqlDataAdapter(command);
                    data.Fill(DT1);
                    command.CommandTimeout = Timeout;
                    command.Connection.Close();
                }
            }
            return new PP<RESPONSE>
            {
                Result = Convert.ToBoolean(DT1.Rows[0]["RESULT"]),
                Message = DT1.Rows[0]["MESSAGE"].ToString(),
                StatusCode = 200,
                Data = null
            };
        }
        public List<PBI_DEPARTMENT> DETAIL_DEPARTMENT(PBI_DEPARTMENT MODEL)
        {
            DataSet DT1 = new DataSet();
            string storedProcedureName = "DETAIL_DEPARTMENT";
            using (SqlConnection connection = new SqlConnection(CONNECTION))
            {
                using (SqlCommand command = new SqlCommand(storedProcedureName, connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Connection.Open();
                    command.Parameters.AddWithValue("@ID", MODEL.ID);
                    SqlDataAdapter data = new SqlDataAdapter(command);
                    data.Fill(DT1);
                    command.CommandTimeout = Timeout;
                    command.Connection.Close();
                }
            }
            var DATA = (from row in DT1.Tables[1].AsEnumerable()
                        select new PBI_DEPARTMENT()
                        {
                            ID = row["ID"].ToString(),
                            DEPARTMENT_ID = row["DEPARTMENT_ID"].ToString(),
                            DEPARTMENT_NAME = row["DEPARTMENT_NAME"].ToString(),
                            DEPARTMENT_DESC = row["DEPARTMENT_DESC"].ToString(),
                            CREATE_USER = row["CREATE_USER"].ToString(),
                            CREATE_DATE = row["CREATE_DATE"].ToString(),
                            UPDATE_USER = row["UPDATE_USER"].ToString(),
                            UPDATE_DATE = row["UPDATE_DATE"].ToString(),
                        }).ToList();
            return DATA;
        }
        public PP<RESPONSE> EDIT_DEPARTMENT(PBI_DEPARTMENT MODEL)
        {
            DataTable DT1 = new DataTable();
            string storedProcedureName = "EDIT_DEPARTMENT";
            using (SqlConnection connection = new SqlConnection(CONNECTION))
            {
                using (SqlCommand command = new SqlCommand(storedProcedureName, connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Connection.Open();
                    command.Parameters.AddWithValue("@ID", MODEL.ID);
                    command.Parameters.AddWithValue("@DEPARTMENT_ID", MODEL.DEPARTMENT_ID);
                    command.Parameters.AddWithValue("@DEPARTMENT_NAME", MODEL.DEPARTMENT_NAME);
                    command.Parameters.AddWithValue("@DEPARTMENT_DESC", MODEL.DEPARTMENT_DESC);
                    command.Parameters.AddWithValue("@USER", COOKIES.GetCookies("Name").ToUpper());
                    SqlDataAdapter data = new SqlDataAdapter(command);
                    data.Fill(DT1);
                    command.CommandTimeout = Timeout;
                    command.Connection.Close();
                }
            }
            return new PP<RESPONSE>
            {
                Result = Convert.ToBoolean(DT1.Rows[0]["RESULT"]),
                Message = DT1.Rows[0]["MESSAGE"].ToString(),
                StatusCode = 200,
                Data = null
            };
        }
        public PP<RESPONSE> DELETE_DEPARTMENT(PBI_DEPARTMENT MODEL)
        {
            DataTable DT1 = new DataTable();
            string storedProcedureName = "DELETE_DEPARTMENT";
            using (SqlConnection connection = new SqlConnection(CONNECTION))
            {
                using (SqlCommand command = new SqlCommand(storedProcedureName, connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Connection.Open();
                    command.Parameters.AddWithValue("@ID", MODEL.ID);
                    SqlDataAdapter data = new SqlDataAdapter(command);
                    data.Fill(DT1);
                    command.CommandTimeout = Timeout;
                    command.Connection.Close();
                }
            }
            return new PP<RESPONSE>
            {
                Result = Convert.ToBoolean(DT1.Rows[0]["RESULT"]),
                Message = DT1.Rows[0]["MESSAGE"].ToString(),
                StatusCode = 200,
                Data = null
            };
        }
        public List<PBI_GROUP> GET_GROUP()
        {
            DataTable DT1 = new DataTable();
            string storedProcedureName = "GET_GROUP";
            using (SqlConnection connection = new SqlConnection(CONNECTION))
            {
                using (SqlCommand command = new SqlCommand(storedProcedureName, connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Connection.Open();
                    SqlDataAdapter data = new SqlDataAdapter(command);
                    data.Fill(DT1);
                    command.CommandTimeout = Timeout;
                    command.Connection.Close();
                }
            }
            var DATA = (from row in DT1.AsEnumerable()
                        select new PBI_GROUP()
                        {
                            ID = row["ID"].ToString(),
                            GROUP_ID = row["GROUP_ID"].ToString(),
                            GROUP_NAME = row["GROUP_NAME"].ToString(),
                            GROUP_DESC = row["GROUP_DESC"].ToString(),
                            ALLOW_CREATE = row["ALLOW_CREATE"].ToString(),
                            ALLOW_SHARE = row["ALLOW_SHARE"].ToString(),
                            ALLOW_VIEW = row["ALLOW_VIEW"].ToString(),
                            ALLOW_EXPORT = row["ALLOW_EXPORT"].ToString(),
                            ACTIVE = row["ACTIVE"].ToString(),
                            CREATE_USER = row["CREATE_USER"].ToString(),
                            CREATE_DATE = row["CREATE_DATE"].ToString(),
                            UPDATE_USER = row["UPDATE_USER"].ToString(),
                            UPDATE_DATE = row["UPDATE_DATE"].ToString(),
                        }).ToList();

            return DATA;
        }
        public PP<RESPONSE> ADD_GROUP(PBI_GROUP MODEL)
        {
            DataTable DT1 = new DataTable();
            string storedProcedureName = "ADD_GROUP";
            using (SqlConnection connection = new SqlConnection(CONNECTION))
            {
                using (SqlCommand command = new SqlCommand(storedProcedureName, connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@GROUP_ID", MODEL.GROUP_ID);
                    command.Parameters.AddWithValue("@GROUP_NAME", MODEL.GROUP_NAME);
                    command.Parameters.AddWithValue("@GROUP_DESC", MODEL.GROUP_DESC);
                    command.Parameters.AddWithValue("@ALLOW_CREATE", MODEL.ALLOW_CREATE);
                    command.Parameters.AddWithValue("@ALLOW_SHARE", MODEL.ALLOW_SHARE);
                    command.Parameters.AddWithValue("@ALLOW_VIEW", MODEL.ALLOW_VIEW);
                    command.Parameters.AddWithValue("@ALLOW_EXPORT", MODEL.ALLOW_EXPORT);
                    command.Parameters.AddWithValue("@ACTIVE", MODEL.ACTIVE);
                    command.Parameters.AddWithValue("@USER", COOKIES.GetCookies("Name").ToUpper());

                    connection.Open();
                    SqlDataAdapter data = new SqlDataAdapter(command);
                    data.Fill(DT1);
                    connection.Close();
                }
            }
            return new PP<RESPONSE>
            {
                Result = Convert.ToBoolean(DT1.Rows[0]["RESULT"]),
                Message = DT1.Rows[0]["MESSAGE"].ToString(),
                StatusCode = 200,
                Data = null
            };
        }
        public List<PBI_GROUP> DETAIL_GROUP(PBI_GROUP MODEL)
        {
            DataSet DT1 = new DataSet();
            string storedProcedureName = "DETAIL_GROUP";
            using (SqlConnection connection = new SqlConnection(CONNECTION))
            {
                using (SqlCommand command = new SqlCommand(storedProcedureName, connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Connection.Open();
                    command.Parameters.AddWithValue("@ID", MODEL.ID);
                    SqlDataAdapter data = new SqlDataAdapter(command);
                    data.Fill(DT1);
                    command.CommandTimeout = Timeout;
                    command.Connection.Close();
                }
            }
            var DATA = (from row in DT1.Tables[1].AsEnumerable()
                        select new PBI_GROUP()
                        {
                            ID = row["ID"].ToString(),
                            GROUP_ID = row["GROUP_ID"].ToString(),
                            GROUP_NAME = row["GROUP_NAME"].ToString(),
                            GROUP_DESC = row["GROUP_DESC"].ToString(),
                            ALLOW_CREATE = row["ALLOW_CREATE"].ToString(),
                            ALLOW_SHARE = row["ALLOW_SHARE"].ToString(),
                            ALLOW_VIEW = row["ALLOW_VIEW"].ToString(),
                            ALLOW_EXPORT = row["ALLOW_EXPORT"].ToString(),
                            ACTIVE = row["ACTIVE"].ToString(),
                            CREATE_USER = row["CREATE_USER"].ToString(),
                            CREATE_DATE = row["CREATE_DATE"].ToString(),
                            UPDATE_USER = row["UPDATE_USER"].ToString(),
                            UPDATE_DATE = row["UPDATE_DATE"].ToString(),
                        }).ToList();
            return DATA;
        }
        public PP<RESPONSE> EDIT_GROUP(PBI_GROUP MODEL)
        {
            DataTable DT1 = new DataTable();
            string storedProcedureName = "EDIT_GROUP";
            using (SqlConnection connection = new SqlConnection(CONNECTION))
            {
                using (SqlCommand command = new SqlCommand(storedProcedureName, connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Connection.Open();
                    command.Parameters.AddWithValue("@ID", MODEL.ID);
                    command.Parameters.AddWithValue("@GROUP_ID", MODEL.GROUP_ID);
                    command.Parameters.AddWithValue("@GROUP_NAME", MODEL.GROUP_NAME);
                    command.Parameters.AddWithValue("@GROUP_DESC", MODEL.GROUP_DESC);
                    command.Parameters.AddWithValue("@ALLOW_CREATE", MODEL.ALLOW_CREATE);
                    command.Parameters.AddWithValue("@ALLOW_SHARE", MODEL.ALLOW_SHARE);
                    command.Parameters.AddWithValue("@ALLOW_VIEW", MODEL.ALLOW_VIEW);
                    command.Parameters.AddWithValue("@ALLOW_EXPORT", MODEL.ALLOW_EXPORT);
                    command.Parameters.AddWithValue("@ACTIVE", MODEL.ACTIVE);
                    command.Parameters.AddWithValue("@USER", COOKIES.GetCookies("Name").ToUpper());

                    SqlDataAdapter data = new SqlDataAdapter(command);
                    data.Fill(DT1);
                    command.CommandTimeout = Timeout;
                    command.Connection.Close();
                }
            }
            return new PP<RESPONSE>
            {
                Result = Convert.ToBoolean(DT1.Rows[0]["RESULT"]),
                Message = DT1.Rows[0]["MESSAGE"].ToString(),
                StatusCode = 200,
                Data = null
            };
        }
        public PP<RESPONSE> DELETE_GROUP(PBI_GROUP MODEL)
        {
            DataTable DT1 = new DataTable();
            string storedProcedureName = "DELETE_GROUP";
            using (SqlConnection connection = new SqlConnection(CONNECTION))
            {
                using (SqlCommand command = new SqlCommand(storedProcedureName, connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Connection.Open();
                    command.Parameters.AddWithValue("@ID", MODEL.ID);
                    SqlDataAdapter data = new SqlDataAdapter(command);
                    data.Fill(DT1);
                    command.CommandTimeout = Timeout;
                    command.Connection.Close();
                }
            }
            return new PP<RESPONSE>
            {
                Result = Convert.ToBoolean(DT1.Rows[0]["RESULT"]),
                Message = DT1.Rows[0]["MESSAGE"].ToString(),
                StatusCode = 200,
                Data = null
            };
        }
        public List<PBI_USER_DC> GET_USER_DC()
        {
            DataTable DT1 = new DataTable();
            string storedProcedureName = "GET_USER_DC";
            using (SqlConnection connection = new SqlConnection(CONNECTION))
            {
                using (SqlCommand command = new SqlCommand(storedProcedureName, connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Connection.Open();
                    SqlDataAdapter data = new SqlDataAdapter(command);
                    data.Fill(DT1);
                    command.CommandTimeout = Timeout;
                    command.Connection.Close();
                }
            }
            var DATA = (from row in DT1.AsEnumerable()
                        select new PBI_USER_DC()
                        {
                            samaccountname = row["samaccountname"].ToString(),
                            givenname = row["givenname"].ToString(),
                            surname = row["surname"].ToString(),
                            mail = row["mail"].ToString(),
                            telephonenumber = row["telephonenumber"].ToString(),
                            department = row["department"].ToString(),
                            pbi_group = row["pbi_group"].ToString(),
                            company = row["company"].ToString(),
                            country = row["country"].ToString(),
                            userprincipalname = row["userprincipalname"].ToString(),
                            lastlogontimestamp = row["lastlogontimestamp"].ToString(),
                            whencreated = row["whencreated"].ToString(),
                            whenchanged = row["whenchanged"].ToString(),
                        }).ToList();
            return DATA;
        }
        public List<PBI_USER> DETAIL_USER_DC(PBI_USER_DC MODEL)
        {
            DataTable DT1 = new DataTable();
            string storedProcedureName = "DETAIL_USER_DC";
            using (SqlConnection connection = new SqlConnection(CONNECTION))
            {
                using (SqlCommand command = new SqlCommand(storedProcedureName, connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Connection.Open();
                    command.Parameters.AddWithValue("@ID", MODEL.samaccountname);
                    SqlDataAdapter data = new SqlDataAdapter(command);
                    data.Fill(DT1);
                    command.CommandTimeout = Timeout;
                    command.Connection.Close();
                }
            }
            var DATA = (from row in DT1.AsEnumerable()
                        select new PBI_USER()
                        {
                            WINDOWSID = row["WINDOWSID"].ToString(),
                            GROUPS_PBI = row["GROUPID"].ToString(),

                        }).ToList();
            return DATA;
        }
        public PP<RESPONSE> UPDATE_USER_GROUP(PBI_USER MODEL)
        {
            DataTable DT1 = new DataTable();
            string storedProcedureName = "UPDATE_USER_GROUP";
            using (SqlConnection connection = new SqlConnection(CONNECTION))
            {
                using (SqlCommand command = new SqlCommand(storedProcedureName, connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Connection.Open();
                    command.Parameters.AddWithValue("@WINDOWSID", MODEL.WINDOWSID);
                    command.Parameters.AddWithValue("@GROUPID", MODEL.GROUPS_PBI);
                    command.Parameters.AddWithValue("@USER", COOKIES.GetCookies("NAME"));
                    SqlDataAdapter data = new SqlDataAdapter(command);
                    data.Fill(DT1);
                    command.CommandTimeout = Timeout;
                    command.Connection.Close();
                }
            }
            return new PP<RESPONSE>
            {
                Result = Convert.ToBoolean(DT1.Rows[0]["RESULT"]),
                Message = DT1.Rows[0]["MESSAGE"].ToString(),
                StatusCode = 200,
                Data = null
            };
        }
        public List<PBI_USER> GET_USER_INFO(string USERID)
        {
            DataSet DT1 = new DataSet();
            string storedProcedureName = "GET_USER_INFO";
            using (SqlConnection connection = new SqlConnection(CONNECTION))
            {
                using (SqlCommand command = new SqlCommand(storedProcedureName, connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Connection.Open();
                    command.Parameters.AddWithValue("@ID", USERID);
                    SqlDataAdapter data = new SqlDataAdapter(command);
                    data.Fill(DT1);
                    command.CommandTimeout = Timeout;
                    command.Connection.Close();
                }
            }
            var DATA1 = (from row in DT1.Tables[1].AsEnumerable()
                         select new PBI_USER()
                         {
                             PLANT = row["PLANT"].ToString(),
                             USERID = row["USERID"].ToString(),
                             WINDOWSID = row["WINDOWSID"].ToString(),
                             FULL_NAME = row["FULL_NAME"].ToString(),
                             DEPT = row["DEPT"].ToString(),
                             EMAIL = row["EMAIL"].ToString(),
                             GROUPS_MDM = row["GROUPS_MDM"].ToString(),
                             GROUPS_PBI = row["GROUPS_PBI"].ToString(),
                         }).ToList();

            return DATA1;
        }
        public PP<RESPONSE> UPDATE_TOTAL_VIEW(PBI_REPORT MODEL, string URL_TYPE)
        {
            DataTable DT1 = new DataTable();
            string storedProcedureName = "UPDATE_TOTAL_VIEW";
            using (SqlConnection connection = new SqlConnection(CONNECTION))
            {
                using (SqlCommand command = new SqlCommand(storedProcedureName, connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Connection.Open();
                    command.Parameters.AddWithValue("@ID", MODEL.ID);
                    command.Parameters.AddWithValue("@URL_TYPE", URL_TYPE);
                    SqlDataAdapter data = new SqlDataAdapter(command);
                    data.Fill(DT1);
                    command.CommandTimeout = Timeout;
                    command.Connection.Close();
                }
            }
            return new PP<RESPONSE>
            {
                Result = Convert.ToBoolean(DT1.Rows[0]["RESULT"]),
                Message = DT1.Rows[0]["MESSAGE"].ToString(),
                StatusCode = 200,
                Data = null
            };
        }
        public List<PBI_REPORT> UPDATE_TOTAL_VIEW_NEW(PBI_REPORT MODEL, string URL_TYPE)
        {
            DataSet DT1 = new DataSet();
            string storedProcedureName = "UPDATE_TOTAL_VIEW_NEW";
            using (SqlConnection connection = new SqlConnection(CONNECTION))
            {
                using (SqlCommand command = new SqlCommand(storedProcedureName, connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Connection.Open();
                    command.Parameters.AddWithValue("@ID", MODEL.ID);
                    command.Parameters.AddWithValue("@URL_TYPE", URL_TYPE);
                    SqlDataAdapter data = new SqlDataAdapter(command);
                    data.Fill(DT1);
                    command.CommandTimeout = Timeout;
                    command.Connection.Close();
                }
            }
            var DATA = (from row in DT1.Tables[1].AsEnumerable()
                        select new PBI_REPORT()
                        {
                            REPORT_URL = row["REPORT_URL"].ToString(),
                        }).ToList();
            return DATA;
        }
        public PP<RESPONSE> LOG_URL(string REPORT_ID, string IP, string MACHINE)
        {
            DataTable DT1 = new DataTable();
            string storedProcedureName = "LOG_URL";
            using (SqlConnection connection = new SqlConnection(CONNECTION))
            {
                using (SqlCommand command = new SqlCommand(storedProcedureName, connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Connection.Open();
                    command.Parameters.AddWithValue("@REPORT_ID", REPORT_ID);
                    command.Parameters.AddWithValue("@IP", IP);
                    command.Parameters.AddWithValue("@MACHINE", MACHINE);
                    SqlDataAdapter data = new SqlDataAdapter(command);
                    data.Fill(DT1);
                    command.CommandTimeout = Timeout;
                    command.Connection.Close();
                }
            }
            return new PP<RESPONSE>
            {
                Result = Convert.ToBoolean(DT1.Rows[0]["RESULT"]),
                Message = DT1.Rows[0]["MESSAGE"].ToString(),
                StatusCode = 200,
                Data = null
            };
        }
        public List<PBI_REPORT> GET_RECENT()
        {
            DataTable DT1 = new DataTable();
            string storedProcedureName = "GET_RECENT";
            using (SqlConnection connection = new SqlConnection(CONNECTION))
            {
                using (SqlCommand command = new SqlCommand(storedProcedureName, connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Connection.Open();
                    command.Parameters.AddWithValue("@USERID", COOKIES.GetCookies("USERID"));
                    SqlDataAdapter data = new SqlDataAdapter(command);
                    data.Fill(DT1);
                    command.CommandTimeout = Timeout;
                    command.Connection.Close();
                }
            }
            var DATA = (from row in DT1.AsEnumerable()
                        select new PBI_REPORT()
                        {
                            ID = row["ID"].ToString(),
                            REPORT_NAME = row["REPORT_NAME"].ToString(),
                            REPORT_DESC = row["REPORT_DESC"].ToString(),
                            REPORT_URL = row["REPORT_URL"].ToString(),
                            REPORT_DEPT = row["REPORT_DEPT"].ToString(),
                            REPORT_CATEGORY = row["REPORT_CATEGORY"].ToString(),
                            REPORT_VISIBLE = row["REPORT_VISIBLE"].ToString(),
                            TOTAL_VIEW = row["TOTAL_VIEW"].ToString(),
                            CREATE_USER = row["CREATE_USER"].ToString(),
                            CREATE_DATE = row["CREATE_DATE"].ToString(),
                            UPDATE_USER = row["UPDATE_USER"].ToString(),
                            UPDATE_DATE = row["UPDATE_DATE"].ToString(),
                            TAG_DEPT = row["TAG_DEPT"].ToString(),
                        }).ToList();
            return DATA;
        }
        public List<MENU_LIST> GET_MENU_LIST()
        {
            DataTable DT1 = new DataTable();
            string query = @"EXEC [GET_MENU_LIST] @USERID";
            using (SqlConnection connection = new SqlConnection(CONNECTION))
            {
                SqlCommand command = new SqlCommand(query, connection);
                command.Connection.Open();
                command.CommandType = CommandType.Text;
                command.Parameters.AddWithValue("@USERID", COOKIES.GetCookies("USERID"));
                SqlDataAdapter data = new SqlDataAdapter(command);
                data.Fill(DT1);
                command.CommandTimeout = Timeout;
                command.Connection.Close();
            }
            var DATA = (from row in DT1.AsEnumerable()
                        select new MENU_LIST()
                        {
                            SYSTEMNAME = row["SYSTEMNAME"].ToString(),
                            CODE = row["CODE"].ToString(),
                            NAME = row["NAME"].ToString(),
                            PARENT = row["PARENT"].ToString(),
                            CONTROLLER = row["CONTROLLER"].ToString(),
                            ACTION = row["ACTION"].ToString(),
                            PARAMETER = row["PARAMETER"].ToString(),
                            PICTURE = row["PICTURE"].ToString(),
                            ICON = row["ICON"].ToString(),
                            TARGET = row["TARGET"].ToString(),
                            METHOD = row["METHOD"].ToString(),
                            ADDITIONALINFO = row["ADDITIONALINFO"].ToString(),
                            SEQUENCE = row["SEQUENCE"].ToString(),
                            STATUS = row["STATUS"].ToString(),
                            CREATEUSER = row["CREATEUSER"].ToString(),
                            CREATEDATE = row["CREATEDATE"].ToString(),
                            CHANGEUSER = row["CHANGEUSER"].ToString(),
                            CHANGEDATE = row["CHANGEDATE"].ToString(),
                        }).ToList();
            return DATA;
        }
        public PP<RESPONSE> SYNC_DEPARTMENT()
        {
            DataTable DT1 = new DataTable();
            string storedProcedureName = "SYNC_DEPARTMENT";
            using (SqlConnection connection = new SqlConnection(CONNECTION))
            {
                using (SqlCommand command = new SqlCommand(storedProcedureName, connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Connection.Open();
                    SqlDataAdapter data = new SqlDataAdapter(command);
                    data.Fill(DT1);
                    command.CommandTimeout = Timeout;
                    command.Connection.Close();
                }
            }
            return new PP<RESPONSE>
            {
                Result = Convert.ToBoolean(DT1.Rows[0]["RESULT"]),
                Message = DT1.Rows[0]["MESSAGE"].ToString(),
                StatusCode = 200,
                Data = null
            };
        }
        public PP<List<PBI_SHARE_URL>> INSERT_PBI_SHARE_URL(string REPORT_NAME, string REPORT_ID, string URL_TYPE, string URL_TXT, string URL_EXPI, string URL_PASS)
        {
            DataSet DT1 = new DataSet();
            string storedProcedureName = "INSERT_PBI_SHARE_URL";
            using (SqlConnection connection = new SqlConnection(CONNECTION))
            {
                using (SqlCommand command = new SqlCommand(storedProcedureName, connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Connection.Open();
                    command.Parameters.AddWithValue("@REPORT_NAME", REPORT_NAME);
                    command.Parameters.AddWithValue("@REPORT_ID", REPORT_ID);
                    command.Parameters.AddWithValue("@URL_TYPE", URL_TYPE);
                    command.Parameters.AddWithValue("@URL_EXPI", URL_EXPI);
                    command.Parameters.AddWithValue("@URL_TXT", URL_TXT);
                    command.Parameters.AddWithValue("@URL_PASS", URL_PASS);
                    command.Parameters.AddWithValue("@USER", COOKIES.GetCookies("USERID"));
                    SqlDataAdapter data = new SqlDataAdapter(command);
                    data.Fill(DT1);
                    command.CommandTimeout = Timeout;
                    command.Connection.Close();
                }
            }
            if (Convert.ToBoolean(DT1.Tables[0].Rows[0]["RESULT"]))
            {
                var DATA = (from row in DT1.Tables[1].AsEnumerable()
                            select new PBI_SHARE_URL()
                            {
                                ID = row["ID"].ToString(),
                                REPORT_ID = row["REPORT_ID"].ToString(),
                                URL_TYPE = row["URL_TYPE"].ToString(),
                                URL_TEXT = row["URL_TEXT"].ToString(),
                                URL_PASS = row["URL_PASS"].ToString()
                            }).ToList();

                return new PP<List<PBI_SHARE_URL>>
                {
                    Result = Convert.ToBoolean(DT1.Tables[0].Rows[0]["RESULT"]),
                    Message = DT1.Tables[0].Rows[0]["MESSAGE"].ToString(),
                    StatusCode = 200,
                    Data = DATA
                };
            }
            else
            {
                return new PP<List<PBI_SHARE_URL>>
                {
                    Result = Convert.ToBoolean(DT1.Tables[0].Rows[0]["RESULT"]),
                    Message = DT1.Tables[0].Rows[0]["MESSAGE"].ToString(),
                    StatusCode = 400,
                    Data = null
                };
            }
        }
        public List<PBI_SHARE_URL> GET_PBI_SHARE_URL(string URL_TYPE, string URL_TXT, string URL_PASS)
        {
            DataSet DT1 = new DataSet();
            string storedProcedureName = "GET_PBI_SHARE_URL";
            using (SqlConnection connection = new SqlConnection(CONNECTION))
            {
                using (SqlCommand command = new SqlCommand(storedProcedureName, connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Connection.Open();
                    command.Parameters.AddWithValue("@URL_TYPE", URL_TYPE);
                    command.Parameters.AddWithValue("@URL_TXT", URL_TXT);
                    command.Parameters.AddWithValue("@URL_PASS", (URL_PASS ?? ""));
                    SqlDataAdapter data = new SqlDataAdapter(command);
                    data.Fill(DT1);
                    command.CommandTimeout = Timeout;
                    command.Connection.Close();
                }
            }
            var DATA = (from row in DT1.Tables[0].AsEnumerable()
                        select new PBI_SHARE_URL()
                        {
                            ID = row["ID"].ToString(),
                            REPORT_ID = row["REPORT_ID"].ToString(),
                            URL_TYPE = row["URL_TYPE"].ToString(),
                            URL_TEXT = row["URL_TEXT"].ToString(),
                        }).ToList();
            return DATA;
        }
        public List<PBI_SHARE_URL> GET_MY_LINK()
        {
            DataTable DT1 = new DataTable();
            string storedProcedureName = "GET_MY_LINK";
            using (SqlConnection connection = new SqlConnection(CONNECTION))
            {
                using (SqlCommand command = new SqlCommand(storedProcedureName, connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Connection.Open();
                    command.Parameters.AddWithValue("@USER", COOKIES.GetCookies("USERID"));
                    SqlDataAdapter data = new SqlDataAdapter(command);
                    data.Fill(DT1);
                    command.CommandTimeout = Timeout;
                    command.Connection.Close();
                }
            }
            var DATA = (from row in DT1.AsEnumerable()
                        select new PBI_SHARE_URL()
                        {
                            ID = row["ID"].ToString(),
                            REPORT_NAME = row["REPORT_NAME"].ToString(),
                            URL_TEXT = row["URL_TEXT"].ToString(),
                            URL_EXPI = row["URL_EXPI"].ToString(),
                            URL_TYPE = row["URL_TYPE"].ToString(),
                        }).ToList();
            return DATA;
        }
        public PP<RESPONSE> EDIT_TIME(PBI_SHARE_URL MODEL)
        {
            DataTable DT1 = new DataTable();
            string storedProcedureName = "EDIT_TIME";
            using (SqlConnection connection = new SqlConnection(CONNECTION))
            {
                using (SqlCommand command = new SqlCommand(storedProcedureName, connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Connection.Open();
                    command.Parameters.AddWithValue("@ID", MODEL.ID);
                    command.Parameters.AddWithValue("@URL_EXPI", MODEL.URL_EXPI);
                    SqlDataAdapter data = new SqlDataAdapter(command);
                    data.Fill(DT1);
                    command.CommandTimeout = Timeout;
                    command.Connection.Close();
                }
            }
            return new PP<RESPONSE>
            {
                Result = Convert.ToBoolean(DT1.Rows[0]["RESULT"]),
                Message = DT1.Rows[0]["MESSAGE"].ToString(),
                StatusCode = 200,
                Data = null
            };
        }
        public List<PBI_SHARE_URL> GET_MY_LOG_LINK()
        {
            DataTable DT1 = new DataTable();
            string storedProcedureName = "GET_LOG_URL";
            using (SqlConnection connection = new SqlConnection(CONNECTION))
            {
                using (SqlCommand command = new SqlCommand(storedProcedureName, connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Connection.Open();
                    command.Parameters.AddWithValue("@USER", COOKIES.GetCookies("USERID"));
                    SqlDataAdapter data = new SqlDataAdapter(command);
                    data.Fill(DT1);
                    command.CommandTimeout = Timeout;
                    command.Connection.Close();
                }
            }
            var DATA = (from row in DT1.AsEnumerable()
                        select new PBI_SHARE_URL()
                        {
                            ID = row["ID"].ToString(),
                            REPORT_NAME = row["REPORT_NAME"].ToString(),
                            URL_TEXT = row["URL_TEXT"].ToString(),
                            URL_EXPI = row["URL_EXPI"].ToString(),
                            URL_TYPE = row["URL_TYPE"].ToString(),
                        }).ToList();
            return DATA;
        }
        public bool RETEIVE_USER_DC()
        {
            try
            {
                var A = SYNC_USER_DC();
                if (A.Result == true)
                {
                    DataTable tblPBIUserDC = new DataTable("TBL_PBI_USER_DC");
                    tblPBIUserDC.Columns.AddRange(new[]
                    {
                        new DataColumn("samaccountname", typeof(string)),
                        new DataColumn("givenname", typeof(string)),
                        new DataColumn("surname", typeof(string)),
                        new DataColumn("mail", typeof(string)),
                        new DataColumn("telephonenumber", typeof(string)),
                        new DataColumn("department", typeof(string)),
                        new DataColumn("company", typeof(string)),
                        new DataColumn("country", typeof(string)),
                        new DataColumn("userprincipalname", typeof(string)),
                        new DataColumn("lastlogontimestamp", typeof(DateTime)),
                        new DataColumn("whencreated", typeof(DateTime)),
                        new DataColumn("whenchanged", typeof(DateTime))
                    });
                    using (DirectoryEntry entry = new DirectoryEntry("LDAP://" + "SHIMANOACE", "", ""))
                    {
                        using (DirectorySearcher searcher = new DirectorySearcher(entry))
                        {
                            searcher.Filter = "(objectClass=user)";
                            searcher.PropertiesToLoad.AddRange(new string[] {
                            "sAMAccountName", "givenName", "sn", "mail", "telephoneNumber",
                            "department", "company", "co", "userPrincipalName",
                            "lastLogonTimestamp", "whenCreated", "whenChanged"
                        });
                            searcher.PageSize = 1000;
                            searcher.SizeLimit = 0;
                            int totalCount = 0;
                            SearchResultCollection results;
                            do
                            {
                                results = searcher.FindAll();

                                foreach (SearchResult result in results)
                                {
                                    var row = tblPBIUserDC.NewRow();
                                    row["sAMAccountName"] = GetStringProperty(result, "sAMAccountName");
                                    row["givenName"] = GetStringProperty(result, "givenName");
                                    row["surname"] = GetStringProperty(result, "sn");
                                    row["mail"] = GetStringProperty(result, "mail");
                                    row["telephoneNumber"] = GetStringProperty(result, "telephoneNumber");
                                    row["department"] = GetStringProperty(result, "department");
                                    row["company"] = GetStringProperty(result, "company");
                                    row["country"] = GetStringProperty(result, "co");
                                    row["userPrincipalName"] = GetStringProperty(result, "userPrincipalName");
                                    row["lastLogonTimestamp"] = GetDateTimeProperty(result, "lastLogonTimestamp");
                                    row["whenCreated"] = GetStringProperty(result, "whenCreated");
                                    row["whenChanged"] = GetStringProperty(result, "whenChanged");

                                    tblPBIUserDC.Rows.Add(row);
                                    totalCount++;
                                }
                                searcher.FindAll().Dispose();
                            } while (results.Count == searcher.PageSize);
                        }
                    }
                    string tableName = "PBI_USER_DC";
                    string connectionString = ConfigurationManager.ConnectionStrings["CONNECTION"].ToString();
                    BulkInsert(tblPBIUserDC, connectionString, tableName);
                    return true;
                }
                return A.Result;
            }
            catch (Exception ex)
            {
                return false;
            }
        }
        public PP<RESPONSE> SYNC_USER_DC()
        {
            DataTable DT1 = new DataTable();
            string storedProcedureName = "SYNC_USER_DC";
            using (SqlConnection connection = new SqlConnection(CONNECTION))
            {
                using (SqlCommand command = new SqlCommand(storedProcedureName, connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Connection.Open();
                    SqlDataAdapter data = new SqlDataAdapter(command);
                    data.Fill(DT1);
                    command.CommandTimeout = Timeout;
                    command.Connection.Close();
                }
            }
            return new PP<RESPONSE>
            {
                Result = Convert.ToBoolean(DT1.Rows[0]["RESULT"]),
                Message = DT1.Rows[0]["MESSAGE"].ToString(),
                StatusCode = 200,
                Data = null
            };
        }
        public List<PBI_GROUP> GET_PERMISSION()
        {
            DataSet DT1 = new DataSet();
            string storedProcedureName = "GET_PERMISSION";
            using (SqlConnection connection = new SqlConnection(CONNECTION))
            {
                using (SqlCommand command = new SqlCommand(storedProcedureName, connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Connection.Open();
                    command.Parameters.AddWithValue("@USERID", (COOKIES.GetCookies("USERID") ?? ""));
                    SqlDataAdapter data = new SqlDataAdapter(command);
                    data.Fill(DT1);
                    command.CommandTimeout = Timeout;
                    command.Connection.Close();
                }
            }
            var DATA = (from row in DT1.Tables[0].AsEnumerable()
                        select new PBI_GROUP()
                        {
                            ALLOW_CREATE = row["ALLOW_CREATE"].ToString(),
                            ALLOW_EXPORT = row["ALLOW_EXPORT"].ToString(),
                            ALLOW_SHARE = row["ALLOW_SHARE"].ToString(),
                            ALLOW_VIEW = row["ALLOW_VIEW"].ToString(),
                        }).ToList();
            return DATA;
        }
        static void BulkInsert(DataTable dataTable, string connectionString, string tableName)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                using (SqlBulkCopy bulkCopy = new SqlBulkCopy(connection))
                {
                    bulkCopy.DestinationTableName = tableName;
                    foreach (DataColumn column in dataTable.Columns)
                    {
                        bulkCopy.ColumnMappings.Add(column.ColumnName, column.ColumnName);
                    }
                    bulkCopy.BulkCopyTimeout = 60;
                    bulkCopy.BatchSize = 1000;
                    bulkCopy.WriteToServer(dataTable);
                }
            }
        }
        private string GetStringProperty(SearchResult result, string propertyName)
        {
            if (result.Properties.Contains(propertyName) && result.Properties[propertyName].Count > 0)
            {
                return result.Properties[propertyName][0].ToString();
            }
            return string.Empty;
        }
        private object GetDateTimeProperty(SearchResult result, string propertyName)
        {
            if (result.Properties.Contains(propertyName) && result.Properties[propertyName].Count > 0)
            {
                string dateString = result.Properties[propertyName][0].ToString();
                if (DateTime.TryParseExact(dateString, "yyyyMMddHHmmss.0Z", CultureInfo.InvariantCulture, DateTimeStyles.AssumeUniversal, out DateTime parsedDateTime))
                {
                    return parsedDateTime;
                }
                else
                {
                    string fileTimeString = dateString;
                    long fileTime;
                    if (long.TryParse(fileTimeString, out fileTime))
                    {
                        DateTime utcDateTime = DateTime.FromFileTime(fileTime);
                        DateTime localDateTime = utcDateTime.ToLocalTime();
                        return localDateTime;
                    }
                }
            }
            return DBNull.Value;
        }
        public class RESPONSE
        {
            public bool RESULT { get; set; }
            public string MESSAGE { get; set; }
            public int STATUSCODE { get; set; }
        }
        public class PP<T>
        {
            public T Data { get; set; }
            public bool Result { get; set; }
            public string Message { get; set; }
            public int StatusCode { get; set; }
        }
    }
}