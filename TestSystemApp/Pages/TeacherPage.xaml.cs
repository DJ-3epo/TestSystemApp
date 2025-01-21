using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace TestSystemApp.Pages
{
    /// <summary>
    /// Логика взаимодействия для TeacherPage.xaml
    /// </summary>
    public partial class TeacherPage : Page
    {
        private readonly Entities _context;

        public TeacherPage()
        {
            InitializeComponent();
            _context = new Entities(); // Инициализация контекста базы данных
            LoadQuestions();
            LoadTestingResults();
        }

        public void LoadQuestions()
        {
            var questions = _context.Questions.ToList();
            QuestionsDataGrid.ItemsSource = null; // Сброс текущего источника данных
            QuestionsDataGrid.ItemsSource = questions; // Установка нового источника данных
        }


        private void LoadTestingResults()
        {
            var results = _context.TestingResults
                        .Select(r => new
                        {
                            StudentName = r.Students.FullName,
                            QuestionText = r.Questions.QuestionText,
                            Answer = r.Answer,
                            Grade = r.Grade.HasValue ? r.Grade.Value.ToString() : "Not graded"
                        })
                        .ToList();

            // Проверяем, что данные есть, и передаем их в DataGrid
            if (results.Any())
            {
                TestingResultsDataGrid.ItemsSource = results; // Привязываем результаты к DataGrid
            }
            else
            {
                MessageBox.Show("No testing results found.");
            }
        }




        private void AddQuestionButton_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Add Question Button Clicked"); // Проверка, что кнопка нажата
            NavigationService.Navigate(new AddQuestionPage());
        }

        private void EditQuestionButton_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Edit Question Button Clicked"); // Проверка, что кнопка нажата
            var selectedQuestion = (Questions)QuestionsDataGrid.SelectedItem;
            if (selectedQuestion != null)
            {
                NavigationService.Navigate(new EditQuestionPage(selectedQuestion));
            }
            else
            {
                MessageBox.Show("Please select a question to edit.");
            }
        }


        private void DeleteQuestionButton_Click(object sender, RoutedEventArgs e)
        {
            var selectedQuestion = (Questions)QuestionsDataGrid.SelectedItem;
            if (selectedQuestion != null)
            {
                _context.Questions.Remove(selectedQuestion);
                _context.SaveChanges();
                LoadQuestions(); // Обновить список вопросов после удаления
            }
            else
            {
                MessageBox.Show("Please select a question to delete.");
            }
        }

        private void GenerateReportButton_Click(object sender, RoutedEventArgs e)
        {
            // Логика для генерации отчета
            var report = GenerateTestReport();
            MessageBox.Show(report);
        }

        private string GenerateTestReport()
        {
            var report = "Test Report\n\n";
            var results = _context.TestingResults.ToList();

            foreach (var result in results)
            {
                // Получаем студента и вопрос
                var student = _context.Students.FirstOrDefault(s => s.StudentId == result.StudentId);
                var question = _context.Questions.FirstOrDefault(q => q.QuestionId == result.QuestionId);

                // Проверяем, что студент и вопрос существуют, чтобы избежать NullReferenceException
                var studentName = student?.FullName ?? "Unknown Student";
                var questionText = question?.QuestionText ?? "Unknown Question";

                // Генерируем отчет
                report += $"Student: {studentName}\n" +
                          $"Question: {questionText}\n" +
                          $"Answer: {result.Answer}\n" +
                          $"Grade: {result.Grade}\n\n";
            }
            return report;
        }

    }
}
