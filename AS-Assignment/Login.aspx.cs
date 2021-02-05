using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Security.Cryptography;
using System.Text;
using System.Web;
using System.Web.Script.Serialization;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace AS_Assignment
{
    public partial class Login : System.Web.UI.Page
    {
        string DBConnectionString = ConfigurationManager.ConnectionStrings["ASDBConnection"].ConnectionString;
        
        public class MyObject
        {
            public string success { get; set; }
            public List<string> ErrorMessage { get; set; }
        }
        
        protected void Page_Load(object sender, EventArgs e)
        {
            
        }

        protected void btn_login_Click(object sender, EventArgs e)
        {
            string pwd = tb_pwdLogin.Text.ToString().Trim();
            string email = tb_emailLogin.Text.ToString().Trim();
            if(email != null)
            {
                ViewState[email] = ViewState[email] == null ? 0 : (int)ViewState[email];
            }
            SHA512Managed hashing = new SHA512Managed();
            string dbHash = getDBHash(email);
            string dbSalt = getDBSalt(email);
            try
            {
                if ((int)ViewState[email] <= 3)
                {
                    if (dbSalt != null && dbSalt.Length != 0 && dbHash != null && dbHash.Length != 0)
                    {
                        string pwdWithSalt = pwd + dbSalt;
                        byte[] hashWithSalt = hashing.ComputeHash(Encoding.UTF8.GetBytes(pwdWithSalt));
                        string userHash = Convert.ToBase64String(hashWithSalt);
                        if (ValidateCaptcha())
                        {
                            if (userHash.Equals(dbHash))
                            {
                                Session["UserID"] = email;
                                string guid = Guid.NewGuid().ToString();
                                Session["AuthToken"] = guid;
                                Response.Cookies.Add(new HttpCookie("AuthToken", guid));
                                ViewState[email]= 0;
                                Response.Redirect("Success.aspx", false);
                            }
                            else
                            {
                                int attempts = (int)ViewState[email];
                                attempts += 1;
                                ViewState[email] = attempts;
                                lbl_error_msg.Text = "Email or Password is incorrect! Please try again.";
                                lbl_error_msg.ForeColor = System.Drawing.Color.Red;


                            }
                        }
                    }
                    else
                    {
                        int attempts = (int)ViewState[email];
                        attempts += 1;
                        ViewState[email] = attempts;
                        Debug.WriteLine(ViewState["attempts"]);
                        lbl_error_msg.Text = "Email or Password is incorrect! Please try again.";
                        lbl_error_msg.ForeColor = System.Drawing.Color.Red;
                    }
                }
                else
                {
                    lbl_error_msg.Text = "Your account is locked. Please contact the administrator.";
                    lbl_error_msg.ForeColor = System.Drawing.Color.Red;
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.ToString());
            }
            finally { }
        }
        protected string getDBHash(string email)
        {
            string h = null;
            SqlConnection sqlCon = new SqlConnection(DBConnectionString);
            string sqlStmt = "select PasswordHash FROM Account WHERE Email=@EMAIL";
            SqlCommand sqlCmd = new SqlCommand(sqlStmt, sqlCon);
            sqlCmd.Parameters.AddWithValue("@EMAIL",email);
            try
            {
                sqlCon.Open();
                using (SqlDataReader reader = sqlCmd.ExecuteReader())
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
            finally { sqlCon.Close(); }
            return h;
        }

        protected string getDBSalt(string email)
        {
            string s = null;
            SqlConnection sqlCon = new SqlConnection(DBConnectionString);
            string sqlStmt = "select PasswordSalt FROM ACCOUNT WHERE Email=@EMAIL";
            SqlCommand sqlCmd = new SqlCommand(sqlStmt, sqlCon);
            sqlCmd.Parameters.AddWithValue("@EMAIL", email);
            try
            {
                sqlCon.Open();
                using (SqlDataReader reader = sqlCmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        if (reader["PasswordSalt"] != null)
                        {
                            if (reader["PasswordSalt"] != DBNull.Value)
                            {
                                s = reader["PasswordSalt"].ToString();
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.ToString());
            }
            finally { sqlCon.Close(); }
            return s;
        }
        public bool ValidateCaptcha()
        {
            bool result = true;
            string captchaResponse = Request.Form["g-recaptcha-response"];
            HttpWebRequest req = (HttpWebRequest)WebRequest.Create("https://www.google.com/recaptcha/api/siteverify?secret=6Lfen0caAAAAAPvivoo9tBc0xpbdjNukb8lWVvkH &response=" + captchaResponse);
            try
            {
                using(WebResponse wResponse = req.GetResponse())
                {
                    using(StreamReader readStream = new StreamReader(wResponse.GetResponseStream()))
                    {
                        string jsonResponse = readStream.ReadToEnd();
                        JavaScriptSerializer js = new JavaScriptSerializer();
                        MyObject jsonObject = js.Deserialize<MyObject>(jsonResponse);
                        result = Convert.ToBoolean(jsonObject.success);
                    }
                }
                return result;
            }
            catch(WebException ex)
            {
                throw ex;
            }
        }
    }
}