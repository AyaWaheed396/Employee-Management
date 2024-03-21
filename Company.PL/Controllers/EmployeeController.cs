using AutoMapper;
using Company.BLL.Interfaces;
using Company.DAL.Entities;
using Company.PL.Helper;
using Company.PL.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace Company.PL.Controllers
{
	public class EmployeeController : Controller
	{
		private readonly IUnitOfWork _unitOfWork;
		private readonly ILogger<DepartmentController> _logger;
		private readonly IMapper _mapper;

		public EmployeeController(IUnitOfWork unitOfWork, ILogger<DepartmentController> logger, IMapper mapper)
		{
			_unitOfWork = unitOfWork;
			_logger = logger;
			_mapper = mapper;
		}

		public IActionResult Index(string SearchValue = "")
		{

            IEnumerable<Employee> employees;
			IEnumerable<EmployeeViewModel> employeeViewModel;

			if (string.IsNullOrEmpty(SearchValue))
			{
				employees = _unitOfWork.EmployeeRepository.GetAll();
				employeeViewModel = _mapper.Map<IEnumerable<EmployeeViewModel>>(employees);
			}
            else
			{
				employees = _unitOfWork.EmployeeRepository.Search(SearchValue);
				employeeViewModel = _mapper.Map<IEnumerable<EmployeeViewModel>>(employees);
			}
                

			return View(employeeViewModel);
            //if (string.IsNullOrEmpty(SearchValue))
            //{
            //	var employees = _unitOfWork.EmployeeRepository.GetAll();

            //	return View(employees);
            //}

            //else 
            //{
            //             var employees = _unitOfWork.EmployeeRepository.Search(SearchValue);

            //             return View(employees);
            //         }

        }

		[HttpGet]
		public IActionResult Create()
		{
			ViewBag.Departments = _unitOfWork.DepartmentRepository.GetAll();
			return View(new EmployeeViewModel());
		}

		[HttpPost]
		public IActionResult Create(EmployeeViewModel employeeViewModel)
		{
			//ModelState["Department"].ValidationState = ModelValidationState.Valid;
			if (ModelState.IsValid)
			{
                //manual mapping
    //            Employee employee = new Employee()
				//{
    //                Name = employeeViewModel.Name,
    //                Email = employeeViewModel.Email,
    //                Address = employeeViewModel.Address,
				//	DepartmentId = employeeViewModel.DepartmentId,
    //                HireDate = employeeViewModel.HireDate,
    //                Salary = employeeViewModel.Salary,
    //                IsActive = employeeViewModel.IsActive,                    
    //            };



				var employee = _mapper.Map<Employee>(employeeViewModel);
				employee.ImageUrl = DocumentSettings.UploadFile(employeeViewModel.Image, "Images");



                _unitOfWork.EmployeeRepository.Add(employee);
				_unitOfWork.Complete();

				return RedirectToAction(nameof(Index));// or RedirectToAction("index")
			}

			ViewBag.Departments = _unitOfWork.DepartmentRepository.GetAll();
			return View(employeeViewModel);
		}

		public IActionResult Update(int? id)
		{
			if (id == null)
				return NotFound();

			var employee = _unitOfWork.EmployeeRepository.GetById(id);
			var employeeViewModel = _mapper.Map< EmployeeViewModel >(employee);
			ViewBag.Departments = _unitOfWork.DepartmentRepository.GetAll();

			if (employee is null)
				return NotFound();


			return View(employeeViewModel);
		}

		//[HttpPost]
		//public IActionResult Update(int? id, EmployeeViewModel employeeViewModel)
		//{
		//	if (id != employeeViewModel.Id)
		//		return NotFound();

		//	try
		//	{

		//		//ModelState["Department"].ValidationState = ModelValidationState.Valid;
		//		if (ModelState.IsValid)
		//		{
		//			var employee = _mapper.Map<Employee>(employeeViewModel);
		//			if (employeeViewModel.Image != null)
		//			{
						
		//				employeeViewModel.ImageUrl = DocumentSettings.UploadFile(employeeViewModel.Image, "Images");
		//			}
		//			else
		//			{
						
		//				employeeViewModel.ImageUrl = employee.ImageUrl;
		//			}
					
		//			_unitOfWork.EmployeeRepository.Update(employee);
					
		//			_unitOfWork.Complete();

		//			return RedirectToAction(nameof(Index));
		//		}

		//	}

		//	catch (Exception ex)
		//	{
		//		throw new Exception(ex.Message);
		//	}


		//	ViewBag.Departments = _unitOfWork.DepartmentRepository.GetAll();
		//	return View(employeeViewModel);
		//}



		[HttpPost]
		public IActionResult Update(int? id, EmployeeViewModel employeeViewModel)
		{
			if (id != employeeViewModel.Id)
				return NotFound();

			try
			{
				//ModelState["Department"].ValidationState = ModelValidationState.Valid;
				if (ModelState.IsValid)
				{
					var employee = _mapper.Map<Employee>(employeeViewModel);
					employee.ImageUrl = DocumentSettings.UploadFile(employeeViewModel.Image, "Images");

					_unitOfWork.EmployeeRepository.Update(employee);
					_unitOfWork.Complete();

					return RedirectToAction(nameof(Index));
				}

			}

			catch (Exception ex)
			{
				throw new Exception(ex.Message);
			}


			ViewBag.Departments = _unitOfWork.DepartmentRepository.GetAll();
			return View(employeeViewModel);
		}



		public IActionResult Details(int? id)
		{
			try
			{
				if (id == null)
					return BadRequest();

				var employee = _unitOfWork.EmployeeRepository.GetById(id);
				var employeeViewModel = _mapper.Map<EmployeeViewModel>(employee);

				ViewBag.Departments = _unitOfWork.DepartmentRepository.GetAll();

				if (employee is null)
					return NotFound();


				return View(employeeViewModel);
			}
			catch (Exception ex)
			{
				_logger.LogError(ex.Message);
				return RedirectToAction("Erorr", "Home");
			}

		}

		public IActionResult Delete(int? id)
		{
			if (id == null || id == 0)
			{
				return NotFound();
			}

			var employee = _unitOfWork.EmployeeRepository.GetById(id);
			var employeeViewModel = _mapper.Map<EmployeeViewModel>(employee);
			ViewBag.Departments = _unitOfWork.DepartmentRepository.GetAll();

			if (employee == null)
			{
				return NotFound();
			}

			return View(employeeViewModel);
		}

		[HttpPost]
		public IActionResult DeletePost(int? id)
		{
			if (id == null)
				return NotFound();

			var employee = _unitOfWork.EmployeeRepository.GetById(id);
			var employeeViewModel = _mapper.Map<EmployeeViewModel>(employee);
		    DocumentSettings.DeleteFile(employeeViewModel.ImageUrl, "Images");

			if (employee is null)
				return NotFound();

			_unitOfWork.EmployeeRepository.Delete(employee);
			_unitOfWork.Complete();

			return RedirectToAction(nameof(Index));

		}
	}
}
