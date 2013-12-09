using System.Drawing;
using System.Drawing.Imaging;

namespace Jps
{
    class JpsFormat
    {
        /// <summary>
        /// Decodes a JPS file in jpeg images
        /// </summary>
        /// <remarks>
        /// A JPS file is a jpeg images containing the left and right channel images side-by-side. To decode, simply
        /// split the jpeg image in half and save each half as a jpeg file.
        /// </remarks>
        /// <param name="jpsFileName">The filename of the input .jps file</param>
        /// <param name="leftFileName">The filename of the output for the left channel image, 0.jpg by default</param>
        /// <param name="rightFileName">The filename of the output for the right channel image, 1.jpg by default</param>
        public static void DecodeJps(string jpsFileName, string leftFileName = "0.jpg", string rightFileName = "1.jpg")
        {
            // Read the input file, it will be loaded as a jpeg
            var jpsFile = Image.FromFile(jpsFileName);

            // Create a bitmap that will hold the left half of the jps file
            using (var left = new Bitmap(jpsFile.Width / 2, jpsFile.Height))
            {
                // Draw the left half of the jps file onto the new bitmap
                using (var g = Graphics.FromImage(left))
                {
                    g.DrawImage(jpsFile, new Rectangle(0, 0, left.Width, left.Height), new Rectangle(0, 0, left.Width, left.Height), GraphicsUnit.Pixel);
                }

                // Save the result as a jpeg
                left.Save(leftFileName, ImageFormat.Jpeg);
            }

            // Create a bitmap that will hold the right half of the jps file
            using (var right = new Bitmap(jpsFile.Width / 2, jpsFile.Height))
            {
                // Draw the right half of the jps file onto the new bitmap
                using (var g = Graphics.FromImage(right))
                {
                    g.DrawImage(jpsFile, new Rectangle(0, 0, right.Width, right.Height), new Rectangle(jpsFile.Width / 2, 0, right.Width, right.Height), GraphicsUnit.Pixel);
                }

                // Save the result as a jpeg
                right.Save(rightFileName, ImageFormat.Jpeg);
            }
        }
        
        public static void Main(string[] args)
        {
            DecodeJps(args[0]);
        }
    }
}
