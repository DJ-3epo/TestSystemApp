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
using System.Windows.Threading;

namespace TestSystemApp
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

        }

        

        // Обработчик нажатия кнопки Назад
        private void BackButton_Click(object sender, RoutedEventArgs e)
        {
            if (MainFrame.CanGoBack) MainFrame.GoBack();

        }
        private void ExitApplication()
        {
            MessageBoxResult result = MessageBox.Show("Вы действительно хотите выйти из приложения?",
                                                      "Подтверждение выхода",
                                                      MessageBoxButton.YesNo,
                                                      MessageBoxImage.Question);
            if (result == MessageBoxResult.Yes)
            {
                Application.Current.Shutdown();
            }
        }

        // Вызов ExitApplication при нажатии на кнопку выхода
        private void ExitButton_Click(object sender, RoutedEventArgs e)
        {
            ExitApplication();
        }

        private void MainFrame_OnNavigated(object sender, NavigationEventArgs e)
        {
            // Проверяем, является ли контент страницы Page
            if (!(e.Content is Page page))
                return;

            // Устанавливаем заголовок приложения в соответствии с шаблоном
            this.Title = $"ProjectByKonashkov - {page.Title}"; // Замените на свою фамилию

            // Если текущая страница является AuthPage, скрываем кнопку "Назад",
            // иначе делаем ее видимой
            if (page is Pages.AuthPage)
                BackButton.Visibility = Visibility.Hidden;
            else
                BackButton.Visibility = Visibility.Visible;
        }

    }
}