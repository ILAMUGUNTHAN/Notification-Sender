using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Drawing.Drawing2D;

namespace Notifier
{
    public partial class MsgBoxForm : Form
    {

        public MsgBoxForm()
        {
            InitializeComponent();
            this.Size = new Size(200, 100);
            

            this.TransparencyKey = Color.Magenta;
            this.BackColor = Color.Magenta;
            mainForm = new Form1();
            txt += SetMsg;
            TopMost = true;
            timer = new Timer();

        }

        public EventHandler<string> txt;
        public EventHandler<MsgBoxForm> OnClickClose;

        public delegate void MsgBoxEventHandler(object sender, EventArgs e, string s);
        public MsgBoxEventHandler MsgBoxClick;

        private string msg;
        private Form1 mainForm;
        private DateTime msgTime;
        private int cornerRadius = 20;
        private Timer timer ;
        private List<string> dispMsg = new List<string>();
        private string finalMsg;
        private Font font;
        private int screenHeight = Screen.PrimaryScreen.Bounds.Height;
        public void SetTime(DateTime t)
        {
            msgTime = t;
        }
        public DateTime GetTime()
        {
            return msgTime;
        }
        public string getMsg()
        {
            return msg;
        }


        public void SetCornerRadius(int rad)
        {
            cornerRadius = rad;
        }
        private void OnPaint(object sender, PaintEventArgs e)
        {
            int msgHeight = ((int)((msg.Length / 20) * 18.75) + 50);
            this.Height = msgHeight > screenHeight ? screenHeight: msgHeight;

            Graphics g = e.Graphics;
            GraphicsPath path = new GraphicsPath();
            font = new Font("Arial", 11.32f);
            SizeF msgSize = g.MeasureString(msg,font);
            //string[] dispMsg = new string[4];
            
            

            GenerateDispMsg();


            


            DoubleBuffered = true;


            int width = this.Width;
            int height = this.Height;
            

            //Rectangle left = new Rectangle(0, 0, this.Height, this.Height);
            //Rectangle right = new Rectangle(this.Width - this.Height, 0, this.Height, this.Height);

            //gPath.AddArc(right, 270, 180);
            //gPath.AddArc(left, 90, 180);

            //g.FillPath(new SolidBrush(Color.White), gPath);



            path.AddArc(0, 0, cornerRadius * 2, cornerRadius * 2, 180, 90); // Top-left corner
            path.AddArc( width - 2 * cornerRadius, 0, cornerRadius * 2, cornerRadius * 2, 270, 90); // Top-right corner
            path.AddArc( width - 2 * cornerRadius, 0 + height - 2 * cornerRadius, cornerRadius * 2, cornerRadius * 2, 0, 90); // Bottom-right corner
            path.AddArc(0,  height - 2 * cornerRadius, cornerRadius * 2, cornerRadius * 2, 90, 90); // Bottom-left corner

            path.CloseFigure();

            g.FillPath(new SolidBrush(Color.Azure), path);
            g.DrawString("Heading:", new Font("Arial", 12, FontStyle.Bold | FontStyle.Underline), new SolidBrush(Color.Black), 5, 5);

            if (msgHeight > screenHeight)
            {
                if(finalMsg.Length>1200) g.DrawString(finalMsg.Substring(0,1200)+"\n. . . See More", font, new SolidBrush(Color.Black), 5, 25);
                else g.DrawString(finalMsg.Substring(0, finalMsg.Length) + "\n. . . See More", font, new SolidBrush(Color.Black), 5, 25);
            }
            else
            {
                g.DrawString(finalMsg, font, new SolidBrush(Color.Black), 5, 25);
            }
            //g.DrawString(dispMsg[1], font, new SolidBrush(Color.Black), 5, 43.75f);
            //g.DrawString(dispMsg[2], font, new SolidBrush(Color.Black), 5, 62.5f);
            //g.DrawString(dispMsg[3], font, new SolidBrush(Color.Black), 5, 81.25f);



        }

        private void GenerateDispMsg()
        {
            Graphics g = CreateGraphics();
            SizeF size = g.MeasureString(msg, font);

            string[] str = msg.Split(' ','\n');
            string s="";
            finalMsg = msg;

            if (size.Width > this.Width - 25)
            {
                finalMsg = "";
                for (int i = 0; i < str.Length; i++)
                {
                    if(str[i].Length>20)
                    {
                        str[i] = s + str[i];
                        s = "";
                        double end = Math.Ceiling(str[i].Length / 20.0);
                        
                        for (int j = 0;j< end; j++)
                        {
                            if(j==end-1)
                            {
                                finalMsg += str[i].Substring((j * 20), str[i].Length - j * 20)+"\n";
                            }
                            else
                            {
                                finalMsg += str[i].Substring(j * 20, 20)+"\n";
                            }
                        }
                    }
                    else if (g.MeasureString(s+str[i],font).Width > this.Width - 25)
                    {
                        finalMsg += s + "\n";
                        s = str[i]+" ";

                    }
                    else
                    {
                        s += str[i] + " ";
                    }


                }
                finalMsg += s;
            }
            else if(str.Length>1)
            {
                finalMsg = "";
                foreach(string st in str)
                {
                    finalMsg += st + " ";
                }
            }

        }

        private void SetMsg(object sender, string s)
        {
            msg = s;
        }

        private void OnLoad(object sender, EventArgs e)
        {
            CloseBtn.Location = new Point(this.Width  - CloseBtn.Width -5 , 5);
        }

        private void OnClickCloseBtn(object sender, EventArgs e)
        {
            //this.Close();
            //this.Dispose();
            OnClickClose?.Invoke(sender, this);
        }

        private void OnClickMsgBoxForm(object sender, MouseEventArgs e)
        {
            
            MsgBoxClick?.Invoke(sender, e,msg);
        }

        private void OnFormClosing(object sender, FormClosingEventArgs e)
        {

            //if (this.Opacity > 0.01f)
            //{
            //    e.Cancel = true;
            //    timer.Tick += TimerTick1;
            //    timer.Interval = 50;
            //    timer.Start();
            //}
            //else
            //{
            //    timer.Enabled = false;
            //}

        }
        private void TimerTick1(object sender, EventArgs e)
        {

            if (this.Opacity > 0.01)
                this.Opacity = this.Opacity - 0.1f;
            else
                this.Close();
        }
        //private void frmFadeClose_FormClosing(object sender, FormClosingEventArgs e)
        //{
        //    if (this.Opacity > 0.01f)
        //    {
        //        e.Cancel = true;
        //        fadeOutTimer.Tick += Timer1_Tick;
        //        fadeOutTimer.Interval = 50;
        //        fadeOutTimer.Start();
        //    }
        //    else
        //    {
        //        fadeOutTimer.Enabled = false;
        //    }
        //}

        //private void Timer1_Tick(object sender, EventArgs e)
        //{
        //    if (this.Opacity > 0.01)
        //        this.Opacity = this.Opacity - 0.1f;
        //    else
        //        this.Close();
        //}
    }
}
