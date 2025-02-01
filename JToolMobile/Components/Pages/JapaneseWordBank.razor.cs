using JToolMobile.Models;
using System.Text;

namespace JToolMobile.Components.Pages
{
    public partial class JapaneseWordBank
    {
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
    }
}