using JToolMobile.Models;
using MudBlazor;

namespace JToolMobile.Components.Pages
{
    public partial class KanaChart
    {
        private bool Loading = true;
        private bool KatakanaMode = false;
        private bool ShowKana = true;

        private List<KanaModel> Hiragana = new List<KanaModel>();
        private List<KanaModel> Katakana = new List<KanaModel>();

        private (string, string) CurrentKana;
        private (string, string) CurrentEnglish;
        private string EnteredKana = "";
        private string EnteredEnglish = "";

        protected override async Task OnInitializedAsync()
        {
            Hiragana.Add(new KanaModel() { AColumn = "a = あ", EColumn = "e = え", IColumn = "i = い", OColumn = "o = お", UColumn = "u = う" });
            Hiragana.Add(new KanaModel() { AColumn = "ka = か", EColumn = "ke = け", IColumn = "ki = き", OColumn = "ko = こ", UColumn = "ku = く" });
            Hiragana.Add(new KanaModel() { AColumn = "sa = さ", EColumn = "se = せ", IColumn = "shi = し", OColumn = "so = そ", UColumn = "su = す" });
            Hiragana.Add(new KanaModel() { AColumn = "ta = た", EColumn = "te = て", IColumn = "chi = ち", OColumn = "to = と", UColumn = "tsu = つ" });
            Hiragana.Add(new KanaModel() { AColumn = "na = な", EColumn = "ne = ね", IColumn = "ni = に", OColumn = "no = の", UColumn = "nu = ぬ" });
            Hiragana.Add(new KanaModel() { AColumn = "ha = は", EColumn = "he = へ", IColumn = "hi = ひ", OColumn = "ho = ほ", UColumn = "fu = ふ" });
            Hiragana.Add(new KanaModel() { AColumn = "ma = ま", EColumn = "me = め", IColumn = "mi = み", OColumn = "mo = も", UColumn = "mu = む" });
            Hiragana.Add(new KanaModel() { AColumn = "ya = や", EColumn = "", IColumn = "", OColumn = "yo = よ", UColumn = "yu = ゆ" });
            Hiragana.Add(new KanaModel() { AColumn = "ra = ら", EColumn = "re = れ", IColumn = "ri = り", OColumn = "ro = ろ", UColumn = "ru = る" });
            Hiragana.Add(new KanaModel() { AColumn = "wa = わ", EColumn = "", IColumn = "", OColumn = "n = ん", UColumn = "wo = を" });

            Katakana.Add(new KanaModel() { AColumn = "a = ア", EColumn = "e = エ", IColumn = "i = イ", OColumn = "o = オ", UColumn = "u = ウ" });
            Katakana.Add(new KanaModel() { AColumn = "ka = カ", EColumn = "ke = ケ", IColumn = "ki = キ", OColumn = "ko = コ", UColumn = "ku = ク" });
            Katakana.Add(new KanaModel() { AColumn = "sa = サ", EColumn = "se = セ", IColumn = "shi = シ", OColumn = "so = ソ", UColumn = "su = ス" });
            Katakana.Add(new KanaModel() { AColumn = "ta = タ", EColumn = "te = テ", IColumn = "chi = チ", OColumn = "to = ト", UColumn = "tsu = ツ" });
            Katakana.Add(new KanaModel() { AColumn = "na = ナ", EColumn = "ne = ネ", IColumn = "ni = ニ", OColumn = "no = ノ", UColumn = "nu = ヌ" });
            Katakana.Add(new KanaModel() { AColumn = "ha = ハ", EColumn = "he = ヘ", IColumn = "hi = ヒ", OColumn = "ho = ホ", UColumn = "fu = フ" });
            Katakana.Add(new KanaModel() { AColumn = "ma = マ", EColumn = "me = メ", IColumn = "mi = ミ", OColumn = "mo = モ", UColumn = "mu = ム" });
            Katakana.Add(new KanaModel() { AColumn = "ya = ヤ", EColumn = "", IColumn = "", OColumn = "yo = ヨ", UColumn = "yu = ユ" });
            Katakana.Add(new KanaModel() { AColumn = "ra = ラ", EColumn = "re = レ", IColumn = "ri = リ", OColumn = "ro = ロ", UColumn = "ru = ル" });
            Katakana.Add(new KanaModel() { AColumn = "wa = ワ", EColumn = "", IColumn = "", OColumn = "n = ン", UColumn = "wo = ヲ" });

            GetRandomKana();
            GetRandomEnglish();

            Loading = false;
        }

        private void GetRandomKana()
        {
            var random = new Random();
            var randomKana = KatakanaMode ? cHiraganaKatakana.KatakanaMap.ElementAt(random.Next(cHiraganaKatakana.KatakanaMap.Count)) : 
                cHiraganaKatakana.HiraganaMap.ElementAt(random.Next(cHiraganaKatakana.HiraganaMap.Count));
            CurrentKana = (randomKana.Key, randomKana.Value);
        }

        private void GetRandomEnglish()
        {
            var random = new Random();
            var randomKana = KatakanaMode ? cHiraganaKatakana.KatakanaMap.ElementAt(random.Next(cHiraganaKatakana.KatakanaMap.Count)) :
                cHiraganaKatakana.HiraganaMap.ElementAt(random.Next(cHiraganaKatakana.HiraganaMap.Count));
            CurrentEnglish = (randomKana.Key, randomKana.Value);
        }

        private void CheckKanaGuess()
        {
            if (EnteredKana.ToLower().Trim() == CurrentKana.Item1.ToLower().Trim())
            {
                Snackbar.Add("Correct!", Severity.Success);

                GetRandomKana();

                EnteredKana = "";
            }
            else
            {
                Snackbar.Add("Try again!", Severity.Error);
            }
        }

        private void CheckEnglishGuess()
        {
            if (EnteredEnglish.ToLower().Trim() == CurrentEnglish.Item2.ToLower().Trim())
            {
                Snackbar.Add("Correct!", Severity.Success);

                GetRandomEnglish();

                EnteredEnglish = "";
            }
            else
            {
                Snackbar.Add("Try again!", Severity.Error);
            }
        }

        private void ToggleKanaMode()
        {
            KatakanaMode = !KatakanaMode;

            GetRandomKana();
            GetRandomEnglish();
        }
    }
}