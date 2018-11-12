using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Drawing;
using System.Threading;

namespace 扫雷_精简版
{
    public enum stateGrid { unknown,open,mark,bomb,query }  //未知、打开、标记、爆炸、疑问状态
    class grid : Button
    {
        public static int width = 36;
        public static int height = 36;
        int _x_Coordinate;  //横坐标
        public int x_Coordinate
        {
            get { return _x_Coordinate; }
            set { _x_Coordinate = value; }
        }
        int _y_Coordinate;  //纵坐标
        public int y_Coordinate
        {
            get { return _y_Coordinate; }
            set { _y_Coordinate = value; }
        }
        Boolean _isMine = false;

        public Boolean isMine
        {
            get { return _isMine; }
            set { _isMine = value; }
        }
        int _roundMineNum = 0;  //周围地雷数量

        public int roundMineNum
        {
            get { return _roundMineNum; }
            set { _roundMineNum = value; }
        }
        stateGrid _facade;  //外观状态

        public stateGrid fasade
        {
            get { return _facade; }
            set 
            { 
                _facade = value;
                switch (value)
                {
                    case stateGrid.open:
                        this.BackgroundImage = Image.FromFile("Images\\扫雷_"+roundMineNum+".jpg");
                        mineSite.openNum++;
                        proliferateMethod();
                        break;
                    case stateGrid.unknown:
                        this.BackgroundImage = Image.FromFile("Images\\未知.jpg");
                        break;
                    case stateGrid.mark:
                        this.BackgroundImage = Image.FromFile("Images\\标记.jpg");
                        break;
                    case stateGrid.bomb:
                        this.BackgroundImage = Image.FromFile("Images\\被动爆炸.jpg");
                        break;
                    case stateGrid.query:
                        this.BackgroundImage = Image.FromFile("Images\\问号.jpg");
                        break;
                    default:

                        break;
                }
            }
        }
        void initialize()
        {
            this.Size = new Size(width, height);
            fasade = stateGrid.unknown;
        }
        public grid()
        {
            initialize();
        }
        /// <summary>
        /// 周围没有雷时，扩散
        /// </summary>
        void proliferateMethod()
        {
            if (roundMineNum == 0)
            {
                foreach (Object control in this.Parent.Controls)
                {
                    if (control is grid)
                    {
                        grid _grid = (grid)control;
                        if (_grid.fasade == stateGrid.unknown || _grid.fasade == stateGrid.query)
                        {
                            try
                            {
                                if (_grid.x_Coordinate + 1 == this.x_Coordinate && _grid.y_Coordinate + 1 == this.y_Coordinate)
                                {
                                    _grid.fasade = stateGrid.open;
                                }
                            }
                            catch { }
                            try
                            {
                                if (_grid.x_Coordinate == this.x_Coordinate && _grid.y_Coordinate + 1 == this.y_Coordinate)
                                {
                                    _grid.fasade = stateGrid.open;
                                }
                            }
                            catch { }
                            try
                            {
                                if (_grid.x_Coordinate - 1 == this.x_Coordinate && _grid.y_Coordinate + 1 == this.y_Coordinate)
                                {
                                    _grid.fasade = stateGrid.open;
                                }
                            }
                            catch { }
                            try
                            {
                                if (_grid.x_Coordinate + 1 == this.x_Coordinate && _grid.y_Coordinate == this.y_Coordinate)
                                {
                                    _grid.fasade = stateGrid.open;
                                }
                            }
                            catch { }
                            try
                            {
                                if (_grid.x_Coordinate - 1 == this.x_Coordinate && _grid.y_Coordinate == this.y_Coordinate)
                                {
                                    _grid.fasade = stateGrid.open;
                                }
                            }
                            catch { }
                            try
                            {
                                if (_grid.x_Coordinate + 1 == this.x_Coordinate && _grid.y_Coordinate - 1 == this.y_Coordinate)
                                {
                                    _grid.fasade = stateGrid.open;
                                }
                            }
                            catch { }
                            try
                            {
                                if (_grid.x_Coordinate == this.x_Coordinate && _grid.y_Coordinate - 1 == this.y_Coordinate)
                                {
                                    _grid.fasade = stateGrid.open;
                                }
                            }
                            catch { }
                            try
                            {
                                if (_grid.x_Coordinate - 1 == this.x_Coordinate && _grid.y_Coordinate - 1 == this.y_Coordinate)
                                {
                                    _grid.fasade = stateGrid.open;
                                }
                            }
                            catch { }
                        }
                    }
                }
            }
        }
    }

