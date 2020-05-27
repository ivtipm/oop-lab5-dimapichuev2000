using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Game_Sapper.Control
{
   public static class MapControl
   {
        public const int sizeMap = 10; // количество клеток в данном случае 10x10
        public const int sizeCell = 30;// размер клетки 

        public static int[,] map = new int[sizeMap, sizeMap];

        public static Button[,] buttons = new Button[sizeMap, sizeMap];

        public static Image spriteSet;

        private static Point firstCoord;// координаты первой нажатой кнопки

        private static int dontUsePicrure=0;// для того чтобы первая кнопка которую нажал пользователь не использовалась в расстановки бомб

        private static bool firstStep;//что бы на первый шаг никогда не было бомбы 

        public static Form form;

        private static void FormSize(Form thi)
        {
            thi.Width = sizeMap * sizeCell + 20;
            thi.Height = sizeMap * sizeCell + 63;

        }
        public static void Init(Form thi)
        {
            form = thi;
            dontUsePicrure = 0;
            firstStep = true; 
            spriteSet = new Bitmap(Path.Combine(new DirectoryInfo(Directory.GetCurrentDirectory()).Parent.Parent.FullName.ToString(), "sprite\\tiles.png"));//подгружаем спрайт
            FormSize(thi);
            InitMap();
            InitButn(thi);

        }
        private static void InitMap()
        {

            for (int i = 0; i < sizeMap; i++)
            {
                for (int j = 0; j < sizeMap; j++)
                {
                    map[i, j] = 0;
                }
            }
        }
        private static void InitButn(Form thi)
        {
            // создание кнопки          
            for (int i = 0; i < sizeMap; i++)
            {
                for (int j = 0; j < sizeMap; j++)
                {
                    Button button = new Button();
                    button.Location = new Point(j * sizeCell, i * sizeCell+23);
                    button.Size = new Size(sizeCell, sizeCell);
                    button.Image = FindNeededImage(0, 0);
                    // если button.Image = FindNeededImage(3, 2);
                    // то во всех ячейках будет 

                    button.MouseUp += new MouseEventHandler(OnButtonPressedMouse);
                    thi.Controls.Add(button);
                    buttons[i, j] = button;
                    

                }
            }
           
        }

        private static void OnButtonPressedMouse(object sender, MouseEventArgs e)
        {
            Button pressedButton = sender as Button;//обработка нажатий 
            switch (e.Button.ToString())
            {
                case "Right":
                    OnRightButtonPressed(pressedButton);
                    break;
                case "Left":
                    OnLeftButtonPressed(pressedButton);
                    break;

            }
            
            

        }

     
        private static void OnLeftButtonPressed(Button pressedButton)
        { 
            pressedButton.Enabled = false;
            int iButton = pressedButton.Location.Y / sizeCell;
            int jButton = pressedButton.Location.X / sizeCell;
            // проверка для того чтобы не попасть первый раз на бомбу
            pressedButton.Enabled=false;
            if (firstStep)
            {
                firstCoord = new Point(jButton, iButton);
                SeedMap();
                CountCellBomb();
                firstStep = false;

            }
            OpenCells(iButton, jButton);
            if (map[iButton, jButton] == -1)
            {
                ShowAllBombs(iButton, jButton);
                MessageBox.Show("Поражение!");
                form.Controls.Clear();
                form.Close();
            }

            checkMap();


        }
        private static void OnRightButtonPressed(Button pressedButton)
        {

            
            dontUsePicrure++;
            dontUsePicrure %= 3;
            int posX = 0;
            int posY = 0;
            switch (dontUsePicrure)
            {
                case 0:
                    posX = 0;
                    posY = 0;
                    break;
                case 1:
                    posX = 0;
                    posY = 2;
                    break;
                case 2:
                    posX = 2;
                    posY = 2;
                    break;
            }
            pressedButton.Image = FindNeededImage(posX, posY);
          
                  
           
            

        }
        /// <summary>
        /// показывает все бомбы если ты проиграл 
        /// </summary>
        private static void ShowAllBombs(int iBomb, int jBomb)
        {
            for (int i = 0; i < sizeMap; i++)
            {
                for (int j = 0; j < sizeMap; j++)
                {
                    if (i == iBomb && j == jBomb)
                        continue;
                    if (map[i, j] == -1)
                    {
                        buttons[i, j].Image = FindNeededImage(3, 2);
                    }
                }
            }
        }

        /// <summary>
        /// добавление картинки(спрайта) на кнопку с определенными координатами
        /// </summary>

        public static Image FindNeededImage(int xPos, int yPos)
        {
            Image image = new Bitmap(sizeCell, sizeCell);
            Graphics g = Graphics.FromImage(image);
            g.DrawImage(spriteSet, new Rectangle(new Point(0, 0), new Size(sizeCell, sizeCell)),  32 * xPos, 32 * yPos, 33, 33, GraphicsUnit.Pixel);


            return image;
        }
        /// <summary>
        /// добавление бомбочек 
        /// </summary>
        private static void SeedMap()
        {
            Random r = new Random();
            int number = 2;

            for (int i = 0; i < number; i++)
            {
                int posI = r.Next(0, sizeMap - 1);
                int posJ = r.Next(0, sizeMap - 1);
                //для исключения повторов с бомбочками 
                while (map[posI, posJ] == -1 || (Math.Abs(posI - firstCoord.Y) <= 1 && Math.Abs(posJ - firstCoord.X) <= 1))
                {
                    posI = r.Next(0, sizeMap - 1);
                    posJ = r.Next(0, sizeMap - 1);
                }
                map[posI, posJ] = -1;
            }
        }

        private static void checkMap()
        {
            int tmp = 0;
           
            for (int i = 0; i < sizeMap; i++)
            {
                for (int j = 0; j < sizeMap; j++) {

                    if ((buttons[i, j].Enabled))
                    {
                        
                            tmp++;
                    }
                    

                    
                }

                
            }




                if (tmp == 2)
                {
                    MessageBox.Show("Победа!!! :)");
                    form.Controls.Clear();
                    form.Close();

                }
            
        
        }


        /// <summary>
        /// считает количество бомб вокруг ячейки
        /// </summary>
        private static void CountCellBomb()
        {
            for(int i = 0; i<sizeMap; i++)
            {
                for(int j = 0; j< sizeMap; j++)
                {
                    if (map[i, j] == -1)
                    {
                        for(int k = i - 1; k<i + 2; k++)
                        {
                            for(int l = j - 1; l<j + 2; l++)
                            {
                                if (!IsInBorder(k, l) || map[k, l] == -1)
                                    continue;
                                map[k, l] = map[k, l] + 1;
                            }
}
                    }
                }
            }
        }
        /// <summary>
        /// какое количество бомб находится вокруг ячейки 
        /// </summary>
        private static void OpenCell(int i, int j)
        {
            buttons[i, j].Enabled = false;

            switch (map[i, j])
            {
                case 1:
                    buttons[i, j].Image = FindNeededImage(1, 0);
                    break;
                case 2:
                    buttons[i, j].Image = FindNeededImage(2, 0);
                    break;
                case 3:
                    buttons[i, j].Image = FindNeededImage(3, 0);
                    break;
                case 4:
                    buttons[i, j].Image = FindNeededImage(4, 0);
                    break;
                case 5:
                    buttons[i, j].Image = FindNeededImage(0, 1);
                    break;
                case 6:
                    buttons[i, j].Image = FindNeededImage(1, 1);
                    break;
                case 7:
                    buttons[i, j].Image = FindNeededImage(2, 1);
                    break;
                case 8:
                    buttons[i, j].Image = FindNeededImage(3, 1);
                    break;
                case -1:
                    buttons[i, j].Image = FindNeededImage(1, 2);
                    break;
                case 0:
                    buttons[i, j].Image = FindNeededImage(0, 0);
                    break;
            }
        }
        /// <summary>
        /// открывает путые клетки 
        /// </summary>
        private static void OpenCells(int i, int j)
        {
            OpenCell(i, j);

            if (map[i, j] > 0)
                return;

            for (int k = i - 1; k < i + 2; k++)
            {
                for (int l = j - 1; l < j + 2; l++)
                {
                    if (!IsInBorder(k, l))
                        continue;
                    if (!buttons[k, l].Enabled)
                        continue;
                    if (map[k, l] == 0)
                        OpenCells(k, l);
                    else if (map[k, l] > 0)
                        OpenCell(k, l);
                }
            }
        }
        /// <summary>
        /// проверка что бы не выйти за поле 
        /// </summary>
        private static bool IsInBorder(int i, int j)
        {
            if (i < 0 || j < 0 || j > sizeMap - 1 || i > sizeMap - 1)
            {
                return false;
            }
            return true;
        }
    

    }
}
