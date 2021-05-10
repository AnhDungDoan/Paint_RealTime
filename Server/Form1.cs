﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Threading;
using System.Net;
using System.Net.Sockets;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

namespace paint_clone
{
    public partial class Server : Form
    {

        /// <summary>
        /// Initialization of global variables
        /// </summary>
        public Color currentColor;

        public Bitmap canvas;
        public Graphics g;

        private Point lastPoint = Point.Empty;
        private bool isMouseDown = new Boolean();

        private List<Bitmap> undoRedoList = new List<Bitmap>();
        protected int counter = 0;

        public Server()
        {
            InitializeComponent();

            CheckForIllegalCrossThreadCalls = false;

            Connect();
            canvasSizeLabel.Text = "Canvas Size: " + pictureBox1.Size.Width + "x" + pictureBox1.Size.Height;
            zoomStatusLabel.Text = "Zoom: " + trackBar1.Value + "x";

            canvas = new Bitmap(pictureBox1.Width, pictureBox1.Height);
            pictureBox1.Image = canvas;
            g = Graphics.FromImage(pictureBox1.Image);
            undoRedoList.Add(canvas);
        }
        #region colors
        [Description("The functions below control the colours of anything drawn")]
        private void CustomColoursBTN_Click(object sender, EventArgs e)
        {
            colorDialog1.ShowDialog();
            currentColor = colorDialog1.Color;
            currentColorBox.BackColor = currentColor;
        }
        private void Form1_Load(object sender, EventArgs e)
        {
            currentColor = Color.Black;//default
            currentColorBox.BackColor = currentColor;
        }
        private void blackBox_Click(object sender, EventArgs e)
        {
            currentColor = blackBox.BackColor;
            currentColorBox.BackColor = currentColor;
        }
        private void whiteBox_Click(object sender, EventArgs e)
        {
            currentColor = whiteBox.BackColor;
            currentColorBox.BackColor = currentColor;
        }
        private void greyBox_Click(object sender, EventArgs e)
        {
            currentColor = greyBox.BackColor;
            currentColorBox.BackColor = currentColor;
        }
        private void silverBox_Click(object sender, EventArgs e)
        {
            currentColor = silverBox.BackColor;
            currentColorBox.BackColor = currentColor;
        }
        private void maroonBox_Click(object sender, EventArgs e)
        {
            currentColor = maroonBox.BackColor;
            currentColorBox.BackColor = currentColor;
        }
        private void brownBox_Click(object sender, EventArgs e)
        {
            currentColor = brownBox.BackColor;
            currentColorBox.BackColor = currentColor;
        }

        private void redBox_Click(object sender, EventArgs e)
        {
            currentColor = redBox.BackColor;
            currentColorBox.BackColor = currentColor;
        }

        private void pinkBox_Click(object sender, EventArgs e)
        {
            currentColor = pinkBox.BackColor;
            currentColorBox.BackColor = currentColor;
        }

        private void orangeBox_Click(object sender, EventArgs e)
        {
            currentColor = orangeBox.BackColor;
            currentColorBox.BackColor = currentColor;
        }

        private void lightOrangeBox_Click(object sender, EventArgs e)
        {
            currentColor = lightOrangeBox.BackColor;
            currentColorBox.BackColor = currentColor;
        }

        private void yellowBox_Click(object sender, EventArgs e)
        {
            currentColor = yellowBox.BackColor;
            currentColorBox.BackColor = currentColor;
        }

        private void lightYellowBox_Click(object sender, EventArgs e)
        {
            currentColor = lightYellowBox.BackColor;
            currentColorBox.BackColor = currentColor;
        }

        private void greenBox_Click(object sender, EventArgs e)
        {
            currentColor = greenBox.BackColor;
            currentColorBox.BackColor = currentColor;
        }

        private void lightGreenBox_Click(object sender, EventArgs e)
        {
            currentColor = lightGreenBox.BackColor;
            currentColorBox.BackColor = currentColor;
        }

