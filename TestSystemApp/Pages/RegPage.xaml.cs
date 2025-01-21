using System;
using System.Linq;
using System.Text;
using System.Security.Cryptography;
using System.Windows;
using System.Windows.Controls;

namespace TestSystemApp.Pages
{
    /// <summary>
    /// Логика взаимодействия для RegPage.xaml
    /// </summary>
    public partial class RegPage : Page
    {
        public RegPage()
        {
            InitializeComponent();
        }

        public static string GetHash(string password)
        {
            using (var hash = SHA1.Create())
            {
                return string.Concat(hash.ComputeHash(Encoding.UTF8.GetBytes(password)).Select(x => x.ToString("X2")));
            }
        }

        private void ButtonCancel_Click(object sender, RoutedEventArgs e)
        {
            TextBoxLogin.Clear();
            PasswordBox.Clear();
            PasswordBoxConfirm.Clear();
            TextBoxFIO.Clear();
            CmbRole.SelectedIndex = 0;
            NavigationService.GoBack();
        }

        private void ButtonRegister_Click(object sender, RoutedEventArgs e)
        {
            
            if (string.IsNullOrEmpty(TextBoxLogin.Text) ||
                string.IsNullOrEmpty(PasswordBox.Password) ||
                string.IsNullOrEmpty(PasswordBoxConfirm.Password) ||
                string.IsNullOrEmpty(TextBoxFIO.Text) ||
                CmbRole.SelectedItem == null)
            {
                MessageBox.Show("Все поля должны быть заполнены.");
                return;
            }

            
            using (Entities db = new Entities())
            {
                if (db.Users.Any(user => user.Username == TextBoxLogin.Text))
                {
                    MessageBox.Show("Пользователь с таким логином уже существует.");
                    return;
                }
            }

            if (PasswordBox.Password.Length < 6 ||
                !PasswordBox.Password.All(c => char.IsLetterOrDigit(c)) ||
                !PasswordBox.Password.Any(char.IsDigit))
            {
                MessageBox.Show("Пароль должен содержать не менее 6 символов, включать только буквы и хотя бы одну цифру.");
                return;
            }

            
            if (PasswordBox.Password != PasswordBoxConfirm.Password)
            {
                MessageBox.Show("Пароли не совпадают.");
                return;
            }

           
            using (Entities db = new Entities())
            {
                string selectedRole = (CmbRole.SelectedItem as ComboBoxItem)?.Content.ToString();

                if (selectedRole == "Student")
                {
                    
                    var newStudent = new Students
                    {
                        FullName = TextBoxFIO.Text,
                        GroupName = TextBoxGroup.Text 
                    };

                    db.Students.Add(newStudent);
                    db.SaveChanges();

                  
                    var newUser = new Users
                    {
                        Username = TextBoxLogin.Text,
                        PasswordHash = GetHash(PasswordBox.Password),
                        Role = "Student",
                        RelatedId = newStudent.StudentId
                    };

                    db.Users.Add(newUser);
                }
                else if (selectedRole == "Teacher")
                {
                   
                    var newTeacher = new Teachers
                    {
                        FullName = TextBoxFIO.Text,
                        Department = TextBoxDepartment.Text
                    };

                    db.Teachers.Add(newTeacher);
                    db.SaveChanges();

                    var newUser = new Users
                    {
                        Username = TextBoxLogin.Text,
                        PasswordHash = GetHash(PasswordBox.Password),
                        Role = "Teacher",
                        RelatedId = newTeacher.TeacherId
                    };

                    db.Users.Add(newUser);
                }
                else
                {
                    MessageBox.Show("Выберите корректную роль пользователя.");
                    return;
                }

                db.SaveChanges();
            }

            // 6. Сообщение об успешной регистрации
            MessageBox.Show("Пользователь успешно зарегистрирован!");
        }
    }
}