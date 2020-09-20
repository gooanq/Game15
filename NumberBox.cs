using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Drawing;
using System.Security.Cryptography.X509Certificates;

namespace Game15
{
    class NumberBox : PictureBox
    {
        static public int empty;
        public int xPos, yPos;

        public  int value;
        public int X => xPos;
        public int Y => yPos;
        public bool isEmpty => this.value == empty;


        public NumberBox(int x, int y, int value, Size size) : base()
        {
            xPos = x;
            yPos = y;
            this.value = value;
            this.Size = size;
            if (value == empty)
            {
                return;
            }
            this.BackColor = Color.GreenYellow;
            Bitmap bmp = new Bitmap(size.Width, size.Height);
            Graphics graphics= Graphics.FromImage(bmp);
            graphics.DrawString(value.ToString(), new Font("Comic Sans MS", 50),
                new SolidBrush(Color.Black), 50, 30);
            this.Image = bmp;
        }


        //public delegate void SwapMethod(NumberBox obj);
        //public SwapMethod Swap;
        public void swap(NumberBox obj)
        {
            Point tmp = obj.Location;
            obj.Location = this.Location;
            this.Location = tmp;

            int x = obj.xPos;
            int y = obj.yPos;

            obj.xPos = this.xPos;
            obj.yPos = this.yPos;

            this.xPos = x;
            this.yPos = y;
        }
    }
}
