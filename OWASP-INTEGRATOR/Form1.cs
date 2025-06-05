using OWASP_INTEGRATOR.Models;
using OWASP_INTEGRATOR.Services;
using System.Text.Json;

namespace OWASP_INTEGRATOR
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            InitializeCustomComponents();
        }

        private void InitializeCustomComponents()
        {
            // Dodaj etykiety opisuj¹ce pola
            var labelEngagementId = new Label
            {
                Text = "Engagement ID (DefectDojo):",
                Top = 20,
                Left = 10,
                Width = 120
            };
            Controls.Add(labelEngagementId);

            textBoxEngagementId.Top = labelEngagementId.Top;
            textBoxEngagementId.Left = 140;
            textBoxEngagementId.Width = 200;

            scanButton.Text = "Skanuj i wyœlij";
            scanButton.Top = 100;
            scanButton.Left = 10;
            scanButton.Width = 150;
            scanButton.Click += scanButton_Click;
        }

        private async void scanButton_Click(object? sender, EventArgs e)
        {
            string dependencyCheckPath = @"C:/Tools/dependency-check/bin";
            // Ustaw projectPath na katalog, który naprawdê istnieje i zawiera pliki do skanowania!
            // Najczêœciej to katalog z Twoim kodem Ÿród³owym lub paczkami NuGet, NIE katalog build/output!
            string projectPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "..", "..", ".."); // katalog projektu
            projectPath = Path.GetFullPath(projectPath);

            string reportOutput = @"C:\Reports\SecurityScan";
            Directory.CreateDirectory(reportOutput);

            if (!Directory.Exists(projectPath))
            {
                MessageBox.Show($"Œcie¿ka do skanowania nie istnieje:\n{projectPath}");
                return;
            }

            try
            {
                var process = new System.Diagnostics.Process();
                process.StartInfo.FileName = Path.Combine(dependencyCheckPath, "dependency-check.bat");
                process.StartInfo.Arguments = $"--project \"MyApp\" --scan \"{projectPath}\" --format \"JSON\" --out \"{reportOutput}\"";
                process.StartInfo.WorkingDirectory = dependencyCheckPath;
                process.StartInfo.UseShellExecute = false;
                process.StartInfo.RedirectStandardOutput = true;
                process.StartInfo.RedirectStandardError = true;
                process.StartInfo.CreateNoWindow = true;


                scanButton.Enabled = false;
                process.Start();
                string output = await process.StandardOutput.ReadToEndAsync();
                string error = await process.StandardError.ReadToEndAsync();
                process.WaitForExit();

                if (process.ExitCode != 0)
                {
                    MessageBox.Show($"Dependency-Check zakoñczy³ siê b³êdem:\n{error}");
                    return;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"B³¹d podczas uruchamiania Dependency-Check:\n{ex.Message}");
                return;
            }

            string[] jsonReports = Directory.GetFiles(reportOutput, "*.json");
            if (jsonReports.Length == 0)
            {
                MessageBox.Show(
                    "Nie znaleziono raportu JSON w folderze:\n" + reportOutput +
                    "\n\nMo¿liwe przyczyny:\n" +
                    "- Dependency-Check nie zosta³ uruchomiony poprawnie\n" +
                    "- Raport nie jest generowany w formacie JSON\n" +
                    "- Nazwa pliku jest inna ni¿ oczekiwana\n\n" +
                    "SprawdŸ logi lub uruchom Dependency-Check rêcznie, aby upewniæ siê, ¿e raport powstaje."
                );
                return;
            }

            string reportPath = jsonReports[0];

            var report = DependencyCheckReport.LoadReport(reportPath);

            var critical = report?.Dependencies?
                .SelectMany(d => d.Vulnerabilities ?? new List<DependencyCheckReport.Vulnerability>())
                .Where(v => v.Severity == "Critical" || v.Cvssv3?.BaseScore >= 9 || v.Cvssv2?.BaseScore >= 9)
                .ToList();

            MessageBox.Show($"Skanowanie zakoñczone.\nZnaleziono {critical?.Count ?? 0} krytycznych podatnoœci.");

            if (!int.TryParse(textBoxEngagementId.Text, out int engagementId))
            {
                MessageBox.Show("Nieprawid³owy Engagement ID.");
                return;
            }

            //if (string.IsNullOrWhiteSpace(textBoxDefectDojoToken.Text))
            //{
            //    MessageBox.Show("Token DefectDojo nie zosta³ ustawiony.");
            //    return;
            //}

            //string token = textBoxDefectDojoToken.Text;
            var dojoService = new DefectDojoService();
            var result = await dojoService.UploadDependencyCheckToDefectDojo(reportPath, engagementId);

            scanButton.Enabled = true;
            MessageBox.Show(result.Message);
        }

        /*private void scanButton_Click(object sender, EventArgs e)
        {

        }*/
    }
}
