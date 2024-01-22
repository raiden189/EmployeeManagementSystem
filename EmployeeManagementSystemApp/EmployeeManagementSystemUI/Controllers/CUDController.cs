using AspNetCoreHero.ToastNotification.Abstractions;
using EmployeeManagementSystemUI.Models;
using EmployeeManagementSystemUI.Repository;
using Microsoft.AspNetCore.Mvc;

namespace EmployeeManagementSystemUI.Controllers
{
    public class CUDController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IEmployeeRepository _employeeRepository;
        private readonly INotyfService _notyf;
        public CUDController(ILogger<HomeController> logger, 
            IEmployeeRepository employeeRepository,
            INotyfService notyf)
        {
            _logger = logger;
            _employeeRepository = employeeRepository;
            _notyf = notyf;
        }

        public IActionResult Index()
        {
            return View(new Employee());
        }

        [HttpGet]
        public async Task<IActionResult> SearchById(int id)
        {
            var response = await _employeeRepository.GetEmployeeByIdAsync(id);
            if(response.ID != 0)
                _notyf.Success($"Success");
            else
                _notyf.Error($"No data found!");
            return View("Index", response);
        }

        [HttpPost]
        public async Task<IActionResult> Add(Employee employee)
        {
            bool isAdded = await _employeeRepository.AddEmployeeAsync(employee);
            if (isAdded)
            {
                _notyf.Success($"Employee {employee.Name} is successfully added");
                return RedirectToAction("Index", "EmployeeList");
            }
            else
            {
                _notyf.Error($"Error on adding employee");
                return View("Index", employee);
            }
        }

        [HttpPost]
        public async Task<IActionResult> Update(Employee employee)
        {
            bool isUpdated = await _employeeRepository.UpdateEmployeeAsync(ViewBag.Id, employee);
            if (isUpdated)
            {
                _notyf.Success($"Employee {employee.Name} is successfully updated");
                return RedirectToAction("Index", "EmployeeList");
            }
            else
            {
                _notyf.Error(message: $"Error on updating employee");
                return View("Index", employee);
            }
        }

        [HttpPost]
        public async Task<IActionResult> Delete(int id)
        {
            var employee = await _employeeRepository.GetEmployeeByIdAsync(id);

            if (employee.ID == 0)
            {
                _notyf.Error($"Employee does not exist!");
                return View("Index", new Employee());
            }

            bool isDeleted = await _employeeRepository.DeleteEmployeeAsync(employee.ID);
            if (isDeleted)
            {
                _notyf.Information($"Employee {employee.Name} is successfully deleted");
                return RedirectToAction("Index", "EmployeeList");
            }
            else
            {
                _notyf.Error($"Error on deleting employee");
                return View("Index", new Employee());
            }
        }
    }
}