using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Web;

namespace Glinterion.PhotoHelpers
{
    public class PhotoConverter
    {
        #region Create Thumbnail
        // Create a thumbnail in byte array format from the image encoded in the passed byte array.  
        // (RESIZE an image in a byte[] variable.)  
        public static byte[] CreateThumbnail(byte[] PassedImage, int LargestSide, int Height, int Width)
        {
            byte[] ReturnedThumbnail;

            using (System.IO.MemoryStream StartMemoryStream = new System.IO.MemoryStream(), NewMemoryStream = new System.IO.MemoryStream())
            {
                // write the string to the stream  
                StartMemoryStream.Write(PassedImage, 0, PassedImage.Length);

                ImageConverter imageConverter = new System.Drawing.ImageConverter();
                Image startBitmap = imageConverter.ConvertFrom(PassedImage) as Image;
                // create the start Bitmap from the MemoryStream that contains the image  
                //System.Drawing.Bitmap startBitmap = new System.Drawing.Bitmap(StartMemoryStream);

                // set thumbnail height and width proportional to the original image.  
                int newHeight;
                int newWidth;
                double HW_ratio;
                if (startBitmap.Height > startBitmap.Width)
                {
                    newHeight = LargestSide;
                    HW_ratio = (double)((double)LargestSide / (double)startBitmap.Height);
                    newWidth = (int)(HW_ratio * (double)startBitmap.Width);
                }
                else
                {
                    newWidth = LargestSide;
                    HW_ratio = (double)((double)LargestSide / (double)startBitmap.Width);
                    newHeight = (int)(HW_ratio * (double)startBitmap.Height);
                }
                newHeight = Height;
                newWidth = Width;
                // create a new Bitmap with dimensions for the thumbnail.  
                System.Drawing.Bitmap newBitmap = new System.Drawing.Bitmap(newWidth, newHeight);

                // Copy the image from the START Bitmap into the NEW Bitmap.  
                // This will create a thumnail size of the same image.  
                newBitmap = ResizeImage(startBitmap as Bitmap, newWidth, newHeight);

                // Save this image to the specified stream in the specified format.  
                newBitmap.Save(NewMemoryStream, System.Drawing.Imaging.ImageFormat.Jpeg);

                // Fill the byte[] for the thumbnail from the new MemoryStream.  
                ReturnedThumbnail = NewMemoryStream.ToArray();
            }
            // return the resized image as a string of bytes.  
            return ReturnedThumbnail;
        }

        public static byte[] Resize(byte[] image, int newWidth, int newHeight)
        {
            var newImage = byteArrayToImage(image);
            return imageToByteArray(ResizeImage(newImage as Bitmap, newWidth, newHeight));
        }

        // Resize a Bitmap  
        private static Bitmap ResizeImage(Bitmap image, int width, int height)
        {
            Bitmap resizedImage = new Bitmap(width, height);
            using (Graphics gfx = Graphics.FromImage(resizedImage))
            {
                gfx.DrawImage(image, new Rectangle(0, 0, width, height),
                    new Rectangle(0, 0, image.Width, image.Height), GraphicsUnit.Pixel);
            }
            return resizedImage;
        }

        private static byte[] imageToByteArray(Image imageIn)
        {
            MemoryStream ms = new MemoryStream();
            imageIn.Save(ms, System.Drawing.Imaging.ImageFormat.Jpeg);
            return ms.ToArray();
        }

        private static Image byteArrayToImage(byte[] byteArrayIn)
        {
            MemoryStream ms = new MemoryStream(byteArrayIn);
            Image returnImage = Image.FromStream(ms);
            return returnImage;
        }

        #endregion

    }



}