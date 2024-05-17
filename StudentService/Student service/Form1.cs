using iTextSharp.text;
using iTextSharp.text.pdf;
using MySql.Data.MySqlClient;
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
using System.Windows.Forms;

namespace Student_service
{
    public partial class Form1 : Form
    {

        //MySqlConnection conn = new MySqlConnection();
        MySqlCommand sqlCmd = new MySqlCommand();
        DataTable dt = new DataTable();
        //String sqlQuery;
        string connstr = "server=localhost;user id=root;password=1234;database=degreeproject";
        MySqlDataAdapter adapter2 = new MySqlDataAdapter();
        MySqlDataReader reader;



        DataSet Ds = new DataSet();

        // [Required]
        public int Student_ID { get; set; }
#pragma warning disable CS0108 // 'Manager.Name' hides inherited member 'Control.Name'. Use the new keyword if hiding was intended.
        public string Name { get; set; }
#pragma warning restore CS0108 // 'Manager.Name' hides inherited member 'Control.Name'. Use the new keyword if hiding was intended.
        public string Surname { get; set; }
        public int Check;


        public string Phone { get; set; }
        public string Faculty_a { get; set; }
        public string Speciality { get; set; }

        public string Speciality_a { get; set; }


        public string Degree { get; set; }
        public DateTime Dateofenrollment { get; set; }
        public int Faculty_ID { get; set; }

        public int Degree_ID { get; set; }



        public int Speciality_ID { get; set; }
        public Form1()
        {
            InitializeComponent();
        }


