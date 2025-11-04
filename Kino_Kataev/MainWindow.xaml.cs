using Kino_Kataev.Pages;
using System.Windows;

namespace Kino_Kataev
{
    public partial class MainWindow : Window
    {
        public static MainWindow mainWindow;
        public enum Pages
        {
            kinoteatr,
            afisha
        }
        public MainWindow()
        {
            InitializeComponent();
            mainWindow = this;
        }

        public void OpenPages(Pages page) 
        {
            if (page == Pages.kinoteatr) 
            {
                frame.Navigate(new Kinoteatr());
            }
            if (page == Pages.afisha) 
            {
                frame.Navigate(new Afisha());
            }
        }

        private void gotoAfishas(object sender, RoutedEventArgs e)
        {
            OpenPages(Pages.kinoteatr);
        }

        private void gotoKinoteatr(object sender, RoutedEventArgs e)
        {
            OpenPages(Pages.afisha);
        }
    }
}
