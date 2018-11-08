﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Drawing;
using System.Threading;

namespace 扫雷_精简版
{
    public enum 方格状态 { 未知,打开,标记,爆炸,疑问 }
    class 方格 : Button
    {
        public static int 宽 = 36;
        public static int 高 = 36;
        int _横坐标;
        public int 横坐标
        {
            get { return _横坐标; }
            set { _横坐标 = value; }
        }
        int _纵坐标;
        public int 纵坐标
        {
            get { return _纵坐标; }
            set { _纵坐标 = value; }
        }
        Boolean _是不是地雷 = false;

        public Boolean 是不是地雷
        {
            get { return _是不是地雷; }
            set { _是不是地雷 = value; }
        }
        int _周围雷数量 = 0;

        public int 周围雷数量
        {
            get { return _周围雷数量; }
            set { _周围雷数量 = value; }
        }
        方格状态 _外观;

        public 方格状态 外观
        {
            get { return _外观; }
            set 
            { 
                _外观 = value;
                switch (value)
                {
                    case 方格状态.打开:
                        this.BackgroundImage = Image.FromFile("Images\\扫雷_"+周围雷数量+".jpg");
                        雷场.打开数量++;
                        扩散方法(横坐标,纵坐标);
                        break;
                    case 方格状态.未知:
                        this.BackgroundImage = Image.FromFile("Images\\未知.jpg");
                        break;
                    case 方格状态.标记:
                        this.BackgroundImage = Image.FromFile("Images\\标记.jpg");
                        break;
                    case 方格状态.爆炸:
                        this.BackgroundImage = Image.FromFile("Images\\被动爆炸.jpg");
                        break;
                    case 方格状态.疑问:
                        this.BackgroundImage = Image.FromFile("Images\\问号.jpg");
                        break;
                    default:

                        break;
                }
            }
        }
        void 初始化()
        {
            this.Size = new Size(宽, 高);
            外观 = 方格状态.未知;
        }
        public 方格()
        {
            初始化();
        }
        /// <summary>
        /// 周围没有雷时，扩散
        /// </summary>
        /// <param name="横坐标"></param>
        /// <param name="纵坐标"></param>
        void 扩散方法(int 横坐标,int 纵坐标)
        {
            if (周围雷数量 == 0)
            {
                foreach (Object control in this.Parent.Controls)
                {
                    if (control is 方格)
                    {
                        方格 _方格 = (方格)control;
                        if (_方格.外观 == 方格状态.未知 || _方格.外观 == 方格状态.疑问)
                        {
                            try
                            {
                                if (_方格.横坐标 + 1 == this.横坐标 && _方格.纵坐标 + 1 == this.纵坐标)
                                {
                                    _方格.外观 = 方格状态.打开;
                                }
                            }
                            catch { }
                            try
                            {
                                if (_方格.横坐标 == this.横坐标 && _方格.纵坐标 + 1 == this.纵坐标)
                                {
                                    _方格.外观 = 方格状态.打开;
                                }
                            }
                            catch { }
                            try
                            {
                                if (_方格.横坐标 - 1 == this.横坐标 && _方格.纵坐标 + 1 == this.纵坐标)
                                {
                                    _方格.外观 = 方格状态.打开;
                                }
                            }
                            catch { }
                            try
                            {
                                if (_方格.横坐标 + 1 == this.横坐标 && _方格.纵坐标 == this.纵坐标)
                                {
                                    _方格.外观 = 方格状态.打开;
                                }
                            }
                            catch { }
                            try
                            {
                                if (_方格.横坐标 - 1 == this.横坐标 && _方格.纵坐标 == this.纵坐标)
                                {
                                    _方格.外观 = 方格状态.打开;
                                }
                            }
                            catch { }
                            try
                            {
                                if (_方格.横坐标 + 1 == this.横坐标 && _方格.纵坐标 - 1 == this.纵坐标)
                                {
                                    _方格.外观 = 方格状态.打开;
                                }
                            }
                            catch { }
                            try
                            {
                                if (_方格.横坐标 == this.横坐标 && _方格.纵坐标 - 1 == this.纵坐标)
                                {
                                    _方格.外观 = 方格状态.打开;
                                }
                            }
                            catch { }
                            try
                            {
                                if (_方格.横坐标 - 1 == this.横坐标 && _方格.纵坐标 - 1 == this.纵坐标)
                                {
                                    _方格.外观 = 方格状态.打开;
                                }
                            }
                            catch { }
                        }
                    }
                }
            }
        }
    }

