using System;
using System.Drawing;
using System.Windows.Forms;
using System.IO;

namespace ImageProcessing
{
    public partial class Image : Form
    {
        public Image()
        {
            InitializeComponent();
        }

        Bitmap bitmapImage, bitmapImage2/*used for tile*/, bitmapImage3;//used for reset
        Color[,] ImageArray = new Color[320, 240];//used for most filters
        Color[,] ImageArray2 = new Color[320, 240];//used for reset method
        Color[,] ImageArray3 = new Color[160, 120];//used for tile filter

        private void btnBrowse_Click(object sender, EventArgs e)
        {
            try
            {
                Stream myStream = null;
                OpenFileDialog openFileDialog1 = new OpenFileDialog();

                openFileDialog1.Filter = "Images (*.BMP;*.JPG;*.GIF)|*.BMP;*.JPG;*.GIF|All files (*.*)|*.*";
                openFileDialog1.FilterIndex = 2;
                openFileDialog1.RestoreDirectory = true;
                openFileDialog1.Title = "Image Browser";


                if (openFileDialog1.ShowDialog() == DialogResult.OK)
                {
                    if ((myStream = openFileDialog1.OpenFile()) != null)
                    {
                        System.Drawing.Image newImage = System.Drawing.Image.FromStream(myStream);
                        bitmapImage = new Bitmap(newImage, 320, 240);
                        bitmapImage2 = new Bitmap(newImage, 160, 120);//smaller resolution for tile filter
                        bitmapImage3 = new Bitmap(newImage, 320, 240);//used for reseting to original image
                        picImage.Image = bitmapImage;
                        myStream.Close();
                    }
                }
                //saving the bitmap to arrays
                SetArrayFromBitmap();
                SetArrayFromBitmap2();
            }
            catch//makes sure valid images are chosen
            {
                MessageBox.Show("Please select valid image");
                return;
            }
        }
        private void btnSave_Click(object sender, EventArgs e)//save picture button
        {
            try
            {
                SaveFileDialog sfd = new SaveFileDialog();
                sfd.Filter = "Images (.BMP;.JPG;.GIF)|.BMP;.JPG;.GIF|All files (.)|.";
                if (sfd.ShowDialog() == DialogResult.OK)
                {
                    bitmapImage.Save(sfd.FileName);
                    MessageBox.Show("Image saved succesfully");
                }
            }
            catch
            {
                MessageBox.Show("Image could not save");//if could not save for any reason
            }
        }

        public void Reset()//reset method
        {
            for (int row = 0; row < 320; row++)
                for (int col = 0; col < 240; col++)
                {
                    ImageArray[row, col] = ImageArray2[row, col];
                    bitmapImage = bitmapImage3;
                }
        }


        private void SetArrayFromBitmap()//save pixel of loaded image into array
        {
            try
            {
                for (int row = 0; row < 320; row++)
                    for (int col = 0; col < 240; col++)
                    {
                        ImageArray[row, col] = bitmapImage.GetPixel(row, col);
                        ImageArray2[row, col] = bitmapImage.GetPixel(row, col);//used for reset method
                    }
            }
            catch
            {
                return;
            }
        }
        private void SetArrayFromBitmap2()//save pixel of loaded image into array used for tile filter
        {
            try
            {
                for (int row = 0; row < 160; row++)
                    for (int col = 0; col < 120; col++)
                    {
                        ImageArray3[row, col] = bitmapImage2.GetPixel(row, col);
                    }
            }
            catch
            {
                return;
            }
        }

        private void picImage_Click(object sender, EventArgs e)
        {

        }

        private void SetBitmapFromArray()//set pixel of image from an array
        {
            for (int row = 0; row < 320; row++)
                for (int col = 0; col < 240; col++)
                {
                    bitmapImage.SetPixel(row, col, ImageArray[row, col]);
                }
        }

