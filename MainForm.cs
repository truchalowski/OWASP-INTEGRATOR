using OWASP_INTEGRATOR.Services;

public class MainForm : Form
{
    private TextBox textBoxDefectDojoToken;
    private string reportPath;

    public async Task UploadReportToDefectDojo()
    {
        var dojoService = new DefectDojoService();
        string token = textBoxDefectDojoToken.Text;
        int engagementId = 123;
        var result = await dojoService.UploadDependencyCheckToDefectDojo(reportPath, engagementId, token);
        MessageBox.Show(result.Message);
    }
}