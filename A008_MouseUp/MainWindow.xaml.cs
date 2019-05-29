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

namespace A008_MouseUp
{
    /// <summary>
    /// MainWindow.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void grid1_MouseUp(object sender, MouseButtonEventArgs e)  //객체 e 메소드 안에 getposition지정
        {
            MessageBox.Show("You clicked me at" + e.GetPosition(this).ToString());  //(this는 메인 윈도우) wpf에서 마우스 위치를 계산할 때
        }
    }
}
