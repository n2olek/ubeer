using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Web;

namespace S9.Utility
{
    public class ImageUtil
    {
        //private Bitmap _bmpSource;
        //private string _fileName;

        public ImageUtil()
        {
        }

        /*public ImageUtil(string fileName)

        {
            // store the file name
            _fileName = fileName;
            //string strExtension = Path.GetExtension(fileName).ToLower();

            _bmpSource = new Bitmap(_fileName);
        }

        public Bitmap Open()
        {
            // Open the image
            return _bmpSource;
        }

        public void Close()
        {
            try
            {
                if (_bmpSource != null)
                    _bmpSource.Dispose();
            }
            catch
            { }

            _bmpSource = null;
        }*/


        public void ResizeImage(string fileNameIn, string fileNameOut, int maxWidth, int maxHeight, bool isPadding, int compressRate)
        {
            // storage = ObjectFactory.GetInstance<IStorageService>();
            string strExtension = Path.GetExtension(fileNameIn).ToLower();

            // Open the image
            Bitmap bmpSource = new Bitmap(fileNameIn);

            Size originalSize = bmpSource.Size;
            Size newSize = new Size(0, 0);

            decimal maxWidthDecimal = Convert.ToDecimal(maxWidth);
            decimal maxHeightDecimal = Convert.ToDecimal(maxHeight);

            decimal imageRatio = Decimal.Divide(originalSize.Width, originalSize.Height);
            decimal frameRatio = Decimal.Divide(maxWidth, maxHeight);

            decimal resizeFactor = 0;

            // check orientation
            if (frameRatio > imageRatio)
            {
                // stick with height
                resizeFactor = Decimal.Divide(originalSize.Width, originalSize.Height);

                newSize.Height = maxHeight;
                newSize.Width = Convert.ToInt32(newSize.Height * resizeFactor);
            }
            else
            {
                // stick with width
                resizeFactor = Decimal.Divide(originalSize.Height, originalSize.Width);

                newSize.Width = maxWidth;
                newSize.Height = Convert.ToInt32(newSize.Width * resizeFactor);
            }

            Bitmap bmpTarget = null;
            Graphics grTarget = null;

            try
            {
                int newWidth = newSize.Width;
                int newHeight = newSize.Height;

                if (isPadding)
                {
                    bmpTarget = new Bitmap(maxWidth, maxHeight, PixelFormat.Format24bppRgb);

                    grTarget = Graphics.FromImage(bmpTarget);
                    grTarget.InterpolationMode = InterpolationMode.HighQualityBicubic;

                    // fill in the with the padding color first
                    grTarget.FillRectangle(new SolidBrush(Color.White), new Rectangle(0, 0, maxWidth, maxHeight));

                    // draw to only calculated area
                    grTarget.DrawImage(
                                            bmpSource,
                                            new Rectangle((maxWidth - newWidth) / 2, (maxHeight - newHeight) / 2, newWidth, newHeight),
                                            0,
                                            0,
                                            originalSize.Width,
                                            originalSize.Height,
                                            GraphicsUnit.Pixel
                                        );
                }
                else
                {
                    bmpTarget = new Bitmap(newWidth, newHeight, PixelFormat.Format24bppRgb);

                    grTarget = Graphics.FromImage(bmpTarget);
                    grTarget.InterpolationMode = InterpolationMode.HighQualityBicubic;

                    grTarget.DrawImage(
                                            bmpSource,
                                            new Rectangle(0, 0, newWidth, newHeight),
                                            0,
                                            0,
                                            originalSize.Width,
                                            originalSize.Height,
                                            GraphicsUnit.Pixel
                                        );
                }


                MemoryStream ms = new MemoryStream();

                // Save the new image
                if (".jpg" == strExtension || ".jpeg" == strExtension)
                {
                    EncoderParameter myEncoderParameter = new EncoderParameter(Encoder.Quality, compressRate);
                    EncoderParameters myEncoderParameters = new EncoderParameters(1);
                    myEncoderParameters.Param[0] = myEncoderParameter;
                    ImageCodecInfo jpgEncoder = GetEncoder(ImageFormat.Jpeg);
                    bmpTarget.Save(ms, jpgEncoder, myEncoderParameters);


                    //bmpTarget.Save(ms, ImageFormat.Jpeg);
                }
                else if (".png" == strExtension)
                    bmpTarget.Save(ms, ImageFormat.Png);
                else if (".gif" == strExtension)
                    bmpTarget.Save(ms, ImageFormat.Gif);
                else if (".bmp" == strExtension)
                    bmpTarget.Save(ms, ImageFormat.Bmp);

                // Write to physical image file
                FileStream outStream = File.OpenWrite(fileNameOut);
                ms.WriteTo(outStream);
                outStream.Flush();
                outStream.Close();

                ms.Close();
            }
            catch (Exception ex)
            {
                string ms = ex.Message;
                throw;
            }
            finally
            {
                if (grTarget != null)
                    grTarget.Dispose();

                if (bmpTarget != null && bmpTarget != bmpSource)
                    bmpTarget.Dispose();

                if (bmpSource != null)
                    bmpSource.Dispose();
            }
        }