    class mineSite : Panel
    {
        public static Boolean isGameContinue = true;    //游戏能不能继续
        static int gridListNum = 10;    //方格行数量
        static int gridRowNum = 10; //方格列数量
        int gridInterval = 1;   //方格间隔
        public static int markNum = 15; //标记数量
        int mineNum = 15;
        public static int openNum = 0;   //打开数量,判断是否win
        grid[,] gridCoordinateGather = new grid[gridListNum,gridRowNum];    //方格坐标集合

        public mineSite()
        {
            initialize();
        }

        void initialize()
        {
            this.Size = new Size((grid.width + gridInterval) * gridListNum - gridInterval, (grid.width + gridInterval) * gridListNum - gridInterval + 140);//加上下面重玩按钮和显示信息的140高度
            this.Location = new Point(40,40);//雷场位置
            createGrid();
            randomInstallMine();
            judgeRoundMineNum();
            againGameButton();
            showSurplusMineNum();
            showMineImage();
        }

        /// <summary>
        /// 根据随机生成坐标布雷
        /// </summary>
        void randomInstallMine()
        {
            int x;
            int y;
            Random rd = new Random();
            for (int i = 0; i < mineNum; i++)
            {
                do
                {
                    x = rd.Next(0, 10);
                    y = rd.Next(0, 10);
                } while (gridCoordinateGather[x, y].isMine == true);
                gridCoordinateGather[x, y].isMine = true;
            }
        }

