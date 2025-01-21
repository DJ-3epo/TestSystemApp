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

namespace TestSystemApp.Pages
{
    /// <summary>
    /// Логика взаимодействия для AddQuestionPage.xaml
    /// </summary>
    public partial class AddQuestionPage : Page
    {
        private readonly Entities _context;

        public AddQuestionPage()
        {
            InitializeComponent();
            _context = new Entities();
        }

        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            var newQuestion = new Questions
            {
                QuestionText = QuestionTextBox.Text,
                Answer1 = Answer1TextBox.Text,
                Answer2 = Answer2TextBox.Text,
                Answer3 = Answer3TextBox.Text,
                Answer4 = Answer4TextBox.Text
            };

            // Попытка преобразования CorrectAnswer (например) из строки в число
            if (int.TryParse(CorrectAnswerTextBox.Text, out int correctAnswer))
            {
                newQuestion.CorrectAnswer = correctAnswer;
            }
            else
            {
                MessageBox.Show("Incorrect format for Correct Answer. Please enter a valid number.");
                return; // Прерываем выполнение, если ввод некорректен
            }

            _context.Questions.Add(newQuestion);
            _context.SaveChanges();
            MessageBox.Show("Question added successfully.");
        }
    }
}
