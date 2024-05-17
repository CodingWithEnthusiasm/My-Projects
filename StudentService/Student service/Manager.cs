using iTextSharp.text;
using iTextSharp.text.pdf;
using MySql.Data.MySqlClient;
using NetTopologySuite.Algorithm;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Web.Security;
using System.Windows.Forms;

namespace Student_service
{
    public partial class Manager : Form
    {

        string connstr = "server=localhost;user id=root;password=1234;database=degreeproject";
       
        public static int Student_ID { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string S_Name { get; set; }
        public string S_Surname { get; set; }
        public string Phone { get; set; }
        public string Faculty { get; set; }
        public string Speciality { get; set; }
        public string Degree { get; set; }
        public DateTime Dateofenrollment { get; set; }

        Subjects Grades = new Subjects();
        Login log = new Login();

        public Manager()
        {
            InitializeComponent();
        }



        private bool dragging = false;
        private Point dragCursorPoint;
        private Point dragFormPoint;

        private void W_MouseDown(object sender, MouseEventArgs e)
        {
            dragging = true;
            dragCursorPoint = Cursor.Position;
            dragFormPoint = Location;
        }

        private void W_MouseMove(object sender, MouseEventArgs e)
        {
            if (dragging)
            {
                Point dif = Point.Subtract(Cursor.Position, new Size(dragCursorPoint));
                Location = Point.Add(dragFormPoint, new Size(dif));
            }
        }

        private void W_MouseUp(object sender, MouseEventArgs e)
        {
            dragging = false;
        }



        private void IndexFac()
        {
            using (MySqlConnection conn = new MySqlConnection(connstr))
            {
                conn.Open();
                DataTable fac_box = new DataTable();
                using (MySqlCommand cmd = new MySqlCommand("SELECT Faculty_Name, Faculty_ID FROM Faculty", conn))
                {

                    using (MySqlDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader != null)
                        {
                            fac_box.Columns.Add("Faculty_Name", typeof(string));
                            fac_box.Load(reader);
                            FacultyBox.ValueMember = "Faculty_Name";
                            FacultyBox.DataSource = fac_box;
                        }
                    }

                }
            }
        }

       private void IndexSpec()
        {
           
                using (MySqlConnection conn = new MySqlConnection(connstr))
                {
                    conn.Open();
                    DataTable spec_box = new DataTable();
                    using (MySqlCommand cmd = new MySqlCommand("SELECT Speciality_Name FROM Speciality WHERE Faculty_Name = @Faculty_a", conn))
                    {
                        Faculty = FacultyBox.Text;
                        cmd.Parameters.AddWithValue("@Faculty_a", Faculty);
                        using (MySqlDataReader reader = cmd.ExecuteReader())
                        {
                            if (reader != null)
                            {

                                spec_box.Columns.Add("Speciality_Name", typeof(string));
                                spec_box.Load(reader);
                                SpecialityBox.ValueMember = "Speciality_Name";
                                SpecialityBox.DataSource = spec_box;


                            }

                        }

                    }
                }
            
        }

        private void IndexDeg()
        {
            using (MySqlConnection conn = new MySqlConnection(connstr))
            {
                conn.Open();
                DataTable deg_box = new DataTable();
                using (MySqlCommand cmd = new MySqlCommand("SELECT Degree_Name FROM Degree WHERE Speciality_Name_a = @Speciality_a", conn))
                {
                    Speciality = SpecialityBox.Text;
                    cmd.Parameters.AddWithValue("@Speciality_a", Speciality);

                    using (MySqlDataReader reader = cmd.ExecuteReader())
                    {

                        deg_box.Columns.Add("Degree_Name", typeof(string));
                        deg_box.Load(reader);
                        DegreeBox.ValueMember = "Degree_Name";
                        DegreeBox.DataSource = deg_box;

                    }

                }
            }
        }

