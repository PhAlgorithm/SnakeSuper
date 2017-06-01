using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Windows.Forms;

namespace SnakeSuper
{
    public partial class FormGame : Form
    {
        const int sConst = 10;                 // габарит элемента змеи, помех и еды 

        const int wConst = 10;                 // колличество элементов препятствия

        int scoreLevel1 = 0;
        int scoreLevel2 = 0;                    // результат уровня
        int scoreLevel3 = 0;

        int H, W;                             //Габариты игрового поля

        bool flagInitialiseBarrier = true;                       //флаг для первой инициализации массива препятствий

        Rectangle[] barrier = new Rectangle[wConst];           //массив препятствий для уровня <3>

        Timer timer = new Timer();

        Random rand = new Random();

        Direction way = Direction.Right;           // направление движения змеи стрелками - по умолчанию

        Appels appleBody;                                    // координаты яблока
        CoordSnake wall;                                     // координаты препятствий

        SnakeBody snakeBodyMove;                            //тело змеи

        int apples = 0;                            // количество собранных яблок

        public static int ChekLevel;          //статическое пля для перехода в форму "старт"

        public FormGame()
        {

            InitializeComponent();
            // условная высота поля
            H = (Size.Height - 2 * SystemInformation.CaptionHeight) / sConst * sConst;
            // условная ширина поля
            W = (Size.Width - 2 * SystemInformation.FrameBorderSize.Width) / sConst * sConst;

            if (ChekLevel == (int)LevelGame.Thierd)
            {
                Paint += new PaintEventHandler(WallBarrier);                      //прорисовка сетки для 3-го уровня
            }

            Paint += new PaintEventHandler(FormGame_Paint);                   // прорисовка

            if (ChekLevel == (int)LevelGame.First)
            {
                // ChekLevel = (int)LevelGame.First;
                Paint += new PaintEventHandler(DrawGrid);                         //прорисовка сетки для 1-го уровня
            }

            KeyDown += new KeyEventHandler(FormGame_KeyDown);                // нажатие на кнопки

            timer.Interval = 120;                                  // таймер срабатывает раз в 120 милисекунд
            timer.Tick += new EventHandler(timer_Tick);            // привязываем обработчик таймера
            timer.Start();

            appleBody = new Appels(H, W, sConst);

            snakeBodyMove = new SnakeBody(H, W, sConst);

            // координаты преграды 
            wall = new CoordSnake()
            {
                X = rand.Next(Size.Width / sConst) * sConst,
                Y = rand.Next(Size.Height / sConst) * sConst
            };

        }

        public void StopGame()
        {
            timer.Stop();

            MessageBox.Show($"К сожалению вы поиграли. Но вы набрали {apples} яблок",
                "Инфа", MessageBoxButtons.OK);

            Close();
        }

        public void StopGameWinner()
        {
            timer.Stop();

            Dictionary<int, int> score = new Dictionary<int, int>();

            if (File.Exists(@"score.txt"))
            {
                using (StreamReader sr = new StreamReader(@"score.txt"))
                {
                    string read = null;

                    while ((read = sr.ReadLine()) != null)
                    {
                        string[] pars = read.Split(',');
                        int key = Convert.ToInt32(pars[0]);
                        int value = Convert.ToInt32(pars[1]);

                        score.Add(key, value);
                    }
                    sr.Close();

                    if (ChekLevel == (int)LevelGame.First)
                    {
                        scoreLevel1 = apples;
                        if (score[1] < scoreLevel1)
                        {
                            MessageBox.Show($"Вы набрали {scoreLevel1} яблок. Поздраляем, новый рекорд уровня {scoreLevel1} яблок", "Инфа", MessageBoxButtons.OK);
                            score[1] = scoreLevel1;
                            Close();
                        }
                        else
                        {
                            MessageBox.Show($"Вы набрали {apples} яблок", "Инфа", MessageBoxButtons.OK);
                            Close();
                        }
                    }

                    if (ChekLevel == 0)
                    {
                        scoreLevel2 = apples;
                        if (score[2] < scoreLevel2)
                        {
                            MessageBox.Show($"Вы набрали {scoreLevel2} яблок. Поздраляем, новый рекорд уровня {scoreLevel2} яблок", "Инфа", MessageBoxButtons.OK);
                            score[2] = scoreLevel2;
                            Close();
                        }
                        else
                        {
                            MessageBox.Show($"Вы набрали {apples} яблок", "Инфа", MessageBoxButtons.OK);
                            Close();
                        }
                    }

                    if (ChekLevel == (int)LevelGame.Thierd)
                    {
                        scoreLevel3 = apples;
                        if (score[3] < scoreLevel3)
                        {
                            MessageBox.Show($"Вы набрали {scoreLevel3} яблок. Поздраляем, новый рекорд уровня {scoreLevel3} яблок", "Инфа", MessageBoxButtons.OK);
                            score[3] = scoreLevel3;
                            Close();
                        }
                        else
                        {
                            MessageBox.Show($"Вы набрали {apples} яблок", "Инфа", MessageBoxButtons.OK);
                            Close();
                        }
                    }

                }

                using (StreamWriter sw = new StreamWriter(@"score.txt"))
                {
                    sw.WriteLine($"{(int)LevelGame.First},{score[1]}");
                    sw.WriteLine($"{(int)LevelGame.Second},{score[2]}");
                    sw.WriteLine($"{(int)LevelGame.Thierd},{score[3]}");
                    sw.Close();
                }

                Application.Exit();
            }

            if (!File.Exists(@"score.txt"))
            {
                if (ChekLevel == (int)LevelGame.First)
                {
                    scoreLevel1 = apples;
                }
                if (ChekLevel == 0)
                {
                    scoreLevel2 = apples;
                }
                if (ChekLevel == (int)LevelGame.Thierd)
                {
                    scoreLevel3 = apples;
                }

                using (StreamWriter sw = new StreamWriter(@"score.txt"))
                {
                    sw.WriteLine($"{(int)LevelGame.First},{scoreLevel1}");
                    sw.WriteLine($"{(int)LevelGame.Second},{scoreLevel2}");
                    sw.WriteLine($"{(int)LevelGame.Thierd},{scoreLevel3}");
                    sw.Close();
                }
                MessageBox.Show($"Вы набрали {apples} яблок", "Инфа", MessageBoxButtons.OK);
                Close();
            }
            Application.Exit();
        }

