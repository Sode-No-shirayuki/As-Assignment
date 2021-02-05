using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace AS_Assignment
{
    public partial class unlock : System.Web.UI.Page
    {
        string DBConnectionString = ConfigurationManager.ConnectionStrings["ASDBConnection"].ConnectionString;
        protected void Page_Load(object sender, EventArgs e)
        {
            
        }

        protected void btn_submit_Click(object sender, EventArgs e)
        {
            if (tb_email.Text == "")
            {
                lbl_errorMsg.Text = "Email is empty";
                lbl_errorMsg.ForeColor = System.Drawing.Color.Red;
            }
            else if (tb_dob.Text == "")
            {
                lbl_errorMsg.Text = "Date of birth is empty";
                lbl_errorMsg.ForeColor = System.Drawing.Color.Red;
            }
            else
            {
                var result = validateAccount(tb_email.Text.ToString().Trim(), tb_dob.Text.ToString().Trim());
                if(result == true)
                {
                    Session[tb_email.Text.ToString().Trim()] = 0;
                    Response.Redirect("Login.aspx", false);
                }
                else
                {
                    lbl_errorMsg.Text = "Incorrect input";
                    lbl_errorMsg.ForeColor = System.Drawing.Color.Red;
                }
            }
        }
        public bool validateAccount(string email, string DOB)
        {
            SqlConnection connection = new SqlConnection(DBConnectionString);
            string sql = "SELECT BirthDate,Email FROM Account WHERE Email=@userId";
            SqlCommand command = new SqlCommand(sql, connection);
            command.Parameters.AddWithValue("@userId", email);
            var validate = true;
            try
            {
                connection.Open();
                using (SqlDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        if (reader["Email"] != DBNull.Value)
                        {
                            string dob = Convert.ToDateTime(reader["BirthDate"].ToString()).ToString("yyyy-MM-dd");
                            if (DOB.Equals(dob)){
                                validate = true;
                            }
                            else
                            {
                                validate = false;
                            }
                        }
                    }
                   
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
            return validate;
        }
    }
}