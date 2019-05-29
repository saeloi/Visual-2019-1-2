using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.IO.Ports;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;
//프로그램의 단위 :프로젝트 ->프로젝트 저장될 때 솔루션으로 저장됨 (전에 했던 파일 열때 솔루션파일열어야)
namespace A023_PhotocellValue
{
    public partial class Form1 : Form
    {
        private double xCount = 200;
        SerialPort sPort;
        List <SensorData> myData = new List<SensorData>();
        Random r = new Random();
        Timer t = new Timer();
        SqlConnection conn;
        string connString= @"Data Source=(LocalDB)\MSSQLLocalDB;
AttachDbFilename=C:\Users\사용자\Desktop\비주얼 프로그래밍\Visual2019_1\A023_PhotocellValue\SensorData.mdf
            ;Integrated Security=True";

        public Form1()
        {
            InitializeComponent();
            // ComboBox
            foreach (var ports in SerialPort.GetPortNames())  //var ports in SerialPort.GetPortNames() 시리얼 포트를 가져올 때
            {
                comboBox1.Items.Add(ports);
            }
            comboBox1.Text = "Select Port";

            // 아두이노의 A0에서 받는 값의 범위
            progressBar.Minimum = 0;
            progressBar.Maximum = 1023;

            // 차트 모양 세팅
            ChartSetting();
            // 숫자 버튼
            btnPortValue.BackColor = Color.Blue;
            btnPortValue.ForeColor = Color.White;
            btnPortValue.Text = "";
            btnPortValue.Font = new Font("맑은 고딕", 16, FontStyle.Bold);

            label1.Text = "Connection Time : ";
            textBox1.TextAlign = HorizontalAlignment.Center;
            btnConnect.Enabled = false; //아직 선택하지 못하게 함
            btnDisconnect.Enabled = false;
        }
        //센서가 10개가 들어올 때 하나의 차트에 표시하고 싶을 때 영역은 1개이고 시리즈가 10개 
        //10개의 센서값을 하나하나 씩 다른 차트에 표시하고 싶을 때는 영역10개 시리즈 10개

        private void ChartSetting() //차트 area과 series가 있다. (기억해야할 2가지)
        {
            chart1.ChartAreas.Clear(); //디폴트로 하나씩 차트값에 들어가 있어서 초기화 해주고 시작해야한다.
            chart1.ChartAreas.Add("draw"); //차트 영역이 하나가 있다.
            chart1.ChartAreas["draw"].AxisX.Minimum = 0;
            chart1.ChartAreas["draw"].AxisX.Maximum = xCount;   // 최초에 차트 폭은 200으로 함
            chart1.ChartAreas["draw"].AxisX.Interval = xCount / 4;
            chart1.ChartAreas["draw"].AxisX.MajorGrid.LineColor = Color.White;
            chart1.ChartAreas["draw"].AxisX.MajorGrid.LineDashStyle = ChartDashStyle.Dash;

            chart1.ChartAreas["draw"].AxisY.Minimum = 0;
            chart1.ChartAreas["draw"].AxisY.Maximum = 1024;
            chart1.ChartAreas["draw"].AxisY.Interval = 200;
            chart1.ChartAreas["draw"].AxisY.MajorGrid.LineColor = Color.White;
            chart1.ChartAreas["draw"].AxisY.MajorGrid.LineDashStyle = ChartDashStyle.Dash;

            chart1.ChartAreas["draw"].BackColor = Color.Blue;

            chart1.ChartAreas["draw"].CursorX.AutoScroll = true;

            chart1.ChartAreas["draw"].AxisX.ScaleView.Zoomable = true;
            chart1.ChartAreas["draw"].AxisX.ScrollBar.ButtonStyle = ScrollBarButtonStyles.SmallScroll;
            chart1.ChartAreas["draw"].AxisX.ScrollBar.ButtonColor = Color.LightSteelBlue;

            chart1.Series.Clear();
            chart1.Series.Add("PhotoCell"); //photocell이라는 시리즈 추가
            chart1.Series["PhotoCell"].ChartType = SeriesChartType.Line;
            chart1.Series["PhotoCell"].Color = Color.LightGreen;
            chart1.Series["PhotoCell"].BorderWidth = 3;
            if (chart1.Legends.Count > 0) //범례를 지우는 것 (legend는 범례) 범례가 있으면 차트가 좁아짐 그래서 지우는 것 
                chart1.Legends.RemoveAt(0);
        }
        //콤보박스에 선택된 것이 바뀌면
        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e) //포트는 콤보박스
        {
            ComboBox cb = sender as ComboBox;
            sPort = new SerialPort(cb.SelectedItem.ToString());
            sPort.Open(); //선택한 포트를 연다 포트로 데이터가 들어올텐데 
          //sPort.DataReceived += SPort_DataReceived;//보내주는 데이터를 받으면 이 함수를 실행해
          //근데 아두이노로 연결 우리는 못하니까 이거는 주석처리함
          //대신 Timer를 하나 만들어서 함 (약간 흉내내서) 

            t.Interval = 1000;  //1초에 한번씩
            t.Start();
            t.Tick += T_Tick;  //원래 randomgen사용했는데 왜 이렇게 바꾼거지?...

            label1.Text = "Connection Time : " + DateTime.Now.ToString();
            btnDisconnect.Enabled = true;
        }

