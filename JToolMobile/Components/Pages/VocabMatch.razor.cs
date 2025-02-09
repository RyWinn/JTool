using JToolMobile.Models;
using Microsoft.AspNetCore.Components.Web;

namespace JToolMobile.Components.Pages
{
    public partial class VocabMatch
    {
        private bool Loading = true;

        private VocabModel RandomEnglishWord = new VocabModel();
        private VocabModel RandomRomanjiWord = new VocabModel();
        private VocabModel RandomJapaneseWord = new VocabModel();

        private Random Random = new Random();

        private string EnglishWord = "";
        private string RomanjiWord = "";
        private string JapaneseWord = "";

        private List<VocabModel> Vocabs = new List<VocabModel>();
        private List<CategoryModel> Categories = new List<CategoryModel>();

        protected override async Task OnInitializedAsync()
        {
            string VocabCSV = await CsvHelper.LoadCsvAsync("VocabData.csv", "EnglishWord,JapaneseWord,RomanjiWord,CategoryID");
            string CategoryCSV = await CsvHelper.LoadCsvAsync("CategoryData.csv", "CategoryID,CategoryName");

            foreach (var line in CategoryCSV.Split('\n', StringSplitOptions.RemoveEmptyEntries))
            {
                var values = line.Split(',');

                if (values.Length >= 2 && int.TryParse(values[0], out int categoryId))
                {
                    Categories.Add(new CategoryModel
                    {
                        CategoryID = categoryId,
                        CategoryName = values[1].Trim()
                    });
                }
            }

            foreach (var line in VocabCSV.Split('\n', StringSplitOptions.RemoveEmptyEntries))
            {
                var values = line.Split(',');

                if (values.Length >= 4 && int.TryParse(values[3], out int categoryId))
                {
                    // Find matching category name
                    string categoryName = Categories.FirstOrDefault(c => c.CategoryID == categoryId)?.CategoryName ?? "Unknown";

                    Vocabs.Add(new VocabModel
                    {
                        EnglishWord = values[0].Trim(),
                        JapaneseWord = values[1].Trim(),
                        RomanjiWord = values[2].Trim(),
                        CategoryID = categoryId,
                        CategoryName = categoryName
                    });
                }
            }

            GetRandomEnglishWord();
            GetRandomJapaneseWord();
            GetRandomRomanjiWord();

            Loading = false;
        }

        private void GetRandomJapaneseWord(int RetryCount = 0)
        {
            if (Vocabs.Count > 0)
            {
                string OriginalWord = RandomJapaneseWord.JapaneseWord;

                var categories = Categories.Where(y => y.Visible);

                if (categories.Any())
                {
                    var vocabs = Vocabs.Where(x => categories.Any(c => c.CategoryID == x.CategoryID)).ToList();

                    if (vocabs.Count > 0)
                    {
                        RandomJapaneseWord = vocabs[Random.Next(vocabs.Count)];

                        if (OriginalWord == RandomJapaneseWord.JapaneseWord && RetryCount < 5)
                        {
                            GetRandomJapaneseWord(RetryCount + 1);
                        }
                    }
                }
            }
        }

        private void GetRandomEnglishWord(int RetryCount = 0)
        {
            if (Vocabs.Count > 0)
            {
                string OriginalWord = RandomEnglishWord.EnglishWord;

                var categories = Categories.Where(y => y.Visible);

                if (categories.Any())
                {
                    var vocabs = Vocabs.Where(x => categories.Any(c => c.CategoryID == x.CategoryID)).ToList();

                    if (vocabs.Count > 0)
                    {
                        RandomEnglishWord = vocabs[Random.Next(vocabs.Count)];

                        if (OriginalWord == RandomEnglishWord.EnglishWord && RetryCount < 5)
                        {
                            GetRandomEnglishWord(RetryCount + 1);
                        }
                    }
                }
            }
        }

        private void GetRandomRomanjiWord(int RetryCount = 0)
        {
            if (Vocabs.Count > 0)
            {
                string OriginalWord = RandomRomanjiWord.RomanjiWord;

                var categories = Categories.Where(y => y.Visible);

                if (categories.Any())
                {
                    var vocabs = Vocabs.Where(x => categories.Any(c => c.CategoryID == x.CategoryID)).ToList();

                    if (vocabs.Count > 0)
                    {
                        RandomRomanjiWord = vocabs[Random.Next(vocabs.Count)];

                        if (OriginalWord == RandomRomanjiWord.RomanjiWord && RetryCount < 5)
                        {
                            GetRandomRomanjiWord(RetryCount + 1);
                        }
                    }
                }
            }
        }

        private void EnglishKeyPress(KeyboardEventArgs e)
        {
            if (e.Key == "Enter")
            {
                SubmitEnglishWord();
            }
        }

        private void SubmitEnglishWord()
        {
            if (EnglishWord.ToLower().Trim() == RandomEnglishWord.RomanjiWord.ToLower().Trim() || EnglishWord.ToLower().Trim() == RandomEnglishWord.JapaneseWord.ToLower().Trim()) //Entered correct translation
            {
                Snackbar.Add("Correct.", MudBlazor.Severity.Success);

                GetRandomEnglishWord();

                EnglishWord = "";
            }
            else //wrong answer
            {
                Snackbar.Add("Wrong answer. Please try again.", MudBlazor.Severity.Error);
            }
        }

        private void RomanjiKeyPress(KeyboardEventArgs e)
        {
            if (e.Key == "Enter")
            {
                SubmitRomanjiWord();
            }
        }

        private void SubmitRomanjiWord()
        {
            if (RomanjiWord.ToLower().Trim() == RandomRomanjiWord.EnglishWord.ToLower().Trim()) //Entered correct translation
            {
                Snackbar.Add("Correct.", MudBlazor.Severity.Success);

                GetRandomRomanjiWord();

                RomanjiWord = "";
            }
            else //wrong answer
            {
                Snackbar.Add("Wrong answer. Please try again.", MudBlazor.Severity.Error);
            }
        }

        private void JapaneseKeyPress(KeyboardEventArgs e)
        {
            if (e.Key == "Enter")
            {
                SubmitJapaneseWord();
            }
        }

        private void SubmitJapaneseWord()
        {
            if (JapaneseWord.ToLower().Trim() == RandomJapaneseWord.EnglishWord.ToLower().Trim()) //Entered correct translation
            {
                Snackbar.Add("Correct.", MudBlazor.Severity.Success);

                GetRandomJapaneseWord();

                JapaneseWord = "";
            }
            else //wrong answer
            {
                Snackbar.Add("Wrong answer. Please try again.", MudBlazor.Severity.Error);
            }
        }

        private void ToggleVisibility(CategoryModel category)
        {
            category.Visible = !category.Visible;

            GetRandomEnglishWord();
            GetRandomJapaneseWord();
            GetRandomRomanjiWord();
        }
    }
}