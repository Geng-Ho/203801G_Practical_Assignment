using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Security.Cryptography;
using System.Text.RegularExpressions;
using System.Drawing;
using System.Text;
using System.Data.SqlClient;
using System.Data;
using System.Net;
using System.IO;
using System.Web.Script.Serialization;

namespace _203801G_Practical_Assignment
{
    public partial class Registration : System.Web.UI.Page
    {
        string MYDBConnectionString = System.Configuration.ConfigurationManager.ConnectionStrings["MYDBConnection"].ConnectionString;
        static string pwdfinalHash;
        static string pwdsalt;
        static string filestr;
        static string fileImg;
        byte[] Key;
        byte[] IV;

        static string line = "\r";

        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void btn_submit_click(object sender, EventArgs e)
        {
            filestr = PhotoUpload.FileName;
            PhotoUpload.PostedFile.SaveAs(Server.MapPath("~/Photo/" + filestr));
            fileImg = "~/Photo/" + filestr.ToString();
            
            string pwd = tb_pwd.Text.ToString().Trim(); ;

            RNGCryptoServiceProvider pwdrng = new RNGCryptoServiceProvider();
            byte[] pwdsaltByte = new byte[8];

            pwdrng.GetBytes(pwdsaltByte);
            pwdsalt = Convert.ToBase64String(pwdsaltByte);

            SHA512Managed pwdhashing = new SHA512Managed();

            string pwdWithSalt = pwd + pwdsalt;
            byte[] pwdplainHash = pwdhashing.ComputeHash(Encoding.UTF8.GetBytes(pwd));
            byte[] pwdhashWithSalt = pwdhashing.ComputeHash(Encoding.UTF8.GetBytes(pwdWithSalt));

            pwdfinalHash = Convert.ToBase64String(pwdhashWithSalt);

            RijndaelManaged cipher = new RijndaelManaged();
            cipher.GenerateKey();
            Key = cipher.Key;
            IV = cipher.IV;

            createAccount();
            if(lb_error1.Text == "")
            {
                Response.Redirect("Login.aspx", false);
            }
        }
        protected void createAccount()
        {

            try
            {
                using (SqlConnection con = new SqlConnection(MYDBConnectionString))
                {
                    using (SqlCommand cmd = new SqlCommand("INSERT INTO UserAccount VALUES(@FirstName, @LastName, @CardName, @CardNo, @CardExpiryMonth, @CardExpiryYear, @CardCVV, @Email, @PasswordHash, @PasswordSalt, @DateOfBirth, @Photo, @IV, @Key, @LoginAttempt, @AccountStatus)"))
                    {
                        using (SqlDataAdapter sda = new SqlDataAdapter())
                        {
                            cmd.CommandType = CommandType.Text;
                            cmd.Parameters.AddWithValue("@FirstName", tb_FirstName.Text.Trim());
                            cmd.Parameters.AddWithValue("@LastName", tb_LastName.Text.Trim());
                            cmd.Parameters.AddWithValue("@CardName", Convert.ToBase64String(encryptData(tb_CCName.Text.Trim())));
                            cmd.Parameters.AddWithValue("@CardNo", Convert.ToBase64String(encryptData(tb_CCNo.Text.Trim())));
                            cmd.Parameters.AddWithValue("@CardExpiryMonth", Convert.ToBase64String(encryptData(ddl_ExpMonth.Text.Trim())));
                            cmd.Parameters.AddWithValue("@CardExpiryYear", Convert.ToBase64String(encryptData(tb_ExpYear.Text.Trim())));
                            cmd.Parameters.AddWithValue("@CardCVV", Convert.ToBase64String(encryptData(tb_CVV.Text.Trim())));
                            cmd.Parameters.AddWithValue("@Email", tb_Email.Text.Trim());
                            cmd.Parameters.AddWithValue("@PasswordHash", pwdfinalHash);
                            cmd.Parameters.AddWithValue("@PasswordSalt", pwdsalt);
                            cmd.Parameters.AddWithValue("@DateOfBirth", tb_DOB.Text.Trim());
                            cmd.Parameters.AddWithValue("@Photo", fileImg);
                            cmd.Parameters.AddWithValue("@IV", Convert.ToBase64String(IV));
                            cmd.Parameters.AddWithValue("@Key", Convert.ToBase64String(Key));
                            cmd.Parameters.AddWithValue("@LoginAttempt", 0);
                            cmd.Parameters.AddWithValue("AccountStatus", "Active");
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
                                lb_error1.Text = "Email already Exist";
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

        protected byte[] encryptData(string data)
        {
            byte[] cipherText = null;
            try
            {
                RijndaelManaged cipher = new RijndaelManaged();
                cipher.IV = IV;
                cipher.Key = Key;
                ICryptoTransform encryptTransform = cipher.CreateEncryptor();
                //ICryptoTransform decryptTransform = cipher.CreateDecryptor();
                byte[] plainText = Encoding.UTF8.GetBytes(data);
                cipherText = encryptTransform.TransformFinalBlock(plainText, 0, plainText.Length);


                //Encrypt
                //cipherText = encryptTransform.TransformFinalBlock(plainText, 0, plainText.Length);
                //cipherString = Convert.ToBase64String(cipherText);
                //Console.WriteLine("Encrypted Text: " + cipherString);

            }
            catch (Exception ex)
            {
                throw new Exception(ex.ToString());
            }

            finally { }
            return cipherText;
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