        private void btnTransform_Click(object sender, EventArgs e)
        {
            if (cBoxFilters.Text == "Original")//Show Original picture
            {
                if (bitmapImage == null)
                    return;

                Reset();

                SetBitmapFromArray();
                picImage.Image = bitmapImage;
            }

            if (cBoxFilters.Text == "Green")//Green filter
            {
                if (bitmapImage == null)
                    return;
                Reset();

                Byte Green;

                int iWidth = 320;
                int iHeight = 240;

                for (int i = 0; i < iWidth; i++)//sets all pixels to only green value
                {
                    for (int j = 0; j < iHeight; j++)
                    {
                        Color col = ImageArray[i, j];

                        Green = col.G;

                        Color newColor = Color.FromArgb(255, 0, Green, 0);
                        ImageArray[i, j] = newColor;

                    }
                }
                SetBitmapFromArray();
                picImage.Image = bitmapImage;
            }

            if (cBoxFilters.Text == "Red")//Red filter
            {
                if (bitmapImage == null)
                    return;

                Reset();
                Byte Red;

                int iWidth = 320;
                int iHeight = 240;


                for (int i = 0; i < iWidth; i++)//sets all pixels to only red value
                {
                    for (int j = 0; j < iHeight; j++)
                    {

                        Color col = ImageArray[i, j];

                        Red = col.R;

                        Color newColor = Color.FromArgb(255, Red, 0, 0);
                        ImageArray[i, j] = newColor;
                    }
                }
                SetBitmapFromArray();
                picImage.Image = bitmapImage;
            }
            if (cBoxFilters.Text == "Blue")//Blue Filter
            {
                if (bitmapImage == null)
                    return;

                Reset();
                Byte Blue;

                int iWidth = 320;
                int iHeight = 240;


                for (int i = 0; i < iWidth; i++)//sets all pixels to only blue value
                {
                    for (int j = 0; j < iHeight; j++)
                    {

                        Color col = ImageArray[i, j];

                        Blue = col.B;

                        Color newColor = Color.FromArgb(255, 0, 0, Blue);
                        ImageArray[i, j] = newColor;
                    }
                }
                SetBitmapFromArray();
                picImage.Image = bitmapImage;
            }
            if (cBoxFilters.Text == "Lighten")//Lighten Filter
            {
                if (bitmapImage == null)
                    return;

                Reset();

                int iWidth = 320;
                int iHeight = 240;

                for (int i = 0; i < iWidth; i++)
                {
                    for (int j = 0; j < iHeight; j++)
                    {
                        Color col = ImageArray[i, j];
                        //caps highest value to 255
                        //0.5 is the adjustment vlaue for lighten
                        //takes lower value which is lighter
                        int Green = (int)Math.Min(255, col.G + 255 * 0.5);
                        int Blue = (int)Math.Min(255, col.B + 255 * 0.5);
                        int Red = (int)Math.Min(255, col.R + 255 * 0.5);

                        Color newColor = Color.FromArgb(255, Red, Green, Blue);
                        ImageArray[i, j] = newColor;//replaces old pixels with new lighten pixels
                    }
                }
                SetBitmapFromArray();
                picImage.Image = bitmapImage;
            }
            if (cBoxFilters.Text == "Darken")//Darken Filter
            {
                if (bitmapImage == null)
                    return;

                Reset();

                int iWidth = 320;
                int iHeight = 240;

                for (int i = 0; i < iWidth; i++)
                {
                    for (int j = 0; j < iHeight; j++)
                    {
                        Color col = ImageArray[i, j];
                        //caps lowest value to 0
                        //0.5 is the adjustment value for darken
                        //takes higher value which is darker 
                        int Green = (int)Math.Max(0, col.G - 255 * 0.5);
                        int Blue = (int)Math.Max(0, col.B - 255 * 0.5);
                        int Red = (int)Math.Max(0, col.R - 255 * 0.5);

                        Color newColor = Color.FromArgb(255, Red, Green, Blue);
                        ImageArray[i, j] = newColor;//replaces old pixels with new darken pixels
                    }
                }
                SetBitmapFromArray();
                picImage.Image = bitmapImage;
            }
            if (cBoxFilters.Text == "Negative")//Negative Filter
            {
                if (bitmapImage == null)
                    return;

                Reset();
                int iWidth = 320;
                int iHeight = 240;

                for (int i = 0; i < iWidth; i++)
                {
                    for (int j = 0; j < iHeight; j++)
                    {
                        Color col = ImageArray[i, j];

                        Color newColor = Color.FromArgb(255, 255 - col.R, 255 - col.G, 255 - col.B);//max value minus color value gives negative value of image
                        ImageArray[i, j] = newColor;
                    }
                }
                SetBitmapFromArray();
                picImage.Image = bitmapImage;
            }
            if (cBoxFilters.Text == "Sunset Effect")//Sunset Effect Filter
            {
                if (bitmapImage == null)
                    return;

                Reset();
                int iWidth = 320;
                int iHeight = 240;

                for (int i = 0; i < iWidth; i++)
                {
                    for (int j = 0; j < iHeight; j++)
                    {
                        Color col = ImageArray[i, j];
                        //adjusting the RGB values so it looks like sunset
                        int Red = (col.R * 99) / 255;
                        int Green = (col.G * 48) / 255;
                        int Blue = (col.B * 34) / 255;

                        Color newColor = Color.FromArgb(255, Red, Green, Blue);
                        ImageArray[i, j] = newColor;
                    }
                }
                SetBitmapFromArray();
                picImage.Image = bitmapImage;
            }
            if (cBoxFilters.Text == "Grayscale")//Grayscale Filter
            {
                if (bitmapImage == null)
                    return;

                Reset();
                int iWidth = 320;
                int iHeight = 240;

                for (int i = 0; i < iWidth; i++)
                {
                    for (int j = 0; j < iHeight; j++)
                    {
                        Color col = ImageArray[i, j];

                        int Gray = (col.R + col.G + col.B) / 3;//same RGB values equals gray

                        Color newColor = Color.FromArgb(255, Gray, Gray, Gray);
                        ImageArray[i, j] = newColor;//replaces old pixels with different shades of grey
                    }
                }
                SetBitmapFromArray();
                picImage.Image = bitmapImage;
            }
            if (cBoxFilters.Text == "Polarize")//Polarize Effect Filter
            {
                if (bitmapImage == null)
                    return;
                Reset();

                int r = 0;
                int g = 0;
                int b = 0;
                int r2, g2, b2;
                int totalpixel = 0;

                int iWidth = 320;
                int iHeight = 240;

                for (int i = 0; i < iWidth; i++)//finds average for red blue green in the entire picture
                {
                    for (int j = 0; j < iHeight; j++)
                    {
                        Color col = ImageArray[i, j];

                        r = r + col.R;
                        b = b + col.B;
                        g = g + col.G;
                        totalpixel++;
                    }
                }
                r = r / totalpixel;//average for total red
                b = b / totalpixel;//average for total blue
                g = g / totalpixel;//average for total green


                for (int i = 0; i < iWidth; i++)
                {
                    for (int j = 0; j < iHeight; j++)//if and elses determines whether pixel is 0 or 255 based on average total rgb of picture
                    {
                        Color col = ImageArray[i, j];

                        int Red = col.R;
                        int Green = col.G;
                        int Blue = col.B;

                        if (Red > r)
                        {
                            r2 = 255;
                        }
                        else 
                        {
                            r2 = 0;
                        }
                        if (Blue > b)
                        {
                            b2 = 255;
                        }
                        else
                        {
                            b2 = 0;
                        }
                        if (Green > g)
                        {
                            g2 = 255;
                        }
                        else 
                        {
                            g2 = 0;
                        }
                        Color newColor = Color.FromArgb(255, r2, g2, b2);
                        ImageArray[i, j] = newColor;
                    }
                }
                SetBitmapFromArray();
                picImage.Image = bitmapImage;
            }
            if (cBoxFilters.Text == "Flip Vertically")//vertical flip
            {
                if (bitmapImage == null)
                    return;
                
                int iWidth = 320;
                int iHeight = 240;
                for (int i = 0; i < iWidth; i++)
                {
                    for (int j = 0; j < iHeight / 2; j++)//makes sure it only goes halfway so it has a complete flip
                    {
                        Color temp;
                        temp = ImageArray[i, j];
                        ImageArray[i, j] = ImageArray[i, 239 - j];
                        ImageArray[i, 239 - j] = temp;
                    }
                }

                SetBitmapFromArray();
                picImage.Image = bitmapImage;

            }
            if (cBoxFilters.Text == "Flip Horizontally")// horizontal flip
            {
                if (bitmapImage == null)
                    return;
                
                int iWidth = 320;
                int iHeight = 240;
                for (int i = 0; i < iWidth / 2; i++)//makes sure it only goes halfway so it has a complete flip
                {
                    for (int j = 0; j < iHeight; j++)
                    {
                        Color temp;
                        temp = ImageArray[i, j];
                        ImageArray[i, j] = ImageArray[319 - i, j];
                        ImageArray[319 - i, j] = temp;
                    }
                }
                SetBitmapFromArray();
                picImage.Image = bitmapImage;
            }
            if (cBoxFilters.Text == "Rotate 180 degrees")//flip horizontally than vertically
            {
                if (bitmapImage == null)
                    return;
                
                int iWidth = 320;
                int iHeight = 240;

                for (int i = 0; i < iWidth/2; i++)//horizontal flip
                {
                    for (int j = 0; j < iHeight; j++)
                    {
                        Color temp;
                        temp  = ImageArray[i, j];
                        ImageArray[i, j] = ImageArray[319-i, j];
                        ImageArray[319 - i, j] = temp;
                    }
                }
                for (int i = 0; i < iWidth; i++)//vertical flip
                {
                    for (int j = 0; j < iHeight/2; j++)
                    {
                        Color temp;
                        temp = ImageArray[i, j];
                        ImageArray[i, j] = ImageArray[i, 239-j];
                        ImageArray[i, 239-j] = temp;
                    }
                }
                SetBitmapFromArray();
                picImage.Image = bitmapImage;
            }
            if (cBoxFilters.Text == "Switch corners")
            {
                if (bitmapImage == null)
                    return;
                
                int iWidth = 320;
                int iHeight = 240;

                for (int i = iWidth/2; i < iWidth; i++)//divide width and height by 2 so it's only the top left corner
                {
                    for (int j = 0; j < iHeight/2; j++)
                    {
                        Color temp;//temporary value
                        temp  = ImageArray[i, j];//top left value
                        ImageArray[i, j] = ImageArray[i-iWidth/2,j+iHeight/2];//gets array value of bottom right corner and replaces it with top right
                        ImageArray[i - iWidth / 2, j + iHeight / 2] = temp;//replaces bottom right corner with top left 
                    }
                }
                SetBitmapFromArray();
                picImage.Image = bitmapImage;
            }
            if (cBoxFilters.Text == "Pixellate")
            {
                if (bitmapImage == null)
                    return;

                int iWidth = 320;
                int iHeight = 240;
                //pixellating image by 4x4
                for (int i = 0; i < iWidth; i=i+4)//goes throught every top left pixel in the picture
                {
                    for (int j = 0; j < iHeight; j=j+4)
                    {
                        Color temp = ImageArray[i, j];//top left corner color

                        for(int x = 0; x < 4; x++)//goes through and replaces with top left color
                        {
                            for(int y = 0; y < 4; y++)
                            {
                                ImageArray[i + x, j + y] = temp;
                            }
                        }
                    }
                }
                SetBitmapFromArray();
                picImage.Image = bitmapImage;
            }
            if (cBoxFilters.Text == "Sort")//Using insertion sort 
            {
                if (bitmapImage == null)
                    return;

                int iWidth = 320;
                int iHeight = 240;
                int avg1, avg2;

                
                for (int i = 0; i < iWidth; i++)// goes through the picture
                {
                    for (int j = 0; j < iHeight; j++)
                    {
                        int x;
                        Color col = ImageArray[i, j];
                        
                        avg1 = (col.R + col.G + col.B) / 3;//finding average value

                        for ( x = i - 1; x >= 0; x--)// insertionn sort
                        {
                            Color col2 = ImageArray[x, j];
                            avg2 = (col2.R + col2.G + col2.B) / 3;//finding second average value

                            // shift places so darkest to lightest
                            //comparing the two averages
                            if (avg1 < avg2)
                            {
                                ImageArray[x + 1, j] = ImageArray[x, j];
                            }
                            else
                            { // else colors do not swap
                                break;
                            }
                        }
                            ImageArray[x + 1, j] = col;//replaces the color according to shift
                    }
                }
                SetBitmapFromArray();
                picImage.Image = bitmapImage;
            }
            if (cBoxFilters.Text == "Tile")//Tile filter
            {
                if (bitmapImage == null)
                    return;

                int iWidth = 320;
                int iHeight = 240;

                for (int i = 0; i < iWidth / 2; i++)//goes through and replaces the image with 4 smaller 160x120 rectangles
                {
                    for (int j = 0; j < iHeight / 2; j++)
                    {
                        
                        ImageArray[i, j] = ImageArray3[i, j];//top left

                        ImageArray[160 + i, j] = ImageArray3[i, j];//top right

                        ImageArray[i, 120 + j] = ImageArray3[i, j];//bottom left

                        ImageArray[160 + i, 120 + j] = ImageArray3[i, j];//bottom right
                    }
                }
                
                for (int i = 160; i < iWidth/4+160; i++)//applies the flip horizontal filter for top right tile
                {
                    for (int j = 0; j < iHeight / 2; j++)
                    {
                        Color temp;
                        temp = ImageArray[i, j];
                        ImageArray[i, j] = ImageArray[479 - i, j];
                        ImageArray[479 - i, j] = temp;
                    }
                }
                
                for (int i = 0; i < iWidth/2; i++)//applies the flip vertical filter for bottom left tile
                {
                    for (int j = 120; j < iHeight / 4+120; j++)
                    {
                        Color temp;
                        temp = ImageArray[i, j];
                        ImageArray[i, j] = ImageArray[i, 359 - j];
                        ImageArray[i, 359 - j] = temp;
                    }
                }
                
                for (int i = 160; i < iWidth / 4 + 160; i++)//applies the flip horizontal then flip vertical filter for bottom right tile
                {
                    for (int j = 120; j < iHeight / 2+120; j++)
                    {
                        Color temp;
                        temp = ImageArray[i, j];
                        ImageArray[i, j] = ImageArray[479 - i, j];
                        ImageArray[479 - i, j] = temp;
                    }
                }

                for (int i = 160; i < iWidth / 2 + 160; i++)
                {
                    for (int j = 120; j < iHeight / 4 + 120; j++)
                    {
                        Color temp;
                        temp = ImageArray[i, j];
                        ImageArray[i, j] = ImageArray[i, 359 - j];
                        ImageArray[i, 359 - j] = temp;
                    }
                }
                SetBitmapFromArray();
                picImage.Image = bitmapImage;
            }
            if (cBoxFilters.Text == "Scroll")//scroll y axis by 80
            {
                if (bitmapImage == null)
                    return;

                int iWidth = 320;
                int iHeight = 240;

                for (int i = 0; i < iWidth; i++)//divide height by 3 so it's only top of picture
                {
                    for (int j = 0; j < iHeight / 3; j++)
                    {
                        Color temp,temp2;//temporary value
                        temp = ImageArray[i, j];//top value
                        temp2 = ImageArray[i, 80+j];//middle value
                        ImageArray[i, j] = ImageArray[i, 160 + j];//cycles through picture by 80 pixels
                        ImageArray[i, 80+j] = temp;
                        ImageArray[i, 160 + j] = temp2;
                    }
                }
                SetBitmapFromArray();
                picImage.Image = bitmapImage;
            }
        }
    }
}
