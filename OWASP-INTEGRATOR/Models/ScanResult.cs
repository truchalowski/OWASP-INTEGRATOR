namespace OWASP_INTEGRATOR.Models
{
    public class ScanResult
    {
        public string ProjectName { get; set; }
        public DateTime ScanDate { get; set; }
        public int CriticalVulnerabilities { get; set; }
        public int HighVulnerabilities { get; set; }
        public int MediumVulnerabilities { get; set; }
        public string ReportPath { get; set; }
    }
}