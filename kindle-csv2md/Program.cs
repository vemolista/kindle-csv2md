using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualBasic.FileIO;
using System.IO;
using System.Threading;
using System.Windows.Forms;

namespace kindle_csv2markdown
{
    public class ExportItem
    {
        public string Type { get; set; }
        public string Page { get; set; }
        public string Content { get; set; }
    }

    class Program
    {
        // CAREFUL. [STAThread] in this context is apparently very naughty as pointed out by 
        // https://stackoverflow.com/questions/12553932/how-do-i-add-a-form-to-a-console-app-so-that-user-can-select-file#comment16909242_12554056
        // https://stackoverflow.com/a/9620540
        [STAThread]
        static void Main(string[] args)
        {
            var exportItems = new List<ExportItem>();

            var dialog = new OpenFileDialog
            {
                Multiselect = false,
                Title = "Select Kindle Highlights CSV",
                Filter = "CSV files|*.csv"
            };

            var highlightsFilePath = "";
            using (dialog)
            {
                if (dialog.ShowDialog() == DialogResult.OK) highlightsFilePath = dialog.FileName;
            }

            using (TextFieldParser parser = new TextFieldParser(highlightsFilePath))
            {
                parser.SetDelimiters(",");
                while (!parser.EndOfData)
                {
                    string[] fields = parser.ReadFields();

                    exportItems.Add(new ExportItem()
                    {
                        Type = fields[0],
                        Page = fields[1],
                        Content = fields[3]
                    });
                }
            }

            exportItems = exportItems.Where(x => !string.IsNullOrEmpty(x.Content) && x.Content != "Annotation").ToList();

            var exportContent = "";
            foreach (var item in exportItems)
            {
                exportContent += $"- {item.Page}, {item.Type}\n";
                exportContent += $"  - {item.Content}\n";
            }

            var exportFilePath = "";
            var saveFileDialog = new SaveFileDialog
            {
                Title = "Save Your Exported Highlights",
                Filter = "Markdown|*.md"
            };

            using (saveFileDialog)
            {
                saveFileDialog.ShowDialog();
                exportFilePath = saveFileDialog.FileName;
            }

            File.WriteAllText(exportFilePath, exportContent);

            Console.WriteLine("Done.");
            Console.WriteLine($"Highlights {highlightsFilePath}");
            Console.WriteLine($"Have been saved to {exportFilePath}");
            Console.WriteLine("--------------");
            Console.WriteLine("--------------");
            Console.WriteLine("--------------");
            Console.WriteLine("You can close this window");
            Console.ReadLine();
        }
    }
}
