using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;

namespace A028_DigitalClock
{
    /// <summary>
    /// MainWindow.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class MainWindow : Window
    {
        DispatcherTimer t = new DispatcherTimer(); //form에서는 Timer wpf에서는 DispatcherTimer
        public MainWindow()
        {
            InitializeComponent();
            t.Interval = new TimeSpan(0, 0, 0, 0, 10); //0.01초마다 일 시 분 초 (100이면 0.1)
            t.Tick += T_Tick;
            t.Start();

            DispatcherTimer t1 = new DispatcherTimer();
            t1.Interval = new TimeSpan(0, 1, 0); //시 분 초 (1분) ex) (0, 0, 10) 10초
            t1.Tick += T1_Tick;
            t1.Start();
        }

        private void T1_Tick(object sender, EventArgs e)
        {
            t.Stop(); //1분후에 멈춤
        }

        private void T_Tick(object sender, EventArgs e)
        {
            //dClock.Text = DateTime.Now.ToString()+DateTime.Now.Millisecond; 
            //( DateTime은 구조체) 이렇게하면 밀리초가 5자리로 나와서 아예 포맷을 만들었음
            string s = String.Format("{0}:{1,3:000}",
                DateTime.Now.ToString(), DateTime.Now.Millisecond); //{1,3:000} 3자리인데 두자리인 경우 0써줌.
            //000안해주면 Text가 움직임 (두자리였다 세자리인 경우 때문에 그래서 두자리인 경우도 세자리로 만들어주기위해
            dClock.Text = s;
        }

        private void Grid_MouseDown(object sender, MouseButtonEventArgs e)
        {
            t.Start(); //다시 시작하게 함 (글자를 누르면)
        }
    }
}
