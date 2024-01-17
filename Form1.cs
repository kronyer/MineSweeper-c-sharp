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

        private bool firstClick = true;
        private Button[,] gridButton = new Button[gridSize, gridSize];
        private bool[,] isMine = new bool[gridSize, gridSize];
        Image Bandeira = Image.FromFile(@"C:\Users\pedro\source\repos\MineSweeper\img/bandeira.png");
        Image Bomba = Image.FromFile(@"C:\Users\pedro\source\repos\MineSweeper\img/bomba.png");
        int vida = 0;
        public Form1()
        {
            InitializeComponent();
            InitializeGrid();
        }

        private void InitializeGrid()
        {
            int margin = 20;
            for (int i = 0; i < gridSize; i++)
            {
                {
                    for (int j = 0; j < gridSize; j++)
                    {
                        gridButton[i, j] = new Button();
                        gridButton[i, j].Size = new System.Drawing.Size(40, 40);
                        gridButton[i, j].Location = new System.Drawing.Point(i * 40 + margin, j * 40 + margin);
                        gridButton[i, j].Tag = new Tuple<int, int>(i, j);
                        gridButton[i, j].MouseDown += GridButton_MouseDown;

                        Controls.Add(gridButton[i, j]);
                        gridButton[i, j].BackColor = Color.Green;

                    }
                }
            }

            Random random = new Random();
            for (int i = 0; i < mineCount; i++)
            {
                int x = random.Next(0, gridSize);
                int y = random.Next(0, gridSize);

                if (!isMine[x, y])
                {
                    isMine[x, y] = true;
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

                    if (newX >= 0 && newX < gridSize && newY >= 0 && newY < gridSize && isMine[newX, newY])
                    {

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
                if (firstClick)
                {
                    EliminateBlocksOnFirstClick(x, y);

                    firstClick = false;
                }
                else
                {
                    if (isMine[x, y])
                    {
                        MessageBox.Show("Game OVER");
                        RevelarBombas();
                        vida--;

                    }
                    else
                    {
                        int mineCount = CountAdjacentMines(x, y);
                        button.Text = mineCount > 0 ? mineCount.ToString() : "";
                        gridButton[x, y].BackColor = Color.LightGray;
                        gridButton[x, y].Enabled = false;
                    }
                }
            }
            else if (e.Button == MouseButtons.Right)
            {
                if (button.Image == null)
                {
                    button.Image = Bandeira;

                }
                else
                {
                    button.Image = null;
                }
            }
        }

        private void RevelarBombas()
        {
            for (int i = 0; i < gridSize; i++)
            {
                for (int j = 0; j < gridSize; j++)
                {
                    if (isMine[i, j])
                    {
                        gridButton[i, j].BackColor = Color.Gray;
                        gridButton[i, j].Image = Bomba;

                    }
                }
            }

        }

        private void EliminateBlocksOnFirstClick(int x, int y)
        {
            Random random = new Random();

            List<Tuple<int, int>> availablePositions = new List<Tuple<int, int>>();

            for (int i = -1; i <= 1; i++)
            {
                for (int j = -1; j <= 1; j++)
                {
                    int newX = x + i;
                    int newY = y + j;

                    if (newX >= 0 && newX < gridSize && newY >= 0 && newY < gridSize && !isMine[newX, newY])
                    {
                        availablePositions.Add(new Tuple<int, int>(newX, newY));
                    }
                }
            }

            availablePositions = availablePositions.OrderBy(p => random.Next()).ToList();

            int numberOfBlocksToEliminate = Math.Min(availablePositions.Count, 5);

            for (int i = 0; i < numberOfBlocksToEliminate; i++)
            {
                int posX = availablePositions[i].Item1;
                int posY = availablePositions[i].Item2;

                int mineCount = CountAdjacentMines(posX, posY);

                gridButton[posX, posY].Text = mineCount > 0 ? mineCount.ToString() : "";
                gridButton[posX, posY].BackColor = Color.LightGray;
                gridButton[posX, posY].Enabled = false;
            }
        }




    }

}

