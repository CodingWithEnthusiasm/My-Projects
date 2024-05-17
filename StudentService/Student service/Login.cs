using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Student_service
{
    public partial class Login : Form
    {
        public Login()
        {
            InitializeComponent();
        }
        public static int ID { get; set; }
        public string  Email { get; set; }
        public string Password { get; set; }

        public string Role { get; set; }

      
        string connstr = "server=localhost;user id=root;password=1234;database=degreeproject";

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

        private void button1_Click(object sender, EventArgs e)
        {

            using (MySqlConnection conn = new MySqlConnection(connstr))
            {
                try
                {

                    ID = Int32.Parse(Regex.Match(IDBox.Text, @"\d+").Value);
                    Email = IDBox.Text;
                    Password = PasswordBox.Text;
                    Role = RoleBox.Text;

                    conn.Open();

                    using (MySqlCommand cmd = new MySqlCommand("SELECT *FROM Accounts WHERE Email LIKE @Email AND Password LIKE @Password AND Role LIKE @Role", conn))       
                    {
                      
                        cmd.Parameters.AddWithValue("@Email", Email);
                        cmd.Parameters.AddWithValue("@Password", Password);
                        cmd.Parameters.AddWithValue("@Role", Role);

                        var result = cmd.ExecuteScalar();

                        if (result != null)
                        {
                            switch (Role)
                            {
                                case "Manager":
                                    Manager Manager = new Manager();
                                    Manager.Show();
                                    Hide();
                                    break;

                                case "Student":
                                    List3 Student = new List3();
                                    Student.Show();
                                    Hide();
                                    break;
                            }
                        }

                        else
                        {
                            MessageBox.Show("Username does not exist");
                        }
                    }
                   
                       }
                catch (Exception ex)
                {

                    MessageBox.Show("Error while exporting Data" + ex.Message);
                }
                finally
                {
                    conn.Close();
                   
                }
            }

        }

    


        private void button2_Click(object sender, EventArgs e)
        {
            Close();
            if (Application.MessageLoop)
            {
                Application.Exit();
            }
            else
            {
                Environment.Exit(1);
            }
        }

    
    }
}

