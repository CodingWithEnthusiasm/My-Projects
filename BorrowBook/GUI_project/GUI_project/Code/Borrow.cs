using System.Configuration;
using System.Data;
using System.Data.SqlClient;



namespace GUI_project.GUIClasses
{
    internal class Borrow
    {
        public int Id { get; set; }
        public int Reader_Id { get; set; }
        public int Book_Id { get; set; }
        public int Click_book { get; set; }
        public int Click_reader { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
        public string PhoneNumber { get; set; }
        public string Email { get; set; }
        public string Title { get; set; }
        public string Author { get; set; }
        public string ISBN { get; set; }
        public int Amount { get; set; }
        public string Comments { get; set; }
        public DateTime BorrowDate { get; set; }
        public DateTime ReturnDate { get; set; }


        static string myconnstrng = ConfigurationManager.ConnectionStrings["connstrng"].ConnectionString;
        SqlConnection conn = new SqlConnection(myconnstrng);


        public DataTable Select_Book()
        {
            return Select_Data("SELECT Id, Title, Author, ISBN,Amount FROM Books");
        }
        public DataTable Select_Reader()
        {
            return Select_Data("SELECT Id, Name, Surname, Email, PhoneNumber FROM Readers");
        }
        public DataTable Select()
        {
            return Select_Data("SELECT bor.Id, ISBN,Title, Name, Surname, PhoneNumber, BorrowDate, ReturnDate, Comments FROM Borrows bor JOIN Books b ON b.Id = bor.Book_Id JOIN Readers r ON bor.Reader_Id = r.Id");
        }


        public DataTable Select_Data(string datasql)
        {

            SqlConnection conn = new SqlConnection(myconnstrng);
            DataTable dt = new DataTable();
            try
            {
                SqlCommand cmd = new SqlCommand(datasql, conn);

                SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                conn.Open();
                adapter.Fill(dt);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            finally
            {
                conn.Close();
            }


            return dt;

        }

        public bool Insert(Borrow b)
        {
            bool isSuccess = false;
            SqlConnection conn = new SqlConnection(myconnstrng);
            try
            {
                b.BorrowDate = DateTime.Now;

                SqlCommand cmd = new SqlCommand("INSERT INTO Borrows (Reader_Id, Book_Id,BorrowDate, Comments) VALUES (@Reader_Id, @Book_Id, @BorrowDate, @Comments)", conn);
                cmd.Parameters.AddWithValue("@Reader_Id", b.Reader_Id);
                cmd.Parameters.AddWithValue("@Book_Id", b.Book_Id);
                cmd.Parameters.AddWithValue("@BorrowDate", b.BorrowDate);
                cmd.Parameters.AddWithValue("@Comments", b.Comments);
                conn.Open();
                int rows = cmd.ExecuteNonQuery();

                if (rows > 0)
                {
                    isSuccess = true;
                }
                else
                {
                    isSuccess = false;
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


            return isSuccess;
        }



        public bool Insert_Book(Borrow b)
        {
            bool isSuccess = false;
            SqlConnection conn = new SqlConnection(myconnstrng);
            try
            {

                SqlCommand cmd = new SqlCommand("INSERT INTO Books (Title, Author,ISBN, Amount) VALUES (@Title, @Author, @ISBN, @Amount)", conn);
                cmd.Parameters.AddWithValue("@Title", b.Title);
                cmd.Parameters.AddWithValue("@Author", b.Author);
                cmd.Parameters.AddWithValue("@ISBN", b.ISBN);
                cmd.Parameters.AddWithValue("@Amount", b.Amount);
                conn.Open();
                int rows = cmd.ExecuteNonQuery();

                if (rows > 0)
                {
                    isSuccess = true;
                }
                else
                {
                    isSuccess = false;
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


            return isSuccess;
        }


        public bool Insert_Readers(Borrow b)
        {
            bool isSuccess = false;
            SqlConnection conn = new SqlConnection(myconnstrng);
            try
            {


                SqlCommand cmd = new SqlCommand("INSERT INTO Readers (Name, Surname ,PhoneNumber, Email) VALUES (@Name, @Surname, @PhoneNumber, @Email)", conn);
                cmd.Parameters.AddWithValue("@Name", b.Name);
                cmd.Parameters.AddWithValue("@Surname", b.Surname);
                cmd.Parameters.AddWithValue("@PhoneNumber", b.PhoneNumber);
                cmd.Parameters.AddWithValue("@Email", b.Email);
                conn.Open();
                int rows = cmd.ExecuteNonQuery();

                if (rows > 0)
                {
                    isSuccess = true;
                }
                else
                {
                    isSuccess = false;
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


            return isSuccess;
        }


        public bool Return(Borrow b)
        {
            b.ReturnDate = DateTime.Now;

            bool isSuccess = false;

            SqlConnection conn = new SqlConnection(myconnstrng);
            conn.Open();
            try
            {
                SqlCommand cmd = new SqlCommand("UPDATE Borrows SET ReturnDate = @ReturnDate WHERE ( Id=@Id ) AND ReturnDate IS NULL  ", conn);


                cmd.Parameters.AddWithValue("@ReturnDate", b.ReturnDate);
                cmd.Parameters.AddWithValue("@Id", b.Id);

                int rows = cmd.ExecuteNonQuery();

                if (rows > 0)
                {
                    isSuccess = true;
                }
                else
                    isSuccess = false;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            finally
            {
                conn.Close();
            }
            return isSuccess;


        }

        public bool Delete(Borrow b, string s, string var, int Id)
        {
            bool isSuccess = false;
            SqlConnection conn = new SqlConnection(myconnstrng);
            try
            {

                conn.Open();
                SqlCommand cmd = new SqlCommand(s, conn);
                cmd.Parameters.AddWithValue(var, Id);
                int rows = cmd.ExecuteNonQuery();
                if (rows > 0)
                {
                    isSuccess = true;
                }
                else
                {
                    isSuccess = false;
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
            return isSuccess;
        }



    }
}
