using System.Diagnostics;

namespace OWASP_INTEGRATOR.Services
{
    public class DependencyCheckService
    {
        private readonly string _dependencyCheckPath;

        public DependencyCheckService(string dependencyCheckPath)
        {
            _dependencyCheckPath = dependencyCheckPath;
        }

        public void RunScan(string projectPath, string outputDirectory)
        {
            string batPath = Path.Combine(_dependencyCheckPath, "dependency-check.bat");
            if (!File.Exists(batPath))
            {
                throw new FileNotFoundException("Nie znaleziono pliku dependency-check.bat", batPath);
            }

            var process = new Process
            {
                StartInfo = new ProcessStartInfo
                {
                    FileName = "cmd.exe",
                    Arguments = $"/C \"{batPath}\" --project \"{Path.GetFileName(projectPath)}\" " +
                                $"--scan \"{projectPath}\" --format JSON --out \"{outputDirectory}\"",
                    RedirectStandardOutput = true,
                    RedirectStandardError = true,
                    UseShellExecute = false,
                    CreateNoWindow = true
                }
            };

            process.Start();
            string output = process.StandardOutput.ReadToEnd();
            string errors = process.StandardError.ReadToEnd();
            process.WaitForExit();

            File.WriteAllText(Path.Combine(outputDirectory, "scan-log.txt"), output + "\n" + errors);
        }
    }
}
