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
using System.Security.Cryptography;

namespace TestSystemApp.Pages
{

    public partial class AuthPage : Page
    {
        public AuthPage()
        {
            InitializeComponent();
        }
        private void TextBoxLogin_Changed(object sender, RoutedEventArgs e)
        {
            txtHintLogin.Visibility = string.IsNullOrEmpty(TextBoxLogin.Text) ? Visibility.Visible : Visibility.Hidden;
        }
        public static string GetHash(string password)
        {
            using (var hash = SHA1.Create())
            {
                return string.Concat(hash.ComputeHash(Encoding.UTF8.GetBytes(password)).Select(x => x.ToString("X2")));
            }
        }
        private void PasswordBoxPassword_Changed(object sender, RoutedEventArgs e)
        {
            txtHintPassword.Visibility = string.IsNullOrEmpty(PasswordBoxPassword.Password) ? Visibility.Visible : Visibility.Hidden;
        }
        private void ButtonRegister_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new RegPage());
        }
        private void ButtonLogin_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(TextBoxLogin.Text) || string.IsNullOrEmpty(PasswordBoxPassword.Password))
            {
                MessageBox.Show("Введите логин и пароль!");
                return;
            }

            using (var DataBase = new Entities())
            {
                var user = DataBase.Users
                    .AsNoTracking()
                    .FirstOrDefault(u => u.Username == TextBoxLogin.Text);

                if (user == null)
                {
                    MessageBox.Show("Пользователь с такими данными не найден!");
                    return;
                }

                string hashedPassword = GetHash(PasswordBoxPassword.Password);

                // Проверка, используется ли хешированный пароль
                if (user.PasswordHash == PasswordBoxPassword.Password)
                {
                    // Пароль сохранён в открытом виде - хешируем его и обновляем запись
                    user.PasswordHash = hashedPassword;
                    DataBase.SaveChanges();
                }
                else if (user.PasswordHash != hashedPassword)
                {
                    // Неправильный пароль
                    MessageBox.Show("Неправильный пароль!");
                    return;
                }

                // Вход успешен
                MessageBox.Show("Вход выполнен успешно!");
                switch (user.Role)
                {
                    case "Student":
                        NavigationService?.Navigate(new StudentPage(user));
                        break;
                    case "Teacher":
                        NavigationService?.Navigate(new TeacherPage());
                        break;
                }
            }
        }



    }
}
