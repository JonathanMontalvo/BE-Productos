using BE_Productos.Models;

namespace BE_Productos.Repository
{
    public interface IEmployeeRepository
    {
        Task<Employee> AddEmployee(Employee employee);
        Task<List<Employee>> GetListEmployees();
        Task<Employee> GetEmployee(int id);
        Task UpdateEmployee(Employee employee);
        Task DeleteEmployee(Employee employee);
    }
}
