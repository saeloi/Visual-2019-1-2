using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
//교재 p.189  class 설명,  p.279 속성 설명,  p.192 Car car= new Car(); Car클래스의 car객체 

//단 일반적인 클래스 사용 외에 Static의 경우 p.201  Math m= new Math();
//m.sin(30)  ---> Math는 static 클래스 이므로 객체를 만들지 않고 클래스 이름으로 사용한다.
//Console.WriteLine(Math.Sin(Math.PI)); 이렇게 사용해야함. (정적인 매소드)

namespace A018_class
{
    class Car
    {
        static void Main(string[] args)
        {
            //클래스를 사용하자
            //클래스의 객체(obhect/ instance)를 만들어서 사용한다.
            //모든 클래스의 조상 object (따로 상속 안받아도 됨)
            Car x = new Car();   //객체를 만들 때 new 사용
            x.SetInTime();
            //...
            x.setOutTime();
            //x.SetCarColor(1);
            x.CarColor = 1;     //속성은 대문자로 시작
        }
    }
}
//모든 프로그램은 class 안에 있다. main도 

class Car           //class이름 대문자로 시작
{
    private int carNumber;      //디폴트 private
    private DateTime inTime; //DateTime은 구조체 (날짜조정할 수 있는 거) class는 참조형
    private DateTime outTime;
   // private int carColor;
    public int CarColor { set; get; } //속성

    public void SetInTime()     //파라미터가 있으면 this 꼭 써야함
    {
        this.inTime = DateTime.Now; //속성 만약 괄호가 있으면 매서드
    }
    public void SetOutTime()
    {
        this.outTime = DateTime.Now;
    }
    public void SetCarColor(int color)
    {
        carColor = color;
    }

}
