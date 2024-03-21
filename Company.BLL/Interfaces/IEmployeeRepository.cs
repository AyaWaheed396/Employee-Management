using Company.DAL.Entities;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Company.BLL.Interfaces
{
    public interface IEmployeeRepository : IGenericRepository<Employee>
    {
        IEnumerable<Employee> GetEmployeesByDepartmentName(String DepartmentName);
		IEnumerable<Employee> Search(String EmployeeName);


		//Employee GetById(int id);
		//IEnumerable<Employee> GetAll();
		//int Add(Employee employee);
		//int Update(Employee employee);
		//int Delete(Employee employee);
	}
}
