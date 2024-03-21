using Company.BLL.Interfaces;
using Company.BLL.Repositories;
using Company.DAL.Entities;
using Microsoft.AspNetCore.DataProtection.Repositories;
using Microsoft.AspNetCore.Mvc;
//using Microsoft.AspNetCore.Razor.Language.Intermediate;

namespace Company.PL.Controllers
{
    public class DepartmentController : Controller
    {
		private readonly IUnitOfWork _unitOfWork;

		//private readonly IDepartmentRepository _departmentRepository;
		private readonly ILogger<DepartmentController> _logger;

        public DepartmentController(
            //IDepartmentRepository departmentRepository,
            IUnitOfWork unitOfWork,
            ILogger<DepartmentController> logger) 
        {
			_unitOfWork = unitOfWork;

			//_departmentRepository = departmentRepository;
			_logger = logger; 
        }

        public IActionResult Index()
        {
            var departments = _unitOfWork.DepartmentRepository.GetAll(); 
            return View(departments);
        }

        [HttpGet]
        public IActionResult Create()
        {
			return View(new Department());
        }

        [HttpPost]
        public IActionResult Create(Department department)
        {
            if(ModelState.IsValid)
            {
				_unitOfWork.DepartmentRepository.Add(department);
                _unitOfWork.Complete();

                return RedirectToAction(nameof(Index));// or RedirectToAction("index")
            }
            
            return View(department);
        }


        public IActionResult Details(int? id)
        {      
            try 
            {
                if (id == null)
                    return BadRequest();

                var department = _unitOfWork.DepartmentRepository.GetById(id);

                if (department is null)
                    return NotFound();


                return View(department);
            }
            catch(Exception ex) 
            {
                _logger.LogError(ex.Message);
                return RedirectToAction("Erorr", "Home");
            }

        }


        public IActionResult Update(int? id)
        {
            if (id == null)
                return NotFound();

            var department = _unitOfWork.DepartmentRepository.GetById(id);

            if (department is null)
                return NotFound();


            return View(department);
        }

        [HttpPost]
        public IActionResult Update(int? id, Department department)
        {
            if (id != department.Id)
                return NotFound();

            try
            {
                if(ModelState.IsValid)
                {
					_unitOfWork.DepartmentRepository.Update(department);
					_unitOfWork.Complete();

					return RedirectToAction(nameof(Index));
                }
            }

            catch(Exception ex)
            {
                throw new Exception(ex.Message);
            }
           


            return View(department);
        }


        public IActionResult Delete(int? id)
        {
            if (id == null || id == 0)
            {
                return NotFound();
            }

            var department = _unitOfWork.DepartmentRepository.GetById(id);

            if (department == null)
            {
                return NotFound();
            }

            return View(department);
        }

        [HttpPost]
        public IActionResult DeletePost(int? id)
        {
            if (id == null)
                return NotFound();

            var department = _unitOfWork.DepartmentRepository.GetById(id);

            if (department is null)
                return NotFound();

			_unitOfWork.DepartmentRepository.Delete(department);
			_unitOfWork.Complete();

			return RedirectToAction(nameof(Index));
            
        }


        //public IActionResult Delete(int? id)
        //{
        //    if (id == null)
        //        return NotFound();

        //    var department = _departmentRepository.GetById(id);

        //    if (department is null)
        //        return NotFound();

        //    _departmentRepository.Delete(department);
        //    return RedirectToAction(nameof(Index));

        //}




    }
}
