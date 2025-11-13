using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace Morozovs_ISP_231_Model_2
{
    public partial class MainWindow : Window
    {
        private Random rng = new Random();
        private int _bombLeft;
        private int _bombFind;
        private int _bombs;
        private string[,] map;
        private const int _size = 9;
        private int _cells;
        private int _bestScore = 0;
        private int _score = 0;

        public MainWindow()
        {
            InitializeComponent();
            CreateMap();
            UpdateTextBomb();
            FirstMessage();
        }

        public void FirstMessage()
        {
            MessageBox.Show("Данная игра создана на основе классического «Сапёра», но с ключевым отличием: здесь отсутствует система отметки бомб флажками. Игроку предстоит проверить не только логическое мышление, но и способность ориентироваться по памяти, запоминая расположение потенциально опасных клеток. Ваша задача - открыть все безопасные ячейки, полагаясь исключительно на память и логику. Это проверка как аналитических способностей, так и пространственного запоминания.");
        }

        public void CreateBombs()
        {
            _bombs = rng.Next(15, 31);
            _bombLeft = _bombs;
            _bombFind = 0;
            _cells = _size * _size - _bombs;
        }

        public void UpdateTextBomb()
        {
            cellsLeft.Content = "Клеток осталось: " + _cells;
            bombFind.Content = "Бомб взорвано: " + _bombFind;
            bestScore.Content = "Рекорд: " + _bestScore;
        }

        public void BombFounded()
        {
            _bombLeft -= 1;
            _bombFind += 1;
            UpdateTextBomb();
            if (_bombLeft == 0)
            {
                MessageBox.Show("Игра окончена! \nВы взорвали все бомбы!");
                reset_Click(reset, new RoutedEventArgs());
            }
        }

        public void CreateMap()
        {
            CreateBombs();
            map = new string[_size, _size];
            int _bomb = _bombLeft;

            for (int i = 0; i < _size; i++)
            {
                for (int j = 0; j < _size; j++)
                {
                    map[i, j] = "0";
                }
            }

            while (_bomb > 0)
            {
                int i = rng.Next(0, _size);
                int j = rng.Next(0, _size);

                if (map[i, j] == "0")
                {
                    map[i, j] = "*";
                    _bomb -= 1;
                }
            }

            int _index;
            for (int i = 0; i < _size; i++)
            {
                for (int j = 0; j < _size; j++)
                {
                    if (map[i, j] == "*")
                    {
                        if (0 <= j + 1 && j + 1 < _size && map[i, j + 1] != "*")
                        {
                            _index = int.Parse(map[i, j + 1]);
                            _index++;
                            map[i, j + 1] = _index.ToString();
                        }
                        if (0 <= i + 1 && i + 1 < _size && map[i + 1, j] != "*")
                        {
                            _index = int.Parse(map[i + 1, j]);
                            _index++;
                            map[i + 1, j] = _index.ToString();
                        }
                        if (0 <= j - 1 && j - 1 < _size && map[i, j - 1] != "*")
                        {
                            _index = int.Parse(map[i, j - 1]);
                            _index++;
                            map[i, j - 1] = _index.ToString();
                        }
                        if (0 <= i - 1 && i - 1 < _size && map[i - 1, j] != "*")
                        {
                            _index = int.Parse(map[i - 1, j]);
                            _index++;
                            map[i - 1, j] = _index.ToString();
                        }
                        if (0 <= i - 1 && i - 1 < _size && 0 <= j + 1 && j + 1 < _size && map[i - 1, j + 1] != "*")
                        {
                            _index = int.Parse(map[i - 1, j + 1]);
                            _index++;
                            map[i - 1, j + 1] = _index.ToString();
                        }
                        if (0 <= i + 1 && i + 1 < _size && 0 <= j + 1 && j + 1 < _size && map[i + 1, j + 1] != "*")
                        {
                            _index = int.Parse(map[i + 1, j + 1]);
                            _index++;
                            map[i + 1, j + 1] = _index.ToString();
                        }
                        if (0 <= i + 1 && i + 1 < _size && 0 <= j - 1 && j - 1 < _size && map[i + 1, j - 1] != "*")
                        {
                            _index = int.Parse(map[i + 1, j - 1]);
                            _index++;
                            map[i + 1, j - 1] = _index.ToString();
                        }
                        if (0 <= i - 1 && i - 1 < _size && 0 <= j - 1 && j - 1 < _size && map[i - 1, j - 1] != "*")
                        {
                            _index = int.Parse(map[i - 1, j - 1]);
                            _index++;
                            map[i - 1, j - 1] = _index.ToString();
                        }
                    }
                }
            }
        }

        public void CheckBomb(Button button)
        {
            string name = button.Name;

            int _row = int.Parse(name[4].ToString());
            int _column = int.Parse(name[5].ToString());

            button.Content = map[_row, _column];
            if (map[_row, _column] == "*")
            {
                if (reset.Content.ToString() == "🙂")
                {
                    reset.Content = "☹";
                }
                button.Background = Brushes.Red;
                BombFounded();
                if (_score > 0)
                {
                    _score--;
                }
            }
            else
            {
                if (button.Background != Brushes.Green)
                {
                    _cells--;
                }
                button.Background = Brushes.Green;
                _score++;
            }
            if (_cells == 0 && _bombLeft == _bombs)
            {
                MessageBox.Show("Вы победили! \nВы нашли все безопасные поля и не взорвали ни одной бомбы!");
                reset_Click(reset, new RoutedEventArgs());
            }
            else if(_cells == 0 && _bombLeft != _bombs)
            {
                MessageBox.Show($"Ничья! \nВы нашли все безопасные поля, но взорвали {_bombFind} бомб!");
                reset_Click(reset, new RoutedEventArgs());
            }
            UpdateTextBomb();
        }

        private void Cell_Click(object sender, RoutedEventArgs e)
        {
            Button button = (Button)sender;
            CheckBomb(button);
        }

        private void reset_Click(object sender, RoutedEventArgs e)
        {

            if (_score > _bestScore)
            {
                _bestScore = _score;
            }

            foreach (var child in mainGrid.Children)
            {
                if (child is Button button && button.Name.StartsWith("cell"))
                {
                    button.Content = "";
                    button.Background = Brushes.Silver;
                    button.IsEnabled = true;
                }
            }

            _score = 0;
            reset.Content = "🙂";

            CreateMap();
            UpdateTextBomb();
        }
    }
}
