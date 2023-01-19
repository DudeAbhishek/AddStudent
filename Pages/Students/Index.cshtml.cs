using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Data.SqlClient;

namespace Student.Pages.Students
{
    public class IndexModel : PageModel
    {
        public List<StudentInfo> listStudents = new List<StudentInfo>();
        
        public void OnGet()
        {
            try
            {
                String connectionString = "Data Source=.\\sqlexpress;Initial Catalog=Student;Integrated Security=True";
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    String sql = "SELECT * FROM students";
                    using (SqlCommand command = new SqlCommand(sql, connection))
                    {
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read()) 
                            {
                                StudentInfo studentInfo= new StudentInfo();
                                studentInfo.id = "" + reader.GetInt32(0);
                                studentInfo.name = reader.GetString(1);
                                studentInfo.email = reader.GetString(2);
                                studentInfo.phone = reader.GetString(3);
                                studentInfo.address= reader.GetString(4);
                                studentInfo.created_at  = reader.GetDateTime(5).ToString();

                                listStudents.Add(studentInfo);
                            }
                        }
                    }
                }
            }
            catch(Exception ex)
            { 
                Console.WriteLine("Exception: " + ex.ToString());
            }
        }
    }

    public class StudentInfo 
    {
        public String id;
        public String name;
        public String email;
        public String phone;
        public String address;
        public String created_at;
    }
}
