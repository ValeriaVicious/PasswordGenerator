using System;
using System.Windows;

namespace PasswordGenerator
{
    public partial class App : Application
    {
        [STAThread]
        private static void Main(string[] args)
        {
            var app = new App();

            app.InitializeComponent();
            app.Run(PasswordGenerator.MainWindow.Instance);
        }
    }
}
