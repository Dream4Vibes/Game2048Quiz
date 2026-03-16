using System.Windows;

namespace Game2048Quiz
{
    public partial class SettingsWindow : Window
    {
        public SettingsWindow()
        {
            InitializeComponent();
        }

        private void BtnMainMenu_Click(object sender, RoutedEventArgs e)
        {
            // Возвращаем true, чтобы сообщить главному окну, что нужно выйти в меню
            DialogResult = true;
            this.Close();
        }

        private void BtnTouchGrass_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Ошибка 404: Трава не найдена в базе данных.\nДоступ заблокирован для программистов.\nПожалуйста, вернитесь к написанию кода.",
                            "Системная ошибка",
                            MessageBoxButton.OK,
                            MessageBoxImage.Error);
        }

        private void BtnExit_Click(object sender, RoutedEventArgs e)
        {
            // Полностью закрывает приложение
            Application.Current.Shutdown();
        }
    }
}