        private void FormGame_KeyDown(object sender, KeyEventArgs e)          //выбор направления
        {
            switch (e.KeyData)
            {
                case Keys.Up:
                    if (way != Direction.Down)
                        way = Direction.Up;
                    break;
                case Keys.Right:
                    if (way != Direction.Left)
                        way = Direction.Right;
                    break;
                case Keys.Down:
                    if (way != Direction.Up)
                        way = Direction.Down;
                    break;
                case Keys.Left:
                    if (way != Direction.Right)
                        way = Direction.Left;
                    break;
            }

            if (e.KeyCode == Keys.Escape)
            {
                StopGameWinner();
            }
        }

        private void WallBarrier(object sender, PaintEventArgs e)            //проверка на пересечение с помехами(уровень 3)
        {
            if (flagInitialiseBarrier)
            {
                for (int i = 0; i < wConst; i++)
                {
                    barrier[i] = new Rectangle(
                    wall.X = rand.Next(20, W - 20) / sConst * sConst,
                    wall.Y = rand.Next(30, H - 20) / sConst * sConst,
                    sConst, sConst);
                }
                flagInitialiseBarrier = false;
            }

            for (int i = 0; i < wConst; i++)
            {
                e.Graphics.FillRectangle(Brushes.Gold, barrier[i]);
            }
        }

        private void DrawGrid(object sender, PaintEventArgs e)       //прорисовка сетки для уровня <1>
        {
            using (Pen pen = new Pen(Color.Gray, 0.05f))
            {
                //Горизонтальные линии
                for (int i = 0; i < H; i += sConst)
                    e.Graphics.DrawLine(pen, 0, i, W, i);
                //Вертикальные линии
                for (int i = 0; i < W; i += sConst)
                    e.Graphics.DrawLine(pen, i, 0, i, H);
            }
        }

        private void FormGame_Paint(object sender, PaintEventArgs e)        //прорисовка графики фигур
        {
            // рисуем красным кружком яблоко, синим квадратом голову змеи и зелеными квадратами тело змеи
            e.Graphics.FillEllipse(Brushes.Red, new Rectangle(appleBody.apple.X, appleBody.apple.Y, sConst, sConst));

            e.Graphics.FillRectangle(Brushes.Blue, new Rectangle(snakeBodyMove.snake[0].X, snakeBodyMove.snake[0].Y, sConst, sConst));
            for (int i = 1; i < snakeBodyMove.snake.Count; i++)
            {
                e.Graphics.FillRectangle(Brushes.Green, new Rectangle(snakeBodyMove.snake[i].X, snakeBodyMove.snake[i].Y, sConst, sConst));
            }
        }

        public void timer_Tick(object sender, EventArgs e)
        {
            int x = snakeBodyMove.snake[0].X, y = snakeBodyMove.snake[0].Y;           //  координаты головы змеи

            switch (way)
            {
                case Direction.Up:
                    y = y - 10;
                    if (y < 0)
                        y = H - 10;
                    break;
                case Direction.Right:
                    x = x + 10;
                    if (x >= W)
                        x = 0;
                    break;
                case Direction.Down:
                    y = y + 10;
                    if (y >= H)
                        y = 0;
                    break;
                case Direction.Left:
                    x = x - 10;
                    if (x < 0)
                        x = W - 10;
                    break;
            }

            CoordSnake c = new CoordSnake() { X = x, Y = y };            // сегмент с новыми координатами головы

            snakeBodyMove.snake.Insert(0, c); // вставляем его в начало списка сегментов змеи(змея выросла на один сегмент)

            for (int i = 1; i < snakeBodyMove.snake.Count; i++)
            {
                if (snakeBodyMove.snake[0].X == snakeBodyMove.snake[i].X
                 && snakeBodyMove.snake[0].Y == snakeBodyMove.snake[i].Y)
                {
                    StopGame();
                }
            }

            for (int i = 0; i < wConst; i++)        //проверка на пересечение с препятствием
            {
                if (snakeBodyMove.snake[0].X == barrier[i].X
                 && snakeBodyMove.snake[0].Y == barrier[i].Y)
                {
                    StopGame();
                }
            }

            if (snakeBodyMove.snake[0].X == appleBody.apple.X
               && snakeBodyMove.snake[0].Y == appleBody.apple.Y) // если координаты головы и яблока совпали
            {
                // располагаем яблоко в новых случайных координатах
                appleBody.apple = new CoordSnake()
                {
                    X = rand.Next(20, W - 20) / sConst * sConst,
                    Y = rand.Next(30, H - 20) / sConst * sConst
                };

                apples++;                                           // увеличиваем счетчик собранных яблок

                if (apples % 5 == 0)                                // после каждого пятого яблока увеличиваем скорость
                {                                                   // уменьшая время таймера
                    timer.Interval -= 10;
                }
            }
            // если координаты головы и яблока не совпали - убираем последний сегмент змеи
            else
            {
                snakeBodyMove.snake.RemoveAt(snakeBodyMove.snake.Count - 1);
            }
            Invalidate();                                            // перерисовываем, 
        }
    }
}
