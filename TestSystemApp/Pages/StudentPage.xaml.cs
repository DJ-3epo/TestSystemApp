using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace TestSystemApp.Pages
{
    public partial class StudentPage : Page
    {
        private int currentQuestionIndex = 0;
        private int correctAnswers = 0;
        private int totalQuestions = 0;
        private Questions[] questions;

        // Конструктор, который принимает текущего студента
        public StudentPage(Users currentUser)
        {
            InitializeComponent();
            CurrentUser.User = currentUser; // Сохраняем текущего пользователя в глобальной переменной
            LoadQuestions();
            DisplayCurrentQuestion();
            LoadResults();
        }

        // Метод загрузки вопросов из базы данных
        private void LoadQuestions()
        {
            using (Entities db = new Entities())
            {
                questions = db.Questions.ToArray();
                totalQuestions = questions.Length;
            }
        }

       
        private void DisplayCurrentQuestion()
        {
            if (currentQuestionIndex < questions.Length)
            {
                var question = questions[currentQuestionIndex];
                TextBlockQuestion.Text = question.QuestionText;

                RadioAnswer1.Content = question.Answer1;
                RadioAnswer2.Content = question.Answer2;
                RadioAnswer3.Content = question.Answer3;
                RadioAnswer4.Content = question.Answer4;

                RadioAnswer3.Visibility = string.IsNullOrEmpty(question.Answer3) ? Visibility.Collapsed : Visibility.Visible;
                RadioAnswer4.Visibility = string.IsNullOrEmpty(question.Answer4) ? Visibility.Collapsed : Visibility.Visible;
            }
            else
            {
                // Завершение теста
                MessageBox.Show($"Тест завершен! Вы правильно ответили на {correctAnswers} из {totalQuestions} вопросов.");
                SaveResults();
            }
        }

        private void SaveResults()
        {
            using (Entities db = new Entities())
            {
                var result = new TestingResults
                {
                    TestDate = DateTime.Now,
                    StudentId = CurrentUser.User.RelatedId, 
                    TotalQuestions = totalQuestions,
                    CorrectAnswers = correctAnswers,
                    Grade = (int)((double)correctAnswers / totalQuestions * 5),
                    TestDuration = 10 
                };

                db.TestingResults.Add(result);
                db.SaveChanges();
            }
        }

        
        private void ButtonSubmitAnswer_Click(object sender, RoutedEventArgs e)
        {
            var selectedAnswer = new[] { RadioAnswer1, RadioAnswer2, RadioAnswer3, RadioAnswer4 }
                .FirstOrDefault(r => r.IsChecked == true);

            if (selectedAnswer == null)
            {
                MessageBox.Show("Выберите ответ!");
                return;
            }

            int selectedAnswerIndex = Array.IndexOf(new[] { RadioAnswer1, RadioAnswer2, RadioAnswer3, RadioAnswer4 }, selectedAnswer) + 1;

            if (questions[currentQuestionIndex].CorrectAnswer == selectedAnswerIndex)
            {
                correctAnswers++;
            }

            currentQuestionIndex++;
            DisplayCurrentQuestion();
        }

       
        private void LoadResults()
        {
            using (Entities db = new Entities())
            {
                var results = db.TestingResults
                    .Where(r => r.StudentId == CurrentUser.User.RelatedId) 
                    .Select(r => new
                    {
                        TestDate = r.TestDate,
                        TestName = "Название теста", // Заменить на реальное название теста
                        CorrectAnswers = r.CorrectAnswers,
                        TotalQuestions = r.TotalQuestions,
                        Grade = r.Grade
                    }).ToList();

                DataGridResults.ItemsSource = results;
            }
        }
    }
}
