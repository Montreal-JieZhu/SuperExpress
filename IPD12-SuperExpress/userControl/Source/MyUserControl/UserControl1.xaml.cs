using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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

namespace MyUserControl
{
    /// <summary>
    /// Interaction logic for UserControl1.xaml
    /// </summary>
    public partial class WaterMarkTextBox : UserControl
    {
        private const string DefaultText = "";

        public WaterMarkTextBox()
        {
            InitializeComponent();
            this.Loaded += new RoutedEventHandler(WateMarkTextbox_Loaded);

            this.GotFocus += new RoutedEventHandler(WateMarkTextbox_GotFocus);

            this.LostFocus += new RoutedEventHandler(WateMarkTextbox_LostFocus);

            this.tbWMTextBox.TextChanged += new TextChangedEventHandler(tbWMTextBox_TextChanged);
        }

        void tbWMTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (!string.IsNullOrWhiteSpace(this.tbWMTextBox.Text) || this.tbWMTextBox.IsFocused)
            {
                this.tbWMTextBox.Text = this.tbWMTextBox.Text;
                this.tbWMTextBox.Foreground = Brushes.Black;
            }
            else
            {
                this.tbWMTextBox.Text = Watermark;
                this.tbWMTextBox.Foreground = Brushes.DarkGray;
            }
            this.Text = this.tbWMTextBox.Text;
        }

        void WateMarkTextbox_LostFocus(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(this.tbWMTextBox.Text))
            {
                this.tbWMTextBox.Text = Watermark;
                this.tbWMTextBox.Foreground = Brushes.DarkGray;
            }
        }

        void WateMarkTextbox_GotFocus(object sender, RoutedEventArgs e)
        {
            if (this.tbWMTextBox.Text.Equals(Watermark))
            {
                this.tbWMTextBox.Text = string.Empty;
            }
        }

        void WateMarkTextbox_Loaded(object sender, RoutedEventArgs e)
        {
            this.tbWMTextBox.Text = Watermark;
            this.tbWMTextBox.Foreground = Brushes.DarkGray;
        }

        public void Clear()
        {
            this.tbWMTextBox.Text = string.Empty;
        }

        public string Watermark
        {
            get
            {
                string result = (string)GetValue(WatermarkProperty);

                if (string.IsNullOrEmpty(result))
                {
                    result = DefaultText;
                }

                return result;
            }

            set { SetValue(WatermarkProperty, value); }
        }

        public string Text
        {
            get
            {
                //string result = 
                return (string)GetValue(TextProperty);
            }

            set
            {
                SetValue(TextProperty, value);
            }
        }

        public static readonly DependencyProperty WatermarkProperty = DependencyProperty.Register("Watermark", typeof(string), typeof(UserControl), new UIPropertyMetadata(DefaultText));
        public static DependencyProperty TextProperty = DependencyProperty.Register("Text", typeof(string), typeof(UserControl), new UIPropertyMetadata(DefaultText));
    }
}
