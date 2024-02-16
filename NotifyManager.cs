using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Notifier
{

    
    public enum Sides
    {
        Left,
        Right
    }


    public class NotifyManager:Component
    {
        public NotifyManager()
        {
            timer = new Timer();
            timer.Interval = 1000;
            timer.Tick += TimerTick;
            timer.Start();
            MessageBtnClick += OnClickMessageBtn;
        }

        public delegate void MsgBtnEventHandler(object sender,MsgBoxForm m, string s);
        public MsgBtnEventHandler MessageBtnClick;

        public List<MsgBoxForm> NotificationList = new List<MsgBoxForm>();
        public List<MsgBoxForm> QueuedMsgList = new List<MsgBoxForm>();
        
        private Timer timer;
        private int msgHeight = Screen.PrimaryScreen.Bounds.Height;
        private int msgWidth = Screen.PrimaryScreen.Bounds.Width;
        private int msgHeiDiff = 30;
        private int msgBxHeight;
        private Form popUpForm;
        private bool queued = false;

        public Sides Side
        {
            get;
            set;
        }
        public int NotificationTime
        {
            get;
            set;
        }
        public int CornerRadius
        {
            get;
            set;
        }


        private void OnClickMessageBtn(object sender, MsgBoxForm msgForm,string message)
        {
            msgForm.txt?.Invoke(sender, message);
            msgForm.MsgBoxClick += OnClickMsgBox;
            msgForm.OnClickClose += OnClickCloseBtn;

            //if (msgHeiDiff > msgHeight)
            //{
            //    MessageBox.Show("Limit Reached wait!!!");
            //    // QueuedMsgList.Add(msgForm);
            //    toSet = false;
            //    return;
            //}
            msgBxHeight = (int)((message.Length / 20) * 18.75) + 60;
            msgHeiDiff += msgBxHeight;
            msgForm.SetCornerRadius(CornerRadius);
            
            



            if (msgHeight - msgHeiDiff < 0)
            {
                if (NotificationList.Count == 0)
                {
                    msgForm.Location = (Side == Sides.Left) ? new Point(0, 5) : new Point(msgWidth - msgForm.Width, 5);
                    msgForm.SetTime(DateTime.Now);
                    NotificationList.Add(msgForm);
                    msgForm.Show();
                }
                else
                {
                    queued = true;
                    QueuedMsgList.Add(msgForm);
                }
            }
            else 
            {
                msgForm.Location = (Side == Sides.Left)? new Point(0, msgHeight - msgHeiDiff) : new Point(msgWidth - msgForm.Width, msgHeight - msgHeiDiff);
                //msgForm.OnClickClose += OnClickCloseBtn;
                msgForm.SetTime(DateTime.Now);
                NotificationList.Add(msgForm);
                msgForm.Show();
            }
        }


        private void TimerTick(object sender, EventArgs e)
        {

            for(int ind=0;ind<NotificationList.Count;ind++)
            {

                MsgBoxForm msgBx = NotificationList[ind];
                TimeSpan difference = DateTime.Now - msgBx.GetTime();
                if (difference.TotalSeconds >= NotificationTime)
                {
                    NotificationList.Remove(msgBx);

                    if (QueuedMsgList.Count > 0)
                    {
                        QueuedMsgList[0].SetTime(DateTime.Now);

                        NotificationList.Add(QueuedMsgList[0]);
                        NotificationList.Last().Show();
                        QueuedMsgList.RemoveAt(0);
                    }

                    msgBx.Close();
                    //msgBx.Dispose();
                    
                    msgHeiDiff = 30;
                    ReOrder();
                    
                }
            }

        }

        private void ReOrder()
        {
            for (int ind = 0;ind<NotificationList.Count;ind++)
            {
                MsgBoxForm msgBx = NotificationList[ind];
                msgHeiDiff += ((int)((msgBx.getMsg().Length / 20) * 18.75) + 60) +10;
                if (Side == Sides.Left)
                {
                    if (msgBx.Location.Y <= 0 && (int)((msgBx.getMsg().Length / 20) * 18.75) + 60 > 1000)
                    {
                        msgBx.Location = new Point(0, 5);
                    }
                    else
                    {
                        msgBx.Location = new Point(0, msgHeight - msgHeiDiff);
                    }
                }
                else
                {
                    if (msgBx.Location.Y <= 0 && (int)((msgBx.getMsg().Length / 20) * 18.75) + 60 > 1000)
                    {
                        msgBx.Location = new Point(msgWidth - msgBx.Width, 5);
                    }
                    else
                    {
                        msgBx.Location = new Point(msgWidth - msgBx.Width, msgHeight - msgHeiDiff);
                    }
                    
                }
            }
           
        }

        private void OnClickCloseBtn(object sender, MsgBoxForm e)
        {
            msgHeiDiff = 30;
            NotificationList.Remove(e);
            e.Close();
            int lastLoc=1000;
            while (QueuedMsgList.Count > 0)
            {
                int qMsgHeight = msgBxHeight;
                if (!queued) qMsgHeight = (int)((QueuedMsgList[0].getMsg().Length / 20) * 18.75);
                
                msgHeiDiff = 30;
                ReOrder();
                if(NotificationList.Count>0)
                {
                    lastLoc = NotificationList.Last().Location.Y;
                }
                if ((NotificationList.Count>0 || QueuedMsgList.Count>0) &&  qMsgHeight < lastLoc)
                {
                    QueuedMsgList[0].SetTime(DateTime.Now);
                    NotificationList.Add(QueuedMsgList[0]);
                    
                    

                    NotificationList.Last().Show();
                    QueuedMsgList.RemoveAt(0);
                    
                }
                else
                {
                    break;
                }
            }

            
            
            //e.Dispose();
            msgHeiDiff = 30;
            ReOrder();
        }

        private void OnClickMsgBox(object sender, EventArgs e, string msg)
        {
            if (msgBxHeight < msgHeight)
            {
                return;

            }

            popUpForm = new Form();

            popUpForm.Size = new Size(600, 400);
            popUpForm.Location = new Point(600,600);

            RichTextBox txtBx = new RichTextBox();

            txtBx.Text = msg;

            txtBx.Dock = DockStyle.Fill;

            popUpForm.Controls.Add(txtBx);

            popUpForm.Show();


        }



    }
}
