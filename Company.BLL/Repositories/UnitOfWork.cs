using Company.BLL.Interfaces;
using Company.DAL.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace Company.BLL.Repositories
{
	public class UnitOfWork : IUnitOfWork
	{
		private readonly AppDbContext _Context;
		public IEmployeeRepository EmployeeRepository { get; set ; }
		public IDepartmentRepository DepartmentRepository { get ; set; }


		public UnitOfWork(AppDbContext context) 
		{
			_Context = context;
			EmployeeRepository = new EmployeeRepository(context);
			DepartmentRepository = new DepartmentRepository(context);
		}

		public int Complete()
		{
			return _Context.SaveChanges();
		}
	}
}
