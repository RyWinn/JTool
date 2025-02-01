using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JToolMobile
{
    public class CsvHelper
    {
        public static async Task<string> SaveCsvAsync(string Filename, string CsvData)
        {
            try
            {
                string FilePath = Path.Combine(FileSystem.AppDataDirectory, Filename);

                await File.WriteAllTextAsync(FilePath, CsvData, Encoding.UTF8);

                return FilePath;
            }
            catch (Exception ex) 
            {
                Console.WriteLine($"Error saving CSV: {ex.Message}");
                return string.Empty;
            }
        }

        public static async Task<string> LoadCsvAsync(string FileName, string DataToInsert = "")
        {
            try
            {
                string FilePath = Path.Combine(FileSystem.AppDataDirectory, FileName);

                if (File.Exists(FilePath))
                {
                    return await File.ReadAllTextAsync(FilePath, Encoding.UTF8);
                }
                else
                {
                    await SaveCsvAsync(FileName, DataToInsert);
                    return string.Empty;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error loading CSV: {ex.Message}");
                return string.Empty;
            }
        }
    }
}