        private void turquoiseBox_Click(object sender, EventArgs e)
        {
            currentColor = turquoiseBox.BackColor;
            currentColorBox.BackColor = currentColor;
        }

        private void lavendarBox_Click(object sender, EventArgs e)
        {
            currentColor = lavendarBox.BackColor;
            currentColorBox.BackColor = currentColor;
        }

        private void blueBox_Click(object sender, EventArgs e)
        {
            currentColor = blueBox.BackColor;
            currentColorBox.BackColor = currentColor;
        }

        private void violetBox_Click(object sender, EventArgs e)
        {
            currentColor = violetBox.BackColor;
            currentColorBox.BackColor = currentColor;
        }
        #endregion
        #region brushes/mouse controls
        private void pictureBox1_MouseDown(object sender, MouseEventArgs e)
        {
            lastPoint = e.Location;
            isMouseDown = true;

            if (paintBucketRadioBtn.Checked == true)//fill function is done, for now, but later try and write own version
            {
                FloodFill(canvas, e.Location, canvas.GetPixel(e.X, e.Y), currentColor);
            }
        }
        private void pictureBox1_MouseMove(object sender, MouseEventArgs e)
        {
            mousePositionLabel.Text = "Pointer Position: " + e.X + ", " + e.Y;
            if (penRadioBtn.Checked == true)
            {
                if (isMouseDown == true)
                {
                    if (lastPoint != null)
                    {
                        Pen p = new Pen(currentColor, (float)thicknessController.Value);
                        p.StartCap = LineCap.Round;
                        p.EndCap = LineCap.Round;

                        g.SmoothingMode = SmoothingMode.AntiAlias;
                        g.DrawLine(p, lastPoint, e.Location);
                        lastPoint = e.Location;

                        p.Dispose();
                    }
                }
            }
            if (paintbrushRadioBtn.Checked == true)
            {
                if (isMouseDown == true)
                {
                    if (lastPoint != null)
                    {
                        Pen paintBrush = new Pen(currentColor, (float)thicknessController.Value * 2);
                        g.SmoothingMode = SmoothingMode.AntiAlias;

                        paintBrush.StartCap = LineCap.Round;
                        paintBrush.EndCap = LineCap.Round;

                        g.DrawLine(paintBrush, lastPoint, e.Location);

                        lastPoint = e.Location;
                        paintBrush.Dispose();
                    }
                }
            }
            if (eraserRadioBtn.Checked == true)
            {
                if (isMouseDown == true)
                {
                    if (lastPoint != null)
                    {
                        Pen eraser = new Pen(Color.White, (float)thicknessController.Value * 2);
                        g.SmoothingMode = SmoothingMode.AntiAlias;

                        eraser.StartCap = LineCap.Square;
                        eraser.EndCap = LineCap.Square;

                        g.DrawLine(eraser, lastPoint, e.Location);

                        lastPoint = e.Location;
                        eraser.Dispose();
                    }
                }
            }
            pictureBox1.Invalidate();
        }
        private void pictureBox1_MouseUp(object sender, MouseEventArgs e)
        {
            lastPoint = e.Location;
            isMouseDown = false;
            //for undo, one pen stroke = 1 complete action
            undoRedoList.Add(canvas);
            if (undoRedoList.Count < 1)
            {
                undoButton.Enabled = false;
            }
            else
            {
                undoButton.Enabled = true;
            }

            foreach (Socket item in clientList)
            {
                item.Send(Serialize(canvas));
            }

        }
        #endregion
        #region fill functionality
        private static bool ColorMatch(Color a, Color b)
        {
            return (a.ToArgb() & 0xffffff) == (b.ToArgb() & 0xffffff);
        }

