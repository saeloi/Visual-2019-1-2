using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace A024_ListArray
{
    class Program
    {
        static void Main(string[] args)
        {
            Random r = new Random(); //랜덤 생성
            /* 세가지 방법으로 사용
             int a = r.Next(); //정수의 범위의 랜덤한 숫자 생성
             int b = r.Next(100); //0~99
             int c = r.Next(10, 20); //10~20 사이의 값

             //또다른 방법
             double d = r.NextDouble(); //0~1사이의 소수점
            Console.WriteLine("{0}, {1}, {2}, {3}", a, b, c, d); //여러 개 출력할 때

            Console.WriteLine("{0,10}, {1,10}, {2,10}, {3,10}", a, b, c, d); 전부 10자리로 찍어주겠다 (자리 수 지정)
            소수점 밑에 자리수를 세자리까지 하고 싶을 때 Console.WriteLine("{0,10}, {1,10}, {2,10}, {3,10:F3}", a, b, c, d);
             {3,10:F3} 10자리 찍는데 소수점 3자리까지 / {0:N0}라고 쓰면 세자리 마다 ,(콤마)를 써준다*/

            /*for (int i = 0; i < 10; i++) //10번 반복
            {
                int a = r.Next();
                int b = r.Next(100); //0~99
                int c = r.Next(10, 20); //10~20 사이의 값

                //또다른 방법
                double d = r.NextDouble(); //0~1사이의 소수점
                Console.WriteLine("{0}, {1}, {2}, {3}", a, b, c, d);
            }*/

            //두 개의 주사위를 100번 던져서 합이 얼마인지 출력하라
            /*int sum = 0;
            for (int i = 0; i < 100; i++) //10번 반복
            {
                int a = r.Next(1,7);
                int b = r.Next(1,7);
                sum = a + b;
                Console.WriteLine("{0}, {1}, {2}", a, b, sum);
            }*/
            /*for(int i = 0; i < 100; i++) //교수님 풀이
            {
                Console.WriteLine("{0}", r.Next(1, 7) + r.Next(1, 7));
            }*/
            /*두 개의 주사위를 1000000번 던져서 각각의 합이 몇 번씩 나왔는지를 출력하시오. 
            [출력]
            2 : 15678
            3 : 12569
            ...
            12 : 56486
            해결책 : 배열이 필요하다 [13]개 짜리
            */
            int[] arr = new int[13];
            int sum = 0;
            for (int i = 0; i < 1000000; i++)
            {
                arr[r.Next(1, 7) + r.Next(1, 7)]++;
                /*내 방식.....ㅜㅜㅜ
                 * int a = r.Next(1, 7);
                int b = r.Next(1, 7);
                sum = a + b;

                if (sum == 2) arr[2] += 1;
                else if (sum == 3) arr[3] += 1;
                else if (sum == 4) arr[4] += 1;
                else if (sum == 5) arr[5] += 1;
                else if (sum == 6) arr[6] += 1;
                else if (sum == 7) arr[7] += 1;
                else if (sum == 8) arr[8] += 1;
                else if (sum == 9) arr[9] += 1;
                else if (sum == 10) arr[10] += 1;
                else if (sum == 11) arr[11] += 1;
                else if (sum == 12) arr[12] += 1;*/
            }
            /*for (int i = 2; i <= 12; i++)
            {
                Console.WriteLine("{0,2} : {1}",i, arr[i]);
            }*/
            Console.WriteLine("foreach arry");
            foreach (var item in arr)
            {
                Console.WriteLine(item);
            }

            //리스트 : Generic <T> List <T>
            List<int> lst = new List<int>();
            for(int i = 0; i < 10; i++)
            {
                lst.Add(r.Next(100));
            }
            /*foreach (var item in lst) //var는 아무 타입와도 상관없음 (여기서는 int의미)
            {
                Console.WriteLine(item);
            }*/
            Console.WriteLine("for list");
            for (int i = 0; i < 10; i++)//근데 이경우에는 리스트의 크기를 정확히 알고 있어야함
            {
                Console.WriteLine(lst[i]); //마치 배열처럼
            }


            //마지막으로 10개짜리 문자열 배열 s1과 문자열 리스트 s2를 만드시오
            string[] s1 = new string[10];
            List<string> s2 = new List<string>();
            Console.WriteLine("문자열 10개를 입력하시오: ");
            //문자열 10개를 콘솔에서 입력받아 배열과 리스트에 저장하시오
            for (int i=0;i<10;i++) //문자열 배열
            {
                string s= Console.ReadLine();
                s1[i] = s;
                s2.Add(s);
            }

            //문자열 10개를 출력
            for(int i = 0; i < 10; i++)
            {
                Console.WriteLine("{0,20} {1,20}", s1[i], s2[i]);
            }

            //정렬하여 출력하기 배열의 정렬과 리스트의 정렬 다름
            Array.Sort(s1);
            s2.Sort();

            Console.WriteLine("배열과 리스트 정렬 후 출력");
            for (int i = 0; i < 10; i++)
            {
                Console.WriteLine("{0,20} {1,20}", s1[i], s2[i]);
            }  
        }
    }
        
}
