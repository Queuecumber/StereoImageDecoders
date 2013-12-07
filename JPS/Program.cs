using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.Drawing.Imaging;

namespace JPS
{
    class Program
    {
        public static void DecodeJps(string jpsFileName, string leftFileName = "0.jpg", string rightFileName = "1.jpg")
        {
            Image jpsFile = Image.FromFile(jpsFileName);

            using (Bitmap left = new Bitmap(jpsFile.Width / 2, jpsFile.Height))
            {
                using (Graphics g = Graphics.FromImage(left))
                {
                    g.DrawImage(jpsFile, new Rectangle(0, 0, left.Width, left.Height), new Rectangle(0, 0, left.Width, left.Height), GraphicsUnit.Pixel);
                }

                left.Save(leftFileName, ImageFormat.Jpeg);
            }

            using (Bitmap right = new Bitmap(jpsFile.Width / 2, jpsFile.Height))
            {
                using (Graphics g = Graphics.FromImage(right))
                {
                    g.DrawImage(jpsFile, new Rectangle(0, 0, right.Width, right.Height), new Rectangle(jpsFile.Width / 2, 0, right.Width, right.Height), GraphicsUnit.Pixel);
                }

                right.Save(rightFileName, ImageFormat.Jpeg);
            }
        }
        

        public static void Main(string[] args)
        {
            DecodeJps(args[0]);
        }
    }
}
