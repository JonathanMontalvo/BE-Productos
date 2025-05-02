using AutoMapper;
using BE_Productos.Models;
using BE_Productos.Models.DTO;
using BE_Productos.Repository;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BE_Productos.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EmployeeController : ControllerBase
    {
        private readonly IEmployeeRepository _employeeRepository;
        private readonly IMapper _mapper;

        public EmployeeController(IEmployeeRepository employeeRepository, IMapper mapper)
        {
            _employeeRepository = employeeRepository;
            _mapper = mapper;
        }

        [HttpGet()]
        public async Task<IActionResult> ListEmpleados()
        {
            try
            {
                var employees = await _employeeRepository.GetListEmployees();
                var employeesDto = _mapper.Map<List<EmployeeDTO>>(employees);
                return Ok(employeesDto);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetEmployee (int id)
        {
            try
            {
                var employee = await _employeeRepository.GetEmployee(id);
                var employeeDto = _mapper.Map<EmployeeDTO>(employee);
                if (employee == null)
                {
                    return NotFound($"There is no employee with the ID: {id}");
                }
                return Ok(employeeDto);
            }
            catch (Exception ex) {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost]
        public async Task<IActionResult> AddEmployee(EmployeeDTO employeeDto)
        {
            try
            {
                if(string.IsNullOrWhiteSpace(employeeDto.Name) || string.IsNullOrWhiteSpace(employeeDto.Lastname))
                {
                    return BadRequest("Some fields have incorrect values");
                }

                var employee = _mapper.Map<Employee>(employeeDto);
                employee.RegistrationDate = DateOnly.FromDateTime(DateTime.Now);
                employee.Active = true;

                var newEmployee = await _employeeRepository.AddEmployee(employee);
                var newEmployeeDto = _mapper.Map<EmployeeDTO>(newEmployee);
                return CreatedAtAction("GetEmployee", new { id = newEmployeeDto.Id }, newEmployeeDto);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateEmployee(int id, EmployeeDTO employeeDto)
        {
            try
            {
                if (id != employeeDto.Id || string.IsNullOrWhiteSpace(employeeDto.Name) || string.IsNullOrWhiteSpace(employeeDto.Lastname))
                {
                    return BadRequest("Some fields have incorrect values");
                }
                var existingEmployee = await _employeeRepository.GetEmployee(id);
                if (existingEmployee == null || existingEmployee.Active == false)
                {
                    return NotFound($"There is no employee with the ID: {id}");
                }

                var employee = _mapper.Map<Employee>(employeeDto);
                await _employeeRepository.UpdateEmployee(employee);
                return NoContent();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteEmployee(int id)
        {
            try
            {
                var employee = await _employeeRepository.GetEmployee(id);
                if (employee == null || employee.Active == false)
                {
                    return NotFound($"There is no employee with the ID: {id}");
                }
                await _employeeRepository.DeleteEmployee(employee);
                return NoContent();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
