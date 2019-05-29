using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace A019_Omok //namespace는 공유 (같은 이름으로)
{
    class Revive
    {
        //속성, property (대문자) : 따로 멤버 메서드 만들어서 멤버변수에 접근하는 것 불편하니까 속성 이용
        //(멤버변수는 private이어야하니까) {set; get;}안쓰면 그냥 필드
        public int X { get; set; } 
        public int Y { get; set; }
        public STONE Stone { get; set; }
        public int Seq { get; set; }

        public Revive(int x, int y, STONE s, int seq)
        {
            this.X = x;
            this.Y = y;
            this.Stone = s;
            this.Seq = seq;
        }
    }
}
