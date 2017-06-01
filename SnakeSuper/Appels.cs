using System;

namespace SnakeSuper
{
    public class Appels
    {
        public CoordSnake apple;

        Random rand = new Random();

        public Appels(int height, int width, int dimension)
        {
            apple = new CoordSnake()
            {
                X = rand.Next(20, width - 20) / dimension * dimension,
                Y = rand.Next(30, height - 20) / dimension * dimension
            };
        }
    }
}
