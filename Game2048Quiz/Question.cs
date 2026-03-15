using System;

namespace Game2048Quiz
{
    public class Question
    {
        public string Text { get; set; }       // Текст вопроса
        public string[] Options { get; set; }  // 4 варианта ответа
        public int CorrectIndex { get; set; }  // Номер правильного ответа (0, 1, 2, 3)

        public Question(string text, string[] options, int correctIndex)
        {
            Text = text;
            Options = options;
            CorrectIndex = correctIndex;
        }
    }
}