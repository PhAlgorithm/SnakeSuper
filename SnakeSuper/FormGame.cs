using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace SnakeSuper
{
    public partial class FormGame : Form
    {
        const int sConst = 10;                 // габарит элемента змеи, помех и еды 

        const int wConst = 10;                 // колличество элементов препятствия

        int H, W;                             //Габариты игрового поля

        bool flag = true;                                      //флаг для первой инициализации массива препятствий
        Rectangle[] barrier = new Rectangle[wConst];           //массив препятствий для уровня <3>

        Timer timer = new Timer();
        Random rand = new Random();

        List<CoordSnake> snake = new List<CoordSnake>();     //тело змеи

        CoordSnake apple;                                    // координаты яблока
        CoordSnake wall;                                     // координаты препятствий

        Direction way = Direction.Right;           // направление движения змеи стрелками - по умолчанию
        int apples = 0;                            // количество собранных яблок

        public static int ChekForLevel;          //статическое пля для перехода в форму "старт"

        public FormGame()
        {
            InitializeComponent();
            // условная высота поля
            H = (Size.Height - 2 * SystemInformation.CaptionHeight) / sConst * sConst;
            // условная ширина поля
            W = (Size.Width - 2 * SystemInformation.FrameBorderSize.Width) / sConst * sConst;

            if (ChekForLevel == 3)
            {
                Paint += new PaintEventHandler(WallBarrier);                      //прорисовка сетки для 3-го уровня
            }

            Paint += new PaintEventHandler(FormGame_Paint);                   // прорисовка

            if (ChekForLevel == 1)
            {
                Paint += new PaintEventHandler(DrawGrid);                         //прорисовка сетки для 1-го уровня
            }

            KeyDown += new KeyEventHandler(FormGame_KeyDown);                // нажатие на кнопки

            timer.Interval = 120;                                  // таймер срабатывает раз в 120 милисекунд
            timer.Tick += new EventHandler(timer_Tick);            // привязываем обработчик таймера
            timer.Start();

            //добавляем элементы змеи. здесь мы будем приводить координаты к константе размера элемента змеи.
            // сначала мы координату делем на S ( в нашем случае 10), отбрасываем дробную часть, а потом
            // умножаем на S - и унас получается координаты кратны размеру элемента змеи

            snake.Add(new CoordSnake() { X = W / 2 / sConst * sConst, Y = H / 2 / sConst * sConst });
            snake.Add(new CoordSnake() { X = W / 2 / sConst * sConst - sConst, Y = H / 2 / sConst * sConst });
            snake.Add(new CoordSnake() { X = W / 2 / sConst * sConst - 2 * sConst, Y = H / 2 / sConst * sConst });

            // координаты яблока 
            apple = new CoordSnake()
            {
                X = rand.Next(20, W - 20) / sConst * sConst,
                Y = rand.Next(30, H - 20) / sConst * sConst
            };

            // координаты преграды 
            wall = new CoordSnake()
            {
                X = rand.Next(Size.Width / sConst) * sConst,
                Y = rand.Next(Size.Height / sConst) * sConst
            };

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
                timer.Stop();
                MessageBox.Show($"Вы набрали {apples} яблок", "Инфа", MessageBoxButtons.OK);
                Close();
            }
        }

        private void WallBarrier(object sender, PaintEventArgs e)
        {
            if (flag)
            {
                for (int i = 0; i < wConst; i++)
                {
                    barrier[i] = new Rectangle(
                    wall.X = rand.Next(20, W - 20) / sConst * sConst,
                    wall.Y = rand.Next(30, H - 20) / sConst * sConst,
                    sConst, sConst);
                }
                flag = false;
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

        private void FormGame_Paint(object sender, PaintEventArgs e)
        {
            // рисуем красным кружком яблоко, синим квадратом голову змеи и зелеными квадратами тело змеи
            e.Graphics.FillEllipse(Brushes.Red, new Rectangle(apple.X, apple.Y, sConst, sConst));

            e.Graphics.FillRectangle(Brushes.Blue, new Rectangle(snake[0].X, snake[0].Y, sConst, sConst));
            for (int i = 1; i < snake.Count; i++)
            {
                e.Graphics.FillRectangle(Brushes.Green, new Rectangle(snake[i].X, snake[i].Y, sConst, sConst));
            }
        }

        public void timer_Tick(object sender, EventArgs e)
        {
            int x = snake[0].X, y = snake[0].Y;           //  координаты головы змеи

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

            snake.Insert(0, c); // вставляем его в начало списка сегментов змеи(змея выросла на один сегмент)

            for (int i = 0; i < wConst; i++)        //проверка на пересечение с препятствием
            {
                if (snake[0].X == barrier[i].X && snake[0].Y == barrier[i].Y)
                {
                    timer.Stop();
                    MessageBox.Show($"К сожалению вы поиграли. Но вы набрали {apples} яблок",
                        "Инфа", MessageBoxButtons.OK);

                    Close();
                }
            }

            if (snake[0].X == apple.X && snake[0].Y == apple.Y) // если координаты головы и яблока совпали
            {
                // располагаем яблоко в новых случайных координатах
                apple = new CoordSnake()
                {
                    X = rand.Next(20, W - 20) / sConst * sConst,
                    Y = rand.Next(30, H - 20) / sConst * sConst
                };

                apples++;                                           // увеличиваем счетчик собранных яблок

                if (apples % 5 == 0)                                // после каждого пятого яблока увеличиваем скорость
                {
                    timer.Interval -= 10;
                }
            }
            // если координаты головы и яблока не совпали - убираем последний сегмент змеи
            else
            {
                snake.RemoveAt(snake.Count - 1);
            }
            Invalidate();                                            // перерисовываем, 
        }
    }
}
