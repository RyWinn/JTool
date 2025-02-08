using CommunityToolkit.Maui.Alerts;
using CommunityToolkit.Maui.Core;
using JToolMobile.Models;
using System.Text;

namespace JToolMobile.Components.Pages
{
    public partial class JapaneseWordCategories
    {
        private bool Loading = true;

        private string CategoryName = "";

        private List<VocabModel> Vocabs = new List<VocabModel>();
        private List<CategoryModel> Categories = new List<CategoryModel>();

        private List<CategoryModel> SelectedCategories = new List<CategoryModel>();

        private bool DeleteDisabled = true;
        private bool Expanded = true;

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

        private void SelectCategory(HashSet<CategoryModel> Selected)
        {
            SelectedCategories = Selected.ToList();

            DeleteDisabled = SelectedCategories.Count == 0;

            StateHasChanged();
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

        private async Task RemoveCategory()
        {
            if (SelectedCategories.Count == 0)
            {
                await Toast.Make("Please select a Category to remove", ToastDuration.Short).Show();
                return;
            }

            if (Vocabs.Count(x => SelectedCategories.Any(y => y.CategoryID == x.CategoryID)) == 0)
            {
                foreach (var category in SelectedCategories)
                {
                    Categories.Remove(category);
                }

                await CsvHelper.SaveCsvAsync("CategoryData.csv", ConvertCategoriesToCsv(Categories));
            }
            else
            {
                await Toast.Make("One or more selected Categories has words assigned to it", ToastDuration.Short).Show();
            }
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

        private string GetGridHeight()
        {
            if (!Expanded)
            {
                return "calc(100vh - 160px)";
            }

            return "calc(100vh - 210px)";
        }
    }
}