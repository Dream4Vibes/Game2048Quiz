using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Linq;

namespace Game2048Quiz
{
    public partial class MainWindow : Window
    {
        private GameEngine _engine;
        private QuestionService _questionService;
        private TextBlock[,] _textBlocks = new TextBlock[4, 4];
        private Border[,] _borders = new Border[4, 4];

        public MainWindow(string difficultyLevel, double scoreMultiplier)
        {
            InitializeComponent();

            // Инициализация движка
            _engine = new GameEngine();
            _engine.SetDifficulty(scoreMultiplier); // Передаем множитель

            // Инициализация вопросов для конкретной сложности
            _questionService = new QuestionService(difficultyLevel);

            InitializeGridVisuals();
            StartNewGame();

        }

        private void InitializeGridVisuals()
        {
            GameContainer.RowDefinitions.Clear();
            GameContainer.ColumnDefinitions.Clear();

            for (int i = 0; i < 4; i++)
            {
                GameContainer.RowDefinitions.Add(new RowDefinition());
                GameContainer.ColumnDefinitions.Add(new ColumnDefinition());
            }

            for (int r = 0; r < 4; r++)
            {
                for (int c = 0; c < 4; c++)
                {
                    Border border = new Border
                    {
                        Margin = new Thickness(8), // Увеличил отступ между плитками
                        CornerRadius = new CornerRadius(8), // Скругление углов побольше
                        Background = Brushes.Gray
                    };

                    TextBlock text = new TextBlock
                    {
                        HorizontalAlignment = HorizontalAlignment.Center,
                        VerticalAlignment = VerticalAlignment.Center,
                        FontSize = 48, // Шрифт внутри плитки намного больше (было 28)
                        FontWeight = FontWeights.Bold,
                        Foreground = Brushes.White
                    };

                    border.Child = text;
                    _textBlocks[r, c] = text;
                    _borders[r, c] = border;

                    Grid.SetRow(border, r);
                    Grid.SetColumn(border, c);
                    GameContainer.Children.Add(border);
                }
            }
        }

        private void StartNewGame()
        {
            _engine.StartNewGame();
            UpdateUI();
        }

        private void BtnSettings_Click(object sender, RoutedEventArgs e)
        {
            // Создаем окно настроек
            SettingsWindow settings = new SettingsWindow();

            // Привязываем владельца (чтобы окно было по центру главного)
            settings.Owner = this;

            // Показываем как диалог
            bool? result = settings.ShowDialog();

            // Если нажали "Главное меню" (DialogResult = true)
            if (result == true)
            {
                ReturnToMainMenu();
            }
        }

        private void Window_KeyDown(object sender, KeyEventArgs e)
        {
            Direction? dir = null;
            switch (e.Key)
            {
                case Key.Left: dir = Direction.Left; break;
                case Key.Right: dir = Direction.Right; break;
                case Key.Up: dir = Direction.Up; break;
                case Key.Down: dir = Direction.Down; break;
            }

            if (dir.HasValue)
            {
                bool moved = _engine.Move(dir.Value);

                if (moved)
                {
                    UpdateUI(); // Сначала показываем результат хода

                    // ПРОВЕРКА: Были ли открытые числа в этом ходу?
                    if (_engine.LastUnlockedNumbers.Count > 0)
                    {
                        // Достаем число (обычно одно, но если открыли сразу два разных - берем первое или последнее)
                        int unlockedValue = _engine.LastUnlockedNumbers.First();

                        // Запускаем викторину
                        ProcessQuizForUnlock(unlockedValue);
                    }

                    // В ЛЮБОМ СЛУЧАЕ (был вопрос или нет) спавним обычную "2"
                    _engine.AddTile();

                    UpdateUI();
                    CheckGameOver();
                }
            }
        }

        // Новый метод обработки вопроса только при открытии
        private void ProcessQuizForUnlock(int unlockedValue)
        {
            var question = _questionService.GetRandomQuestion();

            // ПРОВЕРКА: Если вопросы закончились
            if (question == null)
            {
                MessageBox.Show("Вопросы закончились! Вы гений!", "База пуста", MessageBoxButton.OK, MessageBoxImage.Information);
                return;
            }

            var quizWindow = new QuizWindow(question);
            bool? result = quizWindow.ShowDialog();

            if (result == true)
            {
                // Множитель уже учтен в движке
                _engine.AddScore(100 * unlockedValue);
                int pointsEarned = (int)(100 * unlockedValue * _engine.GetMultiplier());
                MessageBox.Show($"Верно! +{pointsEarned} очков.", "Успех", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            else
            {
                MessageBox.Show($"Ошибка!", "Неудача", MessageBoxButton.OK, MessageBoxImage.Warning);
            }


        }

        private void UpdateUI()
        {
            TxtScore.Text = _engine.Score.ToString();
            for (int r = 0; r < 4; r++)
            {
                for (int c = 0; c < 4; c++)
                {
                    int val = _engine.Board[r, c];
                    _textBlocks[r, c].Text = val == 0 ? "" : val.ToString();
                    _borders[r, c].Background = GetColor(val);
                }
            }
        }

        private Brush GetColor(int val)
        {
            switch (val)
            {
                case 0: return new SolidColorBrush((Color)ColorConverter.ConvertFromString("#cdc1b4"));
                case 2: return new SolidColorBrush((Color)ColorConverter.ConvertFromString("#eee4da"));
                case 4: return new SolidColorBrush((Color)ColorConverter.ConvertFromString("#ede0c8"));
                case 8: return new SolidColorBrush((Color)ColorConverter.ConvertFromString("#f2b179"));
                case 16: return new SolidColorBrush((Color)ColorConverter.ConvertFromString("#f59563"));
                case 32: return new SolidColorBrush((Color)ColorConverter.ConvertFromString("#f67c5f"));
                case 64: return new SolidColorBrush((Color)ColorConverter.ConvertFromString("#f65e3b"));
                case 128: return new SolidColorBrush((Color)ColorConverter.ConvertFromString("#edcf72"));
                default: return Brushes.DarkGray;
            }
        }

        private void CheckGameOver()
        {
            if (_engine.IsGameOver())
            {
                MessageBox.Show($"Игра окончена! Счет: {_engine.Score}", "Game Over");

                // Вместо перезапуска игры возвращаемся в меню
                ReturnToMainMenu();
            }
        }

        private void ReturnToMainMenu()
        {
            // Создаем новое стартовое окно
            StartWindow startWindow = new StartWindow();
            startWindow.Show();

            // Закрываем текущее игровое окно
            this.Close();
        }

    }
}