using GUI_project.GUIClasses;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;



namespace GUI_project
{
    public partial class GUI_project : Form
    {
        static string myconnstrng = ConfigurationManager.ConnectionStrings["connstrng"].ConnectionString;
        SqlConnection conn = new SqlConnection(myconnstrng);
        public GUI_project()
        {
            InitializeComponent();
        }
        Borrow b = new Borrow();


        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            comboBoxBook.ValueMember = "Id";
            int id;
            bool result = int.TryParse(comboBoxBook.SelectedValue.ToString(), out id);
            b.Book_Id = id;
        }


        private void button1_Click(object sender, EventArgs e)
        {
            conn.Open();
            try
            {
                b.Comments = textBoxComments.Text;
                if (!string.IsNullOrEmpty(comboBoxBook.Text))
                {
                    bool success = b.Insert(b);
                    if (success == true)
                        MessageBox.Show("Borrow data was added!");
                }
                else
                    MessageBox.Show("Failed to add borrow data");
                textBoxComments.Text = "";
                SqlCommand cmd = new SqlCommand("SELECT Title, Id FROM Books Bo WHERE NOT EXISTS(SELECT Book_Id FROM Borrows bor WHERE bor.Book_Id = Bo.Id)", conn);
                SqlDataReader reader = cmd.ExecuteReader();
                DataTable dt_box = new DataTable();
                dt_box.Load(reader);
                comboBoxBook.DataSource = dt_box;
                DataTable dt = b.Select();
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



        private void button2_Click(object sender, EventArgs e)
        {
            conn.Open();
            try
            {
                var confirmResult = MessageBox.Show("Are you sure you want to delete this borrow?", "", MessageBoxButtons.YesNo);


                if (confirmResult == DialogResult.Yes)
                {

                    bool success = b.Delete(b, "DELETE FROM Borrows WHERE Id = @Id", "@Id", b.Id);
                    if (success == true)
                    {
                        MessageBox.Show("Borrow was deleted");
                        DataTable dt = b.Select();
                        List.DataSource = dt;
                    }
                    else
                    {
                        MessageBox.Show("Failed to delete borrow");
                    }

                }


                SqlCommand cmd = new SqlCommand("SELECT Title, Id FROM Books Bo WHERE NOT EXISTS(SELECT Book_Id FROM Borrows bor WHERE (BorrowDate ='' AND BorrowDate !='') AND (BorrowDate ='' OR BorrowDate !='') ) ", conn);
                SqlDataReader reader = cmd.ExecuteReader();
                DataTable dt_box = new DataTable();
                dt_box.Columns.Add("Title", typeof(string));
                dt_box.Load(reader);
                comboBoxBook.DataSource = dt_box;
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
        private void button3_Click(object sender, EventArgs e)
        {

            try
            {
                bool success = b.Return(b);
                if (success == true)
                {

                    List.Update();
                    List.Refresh();
                    MessageBox.Show("ReturnData was added");
                    DataTable dt = b.Select();
                    List.DataSource = dt;

                }
                else
                {
                    MessageBox.Show("Failed to set ReturnData");
                }
                conn.Open();

                SqlCommand cmd = new SqlCommand("SELECT Title, Id FROM Books Bo WHERE NOT EXISTS(SELECT Book_Id FROM Borrows bor WHERE (BorrowDate ='' AND BorrowDate !='') AND (BorrowDate ='' OR BorrowDate !='') ) ", conn);
                SqlDataReader reader = cmd.ExecuteReader();
                DataTable dt_box = new DataTable();
                dt_box.Columns.Add("Title", typeof(string));
                dt_box.Load(reader);
                comboBoxBook.ValueMember = "Title";
                comboBoxBook.DataSource = dt_box;
                reader.Dispose();
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

        public string DisplayName
        {
            get
            {
                return this.NameBox + " " + b.Surname;
            }
        }
        private void Tables_Load(object sender, EventArgs e)
        {
            DataTable dt_s = b.Select();
            List.DataSource = dt_s;
            List.Columns["Id"].Visible = false;

            DataTable dt_s_b = b.Select_Book();
            BooksList.DataSource = dt_s_b;
            BooksList.Columns["Id"].Visible = false;

            DataTable dt_s_r = b.Select_Reader();
            ReadersList.DataSource = dt_s_r;
            ReadersList.Columns["Id"].Visible = false;



            try
            {
                conn.Open();


                SqlCommand cmd = new SqlCommand("SELECT Title, Id FROM Books Bo WHERE NOT EXISTS(SELECT Book_Id FROM Borrows bor WHERE (BorrowDate ='' AND BorrowDate !='') AND (BorrowDate ='' OR BorrowDate !='') ) ", conn);
                SqlDataReader reader = cmd.ExecuteReader();
                DataTable dt_box = new DataTable();
                dt_box.Columns.Add("Title", typeof(string));
                dt_box.Load(reader);
                comboBoxBook.ValueMember = "Title";
                comboBoxBook.DataSource = dt_box;


                cmd = new SqlCommand("SELECT Id,Concat(Name,' ',Surname) AS FirstLast FROM Readers", conn);
                reader = cmd.ExecuteReader();
                dt_box = new DataTable();
                dt_box.Columns.Add("FirstLast", typeof(string));
                dt_box.Load(reader);
                comboBoxReader.ValueMember = "FirstLast";
                comboBoxReader.DataSource = dt_box;
                reader.Dispose();


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

        private void List_RowHeaderMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            DataGridView grid = sender as DataGridView;
            int rowIndex = e.RowIndex;

            if (grid.Rows[rowIndex].Cells[0].Value != System.DBNull.Value)
            {
                int id = Convert.ToInt32(grid.Rows[rowIndex].Cells[0].Value);
                if (grid.Tag.ToString() == "Book")
                    b.Click_book = id;
                else if (grid.Tag.ToString() == "Reader")
                    b.Click_reader = id;
                else if (grid.Tag.ToString() == "List")
                    b.Id = id;
            }
        }



        private void comboBox1_SelectedIndexChanged_1(object sender, EventArgs e)
        {

            try
            {
                comboBoxReader.ValueMember = "Id";
                int id;
                bool result = int.TryParse(comboBoxReader.SelectedValue.ToString(), out id);
                b.Reader_Id = id;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }



        private void button1_Click_1(object sender, EventArgs e)
        {
            conn.Open();
            try
            {
                b.Title = AuthorBox.Text;
                b.Author = AuthorBox.Text;
                b.ISBN = ISBNBox.Text;
                b.Amount = Int32.Parse(AmountBox.Text);

                bool success = b.Insert_Book(b);
                if (success == true)
                    MessageBox.Show("Book was added");

                else
                {
                    MessageBox.Show("Failed to add book");
                }
                AuthorBox.Text = "";
                AuthorBox.Text = "";
                ISBNBox.Text = "";
                AmountBox.Text = "";
                SqlCommand cmd = new SqlCommand("SELECT Title, Id FROM Books", conn);
                SqlDataReader reader = cmd.ExecuteReader();
                DataTable dt_box = new DataTable();
                dt_box.Load(reader);
                comboBoxBook.DataSource = dt_box;
                DataTable dt = b.Select_Book();
                BooksList.DataSource = dt;
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



        private void button3_Click_1(object sender, EventArgs e)
        {
            conn.Open();
            try
            {
                b.Name = NameBox.Text;
                b.Surname = SurnameBox.Text;
                b.Email = EmailBox.Text;
                b.PhoneNumber = PhoneBox.Text;

                bool success = b.Insert_Readers(b);
                if (success == true)
                    MessageBox.Show("Book was added");

                else
                {
                    MessageBox.Show("Failed to add reader");
                }
                NameBox.Text = "";
                SurnameBox.Text = "";
                EmailBox.Text = "";
                PhoneBox.Text = "";
                SqlCommand cmd = new SqlCommand("SELECT Id FROM Readers", conn);
                SqlDataReader reader = cmd.ExecuteReader();
                DataTable dt_box = new DataTable();
                dt_box.Load(reader);
                comboBoxReader.DataSource = dt_box;
                DataTable dt = b.Select_Reader();
                ReadersList.DataSource = dt;
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

        private void button5_Click(object sender, EventArgs e)
        {
            conn.Open();
            try
            {
                var confirmResult = MessageBox.Show("Are you sure you want to remove all books of this type ?", "", MessageBoxButtons.YesNo);


                if (confirmResult == DialogResult.Yes)
                {

                    bool success = b.Delete(b, "DELETE FROM Borrows WHERE Book_Id = @Click_book;DELETE FROM Books WHERE Id = @Click_book;", "@Click_book", b.Click_book);
                    if (success == true)
                    {
                        MessageBox.Show("Book was deleted");
                        DataTable dt = b.Select_Book();
                        BooksList.DataSource = dt;
                    }
                    else
                    {
                        MessageBox.Show("Failed to delete book");
                    }

                }


                SqlCommand cmd = new SqlCommand("SELECT Title, Id FROM Books Bo WHERE NOT EXISTS(SELECT Book_Id FROM Borrows bor WHERE (BorrowDate ='' AND BorrowDate !='') AND (BorrowDate ='' OR BorrowDate !='') ) ", conn);
                SqlDataReader reader = cmd.ExecuteReader();
                DataTable dt_box = new DataTable();
                dt_box.Columns.Add("Title", typeof(string));
                dt_box.Load(reader);
                comboBoxBook.DataSource = dt_box;
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

        private void button7_Click(object sender, EventArgs e)
        {
            conn.Open();
            try
            {
                var confirmResult = MessageBox.Show("Are you sure you want to remove this reader ?", "", MessageBoxButtons.YesNo);


                if (confirmResult == DialogResult.Yes)
                {

                    bool success = b.Delete(b, "DELETE FROM Borrows WHERE Reader_Id = @Click_reader;DELETE FROM Readers WHERE Id = @Click_reader;", "@Click_reader", b.Click_reader);
                    if (success == true)
                    {
                        MessageBox.Show("Reader was deleted");
                        DataTable dt = b.Select_Reader();
                        ReadersList.DataSource = dt;
                    }
                    else
                    {
                        MessageBox.Show("Failed to delete reader");
                    }

                }


                SqlCommand cmd = new SqlCommand("SELECT Title, Id FROM Books Bo WHERE NOT EXISTS(SELECT Book_Id FROM Borrows bor WHERE (BorrowDate ='' AND BorrowDate !='') AND (BorrowDate ='' OR BorrowDate !='') ) ", conn);
                SqlDataReader reader = cmd.ExecuteReader();
                DataTable dt_box = new DataTable();
                dt_box.Columns.Add("Title", typeof(string));
                dt_box.Load(reader);
                comboBoxBook.DataSource = dt_box;
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
