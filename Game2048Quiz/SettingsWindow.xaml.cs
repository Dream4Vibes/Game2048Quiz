using System.Windows;

namespace Game2048Quiz
{
    public partial class SettingsWindow : Window
    {
        public SettingsWindow()
        {
            InitializeComponent();
        }

        // НОВЫЙ МЕТОД: Просто закрывает меню
        private void BtnCloseMenu_Click(object sender, RoutedEventArgs e)
        {
            // DialogResult = false закрывает окно и возвращает управление главному окну
            this.DialogResult = false;
        }

        private void BtnMainMenu_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = true;
            this.Close();
        }

        private void BtnExit_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }

        private void BtnTouchGrass_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Ошибка 404: Трава не найдена в базе данных.\nДоступ заблокирован для программистов.\nПожалуйста, вернитесь к написанию кода.",
                            "Системная ошибка",
                            MessageBoxButton.OK,
                            MessageBoxImage.Error);
        }
    }
}