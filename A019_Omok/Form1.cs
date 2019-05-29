using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace A019_Omok
{
    enum STONE {  none, black, white};      //enum은 열거형 ({이렇게이렇게} 열거되어있다.) 새로운 stone타입 (stone은 n,b,w 중 하나 값)
    
    public partial class Form1 : Form // partial: form1 클래스가 나뉘어져있음. (디자인에도 가보면 있음)
    {
        private int mgn=30;
        private int 눈=30;
        private int 화점 = 10;
        private int 돌 = 26;
        private Graphics g;
        private Pen pen = new Pen(Color.Black);
        private Brush bBrush =new SolidBrush(Color.Black);  //그냥 그 색깔대로 딱 채우는거 (흐리게x,...등등)
        private Brush wBrush= new SolidBrush(Color.White);
        private bool flag;
        private STONE[,] 바둑판 = new STONE[19, 19];       //디폴트 0으로 초기화 (다 STONE.none이 들어가 있는 셈)
        private bool imageFlag=true;
        int stoneCnt = 1; // 수순
        Font font = new Font("맑은고딕", 10);       //수순 출력용
        List<Revive> lstRevive = new List<Revive>();  // 리스트  List <T>  T:아무 데이터타입이 와도 된다.
        private bool reviveFlag;
        private string dirName;
        private string filename;

        public Form1()
        {
            InitializeComponent();
            this.Text = "오목 by 김하은";
            panel1.BackColor = Color.Orange;
            this.ClientSize = new Size(2 * mgn + 18 * 눈, 2 * mgn + 18 * 눈+menuStrip2.Height);

            g = panel1.CreateGraphics();


        }
        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);
            DrawBoard(); //바둑판 그리기
            DrawStone();  //바둑돌 그리기

        }
        private void DrawStone()
        {
            for (int x = 0; x < 19; x++)
            {
                for (int y = 0; y < 19; y++)
                {
                    if (바둑판[x, y] == STONE.black)
                    {
                        Rectangle r = new Rectangle(mgn + x * 눈 - 돌 / 2, mgn + y * 눈 - 돌 / 2, 돌, 돌);
                        g.FillEllipse(bBrush, r);
                    }
                    else if(바둑판[x, y] == STONE.white)
                    {
                        Rectangle r = new Rectangle(mgn + x * 눈 - 돌 / 2, mgn + y * 눈 - 돌 / 2, 돌, 돌);
                        g.FillEllipse(wBrush, r);
                    }
                }
            }
        }

        //바둑판 그리기
        //Graphics 객체, Pen 객체, Brush객체
        private void DrawBoard()
        {
            //선을 19개 그리면 된다
            //g.DrawLine(pen, Point, Point)
           
            //시험문제 출제 (이런거 변형해서 5가지)
            for (int i = 0; i < 19; i++)   //for tab tab
            {
                //가로줄
                g.DrawLine(pen, mgn, mgn + i * 눈, mgn + 18 * 눈, mgn + i * 눈);
                //세로줄
                g.DrawLine(pen, mgn + i * 눈, mgn, mgn + i * 눈, mgn + 18 * 눈);
            }
            // 화점 (눈금의 3,9,15 위치에 원을 채워그린다.)
            for(int x=3;x<=15;x+=6)
                for(int y = 3; y <=15; y += 6)
                {
                    //원은 사각형으로 그리는데 사각형은 시작점과 폭, 높이로 지정
                    g.FillEllipse(bBrush, mgn + x * 눈-화점/2, mgn + y * 눈-화점/2, 화점, 화점);
                }
        }

        private void panel1_MouseDown(object sender, MouseEventArgs e)
        {
            if (reviveFlag == true)
            {
                ReviveGame();
                return;
            }

            //마우스 좌표를 자료구조 좌표로 변환
            int x = (e.X-mgn + 눈 / 2) / 눈;
            int y = (e.Y - mgn + 눈 / 2) / 눈;
            //MessageBox.Show(x+" "+y); //확인

            if (바둑판[x, y] != STONE.none)
                return;

            Rectangle r = new Rectangle(mgn+x*눈-돌/2, mgn + y * 눈-돌/2, 돌, 돌);
            g.FillEllipse(bBrush, r);  //g.FillEllipse(mgn+x*눈-돌/2, mgn + y * 눈-돌/2,돌, 돌 ); 이렇게 해도 ok
            if (flag == false)
            {
                if(imageFlag==false)
                    g.FillEllipse(bBrush, r);
                else
                {
                    Bitmap bmp = new Bitmap("../../Images/black.png");  //Bitmap은 0과1로 이루어진 지도
                    g.DrawImage(bmp, r);
                }
                lstRevive.Add(new Revive(x, y, STONE.black, stoneCnt));
                DrawStoneSequence(stoneCnt++, Brushes.White, r); // 추가
                바둑판[x, y] = STONE.black;
                flag = true;
            }
            else 
            {
                if (imageFlag == false)
                    g.FillEllipse(wBrush, r);
                else
                {
                    Bitmap bmp = new Bitmap("../../Images/white.png");  //Bitmap은 0과1로 이루어진 지도
                    g.DrawImage(bmp, r);
                }
                lstRevive.Add(new Revive(x, y, STONE.white, stoneCnt));
                DrawStoneSequence(stoneCnt++, Brushes.Black, r);
                바둑판[x, y] = STONE.white;
                flag = false;
            }
            CheckOmok(x, y);
        }

        private void ReviveGame()
        {
           // if (stoneCnt < lstRevive.Count)
                //DrawAStone(lstRevive[stoneCnt++]);
            foreach (var item in lstRevive)
            {
                DrawStone(item);
            }
        }

        private void DrawStone(Revive item)
        {
            int x = item.X;
            int y = item.Y;
            STONE s = item.Stone;
            int seq = item.Seq;

            Rectangle r = new Rectangle(mgn + 눈 * x - 돌 / 2, mgn + 눈 * y - 돌 / 2, 돌, 돌);

            if (s == STONE.black)
            {
                if (imageFlag == false)
                    g.FillEllipse(bBrush, r);
                else
                {
                    Bitmap bmp = new Bitmap("../../Images/black.png");
                    g.DrawImage(bmp, r);
                }
                DrawStoneSequence(seq, Brushes.White, r);
                바둑판[x, y] = STONE.black;
            }
            else
            {
                if (imageFlag == false)
                    g.FillEllipse(wBrush, r);
                else
                {
                    Bitmap bmp = new Bitmap("../../Images/white.png");
                    g.DrawImage(bmp, r);
                }
                DrawStoneSequence(seq, Brushes.Black, r);
                바둑판[x, y] = STONE.white;
            }
            CheckOmok(x, y);
        }

        //수순표시 메서드
        private void DrawStoneSequence(int v, Brush color, Rectangle r)
        {
            StringFormat stringFormat = new StringFormat();
            stringFormat.Alignment = StringAlignment.Center;
            stringFormat.LineAlignment = StringAlignment.Center;
            g.DrawString(v.ToString(), font, color, r, stringFormat);
        }

        private void CheckOmok(int x, int y)
        {
            int cnt = 1;
            //우방향
            for (int i = x + 1; i < 19; i++)
            {
                if (바둑판[x, y] == 바둑판[i, y])
                    cnt++;
                else
                    break;
            }
            //좌방향
            for (int i = x - 1; i>=0; i--)
            {
                if (바둑판[x, y] == 바둑판[i, y])
                    cnt++;
                else
                    break;
            }
            if (cnt >= 5)
            {
                OmokComplete(x,y);
                return;
            }
            cnt = 1;
            //위방향
            for (int i = y - 1; i >= 0; i--)
            {
                if (바둑판[x, y] == 바둑판[x, i])
                    cnt++;
                else
                    break;
            }
            //아래방향
            for (int i = y + 1; i <19; i++)
            {
                if (바둑판[x, y] == 바둑판[x, i])
                    cnt++;
                else
                    break;
            }
            if (cnt >= 5)
            {
                OmokComplete(x, y);
                return;
            }
            //대각선 방향 
            for (int i = x + 1, j = y - 1; i < 19 && j >= 0; i++, j--)
            {
                if (바둑판[x, y] == 바둑판[i, j])
                    cnt++;
                else
                    break;
            }
            for (int i = x - 1, j = y + 1; i >=0 && j <19; i--, j++)
            {
                if (바둑판[x, y] == 바둑판[i,j])
                    cnt++;
                else
                    break;
            }
            //역대각선 방향
            for (int i = x - 1, j = y - 1; i >= 0 && j >= 0; i--, j--)
            {
                if (바둑판[x, y] == 바둑판[i, j])
                    cnt++;
                else
                    break;
            }
            
            for (int i = x + 1, j = y + 1; i <= 18 && j <= 18; i++, j++)
                if (바둑판[x, y] == 바둑판[i, j])
                    cnt++;
                else
                    break;

            if (cnt >= 5)
            {
                OmokComplete(x, y);
                return;
            }
        }
       

        private void OmokComplete(int x, int y)
        {
            SaveGame();
            MessageBox.Show(바둑판[x, y].ToString().ToUpper() + " Wins!!");
        }

        private void SaveGame()
        {
            if (reviveFlag == true)  // 복기모드에서는 저장하지 않습니다.
                return;
    
            string documentPath =
              Path.Combine(Environment.ExpandEnvironmentVariables
              ("%userprofile%"), "Documents").ToString();       
            //컴퓨터 마다 사용자가 저장되어 있는데(그래서 폴더의 위치 이름이 다 다름) 원하는 폴더의 위치 (여기서는 문서폴더)를 작성
            dirName = documentPath + "/Omok/";      //documentPath 는 파일경로

            if (!Directory.Exists(dirName))     //처음에 디렉토리없으면
                Directory.CreateDirectory(dirName);

            string fileName = dirName + DateTime.Now.ToShortDateString() //DateTime.Now는 지금 시간 ,ToShortDateString() 날짜
            + "-" + DateTime.Now.Hour + DateTime.Now.Minute + ".omk"; //시간, 분
            FileStream fs = new FileStream(fileName, FileMode.Create); //filename, FileMode.Create 이 filename생성
            StreamWriter sw = new StreamWriter(fs, Encoding.Default);

            foreach (Revive item in lstRevive)
            {
                sw.WriteLine("{0} {1} {2} {3}", //sw: stream writer
                   item.X, item.Y, item.Stone, item.Seq);
            }
            sw.Close();
            fs.Close();
        }

        private void Form1_Move(object sender, EventArgs e)
        {
            Invalidate();  //Paint이벤트를 만들어줍니다. onpaint()를 실행시킴.
        }

        private void 이미지ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            imageFlag = true;
        }

        private void 그리기ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            imageFlag = false;
        }

        private void 복기ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            reviveFlag = true;
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.InitialDirectory = dirName;         //현재 디렉토리 
            ofd.Filter = "Omok files(*.omk)|*.omk";
            ofd.ShowDialog();
            string fileName = ofd.FileName;
            //sequenceFlag = true;

            try
            {
                StreamReader r = File.OpenText(fileName);
                string line = "";

                // 파일내용을 한줄씩 읽어서 lstRevive 리스트에 넣는다
                while ((line = r.ReadLine()) != null)
                {
                    string[] items = line.Split(' ');
                    Revive rev = new Revive(int.Parse(items[0]), int.Parse(items[1]),
                      items[2] == "black" ? STONE.black : STONE.white,
                         int.Parse(items[3]));
                    lstRevive.Add(rev);
                }
                r.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

            // 복기준비
            reviveFlag = true;
            stoneCnt= 1;
            NewGame();
            stoneCnt = 0;
        }
        //새로 게임을 시작
        private void NewGame()
        {
            flag = false;
            for(int x=0;x<19;x++)
                for(int y=0;y<19;y++)
                    바둑판[x, y] = STONE.none;

            panel1.Refresh();
            DrawBoard();
        }
    }
}
