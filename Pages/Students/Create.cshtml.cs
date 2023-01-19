using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Data.SqlClient;

namespace Student.Pages.Students
{
    public class CreateModel : PageModel
    {
        public StudentInfo studentInfo = new StudentInfo();

        public String errorMesage = "";

        public String successMessage = "";

        public void OnGet()
        {
        }
        public void OnPost() 
        {
            studentInfo.name= Request.Form["name"];
            studentInfo.email= Request.Form["email"];
            studentInfo.phone = Request.Form["phone"];
            studentInfo.address= Request.Form["address"];

            if(studentInfo.name.Length == 0 || studentInfo.email.Length ==0 || studentInfo.phone.Length == 0 || studentInfo.address.Length == 0)
            {
                errorMesage = "All fields are required.";
                return;
            }

            if (studentInfo.phone.Length != 10)
            {
                errorMesage = "Please check Phone Number.";
                return;
            }

            //save the new student data into the database

            try
            {
                String connectionString = "Data Source=.\\sqlexpress;Initial Catalog=Student;Integrated Security=True";
                using(SqlConnection connection= new SqlConnection(connectionString)) 
                {
                    connection.Open();
                    String sql = "INSERT INTO students" +
                                 "(name, email, phone, address) VALUES " +
                                 "(@name, @email, @phone, @address )";

                    using (SqlCommand command = new SqlCommand(sql, connection)) 
                    {
                        command.Parameters.AddWithValue("@name", studentInfo.name);
                        command.Parameters.AddWithValue("@email", studentInfo.email);
                        command.Parameters.AddWithValue("@phone", studentInfo.phone);
                        command.Parameters.AddWithValue("@address", studentInfo.address);

                        command.ExecuteNonQuery();
                    }

                }
            }

            catch (Exception ex)
            {
                errorMesage = ex.Message;
                return;

            }

            studentInfo.name = "";
            studentInfo.email = "";
            studentInfo.phone =  "";
            studentInfo.address = "";

            successMessage = "New Student Added Successfully.";

            Response.Redirect("/Students/Index");
        }
    }
}
