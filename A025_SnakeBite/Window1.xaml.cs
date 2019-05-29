using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.Windows.Threading;

namespace A025_SnakeBite
{
    /// <summary>
    /// Window1.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class Window1 : Window
    {
        Random r = new Random(); //랜덤하게 위치 지정 
        Ellipse[] snakes = new Ellipse[30]; //30개 원의 배열 (뱀)
        Ellipse egg = new Ellipse();
        private int size = 12;         // Egg와 Body의사이즈 (원의 크기)
        private int visibleCount = 5; // 처음에 보이는 뱀의 길이 (원래 총 30개인데 처음에는 5개만)
        private string move = "";      // 뱀의 이동방향 
        private int eaten = 0;         // 먹은 알의 개수
        DispatcherTimer timer = new DispatcherTimer(); //타이머
        Stopwatch sw = new Stopwatch();  //스톱
        private bool startFlag = false;

        public Window1()
        {
            InitializeComponent();
            //TestSnake();
            InitSnake();
            InitEgg();
            timer.Interval = new TimeSpan(0, 0, 0, 0, 100); //하루, 0시간, 0분 ,0초 
            timer.Tick += Timer_Tick;
            timer.Start();


        }
        //이동할 때 뱀 머리의 위치만 지정해주고 나머지는 그 위의 위치 따라가기
        private void Timer_Tick(object sender, EventArgs e)
        {
            if (move != "")//처음(가만히 있다가 마우스 움직이면 움직임)
            {
                startFlag = true;

                for (int i = visibleCount; i >= 1; i--) // 꼬리 하나를 더 계산
                {
                    Point p = (Point)snakes[i - 1].Tag; //위치는 Tag가 가지고 있음
                    snakes[i].Tag = new Point(p.X, p.Y);
                }

                Point pnt = (Point)snakes[0].Tag; //머리의 위치
                if (move == "Right")
                    snakes[0].Tag = new Point(pnt.X + size, pnt.Y);
                else if (move == "Left")
                    snakes[0].Tag = new Point(pnt.X - size, pnt.Y);
                else if (move == "Up")
                    snakes[0].Tag = new Point(pnt.X, pnt.Y - size);
                else if (move == "Down")
                    snakes[0].Tag = new Point(pnt.X, pnt.Y + size);
                EatEgg();   // 알을 먹었는지 체크 (알과 뱀 머리의 좌표가 같으면)
            }

            if (startFlag == true)
            {
                TimeSpan ts = sw.Elapsed;
                time.Text = String.Format("Time = {0:00}:{1:00}.{2:00}",
                   ts.Hours, ts.Minutes, ts.Seconds, ts.Milliseconds / 10);
                DrawSnakes();
            }
        }

        private void DrawSnakes()
        {
            for (int i = 0; i < visibleCount; i++)
            {
                Point p = (Point)snakes[i].Tag;
                Canvas.SetLeft(snakes[i], p.X);
                Canvas.SetTop(snakes[i], p.Y);
            }
        }

        private void EatEgg()
        {
            Point pS = (Point)snakes[0].Tag;
            Point pE = (Point)egg.Tag;

            if (pS.X == pE.X && pS.Y == pE.Y)
            {
                egg.Visibility = Visibility.Hidden;
                visibleCount++;
                // 꼬리를 하나 늘림
                snakes[visibleCount - 1].Visibility = Visibility.Visible;
                score.Text = "Eggs = " + (++eaten).ToString();

                if (visibleCount == 30)
                {
                    timer.Stop();
                    sw.Stop();
                    DrawSnakes();
                    TimeSpan ts = sw.Elapsed;
                    string tElapsed = String.Format("Time = {1:00}:{2:00}.{3:00}",
                        ts.Minutes, ts.Seconds, ts.Milliseconds / 10);
                    MessageBox.Show("Success!!!  " + tElapsed + " sec");
                    this.Close();
                }

                Point p = new Point(r.Next(1, 480 / size) * size,
                  r.Next(1, 380 / size) * size);
                egg.Tag = p;
                egg.Visibility = Visibility.Visible;
                Canvas.SetLeft(egg, p.X);
                Canvas.SetTop(egg, p.Y);
            }
        }

        private void InitEgg()
        {
            egg = new Ellipse();
            egg.Tag = new Point(r.Next(1, 480 / size) * size,
               r.Next(1, 380 / size) * size);
            egg.Width = size;
            egg.Height = size;
            egg.Stroke = Brushes.Black;
            egg.Fill = Brushes.Red;

            Point p = (Point)egg.Tag;
            Canvas1.Children.Add(egg);
            Canvas.SetLeft(egg, p.X);
            Canvas.SetTop(egg, p.Y);
        }

        private void InitSnake()
        {
            //뱀 만들기(아직 뱀의 위치는 결정되지 않았음)
            for (int i = 0; i < 30; i++) //배열반복하면서
            {
                snakes[i] = new Ellipse();
                snakes[i].Width = size;
                snakes[i].Height = size;

                if (i == 0) //30개 배열중 맨앞 (뱀의 머리)
                    snakes[i].Fill = Brushes.Chocolate; // 머리 색깔변경
                else if (i % 5 == 0) //5번째 나오는 뱀의 몸은 초록색
                    snakes[i].Fill = Brushes.YellowGreen; // 5번째 마디 색깔변경
                else //아니면 금색으로 (5마디마다 색깔 다르게)
                    snakes[i].Fill = Brushes.Gold;

                snakes[i].Stroke = Brushes.Black;
                Canvas1.Children.Add(snakes[i]);
            }

            for (int i = visibleCount; i < 30; i++)
            {
                snakes[i].Visibility = Visibility.Hidden; 
            }

            int x = r.Next(1, 480 / size) * size;
            int y = r.Next(1, 380 / size) * size;
            CreateSnake(x, y); //뱀의 머리 위치는 정해졌는데 나머지 몸통의 위치 지정
        }

        //뱀의 머리에서부터 아래쪽으로 5개 마디를 지정함.
        private void CreateSnake(int x, int y)
        {
            for(int i = 0; i < visibleCount; i++){ //0~4까지 태그를 포인트(x,y)로, 처음 랜덤하게 만들어진 그 위치부터 나란히
                snakes[i].Tag = new Point(x, y + i * size);
                Canvas.SetLeft(snakes[i], x);
                Canvas.SetTop(snakes[i], y + i * size);
            }
        }

        private void window_KeyDown(object sender, KeyEventArgs e)
        {
            if (move == "")  // 맨 처음 키가 눌리면 sw 시작
                sw.Start();

            if (e.Key == Key.Right)
                move = "Right";
            else if (e.Key == Key.Left)
                move = "Left";
            else if (e.Key == Key.Up)
                move = "Up";
            else if (e.Key == Key.Down)
                move = "Down";
            else if (e.Key == Key.Escape)
                move = "";
        }
    }

    //winform에서는 grapics , wpf는 ellipse
    /*private void TestSnake()
    {
        //egg
        Ellipse x = new Ellipse();
        x.Width = 20;
        x.Height = 20;
        x.Stroke = Brushes.Black; //테두리
        x.Fill = Brushes.Red; //색깔지정 Brushes
        Canvas.SetLeft(x, 100);
        Canvas.SetTop(x, 100);
        Canvas1.Children.Add(x);

        //snake
        Ellipse[] snake = new Ellipse[30];
    }*/
}
