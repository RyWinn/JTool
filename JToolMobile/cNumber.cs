using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JToolMobile
{
    public class cNumber
    {
        public static Dictionary<int, string> Numbers { get; } = GetNumbers();

        private static Dictionary<int, string> GetNumbers()
        {
            Dictionary<int, string> Numbers = new Dictionary<int, string>
            {
                {1, "いち (ichi)"}, {2, "に (ni)"}, {3, "さん (san)"}, {4, "よん (yon)"}, {5, "ご (go)"}, {6, "ろく (roku)"}, {7, "なな (nana)"}, 
                {8, "はち (hachi)"}, {9, "きゅ (kyu)"}, {10, " jyuu"},
                {20, "nijyuu"}, {30, "sanjyuu"}, {40, "yonjyuu"}, {50, "gojyuu"}, {60, "rokujuu"}, {70, "nanajyuu"}, {80, "hachijyuu"}, {90, "kyuujuu"},
                {100, "hyaku"}
            };

            return Numbers;
        }
    }
}