    class 雷场 : Panel
    {
        public static Boolean 游戏能不能继续 = true;
        static int 方格列数 = 10;
        static int 方格行数 = 10;
        int 方格间隔 = 1;
        public static int 标记数量 = 15;
        int 地雷数量 = 15;
        public static int 打开数量 = 0;   //判断是否win
        方格[,] 方格坐标集合 = new 方格[方格列数,方格行数];

        public 雷场()
        {
            初始化();
        }

        void 初始化()
        {
            this.Size = new Size((方格.宽 + 方格间隔) * 方格列数 - 方格间隔, (方格.宽 + 方格间隔) * 方格列数 - 方格间隔 + 140);//加上下面重玩按钮和显示信息的140高度
            this.Location = new Point(40,40);//雷场位置
            生成方格();
            随机布雷();
            判断周围雷数量();
            重玩按钮();
            显示剩余地雷数量();
            显示地雷图片();
        }

        /// <summary>
        /// 根据随机生成坐标布雷
        /// </summary>
        void 随机布雷()
        {
            int 随机x;
            int 随机y;
            Random rd = new Random();
            for (int i = 0; i < 地雷数量; i++)
            {
                do
                {
                    随机x = rd.Next(0, 10);
                    随机y = rd.Next(0, 10);
                } while (方格坐标集合[随机x, 随机y].是不是地雷 == true);
                方格坐标集合[随机x, 随机y].是不是地雷 = true;
            }
        }

