using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace IPD12_SuperExpress
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }
     

        private void btnRegister_Click(object sender, RoutedEventArgs e)
        {
            Register dlg = new Register();
            dlg.ShowDialog();
        }

        private void btnLogin_Click(object sender, RoutedEventArgs e)
        {
            
            
            string email = tbEmail.Text;
            if (string.IsNullOrWhiteSpace(email))
            {
                MessageBox.Show("Please enter an email", "Input error", MessageBoxButton.OK, MessageBoxImage.Warning);
                tbEmail.Text = "";
                tbEmail.Focus();
                return;
            }
            MatchCollection mc = Regex.Matches(email, Globals.emailExpression);
            if (mc.Count == 0)
            {
                MessageBox.Show("Please enter a valid email(example@gmail.com)", "Input error", MessageBoxButton.OK, MessageBoxImage.Warning);
                tbEmail.Text = "";
                tbEmail.Focus();
                return;
            }

            Globals.currentUser = Globals.db.GetUserByEmail(email);
             
            if (Globals.currentUser != null)
            {
                string passwordInDB = Globals.currentUser.Password;
                string passwordInput = pbPassword.Password;
                if (passwordInDB != string.Empty)
                {
                    // Check encripted password
                    if (Globals.VerifyMd5Hash(passwordInput, passwordInDB))
                    {
                        this.Hide();
                        MainDialog dlg = new MainDialog();
                        dlg.ShowDialog();

                        this.Close();                        

                    }
                    else
                    {
                        pbPassword.Password = "";
                        pbPassword.Focus();
                        MessageBox.Show("Your password is not correct!", "Input error", MessageBoxButton.OK, MessageBoxImage.Warning);
                    }
                }
            }
            else
            {
                //tbEmail.Text = "";
                tbEmail.Focus();
                MessageBox.Show("This email account does not exist!", "Input error", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
 
        }
    }
}
