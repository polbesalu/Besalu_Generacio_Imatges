using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using static System.Net.Mime.MediaTypeNames;

namespace Besalu_Generacio_Imatges
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }
        private void btnCarregaImg_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.Filter = "Fitxers imatge|*.jpg;*.png";
            bool? clickOK = ofd.ShowDialog();
            if (clickOK == true)
            {
                try
                {
                    img.Source = new BitmapImage(new Uri(ofd.FileName));
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error al carregar la imatge: " + ex.Message);
                }
            }
        }

        private void btnGeneraImg_Click(object sender, RoutedEventArgs e)
        {
            int width = 300;
            int height = 300;
            int bytesPerPixel = PixelFormats.Bgra32.BitsPerPixel / 8;
            int stride = width * bytesPerPixel;
            byte[] pixels = new byte[(width * height) * bytesPerPixel];

            for (int y = 0; y < height; y++)
            {
                for (int x = 0; x < width; x++)
                {
                    int nPixel = y * width + x;
                    int nByte = nPixel * bytesPerPixel;
                    pixels[nByte] = 255;
                    pixels[nByte + 1] = 137;
                    pixels[nByte + 2] = 75;
                    pixels[nByte + 3] = 184;
                }
            }

            WriteableBitmap wb = new WriteableBitmap(width, height, 96, 96, PixelFormats.Bgra32, null);
            wb.WritePixels(new Int32Rect(0, 0, width, height), pixels, stride, 0);
            img.Source = wb;
        }

        private void btnGeneraLinies_Click(object sender, RoutedEventArgs e)
        {
            var imatge = new MyImage(300, 300);

            for (int y = 0; y < imatge.GetHeight(); y++)
            {
                for (int x = 0; x < imatge.GetWidth(); x++)
                {
                    int stripeWidth = imatge.GetHeight() / 33; // Amplada de cada franja

                    // Determina a quina franja pertany el píxel
                    int stripeIndex = y / stripeWidth;

                    // Alterna els colors de manera similar a l'exemple original
                    if (stripeIndex % 2 == 0)
                    {
                        imatge.setPixel(x, y, new byte[4] { 139, 0, 139, 255 });
                    }
                    else
                    {
                        imatge.setPixel(x, y, new byte[4] { 255, 48, 97, 105 });
                    }
                }
            }

            img.Source = imatge.GetWriteableBitmap();
        }

        private void btnGeneraQuadricula_Click(object sender, RoutedEventArgs e)
        {

            MyImage novaImatge = new MyImage(18, 18);

            // Defineix els colors dels quadrats
            Color color1 = Colors.MediumPurple;
            Color color2 = Colors.DeepPink;

            for (int y = 0; y < 18; y++)
            {
                for (int x = 0; x < 18; x++)
                {
                    // Alterna els colors en funció de la fila i la columna
                    Color color = ((x + y) % 2 == 0) ? color1 : color2;

                    byte[] pixel = { color.B, color.G, color.R, 255 }; // Format BGR
                    novaImatge.setPixel(x, y, pixel);
                }
            }

            img.Source = novaImatge.GetWriteableBitmap();

        }

        private void btnGeneraImatgeXifrat_Click(object sender, RoutedEventArgs e)
        {
            MyImage imatge = new MyImage(300, 200);

            // Omplir la imatge amb colors aleatoris
            Random random = new Random();
            for (int y = 0; y < imatge.GetHeight(); y++)
            {
                for (int x = 0; x < imatge.GetWidth(); x++)
                {
                    byte[] pixel = { (byte)random.Next(256), (byte)random.Next(256), (byte)random.Next(256), 255 };
                    imatge.setPixel(x, y, pixel);
                }
            }

            // Aplicar Juli Cèsar a cada component R, G, B amb un decalatge aleatori
            int shift = random.Next(1, 10); // Decalatge aleatori entre 1 i 10

            for (int y = 0; y < imatge.GetHeight(); y++)
            {
                for (int x = 0; x < imatge.GetWidth(); x++)
                {
                    byte[] originalPixel = imatge.getPixel(x, y);

                    // Aplicar Juli Cèsar als components R, G, B
                    for (int i = 0; i < 3; i++) // Index 0: R, 1: G, 2: B
                    {
                        originalPixel[i] = (byte)((originalPixel[i] + shift) % 256);
                    }

                    imatge.setPixel(x, y, originalPixel);
                }
            }

            // Crear una WriteableBitmap i mostrar la imatge modificada
            img.Source = imatge.GetWriteableBitmap();
        }

        private void btnDesxifrarMssg_Click(object sender, RoutedEventArgs e)
        {

            string rutaImagen = "C:\\Users\\pol.besalu\\Downloads\\missatgeOcult.bmp";

            MyImage imagen = new MyImage(new BitmapImage(new Uri(rutaImagen)));

            for (int i = 0; i < imagen.GetWidth(); i++)
            {
                for (int j = 0; j < imagen.GetHeight(); j++)
                {
                    byte[] bytesPerPixel = imagen.getPixel(i, j);
                    int[] bitsFirstByte = imagen.num2bits(bytesPerPixel[0]);
                    int[] bitsSecondByte = imagen.num2bits(bytesPerPixel[1]);
                    int[] bitsThirdByte = imagen.num2bits(bytesPerPixel[2]);

                    var bitsOcults = new int[8];

                    bitsOcults[0] = bitsThirdByte[3];
                    bitsOcults[1] = bitsSecondByte[0];
                    bitsOcults[2] = bitsSecondByte[1];
                    bitsOcults[3] = bitsFirstByte[4];
                    bitsOcults[4] = bitsSecondByte[2];
                    bitsOcults[5] = bitsFirstByte[2];
                    bitsOcults[6] = bitsFirstByte[1];
                    bitsOcults[7] = bitsFirstByte[3];

                    byte mensajeDesencriptado = imagen.bits2num(bitsOcults);

                    tbFraseDesencriptada.Text = tbFraseDesencriptada.Text + ((char)mensajeDesencriptado).ToString();
                }
            }
        }
    }
}