        private DataTable StudentLoad()
        {
            DataTable dt = new DataTable();
            using (MySqlConnection conn = new MySqlConnection(connstr))
            {
                conn.Open();
               
                using (MySqlCommand cmd = new MySqlCommand("SELECT Student_ID as ID, Name, Surname, Phone, Faculty, Speciality, Degree, Email, DATE_FORMAT(Dateofenrollment,'%Y-%M-%D') AS Date FROM Student", conn))
                {
                    using (MySqlDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader != null)
                        {
                            dt.Load(reader);
                        }
                    }
                   
                }
               
                List.DataSource = dt;
               
            }
            return dt;
        }
        private bool Validation()
        {
           
            Regex regex = new Regex("^[A-Za-z ]+$"); 

            if ((regex.IsMatch(NameBox.Text) == false) || (regex.IsMatch(SurnameBox.Text) == false))
            {
                MessageBox.Show("Name or surname fields are empty or non letter symbols");
                return false;

            }
            else if (!PhoneBox.Text.All(char.IsDigit))
            {
                MessageBox.Show("Error, A phone number cannot contain letters");
                return false;

            }
            else if (PhoneBox.TextLength != 10)
            {
                MessageBox.Show("Contact number should contain 10 characters");
                return false;
            }
            else
                return true;
            
              
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            
            try
                {
               
                StudentLoad();

                    IndexFac();

                    IndexSpec();

                    IndexDeg();
                  
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            using (MySqlConnection conn = new MySqlConnection(connstr))
            {
                conn.Open();
                try
                {
                   if (Validation())
                   {
                        Dateofenrollment = DateTime.Today;
                        Random rnd = new Random();
                        Student_ID = rnd.Next(1, 99999) ;
                        Password = Membership.GeneratePassword(6, 3);
                        Email = "UN" + Student_ID + "@uni.com";
                        S_Name = NameBox.Text;
                        S_Surname = SurnameBox.Text;
                        Phone = PhoneBox.Text;
                        Faculty = FacultyBox.Text;
                        Speciality = SpecialityBox.Text;
                        Degree = DegreeBox.Text;


                        S_Name = CultureInfo.CurrentCulture.TextInfo.ToTitleCase(S_Name.ToLower());
                        S_Surname = CultureInfo.CurrentCulture.TextInfo.ToTitleCase(S_Surname.ToLower());

                        using (MySqlCommand cmd = new MySqlCommand("INSERT INTO Student(Student_ID, Name, Surname, Phone, Faculty, Speciality, Degree, Dateofenrollment,Email) " +
                            "VALUES(@Student_ID, @Name, @Surname, @Phone, @Faculty, @Speciality, @Degree, @Dateofenrollment,@Email);" +
                            "INSERT INTO Accounts (Email, Password, Role) VALUES(@Email, @Password, 'Student');", conn)) 
                        {

                            cmd.Parameters.AddWithValue("@Email", Email);
                            cmd.Parameters.AddWithValue("@Password", Password);
                            cmd.Parameters.AddWithValue("@Student_ID", Student_ID);
                            cmd.Parameters.AddWithValue("@Name", S_Name);
                            cmd.Parameters.AddWithValue("@Surname", S_Surname);
                            cmd.Parameters.AddWithValue("@Phone", Phone);
                            cmd.Parameters.AddWithValue("@Faculty", Faculty);
                            cmd.Parameters.AddWithValue("@Speciality", Speciality);
                            cmd.Parameters.AddWithValue("@Dateofenrollment", Dateofenrollment);
                            cmd.Parameters.AddWithValue("@Degree", Degree);
                            cmd.ExecuteNonQuery();
                        }

                        StudentLoad();
                        MessageBox.Show("Borrow was added");
                   }

                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
                finally
                {
                    conn.Close();
                }
            }

        }

        private void button1_Click(object sender, EventArgs e)
        {
            var confirmResult = MessageBox.Show("Are you sure you want to log out?", "", MessageBoxButtons.YesNo);
            if (confirmResult == DialogResult.Yes)
            {
                Grades.Close();
                log.Show();
                Close();
            }
            
        }

        private void button6_Click(object sender, EventArgs e)
        {
            if (List.Rows.Count > 0)
            {
                SaveFileDialog save = new SaveFileDialog();
                save.FileName = "Result.pdf";
                save.Filter = "PDF (*.pdf)|*.pdf";
                bool Error = false;
                if (save.ShowDialog() == DialogResult.OK)
                {
                    if (File.Exists(save.FileName))
                    {
                        try
                        {
                            File.Delete(save.FileName);
                        }
                        catch (Exception ex)
                        {

                            Error = true;
                            MessageBox.Show(ex.Message);
                        }
                    }
                    if (Error == false)
                    {
                        try
                        {
                            PdfPTable Table = new PdfPTable(List.Columns.Count);
                            Table.WidthPercentage = 90;
                            foreach (DataGridViewColumn col in List.Columns)
                            {
                                PdfPCell pCell = new PdfPCell(new Phrase(col.HeaderText));
                                Table.AddCell(pCell);
                            }
                            foreach (DataGridViewRow viewRow in List.Rows)
                            {
                                foreach (DataGridViewCell dcell in viewRow.Cells)
                                {
                                    Table.AddCell(dcell.Value.ToString());
                                }
                            }

                            using (FileStream fileStream = new FileStream(save.FileName, FileMode.Create))
                            {
                                Document document = new Document(PageSize.A2 , 0f, 0f, 50f, 20f);
                                PdfWriter.GetInstance(document, fileStream);
                                document.Open();
                                document.Add(Table);
                                document.Close();
                                fileStream.Close();
                            }
                           

                        }

                        catch (Exception ex)
                        {
                            MessageBox.Show(ex.Message);
                        }
                    }
                }
            }
          
        }

       
        private void button7_Click(object sender, EventArgs e)
        {
            DataTable dt = StudentLoad();
            DataView dv = dt.DefaultView;

            dv.RowFilter = string.Format("Convert([student_id], System.String) like '%{0}%' " +
                "OR name like '%{0}%' " +
                "OR surname like '%{0}%'" +
                "OR Phone like '%{0}%'", SearchBox.Text);

             List.DataSource = dv.ToTable();
        }

        



        private void List_RowHeaderMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            int rowIndex = e.RowIndex;
            if (List.Rows[rowIndex].Cells[0].Value != System.DBNull.Value)
            {
                Student_ID = Convert.ToInt32(List.Rows[rowIndex].Cells[0].Value);
            }
            if (List.SelectedRows.Count > 0)
            {
                NameBox.Text = List.Rows[rowIndex].Cells["Name"].Value.ToString();
                SurnameBox.Text = List.Rows[rowIndex].Cells["Surname"].Value.ToString();
                PhoneBox.Text = List.Rows[0].Cells["Phone"].Value.ToString();
                FacultyBox.Text = List.Rows[rowIndex].Cells["Faculty"].Value.ToString();
                SpecialityBox.Text = List.Rows[rowIndex].Cells["Speciality"].Value.ToString();
                DegreeBox.Text = List.Rows[rowIndex].Cells["Degree"].Value.ToString();
            }
        }

        private void List_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            int rowIndex = List.CurrentCell.RowIndex;
            if (List.Rows[rowIndex].Cells[0].Value != System.DBNull.Value)
            {
                Student_ID = Convert.ToInt32(List.Rows[rowIndex].Cells[0].Value);
            }
        }