        // Use the original image size as a reference
        private void GetProperImageSizeByImage(int FrameWidth, int FrameHeight, int ImageWidth, int ImageHeight,
                                              out int FinalImageLeft, out int FinalImageTop, out int FinalImageWidth,
                                              out int FinalImageHeight)
        {
            float flShowWidth, flShowHeight;
            float flImageRatio, flFrameRatio;

            // * Calculate new size, which remain aspect ratio
            flFrameRatio = ((float)FrameWidth / (float)FrameHeight);
            flImageRatio = ((float)ImageWidth / (float)ImageHeight);

            if (flFrameRatio >= flImageRatio)
            {
                flFrameRatio = (float)FrameHeight / (float)FrameWidth;

                flShowWidth = (float)ImageWidth;
                flShowHeight = (float)ImageWidth * flFrameRatio;
            }
            else
            {
                flFrameRatio = (float)FrameWidth / (float)FrameHeight;

                flShowWidth = (float)ImageHeight * flFrameRatio;
                flShowHeight = (float)ImageHeight;
            }

            // Final size
            FinalImageWidth = (int)flShowWidth;
            FinalImageHeight = (int)flShowHeight;

            // Start position (center)
            FinalImageLeft = (ImageWidth - FinalImageWidth) / 2;
            FinalImageTop = (ImageHeight - FinalImageHeight) / 2;

            return;
        }

        public bool CropImage(string filenameIn, string filenameOut, int frameWidth, int frameHeight, int compressRate)
        {
            try
            {
                int iProfileImageWidth = 0;
                int iProfileImageHeight = 0;
                int iImageLeft = 0;
                int iImageTop = 0;

                //storage = ObjectFactory.GetInstance<IStorageService>();

                string strExtension = Path.GetExtension(filenameIn).ToLower();

                System.Drawing.Image imgOrg = System.Drawing.Image.FromFile(filenameIn);

                // Find out the proper size for the output image
                GetProperImageSizeByImage(frameWidth, frameHeight, imgOrg.Width, imgOrg.Height, out iImageLeft,
                                          out iImageTop, out iProfileImageWidth, out iProfileImageHeight);

                // Prepare the new bitmap
                //Bitmap bmPhoto = new Bitmap(iProfileImageWidth, iProfileImageHeight, PixelFormat.Format24bppRgb);
                Bitmap bmPhoto = new Bitmap(frameWidth, frameHeight, PixelFormat.Format24bppRgb);
                bmPhoto.SetResolution(72, 72);
                Graphics grPhoto = Graphics.FromImage(bmPhoto);
                grPhoto.SmoothingMode = SmoothingMode.AntiAlias;
                grPhoto.InterpolationMode = InterpolationMode.HighQualityBicubic;
                grPhoto.PixelOffsetMode = PixelOffsetMode.HighQuality;

                // Create an output image
                grPhoto.DrawImage(
                    imgOrg,
                    //new Rectangle(0, 0, iProfileImageWidth, iProfileImageHeight),
                    new Rectangle(0, 0, frameWidth, frameHeight),
                    iImageLeft, iImageTop, iProfileImageWidth, iProfileImageHeight,
                    GraphicsUnit.Pixel);

                MemoryStream ms = new MemoryStream();

                // Save the new image
                if (".jpg" == strExtension)
                {
                    //grPhoto.InterpolationMode = InterpolationMode.HighQualityBicubic;
                    EncoderParameter myEncoderParameter = new EncoderParameter(Encoder.Quality, compressRate);
                    EncoderParameters myEncoderParameters = new EncoderParameters(1);
                    myEncoderParameters.Param[0] = myEncoderParameter;
                    ImageCodecInfo jpgEncoder = GetEncoder(ImageFormat.Jpeg);

                    //bmPhoto.Save(ms, ImageFormat.Jpeg);
                    bmPhoto.Save(ms, jpgEncoder, myEncoderParameters);
                }
                else if (".png" == strExtension)
                    bmPhoto.Save(ms, ImageFormat.Png);
                else if (".gif" == strExtension)
                    bmPhoto.Save(ms, ImageFormat.Gif);
                else if (".bmp" == strExtension)
                    bmPhoto.Save(ms, ImageFormat.Bmp);

                //storage.SaveFile(ms, filenameOut);
                ms.Close();

                // Save out to the file
                bmPhoto.Save(filenameOut, System.Drawing.Imaging.ImageFormat.Jpeg);

                // Free up memory
                bmPhoto.Dispose();
                imgOrg.Dispose();
            }
            catch
            {
                return false;
            }

            return true;
        }

