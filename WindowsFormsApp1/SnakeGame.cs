using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace WindowsFormsApp1
{
    public class SnakeGame
    {
        private readonly int gameFieldHeightInCells;
        private readonly int gameFieldWidthInCells;
        private readonly int cellSize;

        private Keys currentSnakeDirection;

        private List<Point> body = new List<Point>();

        private Random random = new Random();

        private int speedX = 0;
        private int speedY = 0;

        private Point snakeHead => body.Last();
        private Point food;

        private const string defeatMessage = "gg wp mother faka";

        public SnakeGame(int gameFieldHeightInCells, int gameFieldWidthInCells, int cellSize)
        {
            this.gameFieldHeightInCells = gameFieldHeightInCells;
            this.gameFieldWidthInCells = gameFieldWidthInCells;
            this.cellSize = cellSize;
        }

        public void Restart()
        {
            food = new Point(random.Next(0, gameFieldWidthInCells), random.Next(0, gameFieldHeightInCells));
            body.Clear();
            body.Add(new Point(4, 4));
            speedX = 0;
            speedY = 0;
        }

        public void Draw(Graphics graphics)
        {
            graphics.FillRectangle(Brushes.Green, food.X * cellSize, food.Y * cellSize, cellSize, cellSize);

            foreach (Point bodyPart in body)
                graphics.FillRectangle(Brushes.Red, bodyPart.X * cellSize, bodyPart.Y * cellSize, cellSize, cellSize);

            Pen blackPen = new Pen(Color.Black, 2);

            for (int i = 0; i <= gameFieldHeightInCells; i++)
            {
                graphics.DrawLine(blackPen, 0, cellSize * i, cellSize * gameFieldWidthInCells, cellSize * i);
            }

            for (int j = 0; j <= gameFieldWidthInCells; j++)
            {
                graphics.DrawLine(blackPen, cellSize * j, 0, cellSize * j, cellSize * gameFieldHeightInCells);
            }
        }

        public void Update()
        {
            if (snakeHead.X + speedX == food.X && snakeHead.Y + speedY == food.Y)
            {
                body.Add(food);
            }
            else
            {
                body.Add(new Point(snakeHead.X + speedX, snakeHead.Y + speedY));
                body.RemoveAt(0);
            }

            if (snakeHead.X < 0 ||
                snakeHead.Y < 0 ||
                snakeHead.X >= gameFieldWidthInCells ||
                snakeHead.Y >= gameFieldHeightInCells ||
                IsSnakeEatItself())
            {
                Restart();
                MessageBox.Show(defeatMessage, "Ты продул", MessageBoxButtons.OK);
            }

            if (snakeHead.X == food.X && snakeHead.Y == food.Y)
                GenerateFood();
        }

        private bool IsOppositeDirections(Keys firstDirection, Keys secondDirection)
        {
            return firstDirection == Keys.Up && secondDirection == Keys.Down ||
                   firstDirection == Keys.Left && secondDirection == Keys.Right ||
                   firstDirection == Keys.Down && secondDirection == Keys.Up ||
                   firstDirection == Keys.Right && secondDirection == Keys.Left;
        }

        public void Move(Keys direction)
        {
            if (IsOppositeDirections(currentSnakeDirection, direction) && body.Count > 1)
                return;
            if (direction == Keys.Up)
            {
                currentSnakeDirection = direction;
                speedX = 0;
                speedY = -1;
            }

            if (direction == Keys.Down)
            {
                currentSnakeDirection = direction;
                speedX = 0;
                speedY = 1;
            }

            if (direction == Keys.Left)
            {
                currentSnakeDirection = direction;
                speedX = -1;
                speedY = 0;
                
            }
            if (direction == Keys.Right)
            {
                currentSnakeDirection = direction;
                speedX = 1;
                speedY = 0;
            }
        }

        private void GenerateFood()
        {
            List<Point> freeCells = new List<Point>();
            for (int x = 0; x < gameFieldWidthInCells; x++)
                for (int y = 0; y < gameFieldHeightInCells; y++)
                {
                    Point cell = new Point(x, y);

                    if (!
                        body.Contains(cell))
                        freeCells.Add(new Point(x, y));
                }

            food = freeCells
                .OrderBy(_ => random.NextDouble())
                .First();
        }

        private bool IsSnakeEatItself()
        {
            return body.Take(body.Count - 1).Contains(snakeHead);
        }
    }
}