        private void button4_Click(object sender, EventArgs e)
        {
            using (MySqlConnection conn = new MySqlConnection(connstr))
            {
                try
                {
                    conn.Open();

                    if (Validation())
                    {
                        S_Name = NameBox.Text;
                        S_Surname = SurnameBox.Text;
                        Phone = PhoneBox.Text;
                        Faculty = FacultyBox.Text;
                        Speciality = SpecialityBox.Text;
                        Degree = DegreeBox.Text;
                        using (MySqlCommand cmd = new MySqlCommand("Update Student set Name = @Name, Surname = @Surname, Phone = @Phone, Faculty = @Faculty, Speciality = @Speciality, Degree = @Degree WHERE Student_ID = @Student_ID", conn))
                        {
                            cmd.Parameters.AddWithValue("@Student_ID", Student_ID);
                            cmd.Parameters.AddWithValue("@Name", S_Name);
                            cmd.Parameters.AddWithValue("@Surname", S_Surname);
                            cmd.Parameters.AddWithValue("@Phone", Phone);
                            cmd.Parameters.AddWithValue("@Faculty", Faculty);
                            cmd.Parameters.AddWithValue("@Speciality", Speciality);
                            cmd.Parameters.AddWithValue("@Degree", Degree);
                            cmd.ExecuteNonQuery();
                        }
                        StudentLoad();
                        MessageBox.Show("Data was updated");
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
                finally
                {
                    conn.Close();
                }
            }
        }


        private void button5_Click(object sender, EventArgs e)
        {
            using (MySqlConnection conn = new MySqlConnection(connstr))
            {

                conn.Open();
                try
                {
                    int rows;
                    var confirmResult = MessageBox.Show("Are you sure you want to delete this borrow?", "", MessageBoxButtons.YesNo);
                    Email = "UN" + Student_ID + "@uni.com";

                    if (confirmResult == DialogResult.Yes)
                    {
                        using (MySqlCommand cmd = new MySqlCommand("DELETE FROM Student WHERE Student_ID = @Student_ID; DELETE FROM Accounts WHERE Email = @Email; DELETE FROM Grade WHERE Student_ID = @Student_ID; DELETE FROM Studies WHERE Student_ID = @Student_ID", conn))
                        {
                            cmd.Parameters.AddWithValue("@Email", Email);
                            cmd.Parameters.AddWithValue("@Student_ID", Student_ID);
                            rows = cmd.ExecuteNonQuery();
                        }
                        if (rows > 0)
                        {
                            MessageBox.Show("Student was deleted");
                            StudentLoad();
                        }
                        else
                        {
                            MessageBox.Show("Failed to delete student");
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
                finally
                {
                    conn.Close();
                }
            }
        }



        private void button3_Click(object sender, EventArgs e)
        {
            if (List.SelectedRows.Count > 0 || List.Rows[0].Cells[0].Selected)
            {
                Close();
                Grades.Show();
            }
        }

        private void FacultyBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            IndexSpec();
        }

        private void SpecialityBox_SelectedIndexChanged(object sender, EventArgs e)
        {

            IndexDeg();

        }


        
    }
}
