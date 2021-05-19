using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UpbitDealer.src
{
    public class CandleType
    {

        public static Dictionary<int, string> CandleNames = new Dictionary<int, string>()
        {
            {1, ac.CANDLE_MIN1 },
            {3, ac.CANDLE_MIN3 },
            {5, ac.CANDLE_MIN5 },
            {10, ac.CANDLE_MIN10 },
            {15, ac.CANDLE_MIN15 },
            {30, ac.CANDLE_MIN30 },
            {60, ac.CANDLE_HOUR1 }
        };

        public int Minute { get; set; }
        public string Name { get; set; }

        public CandleType(int minute)
        {
            if (CandleNames.Keys.Contains(minute))
            {
                Minute = minute;
                Name = CandleNames[minute];
            }
            else
            {
                throw new Exception("유효하지 않은 캔들 주기값(분) 입니다.");
            }
        }
    }
}
