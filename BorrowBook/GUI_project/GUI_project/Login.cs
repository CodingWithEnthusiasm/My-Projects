using System;
using System.Windows.Forms;




namespace GUI_project
{
    public partial class Login : Form
    {

        public string LoginCheck { get; set; }

        public string PasswordCheck { get; set; }
        private string trueLogin = "admin";
        private string truePassword = "123";
        public Login()
        {
            InitializeComponent();
        }


        private void button1_Click(object sender, EventArgs e)
        {
            LoginCheck = LoginBox.Text;
            PasswordCheck = PasswordBox.Text;
            try
            {


                if (LoginCheck == trueLogin && PasswordCheck == truePassword)
                {
                    GUI_project form2 = new GUI_project();
                    form2.ShowDialog();
                }
                else
                {
                    MessageBox.Show("Wrong email or password");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
    }
}
