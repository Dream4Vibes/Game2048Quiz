using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Imaging;

namespace Game2048Quiz
{
    public partial class StartWindow : Window
    {
        private DifficultySettings _currentSettings;

        public StartWindow()
        {
            InitializeComponent();
            // Установим начальное описание
            UpdateDescription("Medium");
        }

        // Обработчик переключения радиокнопок
        private void Difficulty_Checked(object sender, RoutedEventArgs e)
        {
            // Проверяем, что sender - это RadioButton И что у него есть Tag
            if (sender is RadioButton rb && rb.Tag != null)
            {
                string level = rb.Tag.ToString();
                UpdateDescription(level);
            }
        }

        private void UpdateDescription(string level)
        {
            _currentSettings = DifficultySettings.GetSettings(level);

            // Обновляем текст
            TxtTitle.Text = _currentSettings.Name;
            TxtDescription.Text = _currentSettings.Description;

            // Обновляем картинку
            // Создаем новый объект BitmapImage из пути
            if (!string.IsNullOrEmpty(_currentSettings.ImagePath))
            {
                // UriKind.Relative говорит, что путь относительный (внутри проекта)
                ImgPreview.Source = new BitmapImage(new Uri(_currentSettings.ImagePath, UriKind.Relative));
            }
            else
            {
                ImgPreview.Source = null; // Если картинки нет
            }
        }

        // Кнопка "Начать игру"
        private void BtnStart_Click(object sender, RoutedEventArgs e)
        {
            // Определяем, какая сложность выбрана
            string selectedLevel = "Medium";
            if (RbEasy.IsChecked == true) selectedLevel = "Easy";
            else if (RbHard.IsChecked == true) selectedLevel = "Hard";
            else if (RbInsane.IsChecked == true) selectedLevel = "Insane";

            // Создаем главное окно, передавая настройки
            MainWindow gameWindow = new MainWindow(selectedLevel, _currentSettings.ScoreMultiplier);
            gameWindow.Show();

            // Закрываем текущее окно меню
            this.Close();
        }
    }
}