        private void FloodFill(Bitmap bmp, Point pt, Color targetColor, Color replacementColor)
        {
            Stack<Point> pixels = new Stack<Point>();

            targetColor = bmp.GetPixel(pt.X, pt.Y);
            int y1;
            bool spanLeft, spanRight;
            pixels.Push(pt);
            while (pixels.Count != 0)
            {
                Point temp = pixels.Pop();
                y1 = temp.Y;
                while (y1 > 0 && bmp.GetPixel(temp.X, y1) == targetColor)
                {
                    y1--;
                }
                y1++;
                spanLeft = false;
                spanRight = false;
                while (y1 < bmp.Height && bmp.GetPixel(temp.X, y1) == targetColor)
                {
                    bmp.SetPixel(temp.X, y1, replacementColor);

                    if (!spanLeft && temp.X > 0 && bmp.GetPixel(temp.X - 1, y1) == targetColor)
                    {
                        pixels.Push(new Point(temp.X - 1, y1));
                        spanLeft = true;
                    }
                    else if (spanLeft && temp.X - 1 == 0 && bmp.GetPixel(temp.X - 1, y1) != targetColor)
                    {
                        spanLeft = false;
                    }
                    if (!spanRight && temp.X < bmp.Width - 1 && bmp.GetPixel(temp.X + 1, y1) == targetColor)
                    {
                        pixels.Push(new Point(temp.X + 1, y1));
                        spanRight = true;
                    }
                    else if (spanRight && temp.X < bmp.Width - 1 && bmp.GetPixel(temp.X + 1, y1) != targetColor)
                    {
                        spanRight = false;
                    }
                    y1++;
                }

            }
            pictureBox1.Refresh();
            return;
        }
        #endregion
        #region toolstrip menu items
        private void clearCanvasMenuItem_Click(object sender, EventArgs e)
        {
            g.Clear(Color.White);
            pictureBox1.Invalidate();
        }
        private void trackBar1_Scroll(object sender, EventArgs e)
        {
            zoomStatusLabel.Text = "Zoom: " + trackBar1.Value + "x";
        }
        #endregion

        #region undo/redo
        #endregion

        private void pictureBox1_Click(object sender, EventArgs e)
        {

        }

        private void button10_Click(object sender, EventArgs e)
        {

        }

        private void button7_Click(object sender, EventArgs e)
        {

        }

        IPEndPoint IP;
        Socket server;
        List<Socket> clientList;

        void Connect()
        {
            clientList = new List<Socket>();
            //IP: server address
            IP = new IPEndPoint(IPAddress.Any, 9000);
            server = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.IP);

            server.Bind(IP);

            Thread Listen = new Thread(() =>
            {
                try
                {
                    while (true)
                    {
                        server.Listen(100);
                        Socket client = server.Accept();
                        clientList.Add(client);

                        Thread receive = new Thread(Receive);
                        receive.IsBackground = true;
                        receive.Start(client);
                    }
                }
                catch
                {
                    //IP: server address
                    IP = new IPEndPoint(IPAddress.Any, 9000);
                    server = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.IP);

                }

            });

            Listen.IsBackground = true;
            Listen.Start();
        }


        void Send()
        {
        }
        void Receive(object obj)
        {
            Socket Client = obj as Socket;
            try
            {
                while (true)
                {
                    byte[] data = new byte[1024 * 5000];
                    Client.Receive(data);

                    Image x = (Image)Deserialize(data);
                    ShowImage(x);
                }
            }
            catch
            {
                clientList.Remove(Client);
                Client.Close();
            }

        }
        void ShowImage(Image x)
        {
            pictureBox1.Image = x;
        }
        byte[] Serialize(object obj)
        {
            MemoryStream stream = new MemoryStream();
            BinaryFormatter formatterr = new BinaryFormatter();

            formatterr.Serialize(stream, obj);
            return stream.ToArray();
        }
        object Deserialize(byte[] data)
        {
            MemoryStream stream = new MemoryStream(data);
            BinaryFormatter formatterr = new BinaryFormatter();

            return formatterr.Deserialize(stream);
        }

        private void Server_FormClosed(object sender, FormClosedEventArgs e)
        {
            Close();
        }
    }
}

