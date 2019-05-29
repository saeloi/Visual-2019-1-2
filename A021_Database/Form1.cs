using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Data.OleDb;

namespace A021_Database
{
    public partial class Form1 : Form
    {
        OleDbConnection conn = null;   //객체생성은 아직 아니고 이름만 
        OleDbCommand comm = null;
        OleDbDataReader reader = null;

        string connString = @"Provider=Microsoft.ACE.OLEDB.12.0;Data Source= C:\Users\사용자\Desktop\비주얼 프로그래밍\Students.accdb";
        //서 가져옴 @는 특수문자 x (원래 경로 백슬래쉬사용하는데 그거는 특수문자로 인식해서 잘 못 읽어서 )

        public Form1()
        {
            InitializeComponent();
            DisplayStudents();
        }

        private void DisplayStudents()
        {
            ConnectionOpen();
            //명령어 작성: 모든 레코드를 가져와라
            //명령어는 SQL(언어)로 만든다. 
            string sql = "SELECT * FROM StudentTable";
            comm = new OleDbCommand(sql, conn);

            ReadAndAddToListBox();      //함수 사용이유 : 반복을 줄이고 코드 길이를 줄이기 위해
            ConnectionClose();
        }

        private void ConnectionClose()
        {
            conn.Close();
            conn = null;
        }

        //Reader에서 DB값을 읽어와서 ListBox에 표시
        private void ReadAndAddToListBox()
        {
            //명령어를 실행
            reader = comm.ExecuteReader(); // ExecuteReader: DB에서 읽어올 때 사용

            //DB에서 읽어오는 여러개의 레코드
            while (reader.Read())
            {
                string x = "";
                x += reader["ID"] + "\t";
                x += reader["SID"] + "\t";
                x += reader["SName"] + "\t";
                x += reader["Phone"];
                listBox1.Items.Add(x);

            }
            reader.Close();
        }

        //DB의 connnection을 열어주는 메서드
        private void ConnectionOpen()
        {
            if (conn == null) //conn이 연결되어 있지 않다면
            {
                conn = new OleDbConnection(connString); //생성할 때 connString을 포함해서
                conn.Open(); //이문장에 의해서 connection이 만들어진다.
            }
        }

        private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            ListBox lb = sender as ListBox;
            if (lb.SelectedItem == null)
                return;
            string[] s = lb.SelectedItem.ToString().Split('\t');

            txtID.Text = s[0];
            txtSld.Text = s[1];
            txtSName.Text = s[2];
            txtPhone.Text = s[3];
        }

        private void btnInsert_Click(object sender, EventArgs e)
         {
             if (txtSld.Text == "" || txtSName.Text == "" || txtPhone.Text == "")
             {
                 //세 개의 값이 다들어있어야함
                 MessageBox.Show("학번, 이름, 전화번호는 반드시 입력해야합니다.");
                 return;
             }
             ConnectionOpen();

             string sql = string.Format("INSERT INTO StudentTable(SiD, SName, Phone) values('{0}','{1}','{2}')", txtSld.Text, txtSName.Text, txtPhone.Text);
             MessageBox.Show(sql); //테스트용

             comm = new OleDbCommand(sql, conn);
             if(comm.ExecuteNonQuery()==1) //DB 레코드의 숫자 반환 (하나 변경(추가)했으면 1리턴 --하나 추가했을테니까 1이어야함)
                 MessageBox.Show("추가 성공!");
             ConnectionClose();

             listBox1.Items.Clear(); //이문장이 안써지면 원래 있던 값까지 또 추가 됨.
             DisplayStudents();

         }

         private void btnSearch_Click(object sender, EventArgs e)
         {
             if(txtSld.Text=="" && txtSName.Text=="" && txtPhone.Text == "")
             {
                 MessageBox.Show("학번, 이름, 전화번호 중 하나를 반드시 입력해야합니다." ,"검색 에러");
                 return;
             }
             ConnectionOpen();
             string sql = "";
             if(txtSld.Text != "")
             {
                 sql = string.Format("SELECT * FROM StudentTable WHERE Sid={0}", txtSld.Text); //학번은 숫자니까 따옴표 없어도 됨
             }
             else if (txtSName.Text != "")
             {
                 sql = string.Format("SELECT * FROM StudentTable WHERE SName='{0}'", txtSName.Text);
             }
             else if (txtPhone.Text != "")
             {
                 sql = string.Format("SELECT * FROM StudentTable WHERE Phone='{0}'", txtPhone.Text);
             }

             comm = new OleDbCommand(sql, conn);
             listBox1.Items.Clear();
             ReadAndAddToListBox();
             ConnectionClose();
         }

         private void btnDelete_Click(object sender, EventArgs e)
         {
             if (txtSld.Text == "" || txtSName.Text == "" || txtPhone.Text == "")
             {
                 //세 개의 값이 다들어있어야함
                 MessageBox.Show("삭제할 레코드를 선택해야합니다.", "삭제 에러");
                 return;
             }
             ConnectionOpen();

             string sql = string.Format("DELETE FROM StudentTable WHERE ID={0}", txtID.Text);
             MessageBox.Show(sql); //테스트용

             comm = new OleDbCommand(sql, conn);
             if (comm.ExecuteNonQuery() == 1) //DB 레코드의 숫자 반환 (하나 변경(추가)했으면 1리턴 --하나 추가했을테니까 1이어야함)
                 MessageBox.Show("삭제 성공!");
             ConnectionClose();

             listBox1.Items.Clear(); //이문장이 안써지면 원래 있던 값까지 또 추가 됨.
             DisplayStudents();
         }

         private void btnUpdate_Click(object sender, EventArgs e)
         {
             if (txtID.Text == "" )
             {
                 //세 개의 값이 다들어있어야함
                 MessageBox.Show("수정할 레코드를 먼저 선택해야 합니다.", "수정 에러");
                 return;
             }
             ConnectionOpen();

             string sql = string.Format("UPDATE StudentTable SET Sid={0}, SName='{1}', Phone='{2}' WHERE ID={3}", txtID.Text, txtSName.Text, txtPhone.Text);
             MessageBox.Show(sql); //테스트용

             comm = new OleDbCommand(sql, conn);
             if (comm.ExecuteNonQuery() == 1) //DB 레코드의 숫자 반환 (하나 변경(추가)했으면 1리턴 --하나 추가했을테니까 1이어야함)
                 MessageBox.Show("수정 성공!");
             ConnectionClose();

             listBox1.Items.Clear(); //이문장이 안써지면 원래 있던 값까지 또 추가 됨.
             DisplayStudents();
         }

         private void btnViewAll_Click(object sender, EventArgs e)
         {
             listBox1.Items.Clear();
             DisplayStudents();
         }

         private void btnClear_Click(object sender, EventArgs e)
         {
             txtID.Text = "";
             txtPhone.Text = "";
             txtSName.Text = "";
             txtSld.Text = "";

         }

         private void btnExit_Click(object sender, EventArgs e)
         {
             Close();
         }     
    }
}

