using System;
using System.Text;
using System.Windows;


namespace PasswordGenerator
{
    public partial class MainWindow : Window
    {
        #region Fields

        private static StringBuilder _result = new StringBuilder();
        private static Random _random = new Random();

        private static bool _addUpperCase;
        private static bool _addNumbers;
        private static bool _addSymbols;
        private static string _validChars = string.Empty;

        #endregion


        #region ClassLifeCycles

        public MainWindow()
        {
            InitializeComponent();
        }


        #endregion

        #region Methods

        private static string GeneratePassword(int length)
        {
            if (_addUpperCase && _addNumbers && _addSymbols)
            {
                _validChars = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ1234567890?!@#$%^&*";
            }
            else if (_addUpperCase && _addNumbers)
            {
                _validChars = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ1234567890";
            }
            else if (_addUpperCase && _addSymbols)
            {
                _validChars = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ?!@#$%^&*";
            }
            else if (_addNumbers && _addSymbols)
            {
                _validChars = "abcdefghijklmnopqrstuvwxyz1234567890?!@#$%^&*";
            }
            else if (_addUpperCase)
            {
                _validChars = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ";
            }
            else if (_addNumbers)
            {
                _validChars = "abcdefghijklmnopqrstuvwxyz1234567890";
            }
            else if (_addSymbols)
            {
                _validChars = "abcdefghijklmnopqrstuvwxyz?!@#$%^&*";
            }
            else
            {
                _validChars = "abcdefghijklmnopqrstuvwxyz";
            }

            while (0 < length--)
            {
                _result.Append(_validChars[_random.Next((_validChars.Length))]);
            }
            return _result.ToString();
        }

        #endregion


        #region Events

        private void btnGeneratePassword_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                int length = int.Parse(comboBox.Text);
                string generatedPassword = GeneratePassword(length);
                showPassword.Content = generatedPassword;
            }
            catch (Exception)
            {
                MessageBox.Show("Please select a password length.",
                    "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void uppercaseCheckBox_Checked(object sender, RoutedEventArgs e)
        {
            _addUpperCase = true;
        }

        private void uppercaseCheckBox_Unchecked(object sender, RoutedEventArgs e)
        {
            _addUpperCase = false;
        }

        private void numbersCheckBox_Checked(object sender, RoutedEventArgs e)
        {
            _addNumbers = true;
        }

        private void numbersCheckBox_Unchecked(object sender, RoutedEventArgs e)
        {
            _addNumbers = false;
        }

        private void symbolsCheckBox_Checked(object sender, RoutedEventArgs e)
        {
            _addSymbols = true;
        }

        private void symbolsCheckBox_Unchecked(object sender, RoutedEventArgs e)
        {
            _addSymbols = false;
        }

        #endregion
    }
}