        public bool ConvertToGrayScale(string filenameIn, string filenameOut)
        {
            Image imgOrg = Image.FromFile(filenameIn);

            // create a blank bitmap the same size as original
            Bitmap imgNew = new Bitmap(imgOrg.Width, imgOrg.Height);

            // get a graphics object from the new image
            Graphics g = Graphics.FromImage(imgNew);

            //create the grayscale ColorMatrix
            ColorMatrix colorMatrix = new ColorMatrix(
                                                       new float[][]
                                                       {
                                                         new float[] {.3f, .3f, .3f, 0, 0},
                                                         new float[] {.59f, .59f, .59f, 0, 0},
                                                         new float[] {.11f, .11f, .11f, 0, 0},
                                                         new float[] {0, 0, 0, 1, 0},
                                                         new float[] {0, 0, 0, 0, 1}
                                                       });

            // create some image attributes
            ImageAttributes attributes = new ImageAttributes();

            // set the color matrix attribute
            attributes.SetColorMatrix(colorMatrix);

            // draw the original image on the new image
            // using the grayscale color matrix
            g.DrawImage(imgOrg, new Rectangle(0, 0, imgOrg.Width, imgOrg.Height),
               0, 0, imgOrg.Width, imgOrg.Height, GraphicsUnit.Pixel, attributes);

            imgNew.Save(filenameOut);

            // free up memory
            imgNew.Dispose();
            imgOrg.Dispose();

            // dispose the Graphics object
            g.Dispose();

            return true;
        }

        /*/
        public Image CropImage(Image bmpSource, int x, int y, int width, int height)
        {
            try
            {
                // Prepare the blank bitmap according to the given size
                Bitmap bmPhoto = new Bitmap(width, height, PixelFormat.Format24bppRgb);
                bmPhoto.SetResolution(72, 72);
                Graphics grPhoto = Graphics.FromImage(bmPhoto);
                grPhoto.SmoothingMode = SmoothingMode.AntiAlias;
                grPhoto.InterpolationMode = InterpolationMode.HighQualityBicubic;
                grPhoto.PixelOffsetMode = PixelOffsetMode.HighQuality;

                // Create an output image
                grPhoto.DrawImage(
                                    bmpSource,
                                    new Rectangle(0, 0, width, height),
                                    x,
                                    y,
                                    width,
                                    height,
                                    GraphicsUnit.Pixel
                                  );

                // Save out to the file
                //bmPhoto.Save(filenameOut, System.Drawing.Imaging.ImageFormat.Jpeg);
                return bmPhoto;

                // Free up memory
                //bmPhoto.Dispose();
            }
            catch
            {
                return null;
            }
        }
        /*/

        /*/
        public void SaveImage(Image img, string fileName, int width, int height)
        {
            try
            {
                // Prepare the blank bitmap according to the given size
                Bitmap bmPhoto = new Bitmap(width, height, PixelFormat.Format24bppRgb);
                bmPhoto.SetResolution(72, 72);
                Graphics grPhoto = Graphics.FromImage(bmPhoto);
                grPhoto.SmoothingMode = SmoothingMode.AntiAlias;
                grPhoto.InterpolationMode = InterpolationMode.HighQualityBicubic;
                grPhoto.PixelOffsetMode = PixelOffsetMode.HighQuality;

                // Create an output image
                grPhoto.DrawImage(
                                    img,
                                    new Rectangle(0, 0, width, height),
                                    0,
                                    0,
                                    img.Width,
                                    img.Height,
                                    GraphicsUnit.Pixel
                                  );

                // Save out to the file
                bmPhoto.Save(fileName, System.Drawing.Imaging.ImageFormat.Jpeg);

                // Free up memory
                bmPhoto.Dispose();
            }
            catch
            {
                return;
            }
        }
        /*/


        private ImageCodecInfo GetEncoder(ImageFormat format)
        {

            ImageCodecInfo[] codecs = ImageCodecInfo.GetImageDecoders();

            foreach (ImageCodecInfo codec in codecs)
            {
                if (codec.FormatID == format.Guid)
                    return codec;
            }
            return null;
        }
    }
}