using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JToolMobile.Models
{
    public class VocabModel
    {
        public string EnglishWord { get; set; } = "";
        public string JapaneseWord { get; set; } = ""; //ie. Hiragana, Katakana or Kanji form
        public string RomanjiWord { get; set; } = ""; //ie. The Japanese word written using English characters.
        public int CategoryID { get; set; } //Allows us to toggle which topics are shown
        public string CategoryName { get; set; } = "";
    }
}