        private void Form1_Load(object sender, EventArgs e)
        {
            // TODO: This line of code loads data into the 'degreeprojectDataSet.student' table. You can move, or remove it, as needed.
            //this.studentTableAdapter.Fill(this.degreeprojectDataSet.student);

            using (MySqlConnection conn = new MySqlConnection(connstr))
            {

                conn.Open();



                try
                {



                    sqlCmd.Connection = conn;
                    sqlCmd.CommandText = "SELECT * FROM Student";
                    //MySqlDataReader reader = sqlCmd.ExecuteReader();
                    reader = sqlCmd.ExecuteReader();
                    dt.Load(reader);




                    sqlCmd = new MySqlCommand("SELECT Faculty_Name, Faculty_ID FROM Faculty", conn);
                    reader = sqlCmd.ExecuteReader();
                    DataTable dt_box = new DataTable();
                    dt_box.Columns.Add("Faculty_Name", typeof(string));
                    dt_box.Load(reader);
                    FacultyBox.ValueMember = "Faculty_Name";
                    FacultyBox.DataSource = dt_box;
                    reader.Dispose();






                    sqlCmd = new MySqlCommand("SELECT Speciality_Name FROM Speciality WHERE Faculty_Name = @Faculty_a  ", conn);
                    Faculty_a = FacultyBox.Text;
                    sqlCmd.Parameters.AddWithValue("@Faculty_a", Faculty_a);
                    reader = sqlCmd.ExecuteReader();
                    dt_box = new DataTable();
                    dt_box.Columns.Add("Speciality_Name", typeof(string));
                    dt_box.Load(reader);
                    SpecialityBox.ValueMember = "Speciality_Name";
                    SpecialityBox.DataSource = dt_box;
                    reader.Dispose();



                    sqlCmd = new MySqlCommand("SELECT Degree_Name FROM Degree WHERE Speciality_Name_a = @Speciality_a", conn);

                    Speciality_a = SpecialityBox.Text;
                    sqlCmd.Parameters.AddWithValue("@Speciality_a", Speciality_a);
                    reader = sqlCmd.ExecuteReader();
                    dt_box = new DataTable();
                    dt_box.Columns.Add("Degree_Name", typeof(string));
                    dt_box.Load(reader);
                    DegreeBox.ValueMember = "Degree_Name";
                    DegreeBox.DataSource = dt_box;
                    reader.Dispose();


                    reader.Close();
                    List.DataSource = dt;
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

        private void button2_Click(object sender, EventArgs e)
        {
            using (MySqlConnection conn = new MySqlConnection(connstr))
            {
                conn.Open();
                try
                {

                    Regex regex = new Regex("^[A-Za-z ]+$"); //Contains only letters and spaces 

                    if ((regex.IsMatch(NameBox.Text) == false) || (regex.IsMatch(SurnameBox.Text) == false))
                    {
                        MessageBox.Show("Name or surname fields are empty or non letter symbols");
                    }
                    else if (!PhoneBox.Text.All(char.IsDigit))
                    {
                        MessageBox.Show("Error, A phone number cannot contain letters");
                    }
                    else if (PhoneBox.TextLength == 10)
                    {
                        MessageBox.Show("Contact number should contain 10 characters");
                    }


                    else
                    {

                        Dateofenrollment = DateTime.Now;
                        Random rnd = new Random();
                        Student_ID = rnd.Next(1, 100000);



                        Name = NameBox.Text;
                        Surname = SurnameBox.Text;
                        Phone = PhoneBox.Text;
                        Faculty_a = FacultyBox.Text;
                        Speciality = SpecialityBox.Text;
                        Degree = DegreeBox.Text;


                        Name = CultureInfo.CurrentCulture.TextInfo.ToTitleCase(Name.ToLower());
                        Surname = CultureInfo.CurrentCulture.TextInfo.ToTitleCase(Surname.ToLower());

                        Console.WriteLine("Capitalized String: " + Name);

                        Console.WriteLine("Capitalized String: " + Surname);

                        sqlCmd = new MySqlCommand("INSERT INTO Student (Student_ID, Name, Surname,Phone, Faculty, Speciality, Degree,Dateofenrollment) VALUES (@Student_ID, @Name, @Surname, @Phone, @Faculty, @Speciality, @Degree,@Dateofenrollment)", conn);

                        sqlCmd.Parameters.AddWithValue("@Student_ID", Student_ID);
                        sqlCmd.Parameters.AddWithValue("@Name", Name);
                        sqlCmd.Parameters.AddWithValue("@Surname", Surname);
                        sqlCmd.Parameters.AddWithValue("@Phone", Phone);
                        sqlCmd.Parameters.AddWithValue("@Faculty", Faculty_a);
                        sqlCmd.Parameters.AddWithValue("@Speciality", Speciality);
                        sqlCmd.Parameters.AddWithValue("@Dateofenrollment", Dateofenrollment);
                        sqlCmd.Parameters.AddWithValue("@Degree", Degree);
                        sqlCmd.ExecuteNonQuery();



                        adapter2 = new MySqlDataAdapter("SELECT * FROM Student", conn);
                        dt = new DataTable();
                        adapter2.Fill(dt);
                        List.DataSource = dt;

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
                Close();
            else
            {
            }
        }

        private void button6_Click(object sender, EventArgs e)
        {
            if (List.Rows.Count > 0)
            {
                SaveFileDialog save = new SaveFileDialog();
                save.Filter = "PDF (*.pdf)|*.pdf";
                save.FileName = "Result.pdf";
                bool ErrorMessage = false;
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

                            ErrorMessage = true;
                            MessageBox.Show("Unable to wride data in disk" + ex.Message);
                        }
                    }
                    if (!ErrorMessage)
                    {
                        try
                        {
                            PdfPTable pTable = new PdfPTable(List.Columns.Count);
                            pTable.DefaultCell.Padding = 2;
                            pTable.WidthPercentage = 100;
                            pTable.HorizontalAlignment = Element.ALIGN_LEFT;

                            foreach (DataGridViewColumn col in List.Columns)
                            {
                                PdfPCell pCell = new PdfPCell(new Phrase(col.HeaderText));
                                pTable.AddCell(pCell);
                            }
                            foreach (DataGridViewRow viewRow in List.Rows)
                            {
                                foreach (DataGridViewCell dcell in viewRow.Cells)
                                {
                                    pTable.AddCell(dcell.Value.ToString());
                                }
                            }


                            using (FileStream fileStream = new FileStream(save.FileName, FileMode.Create))
                            {
                                Document document = new Document(PageSize.A4, 8f, 16f, 16f, 8f);
                                PdfWriter.GetInstance(document, fileStream);
                                document.Open();
                                document.Add(pTable);
                                document.Close();
                                fileStream.Close();
                            }
                            MessageBox.Show("Data Export Successfully", "info");

                        }

                        catch (Exception ex)
                        {

                            MessageBox.Show("Error while exporting Data" + ex.Message);
                        }
                    }
                }
            }
            else
            {
                MessageBox.Show("No Record Found", "Info");

            }
        }

        private void button5_Click(object sender, EventArgs e)
        {
            Console.WriteLine("HERE" + Student_ID);

            using (MySqlConnection conn = new MySqlConnection(connstr))
            {

                conn.Open();
                try
                {

                    var confirmResult = MessageBox.Show("Are you sure you want to delete this borrow?", "", MessageBoxButtons.YesNo);


                    if (confirmResult == DialogResult.Yes)
                    {

                        sqlCmd = new MySqlCommand("DELETE FROM Student WHERE Student_ID = @Student_ID", conn);

                        sqlCmd.Parameters.AddWithValue("@Student_ID", Student_ID);
                        int rows = sqlCmd.ExecuteNonQuery();
                        if (rows > 0)
                        {
                            MessageBox.Show("Borrow was deleted");

                            adapter2 = new MySqlDataAdapter("SELECT * FROM Student", conn);
                            dt = new DataTable();
                            dt.Load(reader);
                            adapter2.Fill(dt);
                            List.DataSource = dt;
                            reader.Close();


                        }
                        else
                        {
                            MessageBox.Show("Failed to delete borrow");
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

        private void button7_Click(object sender, EventArgs e)
        {
            DataView dv = dt.DefaultView;



            dv.RowFilter = string.Format("Convert([student_id], System.String) like '%{0}%' " +
                "OR name like '%{0}%' " +
                "OR surname like '%{0}%'" +
                "OR Phone like '%{0}%'", SearchBox.Text);

            List.DataSource = dv.ToTable();
        }

        private void List_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            int rowIndex = List.CurrentCell.RowIndex;
            if (List.Rows[rowIndex].Cells[0].Value != System.DBNull.Value)
                Student_ID = Convert.ToInt32(List.Rows[rowIndex].Cells[0].Value);
            if (List.SelectedRows.Count > 0)
            {
                NameBox.Text = List.SelectedRows[0].Cells["Name"].Value.ToString();
                SurnameBox.Text = List.SelectedRows[0].Cells["Surname"].Value.ToString();
                PhoneBox.Text = List.SelectedRows[0].Cells["Surname"].Value.ToString();
                FacultyBox.Text = List.SelectedRows[0].Cells["Faculty"].Value.ToString();
                SpecialityBox.Text = List.SelectedRows[0].Cells["Speciality"].Value.ToString();
                DegreeBox.Text = List.SelectedRows[0].Cells["Degree"].Value.ToString();
            }
          


        }

        private void List_RowHeaderMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            int rowIndex = e.RowIndex;
            if (List.Rows[rowIndex].Cells[0].Value != System.DBNull.Value)
                Student_ID = Convert.ToInt32(List.Rows[rowIndex].Cells[0].Value);

            if (List.SelectedRows.Count > 0)
            {
                NameBox.Text = List.SelectedRows[0].Cells["Name"].Value.ToString();
                SurnameBox.Text = List.SelectedRows[0].Cells["Surname"].Value.ToString();
                PhoneBox.Text = List.SelectedRows[0].Cells["Phone"].Value.ToString();
                FacultyBox.Text = List.SelectedRows[0].Cells["Faculty"].Value.ToString();
                SpecialityBox.Text = List.SelectedRows[0].Cells["Speciality"].Value.ToString();
                DegreeBox.Text = List.SelectedRows[0].Cells["Degree"].Value.ToString();
            }
           
        }

        private void button4_Click(object sender, EventArgs e)
        {
          
            using (MySqlConnection conn = new MySqlConnection(connstr))
            {
                try
                {
                    conn.Open();
                    sqlCmd = new MySqlCommand("Update Student set Name = @Name, Surname = @Surname, Phone = @Phone, Faculty = @Faculty, Speciality = @Speciality, Degree = @Degree WHERE Student_ID = @Student_ID", conn);

                    Name = NameBox.Text;
                    Surname = SurnameBox.Text;
                    Phone = PhoneBox.Text;
                    Faculty_a = FacultyBox.Text;
                    Speciality = SpecialityBox.Text;
                    Degree = DegreeBox.Text;

                    sqlCmd.Parameters.AddWithValue("@Student_ID", Student_ID);
                    sqlCmd.Parameters.AddWithValue("@Name", Name);
                    sqlCmd.Parameters.AddWithValue("@Surname", Surname);
                    sqlCmd.Parameters.AddWithValue("@Phone", Phone);
                    sqlCmd.Parameters.AddWithValue("@Faculty", Faculty_a);
                    sqlCmd.Parameters.AddWithValue("@Speciality", Speciality);
                    sqlCmd.Parameters.AddWithValue("@Degree", Degree);
                    sqlCmd.ExecuteNonQuery();

                    adapter2 = new MySqlDataAdapter("SELECT * FROM Student", conn);
                    dt = new DataTable();
                    dt.Load(reader);
                    adapter2.Fill(dt);
                    List.DataSource = dt;
                    MessageBox.Show("Data was updated");
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
            // Create a new instance of the Form2 class
            Form2 Grades = new Form2();

            // Show the settings form
            Grades.Show();
            
        }
    }
}
