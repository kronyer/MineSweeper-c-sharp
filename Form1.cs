using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MineSweeper
{
    public partial class Form1 : Form
    {

        private const int gridSize = 10;
        private const int mineCount = 15;

        private Button[,] gridButton = new Button[gridSize, gridSize];
        private bool[,] isMine = new bool[gridSize, gridSize];
        public Form1()
        {
            InitializeComponent();
            InitializeGrid();
        }

        private void InitializeGrid()
        {
            for (int i = 0; i < gridSize; i++)
            {
                {
                    for (int j = 0; j < gridSize; j++)
                    {
                        gridButton[i, j] = new Button();
                        gridButton[i, j].Size = new System.Drawing.Size(30, 30);
                        gridButton[i, j].Location = new System.Drawing.Point(i * 30, j * 30);
                        gridButton[i,j].Tag = new Tuple<int, int>(i, j);
                        gridButton[i, j].MouseDown += GridButton_MouseDown;

                        Controls.Add(gridButton[i, j]);

                    }
                }
            }

            Random random = new Random();
            for (int i = 0;i< mineCount;i++)
            {
                int x = random.Next(0, gridSize);
                int y = random.Next(0, gridSize);

                if (!isMine[x,y])
                {
                    isMine[x,y] = true;
                }
                else
                {
                    i--;
                }
            }
        }
     

        private int CountAdjacentMines(int x, int y)
        {
            int count = 0;
            for (int i = -1; i <= 1; i++)
            {
                for (int j = -1; j <= 1; j++)
                {
                    int newX = x + i;
                    int newY = y + j;

                    if (newX >= 0 && newX < gridSize && newY >= 0 && newY < gridSize && isMine[newX, newY]) { 
                    
                    count++;
                    }
                }
            }
            return count;
        }

        private void GridButton_MouseDown(object sender, MouseEventArgs e)
        {

            Button button = (Button)sender;
            Tuple<int, int> position = (Tuple<int, int>)button.Tag;
            int x = position.Item1;
            int y = position.Item2;

            if (e.Button == MouseButtons.Left)
            {
                if (isMine[x, y])
                {
                    MessageBox.Show("Game OVER");
                }
                else
                {
                    int mineCount = CountAdjacentMines(x, y);
                    button.Text = mineCount > 0 ? mineCount.ToString() : "";
                }
            }
            else if (e.Button == MouseButtons.Right)
            {
                button.Text = "F";
            }
        }
    }
}
