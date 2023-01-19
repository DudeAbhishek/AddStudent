using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Data.SqlClient;

namespace Student.Pages.Students
{
    public class EditModel : PageModel
    {
        private const string V = "WHERE id=@id ";
        public StudentInfo studentInfo = new StudentInfo();
        public String errorMessage = "";
        public String successMessage = "";

        public void OnGet()
        {
            String id = Request.Query["id"];

            try
            {
                String connectionString = "Data Source=.\\sqlexpress;Initial Catalog=Student;Integrated Security=True";
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();

                    String sql = "SELECT * FROM students WHERE id=@id";

                    using (SqlCommand command = new SqlCommand(sql, connection))
                    {
                        command.Parameters.AddWithValue("@id", id);

                        using(SqlDataReader reader = command.ExecuteReader())
                        {
                            if(reader.Read())
                            {
                                studentInfo.id = "" + reader.GetInt32(0);
                                studentInfo.name = reader.GetString(1);
                                studentInfo.email = reader.GetString(2);
                                studentInfo.phone = reader.GetString(3);
                                studentInfo.address = reader.GetString(4);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                errorMessage = ex.Message;
                return;
            }
        }

        public void OnPost()
        {
            studentInfo.id = Request.Form["id"];
            studentInfo.name = Request.Form["name"];
            studentInfo.email = Request.Form["email"];
            studentInfo.phone = Request.Form["phone"];
            studentInfo.address = Request.Form["address"];

            if(studentInfo.id.Length == 0 || studentInfo.name.Length ==0 ||studentInfo.email.Length == 0 || studentInfo.phone.Length == 0 ||studentInfo.address.Length == 0 )
            {
                errorMessage = "All fields are required.";
                return;
            }
            
            if(studentInfo.phone.Length != 10)
            {
                errorMessage = "Please check Phone Number.";
                return;
            }
            
            try
            {
                String connectionString = "Data Source=.\\sqlexpress;Initial Catalog=Student;Integrated Security=True";
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();

                    String sql =  "UPDATE students "
                                  + "SET name=@name, email=@email, phone=@phone, address=@address "
                                  + V;

                    using(SqlCommand command = new SqlCommand(sql, connection))
                    {
                        command.Parameters.AddWithValue("@name", studentInfo.name);
                        command.Parameters.AddWithValue("@email", studentInfo.email);
                        command.Parameters.AddWithValue("@phone", studentInfo.phone);
                        command.Parameters.AddWithValue("@address", studentInfo.address);
                        command.Parameters.AddWithValue("@id", studentInfo.id);

                        command.ExecuteNonQuery();
                    }
                }
            }
            catch (Exception ex)
            {
                errorMessage = ex.Message;
                return;
            }

            Response.Redirect("/Students/Index");
        }
    }
}
