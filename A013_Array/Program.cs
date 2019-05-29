using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace A013_Array
{
    class Program
    {
        static void Main(string[] args)
        {
            //배열 내용 정리
            //int a[10];
            //int b[] = { 1, 2, 3, 4, 5 }; //배열공간 컴파일 시 잡음
            
            int[] b = { 1, 2, 3, 4, 5 };
            //TextBox[] c= {txt1, txt2, ..}  학점계산기 코딩 시 참고

            for(int i = 0; i < b.Length; i++)  //배열도 class 왜냐하면 length라는 속성을 갖고 있으니까 때문에 class로 컨트롤가능
                Console.WriteLine(b[i]);

            Array.Sort(b);      //b.sort가 아니라 저렇게 멤버를 사용해야함.
            for (int i = 0; i < b.Length; i++)  
                Console.WriteLine(b[i]);

            Console.WriteLine("Using for reach");
            foreach (var i in b)   //b 배열에 있는 var(타입이 지정되지 않은 변수) 각각(각각의 요소)에 대해서 
                Console.WriteLine(i);       //foreach (int i in b) 여기선 정수 이기 때문에 이렇게 작성해도 ok



        }
    }
}
