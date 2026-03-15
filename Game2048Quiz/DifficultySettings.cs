namespace Game2048Quiz
{
    // Настройки для уровня сложности
    public class DifficultySettings
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public double ScoreMultiplier { get; set; } // Множитель очков
        public string ImagePath { get; set; } // Путь к картинке (пока оставим пустым или заглушкой)

        // Статический метод для получения настроек
        public static DifficultySettings GetSettings(string level)
        {
            switch (level)
            {
                case "Easy":
                    return new DifficultySettings
                    {
                        Name = "Лёгкий",
                        Description = "Базовые вопросы по устройству ПК. Множитель очков: x0.5",
                        ScoreMultiplier = 0.5,
                        // Путь к картинке (папка/файл.png)
                        ImagePath = "/Images/easy.png"
                    };
                case "Medium":
                    return new DifficultySettings
                    {
                        Name = "Средний",
                        Description = "Стандартные вопросы по программированию. Множитель очков: x1",
                        ScoreMultiplier = 1.0,
                        ImagePath = "/Images/medium.png"
                    };
                case "Hard":
                    return new DifficultySettings
                    {
                        Name = "Сложный",
                        Description = "Углубленные вопросы и алгоритмы. Множитель очков: x2",
                        ScoreMultiplier = 2.0,
                        ImagePath = "/Images/hard.png"
                    };
                case "Insane":
                    return new DifficultySettings
                    {
                        Name = "Безумие",
                        Description = "Вопросы на эрудицию и нестандартное мышление. Множитель очков: x4",
                        ScoreMultiplier = 4.0,
                        ImagePath = "/Images/insane.png"
                    };
                default:
                    return GetSettings("Medium");
            }
        }
    }
}