using AutoMapper;
using Company.DAL.Entities;
using Company.PL.Models;

namespace Company.PL.Mapper
{
	public class EmployeeProfile : Profile
	{
		public EmployeeProfile() 
		{
			CreateMap<EmployeeViewModel, Employee>().ReverseMap();	
		}
	}
}
