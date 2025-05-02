using BE_Productos.Data;
using BE_Productos.Models;
using Microsoft.EntityFrameworkCore;

namespace BE_Productos.Repository
{
    public class EmployeeRepository : IEmployeeRepository
    {
        private readonly ApplicationDBContext _context;

        public EmployeeRepository(ApplicationDBContext context)
        {
            _context = context;
        }
        public async Task<Employee> AddEmployee(Employee employee)
        {
            _context.Add(employee);
            await _context.SaveChangesAsync();
            return employee;
        }

        public async Task DeleteEmployee(Employee employee)
        {
            var deleteEmployee = await _context.Employees.FindAsync(employee.Id);
            if (deleteEmployee != null)
            {
                deleteEmployee.Active = false;
                await _context.SaveChangesAsync();
            }
        }

        public async Task<Employee> GetEmployee(int id)
        {
            return await _context.Employees.Where(employee => ((employee.Id == id) && employee.Active)).FirstOrDefaultAsync();
        }

        public async Task<List<Employee>> GetListEmployees()
        {
            return await _context.Employees.Where(employee => employee.Active).ToListAsync();
        }

        public async Task UpdateEmployee(Employee employee)
        {
            var updateEmployee = await _context.Employees.FindAsync(employee.Id);
            if (updateEmployee != null)
            {
                updateEmployee.Name = employee.Name;
                updateEmployee.Lastname = employee.Lastname;
                await _context.SaveChangesAsync();
            }
        }
    }
}
