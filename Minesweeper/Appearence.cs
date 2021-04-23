using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Minesweeper
{
    public static class Appearance
    {
        private static int currentPictureToSet = 0;
        public static int fieldSize = 18;//max количество клеток
        private const int cellSize = 30;//размер клетки
        private static int[,] field = new int[fieldSize, fieldSize];
        private static Button[,] buttons = new Button[fieldSize, fieldSize];
        private static bool isFirstStep;
        private static Image set_of_images;
        private static Point firstCoord;
        private static Form form;
        private static int cellsOpened = 0;
        private static int bombs = 0;

        private static ToolStrip menu = new ToolStrip();
        private static ToolStripButton ts_btnNewGame = new ToolStripButton();
        private static ToolStripButton ts_btnExit = new ToolStripButton();
        private static ToolStripLabel ts_counter = new ToolStripLabel();
        private static ToolStripSeparator tsSeparator1 = new ToolStripSeparator();
        private static ToolStripSeparator tsSeparator2 = new ToolStripSeparator();
        private static ToolStripSeparator tsSeparator3 = new ToolStripSeparator();
        private static ToolStripDropDownButton dropDownButton1 = new ToolStripDropDownButton();
        private static ToolStripDropDown dropDown = new ToolStripDropDown();
        private static ToolStripButton button6 = new ToolStripButton();
        private static ToolStripButton button9 = new ToolStripButton();
        private static ToolStripButton button12 = new ToolStripButton();
        private static ToolStripButton button15 = new ToolStripButton();
        private static ToolStripButton button18 = new ToolStripButton();
        private static ListBox lb = new ListBox();
        private static DateTime date;
        public static Timer timer = new Timer();
        private static ToolStripLabel label1 = new ToolStripLabel();
        private static Label label2 = new Label();
        public static void CreatingFiald(Form spr)
        {
            form = spr;
            currentPictureToSet = 0;
            isFirstStep = true;
            set_of_images = new Bitmap(Path.Combine(new DirectoryInfo(Directory.GetCurrentDirectory()).Parent.Parent.FullName.ToString(), "Images\\3.png"));

            label2.Text = "Таблица побед:";

            lb.Items.AddRange(File.ReadAllLines("12.txt"));


            ToolStrip1(spr);
            FieldSize(spr);
            Field();
            table(spr);
            Button_on_Field(spr);
        }
        private static void table(Form spr)
        {
            lb.Dock = DockStyle.Right;
            lb.Location = new Point(fieldSize * cellSize, 25);
            //lb.Items.Add(label2);
            label2.Location = new Point(455, 0);
            label2.Size = new Size(89, 15);
            spr.Controls.Add(label2);
            spr.Controls.Add(lb);
        }
        private static void ToolStrip1(Form spr)
        {
            menu.Dock = DockStyle.None;
            menu.LayoutStyle = ToolStripLayoutStyle.HorizontalStackWithOverflow;
            ts_btnNewGame.Text = "Новая игра";
            ts_btnExit.Text = "Выход";
            dropDownButton1.Text = "Размер поля";
            menu.Items.Add(ts_btnNewGame);
            menu.Items.Add(tsSeparator1);
            menu.Items.Add(label1);
            label1.Text = "00:00:00:00";
            menu.Items.Add(tsSeparator2);
            menu.Items.Add(dropDownButton1);
            menu.Items.Add(tsSeparator3);
            menu.Items.Add(ts_btnExit);
            spr.Controls.Add(menu);
            ts_btnExit.Click += new EventHandler(Ts_btnExit_Click);
            ts_btnNewGame.Click += new EventHandler(Ts_btnNewGame_Click);
            dropDownButton1.DropDown = dropDown;
            dropDownButton1.DropDownDirection = ToolStripDropDownDirection.BelowRight;
            dropDownButton1.ShowDropDownArrow = false;
            button6.Text = "6х6";
            button9.Text = "9х9";
            button12.Text = "12х12";
            button15.Text = "15х15";
            button18.Text = "18х18";
            button6.Click += new EventHandler(Click6);
            button9.Click += new EventHandler(Click9);
            button12.Click += new EventHandler(Click12);
            button15.Click += new EventHandler(Click15);
            button18.Click += new EventHandler(Click18);

            dropDown.Items.AddRange(new ToolStripItem[] { button6, button9, button12, button15, button18 });
            //spr.Controls.Add(label2);
        }

        private static void Click18(object sender, EventArgs e)
        {
            fieldSize = 18;
            form.Controls.Clear();
            timer.Stop();
            CreatingFiald(form);
        }

        private static void Click15(object sender, EventArgs e)
        {
            fieldSize = 15;
            form.Controls.Clear();
            timer.Stop();
            CreatingFiald(form);
        }

        private static void Click12(object sender, EventArgs e)
        {
            fieldSize = 12;
            form.Controls.Clear();
            timer.Stop();
            CreatingFiald(form);
        }

        private static void Click9(object sender, EventArgs e)
        {
            fieldSize = 9;
            form.Controls.Clear();
            timer.Stop();
            CreatingFiald(form);
        }

        private static void Click6(object sender, EventArgs e)
        {
            fieldSize = 6;
            form.Controls.Clear();
            timer.Stop();
            CreatingFiald(form);

        }

        private static void Ts_btnNewGame_Click(object sender, EventArgs e)
        {
            form.Controls.Clear();
            timer.Stop();
            cellsOpened = 0;
            CreatingFiald(form);
        }

        private static void time2()
        {
            date = DateTime.Now;
            timer.Interval = 10;
            timer.Tick += new EventHandler(Timer1_Tick);
            timer.Start();
        }

        private static void OnLeftButtonPressed(Button pressedButton)
        {
            pressedButton.Enabled = false;
            int iButton = pressedButton.Location.X / cellSize;
            int jButton = pressedButton.Location.Y / cellSize;
            if (isFirstStep)
            {
                time2();
                firstCoord = new Point(jButton, iButton);
                SeedField();
                CountCellBomb();
                isFirstStep = false;

            }
            OpenCells(iButton, jButton);
            if (field[iButton, jButton] == -1)
            {
                timer.Stop();
                ShowAllBombs(iButton, jButton);
                MessageBox.Show("Поражение");
                form.Controls.Clear();
                cellsOpened = 0;
                CreatingFiald(form);
            }
            CheckWin();
        }

        private static void Timer1_Tick(object sender, EventArgs e)
        {
            long tick = DateTime.Now.Ticks - date.Ticks;
            DateTime stopWatch = new DateTime();
            stopWatch = stopWatch.AddTicks(tick);
            label1.Text = String.Format("{00:HH:mm:ss:ff}", stopWatch);
        }

        private static void Ts_btnExit_Click(object sender, EventArgs e)
        {
            form.Close();
        }

        private static void FieldSize(Form spr)
        {
            spr.Width = 680;// fieldSize * cellSize + 500;
            spr.Height = 604;// (fieldSize + 2) * cellSize+4;
        }

        private static void Field()
        {
            for (int i = 0; i < fieldSize; i++)
            {
                for (int j = 0; j < fieldSize; j++)
                {
                    field[i, j] = 0;
                }
            }
        }

        private static void Button_on_Field(Form spr)
        {
            for (int i = 0; i < fieldSize; i++)
            {
                for (int j = 0; j < fieldSize; j++)
                {
                    Button button = new Button();
                    button.Location = new Point((i * cellSize), 25 + (j * cellSize));
                    button.Size = new Size(cellSize, cellSize);
                    button.Image = FindNeededImage(0, 0);
                    button.MouseUp += new MouseEventHandler(OnButtonPressedMouse);
                    spr.Controls.Add(button);
                    buttons[i, j] = button;
                }
            }
        }

        private static void OnButtonPressedMouse(object sender, MouseEventArgs e)
        {
            Button pressedButton = sender as Button;
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

        private static void OnRightButtonPressed(Button pressedButton)
        {
            currentPictureToSet++;
            currentPictureToSet %= 3;
            int posX = 0;
            int posY = 0;
            switch (currentPictureToSet)
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

        private static void ShowAllBombs(int iBomb, int jBomb)
        {
            for (int i = 0; i < fieldSize; i++)
            {
                for (int j = 0; j < fieldSize; j++)
                {
                    if (i == iBomb && j == jBomb)
                    {
                        continue;
                    }
                    if (field[i, j] == -1)
                    {
                        buttons[i, j].Image = FindNeededImage(3, 2);
                    }
                }
            }
        }

        public static Image FindNeededImage(int xPos, int yPos)
        {
            Image image = new Bitmap(cellSize, cellSize);
            Graphics g = Graphics.FromImage(image);
            g.DrawImage(set_of_images, new Rectangle(new Point(0, 0), new Size(cellSize, cellSize)), 0 + 32 * xPos, 0 + 32 * yPos, 33, 33, GraphicsUnit.Pixel);
            return image;
        }

        private static void SeedField()
        {
            Random r = new Random();
            int n = 0;
            if (fieldSize >= 6 && fieldSize < 9)
            {
                n = 7;
            }
            else if (fieldSize == 9 || fieldSize == 10)
            {
                n = 10;
            }
            else if (fieldSize > 10 && fieldSize < 15)
            {
                n = 20;
            }
            else if (fieldSize >= 15)
            {
                n = 30;
            }
            for (int i = 0; i < n; i++)
            {
                int posI = r.Next(0, fieldSize - 1);
                int posJ = r.Next(0, fieldSize - 1);
                while (field[posI, posJ] == -1 || Math.Abs(posI - firstCoord.Y) <= 1 && Math.Abs(posJ - firstCoord.X) <= 1)
                {
                    posI = r.Next(0, fieldSize - 1);
                    posJ = r.Next(0, fieldSize - 1);
                }
                field[posI, posJ] = -1;
            }
            bombs = n;
        }

        private static void CountCellBomb()
        {
            for (int i = 0; i < fieldSize; i++)
            {
                for (int j = 0; j < fieldSize; j++)
                {
                    if (field[i, j] == -1)
                    {
                        for (int k = i - 1; k < i + 2; k++)
                        {
                            for (int l = j - 1; l < j + 2; l++)
                            {
                                if (!IsInBorder(k, l) || field[k, l] == -1)
                                {
                                    continue;
                                }
                                else
                                {
                                    field[k, l] += 1;
                                }
                            }
                        }
                    }
                }
            }
        }

        private static bool IsInBorder(int i, int j)
        {
            if (i < 0 || j < 0 || i > fieldSize - 1 || j > fieldSize - 1)
            {
                return false;
            }
            return true;
        }

        private static void OpenCell(int i, int j)
        {

            buttons[i, j].Enabled = false;

            switch (field[i, j])
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
            cellsOpened++;
        }

        private static void OpenCells(int i, int j)
        {
            OpenCell(i, j);

            if (field[i, j] > 0)
            {
                return;
            }
            for (int k = i - 1; k < i + 2; k++)
            {
                for (int l = j - 1; l < j + 2; l++)
                {
                    if (!IsInBorder(k, l) || !buttons[k, l].Enabled)
                    {
                        continue;
                    }
                    if (field[k, l] == 0)
                    {
                        OpenCells(k, l);
                    }
                    else if (field[k, l] > 0)
                    {
                        OpenCell(k, l);
                    }
                }
            }
        }

        private static void CheckWin()
        {
            int cells = fieldSize * fieldSize;
            //MessageBox.Show(" " + (cells - bombs) + " " + cellsOpened);
            if ((cells - bombs) == (cellsOpened))
            {
                timer.Stop();
                MessageBox.Show("Победа!!! Ваше время:" + label1.Text);
                File.AppendAllText("12.txt", label1 + "\n");
                lb.Items.Add(label1);
            }
        }
    }
}
