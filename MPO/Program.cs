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
        /// <remarks>
        /// An MPO file is a multi-jpeg container that can hold any number of jpeg files. The container itself contains no 
        /// extra data so jpeg images can be found and extracted using their start signature "0xFFD8FFE1" which represents
        /// the first EXIF tag
        /// </remarks>
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

            // For each found index, extract the jpeg
            for (int i = 0; i < indices.Count; i++)
            {
                // Create a temporary buffer to hold the image bytes
                // To do this, make a buffer with length equal to the difference between the start index of this image
                // and the start index of the next image. If there is no next image, use the end of the data instead.
                byte[] image = null;
                if(i + 1 == indices.Count)
                    image = new byte[mpoData.Length - indices[i]];
                else
                    image = new byte[indices[i + 1] - indices[i]];

                // Copy the image data to a temporary file
                Array.Copy(mpoData, indices[i], image, 0, image.Length);

                // Write the image data to a file
                File.WriteAllBytes(i + ".jpg", image);
            }
        }

        static void Main(string[] args)
        {
            DecodeMpo(args[0]);
        }
    }
}
