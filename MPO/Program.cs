using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace MPO
{
    class Program
    {
        /// <summary>
        /// Decodes an MPO file into jpeg images
        /// </summary>
        /// <param name="mpoFileName">The file name of the input mpo file</param>
        public static void DecodeMpo(string mpoFileName)
        {
            // The MPO file will contain N jpgs packed together. There is no extra information stored with the MPO.
            // JPEG images will be separated by the tag bytes FF D8 FF E1 (the start tags of the jpeg)
            int[] jpegHeaderStart = new[] { 0xFF, 0xD8, 0xFF, 0xE1 };

            // Start by reading the bytes
            byte[] mpoData = File.ReadAllBytes(mpoFileName);

            // Find the location of the beginning of each image 
            List<int> indices = new List<int>();
            for (int i = 0; i < mpoData.Length - jpegHeaderStart.Length; i++) 
            {
                // Use a mask to find the tag. ~A & A = 0 (this is a little screwy because we deal with FF here which matches everything)
                int j = 0;
                int maskResult = jpegHeaderStart.Sum(bt => ~mpoData[i + j++] & bt);

                // If the masking result is 0 then the right image start tag is found
                if (maskResult == 0)
                {
                    indices.Add(i);
                }
            }

            for (int i = 0; i < indices.Count; i++)
            {
                byte[] image = null;
                if(i + 1 == indices.Count)
                    image = new byte[mpoData.Length - indices[i]];
                else
                    image = new byte[indices[i + 1] - indices[i]];

                Array.Copy(mpoData, indices[i], image, 0, image.Length);
                File.WriteAllBytes(i + ".jpg", image);
            }
        }

        static void Main(string[] args)
        {
            DecodeMpo(args[0]);
        }
    }
}
