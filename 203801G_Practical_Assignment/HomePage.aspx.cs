using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace _203801G_Practical_Assignment
{
    public partial class Success : System.Web.UI.Page
    {
        string MYDBConnectionString = System.Configuration.ConfigurationManager.ConnectionStrings["MYDBConnection"].ConnectionString;
        static byte[] Key = null;
        static byte[] IV = null;
        static byte[] ccno = null;
        static byte[] ccexpmonth = null;
        static byte[] ccexpyear = null;
        static string email = null;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["LoggedIn"] != null && Session["AuthToken"] != null && Request.Cookies["AuthToken"] != null)
            {
                if (!Session["AuthToken"].ToString().Equals(Request.Cookies["AuthToken"].Value))
                {
                    Response.Redirect("Login.aspx", false);
                }
                else
                {
                    email = Session["LoggedIn"].ToString();
                    displayUserProfile(email);
                    
                }
            }
            else
            {
                Response.Redirect("Login.aspx", false);
            }
        }

        protected void LogOutClick(object sender, EventArgs e)
        {
            Session.Clear();
            Session.Abandon();
            Session.RemoveAll();

            Response.Redirect("Login.aspx", false);

            if (Request.Cookies["ASP.NET_SessionId"] != null)
            {
                Response.Cookies["ASP.NET_SessionId"].Value = string.Empty;
                Response.Cookies["ASP.NET_SessionId"].Expires = DateTime.Now.AddMonths(-20);
            }
            if (Request.Cookies["AuthToken"] != null)
            {
                Response.Cookies["AuthToken"].Value = string.Empty;
                Response.Cookies["AuthToken"].Expires = DateTime.Now.AddMonths(-20);
            }

            createLog();
        }

        protected string decryptData(byte[] cipherText)
        {
            string plainText = null;

            try
            {
                RijndaelManaged cipher = new RijndaelManaged();
                cipher.IV = IV;
                cipher.Key = Key;
                // Create a decrytor to perform the stream transform.
                ICryptoTransform decryptTransform = cipher.CreateDecryptor();
                using (MemoryStream msDecrypt = new MemoryStream(cipherText))
                {
                    using (CryptoStream csDecrypt = new CryptoStream(msDecrypt, decryptTransform, CryptoStreamMode.Read))
                    {
                        using (StreamReader srDecrypt = new StreamReader(csDecrypt))
                        {
                            plainText = srDecrypt.ReadToEnd();

                        }
                    }
                }
            }


            catch (Exception ex)
            {
                throw new Exception(ex.ToString());
            }
            finally { }
            return plainText;
        }

        protected void displayUserProfile(string email)
        {
            SqlConnection connection = new SqlConnection(MYDBConnectionString);
            string sql = "select * FROM UserAccount WHERE Email=@Email";
            SqlCommand command = new SqlCommand(sql, connection);
            command.Parameters.AddWithValue("@Email", email);

            try
            {
                connection.Open();
                using (SqlDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        if (reader["FirstName"] != DBNull.Value)
                        {
                            lbl_FirstName.Text = reader["FirstName"].ToString();
                        }
                        if (reader["LastName"] != DBNull.Value)
                        {
                            lbl_LastName.Text = reader["LastName"].ToString();
                        }
                        if (reader["CardNo"] != DBNull.Value)
                        {
                            ccno = Convert.FromBase64String(reader["CardNo"].ToString());
                        }
                        if (reader["CardExpiryMonth"] != DBNull.Value)
                        {
                            ccexpmonth = Convert.FromBase64String(reader["CardExpiryMonth"].ToString());
                        }
                        if (reader["CardExpiryYear"] != DBNull.Value)
                        {
                            ccexpyear = Convert.FromBase64String(reader["CardExpiryYear"].ToString());
                        }
                        if (reader["Email"] != DBNull.Value)
                        {
                            lbl_email.Text = reader["Email"].ToString();
                            //cipherTextNRIC = (byte[])reader["Nric"];
                        }
                        if (reader["DateOfBirth"] != DBNull.Value)
                        {
                            lbl_DOB.Text = reader["DateOfBirth"].ToString();
                        }
                        if (reader["Photo"] != DBNull.Value)
                        {
                            PhotoImg.ImageUrl = reader["Photo"].ToString();
                        }
                        if (reader["IV"] != DBNull.Value)
                        {
                            IV = Convert.FromBase64String(reader["IV"].ToString());

                            //cipherTextNRIC = (byte[])reader["Nric"];
                        }
                        if (reader["Key"] != DBNull.Value)
                        {
                            Key = Convert.FromBase64String(reader["Key"].ToString());

                            //cipherTextNRIC = (byte[])reader["Nric"];
                        }
                    }
                    string ccnostr = decryptData(ccno);
                    string substr = ccnostr.Substring(ccnostr.Length - 4);
                    lbl_ccNo.Text = substr;
                    lbl_ExpMonth.Text = decryptData(ccexpmonth);
                    lbl_ExpYear.Text = decryptData(ccexpyear);
                }

            }
            catch (Exception ex)
            {
                throw new Exception(ex.ToString());
            }

            finally
            {
                connection.Close();
            }
        }

        protected void createLog()
        {
            try
            {
                using (SqlConnection con = new SqlConnection(MYDBConnectionString))
                {
                    using (SqlCommand cmd = new SqlCommand("INSERT INTO AuditLog VALUES(@UserEmail, @DateTime, @Action)"))
                    {
                        using (SqlDataAdapter sda = new SqlDataAdapter())
                        {
                            cmd.CommandType = CommandType.Text;
                            cmd.Parameters.AddWithValue("@UserEmail", lbl_email.Text.Trim());
                            cmd.Parameters.AddWithValue("@DateTime", DateTime.Now);
                            cmd.Parameters.AddWithValue("@Action", "User Logout of Account");
                            cmd.Connection = con;
                            try
                            {
                                con.Open();
                                cmd.ExecuteNonQuery();
                                //con.Close();
                            }
                            catch (Exception ex)
                            {
                                //throw new Exception(ex.ToString());
                            }
                            finally
                            {
                                con.Close();
                            }
                        }

                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.ToString());
            }
        }
    }
}