using iTextSharp.text;
using iTextSharp.text.pdf;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Student_service
{



    public partial class List3 : Form
    {
        Login log = new Login();
        MySqlCommand sqlCmd = new MySqlCommand();
        DataTable dt = new DataTable();
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

        public int ID { get; set; }

        public List3()
        {
            InitializeComponent();
        }


        private void List3_Load(object sender, EventArgs e)
        {
            using (MySqlConnection conn = new MySqlConnection(connstr))
            {

                ID = Login.ID;
                try
                {
                    conn.Open();

                   
                    using (MySqlCommand cmd = new MySqlCommand("SELECT s.S_Name as Subject, Mark FROM Grade g, Studies s WHERE g.Student_ID = s.Student_ID AND g.Subject_ID = s.Subject_ID AND @ID = s.Student_ID AND @ID = g.Student_ID", conn))
                    {
                        cmd.Parameters.AddWithValue("@ID", ID);
                     
                        using (MySqlDataReader reader = cmd.ExecuteReader())
                        {
                            dt.Load(reader);
                        }
                       
                    }
                    List_s.DataSource = dt;
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
            var confirmResult = MessageBox.Show("Are you sure you want to log out?", "", MessageBoxButtons.YesNo);
            if (confirmResult == DialogResult.Yes)
            {
                log.Show();
                Close();
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (List_s.Rows.Count > 0)
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
                            PdfPTable Table = new PdfPTable(List_s.Columns.Count);
                           
                            Table.WidthPercentage = 100;
                            foreach (DataGridViewColumn col in List_s.Columns)
                            {
                                PdfPCell pCell = new PdfPCell(new Phrase(col.HeaderText));
                                Table.AddCell(pCell);
                            }
                            foreach (DataGridViewRow viewRow in List_s.Rows)
                            {
                                foreach (DataGridViewCell dcell in viewRow.Cells)
                                {
                                    Table.AddCell(dcell.Value.ToString());
                                }
                            }
                            using (FileStream fileStream = new FileStream(save.FileName, FileMode.Create))
                            {
                                Document document = new Document(PageSize.A7, 5f, 5f, 10f, 10f);
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

    }
}
