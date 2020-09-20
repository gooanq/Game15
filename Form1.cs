using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Game15
{
    public partial class Form1 : Form
    {
        enum Direction{Left, Right, Up, Down};
        private const int HORIZONTAL = 4;
        private const int VERTICAL = 4;
        private const int BOXSIZE = 150;
        private const int EMPTY = HORIZONTAL * VERTICAL;

        private NumberBox[,] playField = null;
        private NumberBox chosen = null;
        private double time = 0;


        public Form1()
        {
            InitializeComponent();
            NumberBox.empty = EMPTY;
            CreateField();
        }

        private void CreateField()
        {
            Random rand = new Random();
            playField = new NumberBox[HORIZONTAL, VERTICAL];
            Size size = new Size(BOXSIZE, BOXSIZE);
            List<int> numbers = new List<int>();
            for (int i = 0; i < EMPTY; i++)
                numbers.Add(i);



            int xStart = 10;
            int yStart = 10;
            int space = 10;
            int x = xStart, y = yStart;

            for (int i = 0; i < VERTICAL; i++)
            {
                for (int j = 0; j < HORIZONTAL; j++)
                {
                    int number = rand.Next(0, numbers.Count);
                    int value = numbers[number] + 1;
                    var numberBox = new NumberBox(i, j, value, size);
                    numbers.RemoveAt(number);
                    numberBox.Location = new Point(x, y);


                    this.Controls.Add(numberBox);
                    playField[i, j] = numberBox;
                    numberBox.Show();
                    x = x + space + BOXSIZE;

                    if (value == EMPTY) continue;


                    numberBox.Click += (sender, e) =>
                    {
                        if (chosen != null)
                            chosen.BackColor = Color.GreenYellow;
                        numberBox.BackColor = Color.Orange;
                        chosen = numberBox;
                    };

                }
                x = xStart;
                y = y + BOXSIZE + space;
            }

            timer.Start();
        }


        private void Form1_KeyUp(object sender, KeyEventArgs e)
        {
                switch (e.KeyData)
                {
                    case Keys.A:
                    case Keys.Left:
                        Move(Direction.Left);
                        break;
                    case Keys.W:
                    case Keys.Up:
                        Move(Direction.Up);
                        break;
                    case Keys.D:
                    case Keys.Right:
                        Move(Direction.Right);
                        break;
                    case Keys.S:
                    case Keys.Down:
                        Move(Direction.Down);
                        break;
                //////Test//////
                case Keys.N:
                    timer.Stop();
                    MessageBox.Show($"You Win!!!\nYour time: {Math.Round(time * 100) / 100}s", "Message", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    for (int i = 0; i < VERTICAL; i++)
                    {
                        for (int j = 0; j < HORIZONTAL; j++)
                        {
                            playField[i, j].Dispose();
                        }
                    }
                    time = 0;
                    CreateField();
                    break;
            }
            WinCheck();
        }



        private void Move(Direction direction)
        {
            if (chosen == null)
                return;
            int x = chosen.X;
            int y = chosen.Y;
            bool oneMoveDone = false;
            List<NumberBox> boxes = new List<NumberBox>();
            switch (direction)
            {
                case Direction.Left:
                    {
                        while (y > 0)
                        {
                            oneMoveDone = MoveOne(playField[x, y], direction);
                            if (oneMoveDone)
                            {
                                break;
                            } else
                            {
                                boxes.Add(playField[x, y]);
                                y--;
                            }
                        }
                    }
                break;

                case Direction.Right:
                    {
                        while (y < (HORIZONTAL-1) )
                        {
                            oneMoveDone = MoveOne(playField[x, y], direction);
                            if (oneMoveDone)
                            {
                                break;
                            }
                            else
                            {
                                boxes.Add(playField[x, y]);
                                y++;
                            }
                        }
                    }
                    break;

                case Direction.Up:
                    {
                        while (x > 0)
                        {
                            oneMoveDone = MoveOne(playField[x, y], direction);
                            if (oneMoveDone)
                            {
                                break;
                            }
                            else
                            {
                                boxes.Add(playField[x, y]);
                                x--;
                            }
                        }
                    }
                    break;
                case Direction.Down:
                    {
                        while ( x < (VERTICAL-1) )
                        {
                            oneMoveDone = MoveOne(playField[x, y], direction);
                            if (oneMoveDone)
                            {
                                break;
                            }
                            else
                            {
                                boxes.Add(playField[x, y]);
                                x++;
                            }
                        }
                    }
                    break;
                default:
                    break;
            }
            if(oneMoveDone)
            {
                boxes.Reverse();
                boxes.ForEach(item => MoveOne(item, direction));
            }
        }

        private bool MoveOne(NumberBox box, Direction direction)
        {
            int x = box.X;
            int y = box.Y;
            try
            {
                switch (direction)
                {
                    case Direction.Left:
                        if (playField[x, y - 1].isEmpty == true)
                        {
                            FieldSwap(box, playField[x, y - 1]);
                            return true;
                        }
                        break;
                    case Direction.Right:
                        if (playField[x, y + 1].isEmpty == true)
                        {
                            FieldSwap(box, playField[x, y + 1]);
                            return true;
                        }
                        break;
                    case Direction.Up:
                        if (playField[x - 1, y].isEmpty == true)
                        {
                            FieldSwap(box, playField[x - 1, y]);
                            return true;
                        }
                        break;
                    case Direction.Down:
                        if (playField[x + 1, y].isEmpty == true)
                        {
                            FieldSwap(box, playField[x + 1, y]);
                            return true;
                        }
                        break;
                }
            }
            catch { return false; }
            return false;
        }
        private void FieldSwap(NumberBox obj1, NumberBox obj2)
        { 
            obj1.swap(obj2);
            playField[obj1.X, obj1.Y] = obj1;
            playField[obj2.X, obj2.Y] = obj2;
        }

        private void WinCheck()
        {
            label1.Text = "";
            List<int> currentSequence = new List<int>();

            for (int i = 0; i < VERTICAL; i++)
            {
                for (int j = 0; j < HORIZONTAL; j++)
                {
                    currentSequence.Add(playField[i, j].value);
                }
            }

            bool isWin = true;
            for (int i = 0; i < EMPTY; i++)
            {
                if (currentSequence[i] != (i + 1))
                {
                    isWin = false;
                }
                label1.Text += $"{ ((currentSequence[i] != EMPTY) ? currentSequence[i].ToString() : "__") } ";
            }
            label1.Text += $"\nWin: {isWin}";

            if (isWin)
            {
                timer.Stop();
                MessageBox.Show($"You Win!!!\nYour time: {Math.Round(time * 100) / 100}s", "Message", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                for (int i = 0; i < VERTICAL; i++)
                {
                    for (int j = 0; j < HORIZONTAL; j++)
                    {
                        playField[i, j].Dispose();
                    }
                }
                time = 0;
                CreateField();
            };
        }

        private void timer_Tick(object sender, EventArgs e)
        {
            time = time + 0.1;
            label2.Text = $"Time: {Math.Round(time*100)/100}s";
        }
    }
}
