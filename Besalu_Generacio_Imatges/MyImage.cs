using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;
using System.Windows.Media;
using System.Windows;

namespace Besalu_Generacio_Imatges
{
    public class MyImage
    {
        private byte[] pixels;
        private int width;
        private int height;
        private int bytesPerPixel;
        private int stride;

        public int GetWidth() { return width; }
        public int GetHeight() { return height; }

        public MyImage(int width, int height)
        {
            this.width = width;
            this.height = height;
            this.bytesPerPixel = PixelFormats.Bgra32.BitsPerPixel / 8;
            this.stride = width * bytesPerPixel;

            pixels = new byte[width * height * bytesPerPixel];
        }
        public MyImage(BitmapImage imatgeOriginal)
        {
            //Obtenir una imatge de disc

            if (imatgeOriginal.Format.BitsPerPixel == PixelFormats.Bgra32.BitsPerPixel)
            {
                throw new Exception("Error no podem treballar amb rgba32");
            }
            bytesPerPixel = imatgeOriginal.Format.BitsPerPixel / 8;
            this.width = imatgeOriginal.PixelWidth;
            this.height= imatgeOriginal.PixelHeight;
            imatgeOriginal.CopyPixels(this.pixels, width * bytesPerPixel, 0);
        }

        public int[] num2bits(int num)
        {
            int[] bits = new int[0];
            int i = 7;

            while(num >= 1)
            {
                bits[i] = num % 2;
                num /= 2;
                i--;
            }

            while (i >= 0)
                bits[i] = 0;

            return bits;
        }

        public byte[] getPixel(int x, int y)
        {
            int nPixel = x + y + width;
            int nByte = nPixel * bytesPerPixel;
            return new byte[4] { pixels[nByte], pixels[nByte + 1], pixels[nByte + 2], pixels[nByte + 3] };

        }
        public void setPixel(int x, int y, byte[] pixel)
        {
            int nPixel = y * width + x;
            int nByte = nPixel * bytesPerPixel;


            pixels[nByte] = pixel[0];
            pixels[nByte + 1] = pixel[1];
            pixels[nByte + 2] = pixel[2];
            pixels[nByte + 3] = pixel[3];
        }
        public WriteableBitmap GetWriteableBitmap()
        {
            WriteableBitmap wb = new WriteableBitmap(width, height, 96, 96, PixelFormats.Bgra32, null);
            wb.WritePixels(new Int32Rect(0, 0, width, height), pixels, stride, 0);
            return wb;
        }
    }

}
