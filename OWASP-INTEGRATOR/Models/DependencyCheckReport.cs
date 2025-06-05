using System.Text.Json;

namespace OWASP_INTEGRATOR.Models
{
    public class DependencyCheckReport
    {
        public List<Dependency> Dependencies { get; set; }

        public class Dependency
        {
            public string FileName { get; set; }
            public List<Vulnerability> Vulnerabilities { get; set; }
        }

        public class Vulnerability
        {
            public string Name { get; set; }
            public string Severity { get; set; }
            public string Description { get; set; }
            public Cvss Cvssv3 { get; set; }
            public Cvss Cvssv2 { get; set; }
        }

        public class Cvss
        {
            public double BaseScore { get; set; }
        }

        public static DependencyCheckReport LoadReport(string reportPath)
        {
            string json = File.ReadAllText(reportPath);
            return JsonSerializer.Deserialize<DependencyCheckReport>(json, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            });
        }
    }
}
