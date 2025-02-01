using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JTool
{
    public class cNumber
    {
        public enum NumberCounts
        {
            Things,
            People,
            Machines,
            Years,
            Months,
            Weeks,
            Days,
            Hours,
            Minutes,
            FlatThings,
            Times
        }

        public static Dictionary<int, string> Numbers { get; } = GetNumbers();

        private static Dictionary<int, string> GetNumbers()
        {
            Dictionary<int, string> Numbers = new Dictionary<int, string>
            {
                {1, "ichi"}, {2, "ni"}, {3, "san"}, {4, "yon"}, {5, "go"}, {6, "roku"}, {7, "nana"}, {8, "hachi"}, {9, "kyuu"}, {10, "jyuu"},
                {20, "nijyuu"}, {30, "sanjyuu"}, {40, "yonjyuu"}, {50, "gojyuu"}, {60, "rokujuu"}, {70, "nanajyuu"}, {80, "hachijyuu"}, {90, "kyuujuu"},
                {100, "hyaku"}
            };

            for (int i = 11; i <= 19; i++)
            {
                int onesDigit = i % 10;
                Numbers.Add(i, $"{Numbers[10]} {Numbers[onesDigit]}");
            }

            for (int i = 21; i <= 99; i++)
            {
                if (i % 10 == 0) continue;

                int tensDigit = i / 10 * 10;
                int onesDigit = i % 10;
                Numbers.Add(i, $"{Numbers[tensDigit]} {Numbers[onesDigit]}");
            }

            return Numbers;
        }

        public static string GetThingCounter(KeyValuePair<int, string> Value)
        {
            string ReturnValue = "";

            if (Value.Key <= 10)
            {
                if (Value.Key == 1)
                {
                    ReturnValue = "hitotsu";
                }
                else if (Value.Key == 2)
                {
                    ReturnValue = "futatsu";
                }
                else if (Value.Key == 3)
                {
                    ReturnValue = "mittsu";
                }
                else if (Value.Key == 4)
                {
                    ReturnValue = "yottsu";
                }
                else if (Value.Key == 5)
                {
                    ReturnValue = "itsutsu";
                }
                else if (Value.Key == 6)
                {
                    ReturnValue = "muttsu";
                }
                else if (Value.Key == 7)
                {
                    ReturnValue = "nanatsu";
                }
                else if (Value.Key == 8)
                {
                    ReturnValue = "yattsu";
                }
                else if (Value.Key == 9)
                {
                    ReturnValue = "kokonotsu";
                }
                else if (Value.Key == 10)
                {
                    ReturnValue = "too";
                }
            }
            else
            {
                ReturnValue = Value.Value;
            }

            return ReturnValue;
        }

        public static string GetPeopleCounter(KeyValuePair<int, string> Value)
        {
            string ReturnValue = "";

            if (Value.Key == 1)
            {
                ReturnValue = "hitori";
            }
            else if (Value.Key == 2)
            {
                ReturnValue = "futari";
            }
            else
            {
                ReturnValue = Value.Value + "nin";
            }

            return ReturnValue;
        }

        public static string GetMonthCounter(KeyValuePair<int, string> Value)
        {
            string ReturnValue = "";

            if (Value.Key == 1)
            {
                ReturnValue = "ikkagetsu";
            }
            else
            {
                ReturnValue = Value.Value + "kagetsu";
            }

            return ReturnValue;
        }

        public static string GetWeekCounter(KeyValuePair<int, string> Value)
        {
            string ReturnValue = "";

            if (Value.Key == 1)
            {
                ReturnValue = "isshuukan";
            }
            else
            {
                ReturnValue = Value.Value + "shuukan";
            }

            return ReturnValue;
        }

        public static string GetDayCounter(KeyValuePair<int, string> Value)
        {
            string ReturnValue = "";

            if (Value.Key == 2)
            {
                ReturnValue = "futsuka";
            }
            else if (Value.Key == 3)
            {
                ReturnValue = "mikka";
            }
            else if (Value.Key == 4)
            {
                ReturnValue = "yokka";
            }
            else if (Value.Key == 5)
            {
                ReturnValue = "itsuka";
            }
            else if (Value.Key == 6)
            {
                ReturnValue = "muika";
            }
            else if (Value.Key == 7)
            {
                ReturnValue = "nanoka";
            }
            else if (Value.Key == 8)
            {
                ReturnValue = "youka";
            }
            else if (Value.Key == 9)
            {
                ReturnValue = "kokonoka";
            }
            else if (Value.Key == 10)
            {
                ReturnValue = "tooka";
            }
            else if (Value.Key == 14)
            {
                ReturnValue = "juu yokka";
            }
            else if (Value.Key == 20)
            {
                ReturnValue = "hatsuka";
            }
            else if (Value.Key == 24)
            {
                ReturnValue = "nijuu yokka";
            }
            else
            {
                ReturnValue = Value.Value + "nichi";
            }

            return ReturnValue;
        }

        public static string GetTimeCounter(KeyValuePair<int, string> Value)
        {
            string ReturnValue = "";

            if (Value.Key == 1)
            {
                ReturnValue = "ikkai";
            }
            else
            {
                ReturnValue = Value.Value + "kai";
            }

            return ReturnValue;
        }

        public static string GetMinuteCounter(KeyValuePair<int, string> Value)
        {
            string ReturnValue = "";

            if (Value.Key == 1)
            {
                ReturnValue = "ippun";
            }
            else
            {
                int lastNumber = Value.Key % 10;

                if (lastNumber == 2 || lastNumber == 5 || lastNumber == 7 || lastNumber == 9)
                {
                    ReturnValue = Value.Value + "fun";
                }
                else
                {
                    ReturnValue = Value.Value + "pun";
                }
            }

            return ReturnValue;
        }
    }
}