        /// <summary>
        /// try catch 判断周围地雷数量
        /// </summary>
        void judgeRoundMineNum()
        {
            for (int i = 0; i < gridListNum; i++)
            {
                for (int j = 0; j < gridRowNum; j++)
                {
                    if (gridCoordinateGather[i, j].isMine != true)
                    {
                        try
                        {
                            if (gridCoordinateGather[i - 1, j - 1].isMine == true)
                            {
                                gridCoordinateGather[i, j].roundMineNum++;
                            }
                        }
                        catch { }
                        try
                        {
                            if (gridCoordinateGather[i, j - 1].isMine == true)
                            {
                                gridCoordinateGather[i, j].roundMineNum++;
                            }
                        }
                        catch { }
                        try
                        {
                            if (gridCoordinateGather[i + 1, j - 1].isMine == true)
                            {
                                gridCoordinateGather[i, j].roundMineNum++;
                            }
                        }
                        catch { }
                        try
                        {
                            if (gridCoordinateGather[i - 1, j].isMine == true)
                            {
                                gridCoordinateGather[i, j].roundMineNum++;
                            }
                        }
                        catch { }
                        try
                        {
                            if (gridCoordinateGather[i + 1, j].isMine == true)
                            {
                                gridCoordinateGather[i, j].roundMineNum++;
                            }
                        }
                        catch { }
                        try
                        {
                            if (gridCoordinateGather[i - 1, j + 1].isMine == true)
                            {
                                gridCoordinateGather[i, j].roundMineNum++;
                            }
                        }
                        catch { }
                        try
                        {
                            if (gridCoordinateGather[i, j + 1].isMine == true)
                            {
                                gridCoordinateGather[i, j].roundMineNum++;
                            }
                        }
                        catch { }
                        try
                        {
                            if (gridCoordinateGather[i + 1, j + 1].isMine == true)
                            {
                                gridCoordinateGather[i, j].roundMineNum++;
                            }
                        }
                        catch { }
                    }
                }
            }
        }
        void createGrid()
        {
            for (int i = 0; i < gridListNum; i++)
            {
                for (int j = 0; j < gridRowNum; j++)
                {
                    grid createGrid = new grid();
                    createGrid.Location = new Point((grid.width + gridInterval) * i, (grid.height + gridInterval) * j);
                    createGrid.MouseDown += gridClickInsident;
                    createGrid.x_Coordinate = i;
                    createGrid.y_Coordinate = j;
                    gridCoordinateGather[i, j] = createGrid;
                    this.Controls.Add(createGrid);
                }
            }
        }
        /// <summary>
        /// 方格点击事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void gridClickInsident(object sender,MouseEventArgs e)
        {
            grid fg = (grid)sender;
            switch (e.Button)
            {
                case MouseButtons.Left:
                    if (isGameContinue == true)
                    {
                        if (fg.fasade == stateGrid.unknown || fg.fasade == stateGrid.query)
                        {
                            fg.fasade = stateGrid.open;
                            bombMethod(fg);
                            judgeWin();
                        }
                    }
                    break;
                case MouseButtons.Right:
                    if (isGameContinue == true)
                    {
                        if (markNum > 0)
                        {
                            if (fg.fasade == stateGrid.unknown || fg.fasade == stateGrid.query)
                            {
                                fg.fasade = stateGrid.mark;
                                markNum--;
                                changeSurplusMineNum();
                            }
                            else if (fg.fasade == stateGrid.mark)
                            {
                                fg.fasade = stateGrid.query;
                                markNum++;
                                changeSurplusMineNum();
                            }
                            
                        }
                        else
                        {
                            if (fg.fasade == stateGrid.mark)
                            {
                                fg.fasade = stateGrid.query;
                                markNum++;
                                changeSurplusMineNum();
                            }
                        }
                    }
                    break;
            }
        }
        /// <summary>
        /// 爆炸方法
        /// </summary>
        /// <param name="bomb"></param>
        void bombMethod(grid bomb)
        {
            if (bomb.isMine == true)
            {
                try
                {
                    foreach (grid control in this.Controls)
                    {
                        if (control.isMine == true)
                        {
                            control.fasade = stateGrid.bomb;
                        }
                    }
                }
                catch (Exception) { ;}
                isGameContinue = false;
                bomb.BackgroundImage = Image.FromFile("Images\\主动爆炸.jpg");
                openNum = -1;  //最后一个打开是雷时，不判断赢
                MessageBox.Show("GameOver!!!游戏结束");
            }
        }
        /// <summary>
        /// 根据打开数量判断是否胜利
        /// </summary>
        void judgeWin()
        {
            if (openNum == (gridListNum * gridRowNum) - mineNum)
            {
                isGameContinue = false;
                MessageBox.Show("You Win!!!游戏结束");
            }
        }   
        /// <summary>
        /// 重新这一局
        /// </summary>
        void againTheGame()
        {
            initializeVariable();
            foreach (Object control in this.Controls)
            {
                if (control is grid)
                {
                    grid fg = (grid)control;
                    fg.fasade = stateGrid.unknown;
                }
            }
            changeSurplusMineNum();
        }
        /// <summary>
        /// 重新下一局
        /// </summary>
        void againNextGame()
        {
            initializeVariable();
            foreach (Object control in this.Controls)
            {
                if (control is grid)
                {
                    grid fg = (grid)control;
                    fg.fasade = stateGrid.unknown;
                    fg.isMine = false;
                    fg.roundMineNum = 0;
                }
            }
            randomInstallMine();
            judgeRoundMineNum();
            changeSurplusMineNum();
        }
        /// <summary>
        /// 初始化变量
        /// </summary>
        void initializeVariable()
        {
            isGameContinue = true;
            markNum = 15;
            openNum = 0;
        }
        /// <summary>
        /// 实例重玩按钮
        /// </summary>
        void againGameButton()
        {
            againGameButton cw = new againGameButton();
            cw.MouseDown += againGameClickIncident;
            this.Controls.Add(cw);
        }
        /// <summary>
        /// 重玩点击事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void againGameClickIncident(object sender, MouseEventArgs e)
        {
            switch (e.Button)
            {
                case MouseButtons.Left:
                    againTheGame();
                    break;
                case MouseButtons.Right:
                    againNextGame();
                    break;
                default:
                    break;
            }
        }
        /// <summary>
        /// 显示剩余地雷数量
        /// </summary>
        void showSurplusMineNum()
        {
            showInformation xs = new showInformation();
            xs.Text = markNum.ToString();
            xs.Location = new Point(85, 467);
            this.Controls.Add(xs);
        }
        /// <summary>
        /// 更改剩余地雷数量
        /// </summary>
        void changeSurplusMineNum()
        {
            foreach (Object control in this.Controls)
            {
                if (control is showInformation)
                {
                    showInformation sy = (showInformation)control;
                    if (markNum < 10)
                    {
                        sy.Text = "0" + markNum;
                    }
                    else
                    {
                        sy.Text = markNum.ToString();
                    }
                }
            }
        }
        /// <summary>
        /// 显示地雷图片
        /// </summary>
        void showMineImage()
        {
            mineImage dlImg = new mineImage();
            this.Controls.Add(dlImg);
        }
    }

    class againGameButton : Button
    {
        public againGameButton()
        {
            initialize();
        }
        void initialize()
        {
            this.Size = new Size(170,40);
            this.BackColor = Color.Red;
            this.Font = new Font("黑体", 12);
            this.Text = "左键重新这一局，右键下一局";
            this.Location = new Point(100,400);
        }
    }
    class showInformation : Label
    {
        public showInformation()
        {
            initialize();
        }
        void initialize()
        {
            this.Size = new Size(40,25);
            this.Font = new Font("黑体", 20);
            this.BackColor = Color.Red;
        }
    }
    class mineImage : PictureBox
    {
        public mineImage()
        {
            initialize();
        }
        void initialize()
        {
            this.Image = 扫雷_精简版.Properties.Resources.地雷图标;
            this.Size = new Size(54,54);
            this.Location = new Point(20,450);
        }
    }
}
