using MineSweeper.Logic;
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
        private Board board = new Board(); 
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
            board.InitializeBoard();
            board.PlaceMines();

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
                    board.EliminateBlocksOnFirstClick(x, y, gridButton);

                    firstClick = false;
                }
                else
                {
                    if (board.IsMine(x,y))
                    {
                        MessageBox.Show("Game OVER");
                        board.RevealAllMines(gridButton);
                        vida--;

                    }
                    else
                    {
                        int mineCount = board.CountAdjacentMines(x, y);
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

        
        


        




    }

}

