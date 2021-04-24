using System;
using System.Windows.Forms;

namespace WindowsFormsApp1
{
    public partial class Form1 : Form
    {
        SnakeGame snakeGame = new SnakeGame(8, 16, 50);

        public Form1()
        {
            InitializeComponent();
            snakeGame.Restart();
        }

        private void pictureBox1_Paint_1(object sender, PaintEventArgs e)
        {
            this.snakeGame.Draw(e.Graphics);
        }

        private void timer_Tick(object sender, EventArgs e)
        {
            this.snakeGame.Update();
            this.pictureBox1.Refresh();
        }

        private void Form1_KeyDown_1(object sender, KeyEventArgs e)
        {
            this.snakeGame.Move(e.KeyCode);
            this.pictureBox1.Refresh();
        }
    }
}