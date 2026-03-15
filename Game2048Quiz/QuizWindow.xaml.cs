using System.Windows;
using System.Windows.Controls;

namespace Game2048Quiz
{
    public partial class QuizWindow : Window
    {
        private int _correctIndex;

        // Конструктор принимает объект вопроса
        public QuizWindow(Question question)
        {
            InitializeComponent();

            // Заполняем элементы окна данными
            TxtQuestion.Text = question.Text;
            BtnAnswer1.Content = question.Options[0];
            BtnAnswer2.Content = question.Options[1];
            BtnAnswer3.Content = question.Options[2];
            BtnAnswer4.Content = question.Options[3];

            _correctIndex = question.CorrectIndex;
        }

        // Общий обработчик клика для всех 4 кнопок
        private void BtnAnswer_Click(object sender, RoutedEventArgs e)
        {
            Button btn = sender as Button;
            int selectedIndex = int.Parse(btn.Tag.ToString());

            // Проверяем ответ
            if (selectedIndex == _correctIndex)
            {
                DialogResult = true; // Верно
            }
            else
            {
                DialogResult = false; // Ошибка
            }

            this.Close(); // Закрываем окно
        }
    }
}