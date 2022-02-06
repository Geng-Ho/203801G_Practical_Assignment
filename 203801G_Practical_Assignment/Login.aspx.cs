using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Net;
using System.Security.Cryptography;
using System.Text;
using System.Web;
using System.Web.Script.Serialization;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace _203801G_Practical_Assignment
{
    public partial class Login : System.Web.UI.Page
    {
        string MYDBConnectionString = System.Configuration.ConfigurationManager.ConnectionStrings["MYDBConnection"].ConnectionString;
        static string errorMsg = "";
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void btn_register_click(object sender, EventArgs e)
        {
            Response.Redirect("Registration.aspx", false);
        }

        protected void btn_submit_click(object sender, EventArgs e)
        {
            if (tb_userid.Text.Length > 0 && tb_pwd.Text.Length >0)
            {
                SqlConnection connection = new SqlConnection(MYDBConnectionString);

                connection.Open();
                string pwd = tb_pwd.Text.ToString().Trim();
                string email = tb_userid.Text.ToString().Trim();

                SHA512Managed hashing = new SHA512Managed();
                string dbHash = getDBHash(email);
                string dbSalt = getDBSalt(email);
                try
                {
                    if (dbSalt != null && dbSalt.Length > 0 && dbHash != null && dbHash.Length > 0)
                    {
                        string pwdWithSalt = pwd + dbSalt;
                        byte[] hashWithSalt = hashing.ComputeHash(Encoding.UTF8.GetBytes(pwdWithSalt));
                        string userHash = Convert.ToBase64String(hashWithSalt);
                        string sqlMain = "select AccountStatus FROM UserAccount WHERE Email=@Email";
                        SqlCommand command = new SqlCommand(sqlMain, connection);
                        command.Parameters.AddWithValue("@Email", email);
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                if (reader["AccountStatus"].ToString() == "Active")
                                {
                                    if (userHash.Equals(dbHash))
                                    {
                                        string sqlsuccess = "UPDATE UserAccount SET LoginAttempt = 0 WHERE Email=@Email";
                                        SqlCommand commandsuccess = new SqlCommand(sqlsuccess, connection);
                                        commandsuccess.Parameters.AddWithValue("@Email", email);
                                        int updated = commandsuccess.ExecuteNonQuery();
                                        Session["LoggedIn"] = email;
                                        string guid = Guid.NewGuid().ToString();
                                        Session["AuthToken"] = guid;
                                        Response.Cookies.Add(new HttpCookie("AuthToken", guid));
                                        Response.Redirect("HomePage.aspx", false);

                                    }
                                    else
                                    {

                                        string sql = "select LoginAttempt FROM UserAccount WHERE Email=@Email";
                                        string sql1 = "UPDATE UserAccount SET LoginAttempt = 1 WHERE Email=@Email";
                                        string sql2 = "UPDATE UserAccount SET LoginAttempt = 2 WHERE Email=@Email";
                                        string sql3 = "UPDATE UserAccount SET AccountStatus = 'LockedOut' WHERE Email=@Email";
                                        SqlCommand command1 = new SqlCommand(sql, connection);
                                        command1.Parameters.AddWithValue("@Email", email);
                                        using (SqlDataReader reader1 = command1.ExecuteReader())
                                        {
                                            while (reader1.Read())
                                            {
                                                if (Convert.ToInt32(reader1["LoginAttempt"]) < 2)
                                                {
                                                    if (Convert.ToInt32(reader1["LoginAttempt"]) == 0)
                                                    {
                                                        SqlCommand command2 = new SqlCommand(sql1, connection);
                                                        command2.Parameters.AddWithValue("@Email", email);
                                                        int updated = command2.ExecuteNonQuery();
                                                        lb_error.Text = "Email or Password Incorrect, Please try again!";
                                                    }
                                                    else if (Convert.ToInt32(reader1["LoginAttempt"]) == 1)
                                                    {
                                                        SqlCommand command3 = new SqlCommand(sql2, connection);
                                                        command3.Parameters.AddWithValue("@Email", email);
                                                        int updated = command3.ExecuteNonQuery();
                                                        lb_error.Text = "Email or Password Incorrect, Please try again!";
                                                    }
                                                }
                                                else
                                                {
                                                    SqlCommand command4 = new SqlCommand(sql3, connection);
                                                    command4.Parameters.AddWithValue("@Email", email);
                                                    int updated = command4.ExecuteNonQuery();
                                                    lb_error.Text = "Account Locked, Please contact an administrator!";
                                                }
                                            }
                                        }

                                    }
                                }
                                else
                                {
                                    lb_error.Text = "Account Locked, Please contact an administrator!";
                                }

                            }

                        }

                    }
                }
                catch (Exception ex)
                {
                    throw new Exception(ex.ToString());
                }

                finally { connection.Close(); }
            }
            else
            {
                lb_error.Text = "Please enter an email and password";
            }
            
        }

        protected string getDBSalt(string email)
        {

            string s = null;

            SqlConnection connection = new SqlConnection(MYDBConnectionString);
            string sql = "select PasswordSalt FROM UserAccount WHERE Email=@Email";
            SqlCommand command = new SqlCommand(sql, connection);
            command.Parameters.AddWithValue("@Email", email);

            try
            {
                connection.Open();

                using (SqlDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        if (reader["PASSWORDSALT"] != null)
                        {
                            if (reader["PASSWORDSALT"] != DBNull.Value)
                            {
                                s = reader["PASSWORDSALT"].ToString();
                            }
                        }
                    }
                }

            }
            catch (Exception ex)
            {
                throw new Exception(ex.ToString());
            }

            finally { connection.Close(); }
            return s;

        }

        protected string getDBHash(string email)
        {

            string h = null;

            SqlConnection connection = new SqlConnection(MYDBConnectionString);
            string sql = "select PasswordHash FROM UserAccount WHERE Email=@Email";
            SqlCommand command = new SqlCommand(sql, connection);
            command.Parameters.AddWithValue("@Email", email);

            try
            {
                connection.Open();

                using (SqlDataReader reader = command.ExecuteReader())
                {

                    while (reader.Read())
                    {
                        if (reader["PasswordHash"] != null)
                        {
                            if (reader["PasswordHash"] != DBNull.Value)
                            {
                                h = reader["PasswordHash"].ToString();
                            }
                        }
                    }

                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.ToString());
            }

            finally { connection.Close(); }
            return h;
        }

        protected string decryptData(byte[] cipherText)
        {

            string decryptedString = null;
            //byte[] cipherText = Convert.FromBase64String(cipherString);

            try
            {
                RijndaelManaged cipher = new RijndaelManaged();
                ICryptoTransform decryptTransform = cipher.CreateDecryptor();

                //Decrypt
                //byte[] decryptedText = decryptTransform.TransformFinalBlock(cipherText, 0, cipherText.Length);
                //decryptedString = Encoding.UTF8.GetString(decryptedText);


            }
            catch (Exception ex)
            {
                throw new Exception(ex.ToString());
            }

            finally { }
            return decryptedString;
        }

        public class MyObject
        {
            public string success { get; set; }

            public List<string> ErrorMessage { get; set; }
        }

        public bool ValidateCaptcha()
        {
            bool result = true;

            string captchaResponse = Request.Form["g-recaptcha-response"];

            HttpWebRequest req = (HttpWebRequest)WebRequest.Create("https://www.google.com/recaptcha/api/siteverify?secret=6LfoDGAeAAAAABEqaSO6APz9McEcLt50dM5dbmD0 &response=" + captchaResponse);

            try
            {
                using (WebResponse wResponse = req.GetResponse())
                {
                    using (StreamReader readStream = new StreamReader(wResponse.GetResponseStream()))
                    {
                        string jsonResponse = readStream.ReadToEnd();
                        JavaScriptSerializer js = new JavaScriptSerializer();
                        MyObject jsonObject = js.Deserialize<MyObject>(jsonResponse);
                        result = Convert.ToBoolean(jsonObject.success);
                    }
                }
                return result;
            }
            catch (WebException ex)
            {
                throw ex;
            }
        }
    }
}