using MySql.Data.MySqlClient;
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
using System.Windows.Shapes;

namespace IPD12_SuperExpress
{
    /// <summary>
    /// Interaction logic for Register.xaml
    /// </summary>
    public partial class Register : Window
    {
        List<Country> countryList = new List<Country>();
        List<Province> provinceList = new List<Province>();

        public Register()
        {
            InitializeComponent();
            InitializeDataFromDatabase();
            InitializeRegisterWindow();
        }

        private void InitializeDataFromDatabase()
        {
            try
            {
                countryList = Globals.db.GetAllCountry();
                //provinceList = Globals.db.GetAllProvice();
            }
            catch (MySqlException ex)
            {
                Console.WriteLine(ex.StackTrace);
                Console.WriteLine("Error fetching country or provice data from database: " + ex.Message);
                Environment.Exit(1);
            }
        }

        private void InitializeRegisterWindow()
        {
            regDlg_tbUserId.Clear();
            regDlg_pbPassword.Clear();
            regDlg_pbConfirmPassword.Clear();
            regDlg_tbName.Clear();
            regDlg_tbPhoneNumber.Clear();
            regDlg_tbEmail.Text = Globals.DEFAULT_EMAIL;
            regDlg_cbCountry.ItemsSource = countryList;
            regDlg_cbCountry.Text = Globals.COUNTRY_CANADA;
            regDlg_cbProvince.SelectedIndex = 0;
            regDlg_tbCity.Clear();
            regDlg_tbAddress.Clear();
            regDlg_tbPostalCode.Clear();
        }

        private void regDlg_tbEmail_GotFocus(object sender, RoutedEventArgs e)
        {
            if (regDlg_tbEmail.Text.Equals(Globals.DEFAULT_EMAIL))
            {
                regDlg_tbEmail.Text = "";
            }
            
        }

        private void regDlg_cbCountry_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            string countryCode = ((Country)regDlg_cbCountry.SelectedItem).Code;
            List<Province> provinceInSelectedCountryList = Globals.db.GetAllProviceByCountryCode(countryCode);
            regDlg_cbProvince.ItemsSource = provinceInSelectedCountryList;
            regDlg_cbProvince.SelectedIndex = 0;
        }

        private void btRegister_Click(object sender, RoutedEventArgs e)
        {
            string userId = regDlg_tbUserId.Text;
            if (string.IsNullOrWhiteSpace(userId))
            {
                MessageBox.Show("Please enter your User ID.", "Input error", MessageBoxButton.OK, MessageBoxImage.Warning);
                regDlg_tbUserId.Clear();
                regDlg_tbUserId.Focus();
                return;
            }
            string password1 = regDlg_pbPassword.Password;
            string password2 = regDlg_pbConfirmPassword.Password;
            if (string.IsNullOrWhiteSpace(password1))
            {
                MessageBox.Show("Please enter a password.", "Input error", MessageBoxButton.OK, MessageBoxImage.Warning);
                regDlg_pbPassword.Clear();
                regDlg_pbPassword.Focus();
                return;
            }
            if (string.IsNullOrWhiteSpace(password2))
            {
                MessageBox.Show("Please enter confirm password.", "Input error", MessageBoxButton.OK, MessageBoxImage.Warning);
                regDlg_pbConfirmPassword.Clear();
                regDlg_pbConfirmPassword.Focus();
                return;
            }
            if (password1.CompareTo(password2) != 0)
            {
                MessageBox.Show("Two password are not matched!", "Input error", MessageBoxButton.OK, MessageBoxImage.Warning);
                regDlg_pbPassword.Clear();
                regDlg_pbConfirmPassword.Clear();
                regDlg_pbPassword.Focus();
                return;
            }
            string name = regDlg_tbName.Text;
            if (string.IsNullOrWhiteSpace(name))
            {
                MessageBox.Show("Please enter your name.", "Input error", MessageBoxButton.OK, MessageBoxImage.Warning);
                regDlg_tbName.Clear();
                regDlg_tbName.Focus();
                return;
            }
            long phone;
            if (!long.TryParse(regDlg_tbPhoneNumber.Text, out phone))
            {
                MessageBox.Show("Please enter a digit number.", "Input error", MessageBoxButton.OK, MessageBoxImage.Warning);
                regDlg_tbPhoneNumber.Clear();
                regDlg_tbPhoneNumber.Focus();
                return;
            }
            string email = regDlg_tbEmail.Text;
            if (string.IsNullOrWhiteSpace(email))
            {
                MessageBox.Show("Please enter an email.", "Input error", MessageBoxButton.OK, MessageBoxImage.Warning);
                regDlg_tbEmail.Clear();
                regDlg_tbEmail.Focus();
                return;
            }
            MatchCollection mc = Regex.Matches(email, Globals.emailExpression);
            if (mc.Count == 0)
            {
                MessageBox.Show("Please enter an valid email(example@superexpress.com).", "Input error", MessageBoxButton.OK, MessageBoxImage.Warning);
                regDlg_tbEmail.Clear();
                regDlg_tbEmail.Focus();
                return;
            }
            
            string countryCode = ((Country)regDlg_cbCountry.SelectedItem).Code;
            string provinceCode = ((Province)regDlg_cbProvince.SelectedItem).ProvinceStateCode;
            string cityName = regDlg_tbCity.Text;
            if (string.IsNullOrWhiteSpace(cityName))
            {
                MessageBox.Show("Please enter your city.", "Input error", MessageBoxButton.OK, MessageBoxImage.Warning);
                regDlg_tbCity.Clear();
                regDlg_tbCity.Focus();
                return;
            }
            string address = regDlg_tbAddress.Text;
            if (string.IsNullOrWhiteSpace(address))
            {
                MessageBox.Show("Please enter your address.", "Input error", MessageBoxButton.OK, MessageBoxImage.Warning);
                regDlg_tbAddress.Clear();
                regDlg_tbAddress.Focus();
                return;
            }
            string postalCode = regDlg_tbPostalCode.Text;
            if (string.IsNullOrWhiteSpace(postalCode))
            {
                MessageBox.Show("Please enter your Postal Code.", "Input error", MessageBoxButton.OK, MessageBoxImage.Warning);
                regDlg_tbPostalCode.Clear();
                regDlg_tbPostalCode.Focus();
                return;
            }

            try
            {

                // Encript Password
                string hashPassword = Globals.GetMd5Hash(password1);

                User user = new User(0, userId, hashPassword, name, phone, email, countryCode, provinceCode, cityName, address, postalCode);
                int newId = Globals.db.AddUser(user);

                //Globals.currentUser.Id = newId;
                //Globals.currentUser.Password = string.Empty;

                MessageBox.Show("Add user Successfully, \nplease login by using your email.", "Succusss", MessageBoxButton.OK, MessageBoxImage.Information);
                this.Close();
            }
            catch (MySqlException ex)
            {
                if (ex.Number == 1062)
                {
                    MessageBox.Show("Your UserID or email are duplicate!" , "Input error", MessageBoxButton.OK, MessageBoxImage.Error);
                }else
                {
                    MessageBox.Show("Add user failure!", "Database error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }

        }

        private void btClear_Click(object sender, RoutedEventArgs e)
        {
            InitializeRegisterWindow();
        }
    }
}
