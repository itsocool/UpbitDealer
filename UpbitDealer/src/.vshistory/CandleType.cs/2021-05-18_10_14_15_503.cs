using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UpbitDealer.src
{
    public class CandleType
    {
        public static int[] minutes = new int[] { 1, 3, 5, 10, 15, 30, 60 };

        public int Minute { get; set; } = 1;
        public string Name { get; set; } = ac.CANDLE_MIN1;

        public CandleType(int minute, string name)
        {
            if (minutes.Contains(minute))
            {
                Minute = minute;
                Name = name;
            }
            else
            {
                throw new Exception("유효하지 않은 캔들 주기값(분) 입니다.");
            }
        }
    }
}
