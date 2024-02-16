using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Notifier
{
  
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();

        }



        private MsgBoxForm msgForm;
        //private int msgHeight = Screen.PrimaryScreen.Bounds.Height;
        //private int msgWidth = Screen.PrimaryScreen.Bounds.Width;
        //private int msgHeiDiff = 150;
        



        private void OnClickMessageBtn(object sender, EventArgs e)
        {

            msgForm = new MsgBoxForm();
            notifyManager1.MessageBtnClick?.Invoke(sender, msgForm,richTextBox1.Text);

            


        }

        private void OnLoadForm1(object sender, EventArgs e)
        {
            
        }

        

    }
}
