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
    public partial class Form2 : Form
    {
        public int Student { get; set; }
        public string Subject { get; set; }
        public string Grade { get; set; }
        public Form1 f;
        public Form2()
        {
            InitializeComponent();
           
        }




        private bool dragging = false;
        private Point dragCursorPoint;
        private Point dragFormPoint;

        private void Form1_MouseDown(object sender, MouseEventArgs e)
        {
            dragging = true;
            dragCursorPoint = Cursor.Position;
            dragFormPoint = this.Location;
        }

        private void Form1_MouseMove(object sender, MouseEventArgs e)
        {
            if (dragging)
            {
                Point dif = Point.Subtract(Cursor.Position, new Size(dragCursorPoint));
                this.Location = Point.Add(dragFormPoint, new Size(dif));
            }
        }

        private void Form1_MouseUp(object sender, MouseEventArgs e)
        {
            dragging = false;
        }


      
        MySqlCommand sqlCmd2 = new MySqlCommand();
        DataTable dt = new DataTable();
        string connstr = "server=localhost;user id=root;password=1234;database=degreeproject";
        MySqlDataAdapter adapter2 = new MySqlDataAdapter();
        //MySqlDataReader reader2;
        
        private void Form2_Load(object sender, EventArgs e)

        {

          
            using (MySqlConnection conn = new MySqlConnection(connstr))
            {
                conn.Open();
               // try
               // {
                   
                    GradeBox.SelectedItem = "5";
                    sqlCmd2 = new MySqlCommand("SELECT s.Subject, Mark FROM Grade g, Subjects s WHERE g.Student_ID = s.Student_ID AND g.Subject_ID = s.Subject_ID AND @ID = s.Student_ID AND @ID = g.Student_ID", conn);
                    sqlCmd2.Parameters.AddWithValue("@ID", Form1.Student_ID);
                    using (MySqlDataReader reader2 = sqlCmd2.ExecuteReader())
                    {
                        if (reader2 != null)
                        {
                                dt.Load(reader2);
                                List2.DataSource = dt;
                        }
                    }
                    //reader2.Dispose();
                    //reader2.Close();
                    


                   /*
                    sqlCmd2 = new MySqlCommand("SELECT Subject FROM Subjects su,Student st WHERE st.Student_ID = su.Student_ID", conn);
                    DataTable dt_box = new DataTable();
                    using (MySqlDataReader reader2 = sqlCmd2.ExecuteReader())
                    {
                        //reader2 = sqlCmd2.ExecuteReader();
                        if (reader2 != null)
                        {
                            while (reader2.Read())
                            {
                                dt_box.Columns.Add("Subject", typeof(string));
                                dt_box.Load(reader2);
                                SubjectBox.ValueMember = "Subject";
                            }
                        }
                    }
                    SubjectBox.DataSource = dt_box;
                   */
                //}
                //catch (Exception ex)
               // {
               //     MessageBox.Show(ex.Message);
                //}
                //finally
                //{
                    //conn.Close();
               // }
            }
        }

        private void g_List_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void label2_Click(object sender, EventArgs e)
        {

        }

        private void button3_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            //using (MySqlConnection conn = new MySqlConnection(connstr))
           // {
                /*
                try
                {
                    conn.Open();
                    sqlCmd2 = new MySqlCommand("Update Grade g, Subjects s set g.Mark = @Grade WHERE  @Student_ID = s.Student_ID  AND g.Subject_ID = s.Subject_ID AND s.Subject = @Subject ", conn);

                    Subject = SubjectBox.Text;
                    Grade = GradeBox.Text;
          

                    sqlCmd2.Parameters.AddWithValue("@Student_ID", Form1.Student_ID);
                    sqlCmd2.Parameters.AddWithValue("@Subject", Subject);
                    sqlCmd2.Parameters.AddWithValue("@Grade", Grade);

                    sqlCmd2.ExecuteNonQuery();

                    adapter2 = new MySqlDataAdapter("SELECT s.Subject, Mark FROM Grade g, Subjects s WHERE g.Student_ID = s.Student_ID AND g.Subject_ID = s.Subject_ID", conn);
                    dt = new DataTable();
                    dt.Load(reader2);
                    adapter2.Fill(dt);
                    List2.DataSource = dt;
                    MessageBox.Show("Grade was inserted");
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
                finally
                {
                    conn.Close();
                }
                */
            //}

        }

        }
}
