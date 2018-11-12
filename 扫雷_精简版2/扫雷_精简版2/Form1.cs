using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace 扫雷_精简版
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            mineSite lc = new mineSite();
            againGameButton cw = new againGameButton();
            this.Controls.Add(lc);
            this.Size = new Size(lc.Size.Width + 100,lc.Size.Height + 80);
            this.Location = new Point(760,240);//大致居中
        }
    }
}
