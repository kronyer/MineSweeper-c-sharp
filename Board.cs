using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MineSweeper.Logic
{

    internal class Board
    {
        private const int gridSize = 10;

        private const int mineCount = 15; 
        // ver sobre dificuldade
        private Cell[,] cells = new Cell[gridSize, gridSize];


        public void InitializeBoard()
        {
            for (int i = 0; i < gridSize; i++)
            {
                for (int j = 0; j < gridSize; j++)
                {
                    cells[i, j] = new Cell();
                }
            }
        }

        public void PlaceMines()
        {
            Random random = new Random();
            for (int i = 0; i < mineCount; i++)
            {
                int x = random.Next(0, gridSize);
                int y = random.Next(0, gridSize);

                if (!cells[x,y].IsMine)
                {
                    cells[x, y].IsMine = true;
                }
                else
                {
                    i--;
                }
            }
        }


        public int CountAdjacentMines(int x, int y)
        {
            int count = 0;
            for (int i = -1; i <= 1; i++)
            {
                for (int j = -1; j <= 1; j++)
                {
                    int newX = x + i;
                    int newY = y + j;

                    if (newX >= 0 && newX < gridSize && newY >= 0 && newY < gridSize && cells[newX, newY].IsMine)
                    {

                        count++;
                    }
                }
            }
            return count;
        }

        public bool IsMine(int x, int y)
        {
            return cells[x,y].IsMine;
        }

        public void EliminateBlocksOnFirstClick(int x, int y, Button[,] gridButtons)
        {
            Random random = new Random();

            List<Tuple<int, int>> availablePositions = new List<Tuple<int, int>>();

            for (int i = -1; i <= 1; i++)
            {
                for (int j = -1; j <= 1; j++)
                {
                    int newX = x + i;
                    int newY = y + j;

                    if (newX >= 0 && newX < gridSize && newY >= 0 && newY < gridSize && !cells[newX,newY].IsMine)
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

                gridButtons[posX, posY].Text = mineCount > 0 ? mineCount.ToString() : "";
                gridButtons[posX, posY].BackColor = Color.LightGray;
                gridButtons[posX, posY].Enabled = false;
            }
        }

        public void RevealAllMines(Button[,] gridButtons)
        {
            for (int i = 0; i < gridSize; i++)
            {
                for (int j = 0; j < gridSize; j++)
                {
                    if (cells[i,j].IsMine)
                    {
                        gridButtons[i, j].BackColor = Color.Gray;
                        gridButtons[i, j].Image = Image.FromFile(@"C:\Users\pedro\source\repos\MineSweeper\img/bomba.png");
                    }
                }
            }
        }
    }
}
