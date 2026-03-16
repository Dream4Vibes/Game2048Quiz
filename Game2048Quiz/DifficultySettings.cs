namespace Game2048Quiz
{
    public class DifficultySettings
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public double ScoreMultiplier { get; set; }
        public string ImagePath { get; set; }

        // НОВОЕ СВОЙСТВО: Текст на кнопке
        public string ButtonText { get; set; }

        public static DifficultySettings GetSettings(string level)
        {
            switch (level)
            {
                case "Easy":
                    return new DifficultySettings
                    {
                        Name = "Мама говорит я умный",
                        Description = "Для тех, кто впервые сел за компьютер (мама тебе врёт). Да-да тебе важен сюжет, КАЗУАЛ. Множитель очков: x0.5",
                        ScoreMultiplier = 0.5,
                        ImagePath = "/Images/easy.png",
                        ButtonText = "НУ Я ВРОДЕ ГОТОВ"
                    };
                case "Medium":
                    return new DifficultySettings
                    {
                        Name = "В школе я хорошист",
                        Description = "Ты нормис, что с тебя взять :). Ты точно не главный герой аниме, но и не NPC! Множитель очков: x1",
                        ScoreMultiplier = 1.0,
                        ImagePath = "/Images/medium.png",
                        ButtonText = "ГОТОВ!" // Стандартный текст
                    };
                case "Hard":
                    return new DifficultySettings
                    {
                        Name = "Я программист, но не супер...",
                        Description = "Написал калькулятор на Питоне? - неплохо, карапуз. А что насчёт написать свою игру на C#? Множитель очков: x2",
                        ScoreMultiplier = 2.0,
                        ImagePath = "/Images/hard.png",
                        ButtonText = "Я БЫЛ ГОТОВ С РОЖДЕНИЯ!"
                    };
                case "Insane":
                    return new DifficultySettings
                    {
                        Name = "Зови меня Третьяк, малыш",
                        Description = "Не надо делать вид, что ты крутой, сразу видно — ты пусичка. Даже мой тестировщик Алексей Краснопёров заплакал. Множитель очков: x4",
                        ScoreMultiplier = 4.0,
                        ImagePath = "/Images/insane.png",
                        ButtonText = "ROCK N'CODE!"
                    };
                default:
                    return GetSettings("Medium");
            }
        }
    }
}