using EmployeeManagementSystemUI.Repository;
using Microsoft.AspNetCore.Mvc;

namespace EmployeeManagementSystemUI.Controllers
{
    public class EmployeeListController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IEmployeeRepository _employeeRepository;

        public EmployeeListController(ILogger<HomeController> logger, IEmployeeRepository employeeRepository)
        {
            _logger = logger;
            _employeeRepository = employeeRepository;
        }

        public async Task<IActionResult> Index(string sortOrder, string searchString)
        {
            ViewBag.NameSortParm = String.IsNullOrEmpty(sortOrder) ? "name_desc" : "";
            ViewBag.SalarySortParm = String.IsNullOrEmpty(sortOrder) ? "salary_desc" : "";

            var employees = await _employeeRepository.GetEmployeesAsync();

            if (!String.IsNullOrEmpty(searchString))
            {
                employees = employees.Where(w => w.Name.Contains(searchString))
                    .ToList();
            }

            switch (sortOrder)
            {
                case "name_desc":
                    employees = employees.OrderByDescending(s => s.Name).ToList();
                    break;
                case "salary_desc":
                    employees = employees.OrderByDescending(s => s.Salary).ToList();
                    break;
                default:
                    employees = employees.OrderBy(s => s.Name).ToList();
                    break;
            }

            return View(employees);
        }
    }
}
