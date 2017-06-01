using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace SnakeSuper
{
    public class SnakeBody
    {
        public List<CoordSnake> snake = new List<CoordSnake>();     //тело змеи
               
        public SnakeBody(int height, int width, int dimension)
        {
            //добавляем элементы змеи. здесь мы будем приводить координаты к константе размера элемента змеи.
            // сначала мы координату делем на S ( в нашем случае 10), отбрасываем дробную часть, а потом
            // умножаем на S - и унас получается координаты кратны размеру элемента змеи

            snake.Add(new CoordSnake()
            {
                X = width / 2 / dimension * dimension,
                Y = height / 2 / dimension * dimension
            });
            snake.Add(new CoordSnake()
            {
                X = width / 2 / dimension * dimension - dimension,
                Y = height / 2 / dimension * dimension
            });
            snake.Add(new CoordSnake()
            {
                X = width / 2 / dimension * dimension - 2 * dimension,
                Y = height / 2 / dimension * dimension
            });
        }
    }
}
