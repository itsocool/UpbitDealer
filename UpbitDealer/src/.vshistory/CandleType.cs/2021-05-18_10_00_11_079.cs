using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UpbitDealer.src
{
    [Serializable]
    public class CandleType
    {
        public int Minute { get; set; } = 1;
        public string Name { get; set; } = "1분 봉";
    }
}