        private void T_Tick(object sender, EventArgs e)
        {
            string s = r.Next(1024).ToString();
            //여기서는 ShowValue(s);만 해줘도 실행됨.
            this.BeginInvoke((new Action(delegate { ShowValue(s); })));
        }
        private void SPort_DataReceived(object sender, SerialDataReceivedEventArgs e) //showvalue에 값 보내주는 역할
        {
            string s = sPort.ReadLine(); //sPort에서 값을 읽어라
            this.BeginInvoke((new Action(delegate { ShowValue(s); }))); //어려운 부분
            //이벤트가 아니라 그냥 함수면 Showvalue(s)하면 되는데 DataReceived는 정형적인 동작이 아니라 비동기식 동작 (보내줘야할 때 동작해야하니까 예측x)
            //(컴퓨터 cpu가 일 처리하다가 여유가 있으니까 다른일도 같이 처리 , 여러개의 일 하나하나를 process라고 함 -> main process에서 그 일들을 처리)
            //main process(스레드)가 아니라 하나의 가지를 만든 process (이런 경우에 처리하라고 따로 하나 내어주는 것)
            //main process가 아닌 또 하나의 process처리할 때 이런식으로 처리해줘야한다.
        }

        private void ShowValue(string s) //아두이노에서 넘겨주는 값: string s (숫자 하나) 
        {
            int v = Int32.Parse(s); // 정수로 변환
            if (v < 0 || v > 1023)  // 처음 시작할 때 작은 값이나 큰 값이 들어오는 경우 배제 (아날로그 포트에 들어오는 값은 저 0~1023 사이의 값이니까)
                return;

            SensorData data = new SensorData( //SensorData :날짜 시간 값
              DateTime.Now.ToShortDateString(), //몇년 몇월 몇일
              DateTime.Now.ToString("HH:mm:ss"), v); //시간 분 초
            myData.Add(data); //SensorData의 list : myData (프로그램 끄면 없어짐)
            DBInsert(data);

            textBox1.Text = myData.Count.ToString();    // myData의 갯수를 표시
            progressBar.Value = v; //원래는 v 대신 Int32.Parse(s)였음.

            // ListBox에 시간과 값을 표시
            string item = DateTime.Now.ToString() + "\t" + s; //Now.ToString()이렇게 하면 오늘 날짜 시간이 다 나옴
            listBox.Items.Add(item);
            listBox.SelectedIndex = listBox.Items.Count - 1;  // 계속 스크롤이 되도록 처리

            // Chart 표시
            chart1.Series["PhotoCell"].Points.Add(v); //v라는 값 집어넣음
            
            //zoom은 나중에 설명 (차트에 그려지는 대로 이동시키는 기능)


            // zoom을 위해 200개까지는 기본, 데이터 갯수가 많아지면 200개만 보이지만, 스크롤 나타남
            chart1.ChartAreas["draw"].AxisX.Minimum = 0;
            chart1.ChartAreas["draw"].AxisX.Maximum = (myData.Count >= xCount) ? myData.Count : xCount;

            // change chart range : Zoom 사용  
            if (myData.Count > xCount)
            {
                chart1.ChartAreas["draw"].AxisX.ScaleView.Zoom(myData.Count - xCount, myData.Count);
            }
            else
            {
                chart1.ChartAreas["draw"].AxisX.ScaleView.Zoom(0, xCount);
            }

            // 숫자버튼 표시 (버튼에 포트 이름 표시해주려고)
            btnPortValue.Text = /*sPort.PortName +*/ "\n" + s;
        }
        private void DBInsert(SensorData data)
        {
            string sql = string.Format("Insert into SensorTable" + "(Date, Time, Value) Values('{0}','{1}',{2})",
        data.Date, data.Time, data.Value);

            try
            {
                using (conn = new SqlConnection(connString))
                using (SqlCommand comm = new SqlCommand(sql, conn))
                {
                    conn.Open();
                    comm.ExecuteNonQuery();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            finally
            {
                conn.Close();
            }
        }
    
        //연결안해도 실행되게 하려고 
        private void btnPortValue_Click(object sender, EventArgs e)
        {
            // Timer를 버튼 눌렀을 때 동작하게 하려고
            t.Interval = 1000;
            t.Start();
            t.Tick += T_Tick;  //T_Tick은 위에서 만들었으니까 

            label1.Text = "Connection Time : " + DateTime.Now.ToString();
            btnDisconnect.Enabled = true;
        }

        private void btnDisconnect_Click(object sender, EventArgs e)
        {
            t.Stop();
        }
    }
}
