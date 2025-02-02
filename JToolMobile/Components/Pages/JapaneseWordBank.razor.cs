using CommunityToolkit.Maui.Alerts;
using CommunityToolkit.Maui.Core;
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
        private int CategoryID = 0;
        private string CategoryName = "";

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

            Loading = false;
        }

        private async Task AddCategory()
        {
            if (Categories.Any(x => x.CategoryName == CategoryName) || string.IsNullOrEmpty(CategoryName))
            {
                return;
            }

            Categories.Add(new CategoryModel { CategoryName = CategoryName, CategoryID = Categories.Count + 1 });

            await CsvHelper.SaveCsvAsync("CategoryData.csv", ConvertCategoriesToCsv(Categories));

            CategoryName = ""; //clear textbox after inserting
        }

        private static string ConvertCategoriesToCsv(List<CategoryModel> categories)
        {
            var csvBuilder = new StringBuilder();
            csvBuilder.AppendLine("CategoryID,CategoryName"); // Header

            foreach (var category in categories)
            {
                csvBuilder.AppendLine($"{category.CategoryID},{category.CategoryName}");
            }

            return csvBuilder.ToString();
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
                await Toast.Make(ToastMessage, ToastDuration.Short).Show();
            }

            return ReturnValue;
        }

        private async Task AddVocab()
        {
            if (await ValidateVocab())
            {
                Vocabs.Add(new VocabModel { EnglishWord = EnglishWord, JapaneseWord = JapaneseWord, RomanjiWord = RomanjiWord, CategoryID = CategoryID, CategoryName = Categories.FirstOrDefault(x => x.CategoryID == CategoryID).CategoryName });

                await CsvHelper.SaveCsvAsync("VocabData.csv", ConvertVocabsToCsv(Vocabs));

                EnglishWord = ""; //clear textbox after inserting
                RomanjiWord = "";
                JapaneseWord = "";
                CategoryID = 0;
            }
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
    }
}