        /// <summary>
        /// try catch 判断周围类数量
        /// </summary>
        void 判断周围雷数量()
        {
            for (int i = 0; i < 方格列数; i++)
            {
                for (int j = 0; j < 方格行数; j++)
                {
                    if (方格坐标集合[i, j].是不是地雷 != true)
                    {
                        try
                        {
                            if (方格坐标集合[i - 1, j - 1].是不是地雷 == true)
                            {
                                方格坐标集合[i, j].周围雷数量++;
                            }
                        }
                        catch { }
                        try
                        {
                            if (方格坐标集合[i, j - 1].是不是地雷 == true)
                            {
                                方格坐标集合[i, j].周围雷数量++;
                            }
                        }
                        catch { }
                        try
                        {
                            if (方格坐标集合[i + 1, j - 1].是不是地雷 == true)
                            {
                                方格坐标集合[i, j].周围雷数量++;
                            }
                        }
                        catch { }
                        try
                        {
                            if (方格坐标集合[i - 1, j].是不是地雷 == true)
                            {
                                方格坐标集合[i, j].周围雷数量++;
                            }
                        }
                        catch { }
                        try
                        {
                            if (方格坐标集合[i + 1, j].是不是地雷 == true)
                            {
                                方格坐标集合[i, j].周围雷数量++;
                            }
                        }
                        catch { }
                        try
                        {
                            if (方格坐标集合[i - 1, j + 1].是不是地雷 == true)
                            {
                                方格坐标集合[i, j].周围雷数量++;
                            }
                        }
                        catch { }
                        try
                        {
                            if (方格坐标集合[i, j + 1].是不是地雷 == true)
                            {
                                方格坐标集合[i, j].周围雷数量++;
                            }
                        }
                        catch { }
                        try
                        {
                            if (方格坐标集合[i + 1, j + 1].是不是地雷 == true)
                            {
                                方格坐标集合[i, j].周围雷数量++;
                            }
                        }
                        catch { }
                    }
                }
            }
        }
        void 生成方格()
        {
            for (int i = 0; i < 方格列数; i++)
            {
                for (int j = 0; j < 方格行数; j++)
                {
                    方格 tld = new 方格();
                    tld.Location = new Point((方格.宽 + 方格间隔) * i, (方格.高 + 方格间隔) * j);
                    tld.MouseDown += 点击事件;
                    tld.横坐标 = i;
                    tld.纵坐标 = j;
                    方格坐标集合[i, j] = tld;
                    this.Controls.Add(tld);
                }
            }
        }
        void 点击事件(object sender,MouseEventArgs e)
        {
            方格 fg = (方格)sender;
            switch (e.Button)
            {
                case MouseButtons.Left:
                    if (游戏能不能继续 == true)
                    {
                        if (fg.外观 == 方格状态.未知 || fg.外观 == 方格状态.疑问)
                        {
                            fg.外观 = 方格状态.打开;
                            爆炸方法(fg);
                            判断赢();
                        }
                    }
                    break;
                case MouseButtons.Right:
                    if (游戏能不能继续 == true)
                    {
                        if (标记数量 > 0)
                        {
                            if (fg.外观 == 方格状态.未知 || fg.外观 == 方格状态.疑问)
                            {
                                fg.外观 = 方格状态.标记;
                                标记数量--;
                                更改剩余地雷数量();
                            }
                            else if (fg.外观 == 方格状态.标记)
                            {
                                fg.外观 = 方格状态.疑问;
                                标记数量++;
                                更改剩余地雷数量();
                            }
                            
                        }
                        else
                        {
                            if (fg.外观 == 方格状态.标记)
                            {
                                fg.外观 = 方格状态.疑问;
                                标记数量++;
                                更改剩余地雷数量();
                            }
                        }
                    }
                    break;
            }
        }
        void 爆炸方法(方格 bomb)
        {
            if (bomb.是不是地雷 == true)
            {
                try
                {
                    foreach (方格 control in this.Controls)
                    {
                        if (control.是不是地雷 == true)
                        {
                            control.外观 = 方格状态.爆炸;
                        }
                    }
                }
                catch (Exception) { ;}
                游戏能不能继续 = false;
                bomb.BackgroundImage = Image.FromFile("Images\\主动爆炸.jpg");
                打开数量 = -1;  //最后一个打开是雷时，不判断赢
                MessageBox.Show("GameOver!!!游戏结束");
            }
        }
        /// <summary>
        /// 根据打开数量判断是否胜利
        /// </summary>
        void 判断赢()
        {
            if (打开数量 == (方格列数 * 方格行数) - 地雷数量)
            {
                游戏能不能继续 = false;
                MessageBox.Show("You Win!!!游戏结束");
            }
        }
        void 重新这一局()
        {
            初始化变量();
            foreach (Object control in this.Controls)
            {
                if (control is 方格)
                {
                    方格 fg = (方格)control;
                    fg.外观 = 方格状态.未知;
                }
            }
            更改剩余地雷数量();
        }
        void 新开下一局()
        {
            初始化变量();
            foreach (Object control in this.Controls)
            {
                if (control is 方格)
                {
                    方格 fg = (方格)control;
                    fg.外观 = 方格状态.未知;
                    fg.是不是地雷 = false;
                    fg.周围雷数量 = 0;
                }
            }
            随机布雷();
            判断周围雷数量();
            更改剩余地雷数量();
        }
        void 初始化变量()
        {
            游戏能不能继续 = true;
            标记数量 = 15;
            打开数量 = 0;
        }
        void 重玩按钮()
        {
            重玩按钮 cw = new 重玩按钮();
            cw.MouseDown += 重玩点击事件;
            this.Controls.Add(cw);
        }
        void 重玩点击事件(object sender, MouseEventArgs e)
        {
            switch (e.Button)
            {
                case MouseButtons.Left:
                    重新这一局();
                    break;
                case MouseButtons.Right:
                    新开下一局();
                    break;
                default:
                    break;
            }
        }
        void 显示剩余地雷数量()
        {
            显示信息 xs = new 显示信息();
            xs.Text = 标记数量.ToString();
            xs.Location = new Point(85, 467);
            this.Controls.Add(xs);
        }
        void 更改剩余地雷数量()
        {
            foreach (Object control in this.Controls)
            {
                if (control is 显示信息)
                {
                    显示信息 sy = (显示信息)control;
                    if (标记数量 < 10)
                    {
                        sy.Text = "0" + 标记数量;
                    }
                    else
                    {
                        sy.Text = 标记数量.ToString();
                    }
                }
            }
        }
        void 显示地雷图片()
        {
            地雷图片 dlImg = new 地雷图片();
            this.Controls.Add(dlImg);
        }
    }

    class 重玩按钮 : Button
    {
        public 重玩按钮()
        {
            初始化外观();
        }
        void 初始化外观()
        {
            this.Size = new Size(170,40);
            this.BackColor = Color.Red;
            this.Font = new Font("黑体", 12);
            this.Text = "左键重新这一局，右键下一局";
            this.Location = new Point(100,400);
        }
    }
    class 显示信息 : Label
    {
        public 显示信息()
        {
            初始化();
        }
        void 初始化()
        {
            this.Size = new Size(40,25);
            this.Font = new Font("黑体", 20);
            this.BackColor = Color.Red;
        }
    }
    class 地雷图片 : PictureBox
    {
        public 地雷图片()
        {
            初始化();
        }
        void 初始化()
        {
            this.Image = 扫雷_精简版.Properties.Resources.地雷图标;
            this.Size = new Size(54,54);
            this.Location = new Point(20,450);
        }
    }
}
