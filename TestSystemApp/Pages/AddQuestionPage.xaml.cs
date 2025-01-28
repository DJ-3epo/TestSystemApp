using System;
using System.Windows;
using System.Windows.Controls;

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
            // Создаем новый вопрос
            var newQuestion = new Questions
            {
                QuestionType = QuestionTypeTextBox.Text,  // Сохраняем тип вопроса
                QuestionText = QuestionTextBox.Text,
                Answer1 = Answer1TextBox.Text,
                Answer2 = Answer2TextBox.Text,
                Answer3 = Answer3TextBox.Text,
                Answer4 = Answer4TextBox.Text
            };

            // Попытка преобразования CorrectAnswer из строки в число
            if (int.TryParse(CorrectAnswerTextBox.Text, out int correctAnswer))
            {
                newQuestion.CorrectAnswer = correctAnswer;
            }
            else
            {
                MessageBox.Show("Некорректный формат для правильного ответа. Пожалуйста, введите число.");
                return; // Прерываем выполнение, если формат некорректен
            }

            try
            {
                // Добавляем вопрос в базу данных и сохраняем изменения
                _context.Questions.Add(newQuestion);
                _context.SaveChanges();
                MessageBox.Show("Вопрос успешно добавлен.");
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при сохранении: {ex.Message}");
            }
        }
    }
}
