using System;
using System.Collections.Generic;
using System.Linq;

namespace Game2048Quiz
{
    public enum Direction { Left, Right, Up, Down }

    public class GameEngine
    {
        private double _scoreMultiplier = 1.0; // Добавьте поле

        public void SetDifficulty(double multiplier)
        {
            _scoreMultiplier = multiplier;
        }

        public double GetMultiplier()
        {
            return _scoreMultiplier;
        }

        public void AddScore(int points)
        {
            // Применяем множитель. Приводим к int (округление)
            Score += (int)(points * _scoreMultiplier);
        }

        private const int Size = 4;
        public int[,] Board { get; private set; }
        public int Score { get; private set; }

        // --- НОВАЯ ЛОГИКА: Хранение открытых чисел ---
        private HashSet<int> _discoveredNumbers = new HashSet<int>();
        public List<int> LastUnlockedNumbers { get; private set; } // Список того, что открылось в этом ходу
        // ---------------------------------------------

        private Random random = new Random();

        public GameEngine()
        {
            Board = new int[Size, Size];
        }

        public void StartNewGame()
        {
            Board = new int[Size, Size];
            Score = 0;

            // Сбрасываем прогресс открытий
            _discoveredNumbers.Clear();
            _discoveredNumbers.Add(2); // Число 2 считается открытым изначально

            SpawnTile(2);
            SpawnTile(2);
        }

        public bool Move(Direction direction)
        {
            // Очищаем список открытий перед ходом
            LastUnlockedNumbers = new List<int>();

            bool moved = false;
            switch (direction)
            {
                case Direction.Left: moved = MoveLeft(); break;
                case Direction.Right: moved = MoveRight(); break;
                case Direction.Up: moved = MoveUp(); break;
                case Direction.Down: moved = MoveDown(); break;
            }
            return moved;
        }

        // Метод спавна теперь ВСЕГДА ставит 2 (как в классике)
        public void AddTile()
        {
            SpawnTile(2);
        }

        

        public bool IsGameOver()
        {
            for (int r = 0; r < Size; r++)
                for (int c = 0; c < Size; c++)
                    if (Board[r, c] == 0) return false;

            for (int r = 0; r < Size; r++)
                for (int c = 0; c < Size; c++)
                {
                    if (c < Size - 1 && Board[r, c] == Board[r, c + 1]) return false;
                    if (r < Size - 1 && Board[r, c] == Board[r + 1, c]) return false;
                }
            return true;
        }

        private void SpawnTile(int value)
        {
            List<(int r, int c)> emptyCells = new List<(int, int)>();
            for (int r = 0; r < Size; r++)
                for (int c = 0; c < Size; c++)
                    if (Board[r, c] == 0) emptyCells.Add((r, c));

            if (emptyCells.Count > 0)
            {
                var (r, c) = emptyCells[random.Next(emptyCells.Count)];
                Board[r, c] = value;
            }
        }

        // Ядро логики с проверкой открытий
        private int[] SlideAndMerge(int[] line)
        {
            var temp = line.Where(x => x != 0).ToList();

            for (int i = 0; i < temp.Count - 1; i++)
            {
                if (temp[i] == temp[i + 1])
                {
                    int newVal = temp[i] * 2;

                    // --- ПРОВЕРКА ОТКРЫТИЯ ---
                    if (!_discoveredNumbers.Contains(newVal))
                    {
                        _discoveredNumbers.Add(newVal);
                        LastUnlockedNumbers.Add(newVal); // Добавляем в отчет за этот ход
                    }
                    // -------------------------

                    temp[i] = newVal;
                    Score += temp[i];
                    temp.RemoveAt(i + 1);
                    temp.Insert(i + 1, 0);
                }
            }

            temp = temp.Where(x => x != 0).ToList();
            int[] result = new int[Size];
            temp.CopyTo(result);
            return result;
        }

        // Методы движения (без изменений, используют SlideAndMerge)
        private bool MoveLeft()
        {
            bool changed = false;
            for (int r = 0; r < Size; r++)
            {
                int[] row = new int[Size];
                for (int c = 0; c < Size; c++) row[c] = Board[r, c];
                int[] newRow = SlideAndMerge(row);
                for (int c = 0; c < Size; c++) { if (Board[r, c] != newRow[c]) changed = true; Board[r, c] = newRow[c]; }
            }
            return changed;
        }

        private bool MoveRight()
        {
            bool changed = false;
            for (int r = 0; r < Size; r++)
            {
                int[] row = new int[Size];
                for (int c = 0; c < Size; c++) row[c] = Board[r, Size - 1 - c];
                int[] newRow = SlideAndMerge(row);
                for (int c = 0; c < Size; c++) { if (Board[r, Size - 1 - c] != newRow[c]) changed = true; Board[r, Size - 1 - c] = newRow[c]; }
            }
            return changed;
        }

        private bool MoveUp()
        {
            bool changed = false;
            for (int c = 0; c < Size; c++)
            {
                int[] col = new int[Size];
                for (int r = 0; r < Size; r++) col[r] = Board[r, c];
                int[] newCol = SlideAndMerge(col);
                for (int r = 0; r < Size; r++) { if (Board[r, c] != newCol[r]) changed = true; Board[r, c] = newCol[r]; }
            }
            return changed;
        }

        private bool MoveDown()
        {
            bool changed = false;
            for (int c = 0; c < Size; c++)
            {
                int[] col = new int[Size];
                for (int r = 0; r < Size; r++) col[r] = Board[Size - 1 - r, c];
                int[] newCol = SlideAndMerge(col);
                for (int r = 0; r < Size; r++) { if (Board[Size - 1 - r, c] != newCol[r]) changed = true; Board[Size - 1 - r, c] = newCol[r]; }
            }
            return changed;
        }
    }
}