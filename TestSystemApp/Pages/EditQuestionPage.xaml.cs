using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace TestSystemApp.Pages
{
    public partial class EditQuestionPage : Page
    {
        private readonly Entities _context;
        private readonly Questions _question;

        public EditQuestionPage(Questions question)
        {
            InitializeComponent();
            _context = new Entities();
            _question = question;

            // Заполнение полей существующими данными
            QuestionTextBox.Text = _question.QuestionText;
            Answer1TextBox.Text = _question.Answer1;
            Answer2TextBox.Text = _question.Answer2;
            Answer3TextBox.Text = _question.Answer3;
            Answer4TextBox.Text = _question.Answer4;

            // Преобразование int? в строку перед присваиванием в TextBox
            CorrectAnswerTextBox.Text = _question.CorrectAnswer.HasValue ? _question.CorrectAnswer.Value.ToString() : string.Empty;
        }

        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            // Логика для редактирования вопроса
            _question.QuestionText = QuestionTextBox.Text;
            _question.Answer1 = Answer1TextBox.Text;
            _question.Answer2 = Answer2TextBox.Text;
            _question.Answer3 = Answer3TextBox.Text;
            _question.Answer4 = Answer4TextBox.Text;

            // Преобразование строки в int? перед сохранением в базу данных
            int? correctAnswer = null;

            if (int.TryParse(CorrectAnswerTextBox.Text, out int result))
            {
                correctAnswer = result; // Если текст можно преобразовать в число, сохраняем его
            }
            else
            {
                correctAnswer = null; // Если текст не является числом, оставляем null
            }

            // Присваиваем преобразованное значение
            _question.CorrectAnswer = correctAnswer;

            // Сохраняем изменения в базе данных
            _context.SaveChanges();

            // Уведомление о успешном обновлении вопроса
            MessageBox.Show("Question updated successfully.");
        }

    }
}
