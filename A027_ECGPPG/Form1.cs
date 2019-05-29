using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting; //차트 사용시 필요

namespace A027_ECGPPG
{
    public partial class Form1 : Form
    {
        Timer myTimer = new Timer();
        //form1_Load (이벤트 메소드)와 같음  FORM이 시작할 때 뜨는 것이라서 
        public Form1() //form1 클래스의 생성자 메소드 (form1 만들어질때 자동으로 만들어짐 사용자가 사용할수 없음)
        {
            InitializeComponent();
            chart1.Dock = DockStyle.Fill;
            this.WindowState = FormWindowState.Maximized;
            this.Text = "ECG/PPG";
            EcgRead(); //메소드는 전부 대문자로 시작
            PpgRead();
            myTimer.Interval = 10; //0.01초
            myTimer.Tick += MyTimer_Tick; //myTimer.Tick += 작성하고 tab 누르면 만들어짐

        }
        int cursorX = 0; //현재 그래프에 표시되는 데이터의 시작점
        private void MyTimer_Tick(object sender, EventArgs e)
        {
            if (cursorX + 500 <= ecgCount)
                chart1.ChartAreas["Draw"].AxisX.ScaleView.Zoom(cursorX, cursorX + 500);//CursorX위치부터 500씩 보여줘라
            else
                myTimer.Stop();
            cursorX += 2;
        }

        private void ChartSetting()
        {
            chart1.ChartAreas.Clear();
            chart1.Series.Clear();

            chart1.ChartAreas.Add("Draw");
            chart1.ChartAreas["Draw"].BackColor = Color.Black;
            chart1.ChartAreas["Draw"].AxisX.Minimum = 0;
            chart1.ChartAreas["Draw"].AxisX.Maximum = ecgCount;
            chart1.ChartAreas["Draw"].AxisX.Interval = 50;
            chart1.ChartAreas["Draw"].AxisX.MajorGrid.LineColor = Color.Gray;
            chart1.ChartAreas["Draw"].AxisX.MajorGrid.LineDashStyle =ChartDashStyle.Dash;

            chart1.ChartAreas["Draw"].AxisY.Minimum = -3;
            chart1.ChartAreas["Draw"].AxisY.Maximum = 5;
            chart1.ChartAreas["Draw"].AxisY.Interval = 0.5;
            chart1.ChartAreas["Draw"].AxisY.MajorGrid.LineColor = Color.Gray;
            chart1.ChartAreas["Draw"].AxisY.MajorGrid.LineDashStyle = ChartDashStyle.Dash;

            chart1.Series.Add("ECG");
            chart1.Series["ECG"].ChartType = SeriesChartType.Line;
            chart1.Series["ECG"].Color = Color.LightGreen;
            chart1.Series["ECG"].BorderWidth = 2;
            chart1.Series["ECG"].LegendText = "ECG";

            chart1.Series.Add("PPG");
            chart1.Series["PPG"].ChartType = SeriesChartType.Line;
            chart1.Series["PPG"].Color = Color.Orange;
            chart1.Series["PPG"].BorderWidth = 2;
            chart1.Series["PPG"].LegendText = "PPG";

        }
        private void PpgRead()
        {
            string fileName = "../../Data/ppg.txt"; //../ 전 폴더
            string[] lines = File.ReadAllLines(fileName);

            double min = double.MaxValue; //min값을 너무 작은 값으로 설정해놓으면 배열에 있는 모든 값보다 작을 수도 있기 때문에 가장 큰 값으로 설정
            double max = double.MinValue;
            for (int i = 0; i < lines.Length; i++)
            {
                ppg[i] = double.Parse(lines[i]);
                //배열에서 최소, 최대값 찾기 (탐색)
                if (ppg[i] > max)
                    max = ppg[i];
                if (ppg[i] < min)
                    min = ppg[i];
            }
            ppgCount = lines.Length;

            string s = String.Format("PPG : Count = {0}, Min = {1}, Max ={2}",
                ppgCount, min, max);
            MessageBox.Show(s);
        }
        
        double[] ecg = new double[50000];
        double[] ppg = new double[50000];
        int ecgCount;
        int ppgCount;

        private void EcgRead()
        {
            string fileName = "../../Data/ecg.txt"; //../ 전 폴더
            string[] lines = File.ReadAllLines(fileName);

            //int i = 0;
            //배열의 각 개수를 정확히 모른다면 foreach
            /*foreach(var line in lines)
            {
                ecg[i] = double.Parse(line);
                i++;
            }*/
            double min = double.MaxValue; //min값을 너무 작은 값으로 설정해놓으면 배열에 있는 모든 값보다 작을 수도 있기 때문에 가장 큰 값으로 설정
            double max = double.MinValue;
            for(int i = 0; i < lines.Length; i++)
            {
                ecg[i] = double.Parse(lines[i])+3; //ppg와 그래프가 안겹치게 하기위해 3을 더함
                //배열에서 최소, 최대값 찾기 (탐색)
                if (ecg[i] > max)
                    max = ecg[i];
                if (ecg[i] < min)
                    min = ecg[i];
            }
            ecgCount = lines.Length;

            string s = String.Format("ECG : Count = {0}, Min = {1}, Max ={2}",
                ecgCount, min, max);
            MessageBox.Show(s);
            /* 위와 같은 의미 MessageBox.Show("ECG : Count = "
                +ecgCount + ", Min = "+min+", Max = "+max);*/
        }
        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);
            ChartSetting();
            foreach (var v in ecg)
                chart1.Series["ECG"].Points.Add(v);
            foreach (var v in ppg)
                chart1.Series["PPG"].Points.Add(v);
        }

        private void autoScrollToolStripMenuItem_Click(object sender, EventArgs e)
        {
            myTimer.Start();
        }

        private void viewAllToolStripMenuItem_Click(object sender, EventArgs e)
        {
            myTimer.Stop();
            //Invalidate();//onpaint함수 실행
            chart1.ChartAreas["Draw"].AxisX.ScaleView.Zoom(0, ecgCount);
        }
        //auto scroll은 타이머가 시작되면

        bool autoScrollFlag = false; //form1 클래스의 멤버변수 (필드) 메소드 안에 변수 선언할 때는 꼭 초기화해야함 (밖은 알아서 초기화)
        private void chart1_Click(object sender, EventArgs e)
        {
            if (autoScrollFlag == false)//한번 누르면 auto scroll
            {
                myTimer.Start();
                autoScrollFlag = true;
            }
            else //또 다시 누르면 stop
            {
                myTimer.Stop();
                autoScrollFlag = false;
            } 

        }
        
    }
}
