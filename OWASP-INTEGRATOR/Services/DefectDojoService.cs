using System.Net.Http.Headers;

namespace OWASP_INTEGRATOR.Services
{
    public class DefectDojoService
    {
        public async Task<(bool Success, string Message)> UploadDependencyCheckToDefectDojo(string filePath, int engagementId)
        {
            string token = "21df68a15bab5659094b964344fac70b98c24d9f";
            using var client = new HttpClient();
            client.BaseAddress = new Uri("http://localhost:8080/");
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Token", token);

            using var form = new MultipartFormDataContent();
            form.Add(new StringContent("Dependency Check Scan"), "scan_type");
            form.Add(new StringContent(engagementId.ToString()), "engagement");
            form.Add(new StringContent("Info"), "minimum_severity");
            form.Add(new StringContent("true"), "active");
            form.Add(new StringContent("false"), "verified");

            if (!File.Exists(filePath))
            {
                return (false, $"Plik JSON nie został znaleziony: {filePath}");
            }

            var fileBytes = File.ReadAllBytes(filePath);
            form.Add(new ByteArrayContent(fileBytes), "file", Path.GetFileName(filePath));

            var response = await client.PostAsync("api/v2/import-scan/", form);

            if (response.IsSuccessStatusCode)
            {
                return (true, "Raport został przesłany do DefectDojo!");
            }
            else
            {
                var err = await response.Content.ReadAsStringAsync();
                return (false, $"Błąd przy wysyłaniu: {response.StatusCode}\n{err}");
            }
        }
    }
}
