using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace AS_Assignment
{
    public partial class Registration : System.Web.UI.Page
    {
        string DBConnectionString = ConfigurationManager.ConnectionStrings["ASDBConnection"].ConnectionString;
        static string Hash;
        static string Salt;
        byte[] Key;
        byte[] IV;
        protected void Page_Load(object sender, EventArgs e)
        {
            tb_pwd.Attributes.Add("onKeyUp", "validate()");
            
        }

        protected void btn_submit_Click(object sender, EventArgs e)
        {
            if(tb_fname.Text == "")
            {
                lbl_errorMsg.Text = "First Name field cannot be empty";
            }
            else if(tb_lname.Text == "")
            {
                lbl_errorMsg.Text = "Last name field cannot be empty";
            }
            else if (tb_credit_card.Text == "")
            {
                lbl_errorMsg.Text = "Credit Card field cannot be empty";
            }
            else if (tb_validDate.Text == "")
            {
                lbl_errorMsg.Text = "Valid date field cannot be empty";
            }
            else if (tb_pwd.Text == "")
            {
                lbl_errorMsg.Text = "Password field cannot be empty";
            }
            else if (tb_email.Text == "")
            {
                lbl_errorMsg.Text = "Email Field cannot be empty";
            }
            else if (tb_dob.Text == "")
            {
                lbl_errorMsg.Text = "Date of birth field cannot be empty";
            }
            else
            {
                lbl_errorMsg.Text = "";
            }
            int strength = pwdChecker(tb_pwd.Text);
            string status = "";
            switch (strength)
            {
                case 1:
                    status = "Very Weak";
                    break;

                case 2:
                    status = "Weak";
                    break;
                case 3:
                    status = "Medium";
                    break;
                case 4:
                    status = "Strong";
                    break;
                case 5:
                    status = "Very Strong";
                    break;
                default:
                    break;
            }
            lbl_pwdCheck.Text = status;
            if(strength < 4)
            {
                lbl_pwdCheck.ForeColor = Color.Red;
                return;
            }
            lbl_pwdCheck.ForeColor = Color.Green;

            string pwd = tb_pwd.Text.ToString().Trim();
            RNGCryptoServiceProvider rng = new RNGCryptoServiceProvider();
            byte[] saltByte = new byte[8];
            rng.GetBytes(saltByte);
            Salt = Convert.ToBase64String(saltByte);
            SHA512Managed hashing = new SHA512Managed();
            string pwdWithSalt = pwd + Salt;
            byte[] hashWithSalt = hashing.ComputeHash(Encoding.UTF8.GetBytes(pwdWithSalt));
            Hash = Convert.ToBase64String(hashWithSalt);
            RijndaelManaged cipher = new RijndaelManaged();
            cipher.GenerateKey();
            Key = cipher.Key;
            IV = cipher.IV;

            createAccount();
            Response.Redirect("Login.aspx", false);
        }
        private int pwdChecker(string password)
        {
            int strength = 0;
            if(password.Length > 8)
            {
                strength = 1;
            }
            if (Regex.IsMatch(password, "[a-z]"))
            {
                strength++;
            }
            if (Regex.IsMatch(password, "[A-Z]"))
            {
                strength++;
            }
            if (Regex.IsMatch(password, "[0-9]"))
            {
                strength++;
            }
            if (Regex.IsMatch(password, "[^A-Za-z0-9]"))
            {
                strength++;
            }
            return strength;
        }
        public void createAccount()
        {
            try
            {
                using(SqlConnection sqlCon = new SqlConnection(DBConnectionString))
                {
                    using(SqlCommand sqlCmd = new SqlCommand("INSERT INTO ACCOUNT VALUES(@Name,@CreditNum,@ValidDate,@Email,@PwdHash,@PwdSalt,@BirthDate,@IV,@Key)"))
                    {
                        sqlCmd.CommandType = CommandType.Text;
                        sqlCmd.Parameters.AddWithValue("@Name", tb_fname.Text.Trim() + tb_lname.Text.Trim());
                        sqlCmd.Parameters.AddWithValue("@CreditNum", Convert.ToBase64String(encrypt_data(tb_credit_card.Text.Trim())));
                        sqlCmd.Parameters.AddWithValue("@ValidDate", tb_validDate.Text.Trim());
                        sqlCmd.Parameters.AddWithValue("@Email", tb_email.Text.Trim());
                        sqlCmd.Parameters.AddWithValue("@PwdHash", Hash);
                        sqlCmd.Parameters.AddWithValue("@PwdSalt", Salt);
                        sqlCmd.Parameters.AddWithValue("@BirthDate", tb_dob.Text);
                        sqlCmd.Parameters.AddWithValue("@IV", Convert.ToBase64String(IV));
                        sqlCmd.Parameters.AddWithValue("@Key", Convert.ToBase64String(Key));
                        sqlCmd.Connection = sqlCon;
                        sqlCon.Open();
                        sqlCmd.ExecuteNonQuery();
                        sqlCon.Close();
                    }
                }
            }
            catch(Exception ex)
            {
                throw new Exception(ex.ToString());
            }
        }

        protected byte[] encrypt_data(string data)
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
                cipherText = encryptTransform.TransformFinalBlock(plainText, 0,
               plainText.Length);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.ToString());
            }
            finally { }
            return cipherText;
        }
    }   
}