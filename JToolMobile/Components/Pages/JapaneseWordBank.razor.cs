using JToolMobile.Models;
using System.Text;

namespace JToolMobile.Components.Pages
{
    public partial class JapaneseWordBank
    {
        private bool Loading = true;

        private string EnglishWord = "";
        private string RomanjiWord = "";
        private string JapaneseWord = "";
        private int? CategoryID;

        private List<VocabModel> Vocabs = new List<VocabModel>();
        private List<CategoryModel> Categories = new List<CategoryModel>();

        private List<VocabModel> SelectedVocabs = new List<VocabModel>();

        private bool DeleteDisabled = true;
        private bool Expanded = false;

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

            Loading = false;
        }

        private async Task<bool> ValidateVocab()
        {
            bool ReturnValue = true;

            var ToastMessage = "";

            if (Vocabs.Any(v => v.EnglishWord.Trim().ToLower() == EnglishWord.Trim().ToLower() && v.JapaneseWord.Trim().ToLower() == JapaneseWord.Trim().ToLower() && v.RomanjiWord.Trim().ToLower() == RomanjiWord.Trim().ToLower()))
            {
                ToastMessage = "This word already exists in the word bank";

                ReturnValue = false;
            }
            else if (string.IsNullOrEmpty(EnglishWord))
            {
                ToastMessage = "Please enter an English word";

                ReturnValue = false;
            }
            else if (string.IsNullOrEmpty(RomanjiWord) || string.IsNullOrEmpty(JapaneseWord))
            {
                ToastMessage = "Please enter either a Romanji or Japanese word";

                ReturnValue = false;
            }
            else if (CategoryID <= 0)
            {
                ToastMessage = "Please select a Category";

                ReturnValue = false;    
            }

            if (!ReturnValue)
            {
                Snackbar.Add(ToastMessage, MudBlazor.Severity.Warning);
            }

            return ReturnValue;
        }

        private void SelectVocab(HashSet<VocabModel> Selected)
        {
            SelectedVocabs = Selected.ToList();

            DeleteDisabled = SelectedVocabs.Count == 0;

            StateHasChanged();
        }

        private async Task AddVocab()
        {
            if (await ValidateVocab())
            {
                Vocabs.Add(new VocabModel { EnglishWord = EnglishWord, JapaneseWord = JapaneseWord, RomanjiWord = RomanjiWord, CategoryID = CategoryID.Value, CategoryName = Categories.FirstOrDefault(x => x.CategoryID == CategoryID).CategoryName });

                await CsvHelper.SaveCsvAsync("VocabData.csv", ConvertVocabsToCsv(Vocabs));

                EnglishWord = ""; //clear textbox after inserting
                RomanjiWord = "";
                JapaneseWord = "";
                CategoryID = null;
            }
        }

        private async Task RemoveVocab()
        {
            if (SelectedVocabs.Count == 0)
            {
                Snackbar.Add("Please select a Vocab to remove", MudBlazor.Severity.Warning);

                return;
            }

            foreach (var vocab in SelectedVocabs)
            {
                Vocabs.Remove(vocab);
            }

            await CsvHelper.SaveCsvAsync("VocabData.csv", ConvertVocabsToCsv(Vocabs));
        }

        private static string ConvertVocabsToCsv(List<VocabModel> vocabs)
        {
            var csvBuilder = new StringBuilder();
            csvBuilder.AppendLine("EnglishWord,JapaneseWord,RomanjiWord,CategoryID"); // Header

            foreach (var vocab in vocabs)
            {
                csvBuilder.AppendLine($"{vocab.EnglishWord},{vocab.JapaneseWord},{vocab.RomanjiWord},{vocab.CategoryID}");
            }

            return csvBuilder.ToString();
        }

        private string GetGridHeight()
        {
            if (!Expanded)
            {
                return "calc(100vh - 170px)";
            }

            return "calc(100vh - 300px)";
        }
    }
}