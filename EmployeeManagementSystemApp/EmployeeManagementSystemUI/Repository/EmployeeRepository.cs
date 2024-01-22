
using EmployeeManagementSystemUI.Models;
using Microsoft.AspNetCore.Components.Forms;
using Newtonsoft.Json;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Http.Json;

namespace EmployeeManagementSystemUI.Repository
{
    public class EmployeeRepository : IEmployeeRepository
    {
        private HttpClient client = new HttpClient();
        private string token;
        public EmployeeRepository()
        {
            client.BaseAddress = new Uri(Constants.BaseAddress);
            client.DefaultRequestHeaders.Accept.Clear();
            token = GetToken().GetAwaiter().GetResult().Replace("\"", "");
        }

        public async Task<List<Employee>> GetEmployeesAsync()
        {
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            var response = await client.GetAsync("api/getEmployees");
            if (response.IsSuccessStatusCode)
            {
                var result = await response.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<List<Employee>>(result);
            }
            return new List<Employee>();
        }

        public async Task<Employee> GetEmployeeByIdAsync(int id)
        {
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            var response = await client.GetAsync($"api/getEmployeeById/{id}");
            if (response.IsSuccessStatusCode)
            {
                var result = await response.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<Employee>(result);
            }
            return new Employee();
        }

        public async Task<bool> AddEmployeeAsync(Employee employee)
        {
            using var jsonContent = new StringContent(JsonConvert.SerializeObject(employee));
            jsonContent.Headers.ContentType = MediaTypeHeaderValue.Parse("application/json");
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            var response = await client.PostAsync("api/addEmployee", jsonContent);
            if (response.IsSuccessStatusCode)
                return true;
            return false;
        }

        public async Task<bool> UpdateEmployeeAsync(int id, Employee employee)
        {
            using var jsonContent = new StringContent(JsonConvert.SerializeObject(employee));
            jsonContent.Headers.ContentType = MediaTypeHeaderValue.Parse("application/json");
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            var response = await client.PostAsync($"api/updateEmployee/{id}", jsonContent);
            if (response.IsSuccessStatusCode)
                return true;
            return false;
        }

        public async Task<bool> DeleteEmployeeAsync(int id)
        {
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            var response = await client.DeleteAsync($"api/deleteEmployee/{id}");
            if (response.IsSuccessStatusCode)
                return true;
            return false;
        }

        private async Task<string> GetToken()
        {
            using var jsonContent = new StringContent(JsonConvert.SerializeObject(new { Username = Constants.DefaultAPIUser, Password = Constants.DefaultAPIPass }));
            jsonContent.Headers.ContentType = MediaTypeHeaderValue.Parse("application/json");
            var response = await client.PostAsync("api/login", jsonContent);
            if (response.IsSuccessStatusCode)
            {
                var result = await response.Content.ReadAsStringAsync();
                return result;
            }
            return string.Empty;
        }
    }
}
