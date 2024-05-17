using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Student_service
{
    public partial class Subjects : Form
    {
        public int Grade_ID { get; set; }
        public int Studies_ID { get; set; }
        public int Subject_ID { get; set; }
        public string Subject { get; set; }
        public string Grade { get; set; }
        public Subjects()
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


        private void List2_RowHeaderMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            int rowIndex = e.RowIndex;
            if (List2.Rows[rowIndex].Cells[0].Value != DBNull.Value)
            {
                Subject_ID = Convert.ToInt32(List2.Rows[rowIndex].Cells[0].Value);
            }


        }

        private void List2_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            int rowIndex = List2.CurrentCell.RowIndex;
            if (List2.Rows[rowIndex].Cells[0].Value != DBNull.Value)
            {
                Subject_ID = Convert.ToInt32(List2.Rows[rowIndex].Cells[0].Value);
            }

        }


        private void FillBox()
        {
            using (MySqlConnection conn = new MySqlConnection(connstr))
            {
                conn.Open();
                DataTable dt = new DataTable();
                using (MySqlCommand cmd = new MySqlCommand("SELECT s.Subject_ID, s.S_Name, Mark FROM Grade g, Studies s WHERE g.Subject_ID =s.Subject_ID AND @Student_ID = s.Student_ID AND @Student_ID = g.Student_ID AND g.Student_ID = @Student_ID", conn))
   
                {

                    cmd.Parameters.AddWithValue("@Student_ID", Manager.Student_ID);

                    using (MySqlDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader != null)
                        {
                            dt.Load(reader);
                        }
                    }
                }
                List2.DataSource = dt;
                List2.Columns["Subject_ID"].Visible = false;
            }
        }

        private void UpdateSubj()
        {
            using (MySqlConnection conn = new MySqlConnection(connstr))
            {
                conn.Open();
                DataTable dt_box = new DataTable();
                using (MySqlCommand cmd = new MySqlCommand("SELECT S_Name FROM Studies su WHERE @Student_ID = su.Student_ID", conn))
                {

                    cmd.Parameters.AddWithValue("@Student_ID", Manager.Student_ID);
                    using (MySqlDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader != null)
                        {
                            dt_box.Columns.Add("S_Name", typeof(string));
                            dt_box.Load(reader);
                        }
                    }
                }
              
                UpdateBox.ValueMember = "S_Name";
                UpdateBox.DataSource = dt_box;
            }
        }

        
        private void AddSubj()
        {
            using (MySqlConnection conn = new MySqlConnection(connstr))
            {
                conn.Open();
                DataTable dt_box = new DataTable();
                using (MySqlCommand cmd = new MySqlCommand("SELECT Subject_ID, S_Name FROM Subjects su", conn))
                {

                    using (MySqlDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader != null)
                        {
                            dt_box.Columns.Add("S_Name", typeof(string));
                            dt_box.Load(reader);
                        }
                    }
                }
                AddBox.DisplayMember = "S_Name";
                AddBox.ValueMember = "Subject_ID";
                AddBox.DataSource = dt_box;
            }
        }

        
        string connstr = "server=localhost;user id=root;password=1234;database=degreeproject";


        private void Form2_Load(object sender, EventArgs e)

        {
           

            using (MySqlConnection conn = new MySqlConnection(connstr))
            {
                conn.Open();
                try
                {
                    FillBox();
                    UpdateSubj();
                    AddSubj();

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
            Manager manage = new Manager();
            manage.Show();
            Close();
            
        }

        private void button2_Click(object sender, EventArgs e)
        {
            using (MySqlConnection conn = new MySqlConnection(connstr))
            {
                conn.Open();
                int value = Convert.ToInt32(AddBox.SelectedValue);
               
                try
                {
                    using (MySqlCommand cmd = new MySqlCommand("Update Grade g set g.Mark = @Grade WHERE @Student_ID = g.Student_ID AND @Subject_ID = g.Subject_ID", conn))
                    {

                        Subject = UpdateBox.Text;
                        Grade = GradeBox.Text;
                        cmd.Parameters.AddWithValue("@Subject_ID", value);
                        cmd.Parameters.AddWithValue("@Student_ID", Manager.Student_ID);
                        cmd.Parameters.AddWithValue("@S_Name", Subject);
                        cmd.Parameters.AddWithValue("@Grade", Grade);
                        cmd.ExecuteNonQuery();
                    }

                    FillBox();
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
            using (MySqlConnection conn = new MySqlConnection(connstr))
            {
                conn.Open();

                try
                {
                    Random rnd = new Random();
                    Studies_ID = rnd.Next(1, 99999);
                    Grade_ID = rnd.Next(1, 99999);
                    int value = Convert.ToInt32(AddBox.SelectedValue);
                             Subject = AddBox.Text;
                             Grade = GradeAdd.Text;
                            using (MySqlCommand cmd = new MySqlCommand("SELECT *FROM Studies WHERE S_Name LIKE @S_Name AND Student_ID = @Student_ID", conn))
                            {

                                cmd.Parameters.AddWithValue("@S_Name", Subject);
                                cmd.Parameters.AddWithValue("@Student_ID", Manager.Student_ID); 


                        var result = cmd.ExecuteScalar();

                                if (result == null)
                                {
                                using (MySqlCommand cmd2 = new MySqlCommand("INSERT INTO Studies(Studies_ID, Subject_ID, Student_ID, S_Name) " +
                               "VALUES(@Studies_ID, @Subject_ID, @Student_ID, @S_Name);" +
                               "INSERT INTO Grade (Grade_ID, Subject_ID, Student_ID, Mark) VALUES(@Grade_ID, @Subject_ID, @Student_ID, @Grade);", conn))
                                {
                                cmd2.Parameters.AddWithValue("@Grade_ID", Grade_ID);
                                cmd2.Parameters.AddWithValue("@Studies_ID", Studies_ID);
                                cmd2.Parameters.AddWithValue("@Subject_ID", value);
                                cmd2.Parameters.AddWithValue("@Student_ID", Manager.Student_ID);
                                cmd2.Parameters.AddWithValue("@S_Name", Subject);
                                cmd2.Parameters.AddWithValue("@Grade", Grade);
                                cmd2.ExecuteNonQuery();
                                }
                                 FillBox();
                                 UpdateSubj();
                                }

                                else
                                {
                           
                                MessageBox.Show("Subject was already added");
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

        private void button4_Click(object sender, EventArgs e)
        {
            using (MySqlConnection conn = new MySqlConnection(connstr))
            {

                conn.Open();
                try
                {
                    int rows;
                    var confirmResult = MessageBox.Show("Are you sure you want to delete this subject and it's grade?", "", MessageBoxButtons.YesNo);


                    if (confirmResult == DialogResult.Yes)
                    {
                        using (MySqlCommand cmd = new MySqlCommand("DELETE FROM Studies WHERE Student_ID = @Student_ID AND Subject_ID = @Subject_ID; DELETE FROM Grade WHERE Student_ID = @Student_ID AND Subject_ID = @Subject_ID", conn))
                        {
                            cmd.Parameters.AddWithValue("@Student_ID", Manager.Student_ID);
                            cmd.Parameters.AddWithValue("@Subject_ID", Subject_ID);
                            rows = cmd.ExecuteNonQuery();
                        }
                        if (rows > 0)
                        {
                            MessageBox.Show("Subject was deleted");
                            FillBox();
                        }
                        else
                        {
                            MessageBox.Show("Failed to delete subject");
                        }
                    }
                    FillBox();
                    UpdateSubj();

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


    }
}
