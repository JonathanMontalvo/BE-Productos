using AutoMapper;
using BE_Productos.Models.DTO;

namespace BE_Productos.Models.Profiles
{
    public class EmployeeProfile:Profile
    {
        public EmployeeProfile() {
            CreateMap<Employee, EmployeeDTO>();
            CreateMap<EmployeeDTO, Employee>();
        }
    }
}
