using Microsoft.AspNetCore.Mvc;
using Shop.ApplicationServices.Services;
using Shop.Core.Dto;
using Shop.Core.ServiceInterface;
using Shop.Data;
using Shop.Models.Kindergartens;
using Shop.Models.Spaceships;

namespace Shop.Controllers
{
    public class KindergartensController : Controller
    {
        public readonly ShopContext _context;
        private readonly IKindergartenServices _kindergartenServices;

        public KindergartensController
            (
            ShopContext context,
            IKindergartenServices kindergartenServices
            )
        {
            _context = context;
            _kindergartenServices = kindergartenServices;
        }

        public IActionResult Index()
        {
            var result = _context.Kindergartens
                .Select(x => new KindergartensIndexViewModel
                {
                    Id = x.Id,
                    GroupName = x.GroupName,
                    ChildrenCount = x.ChildrenCount,
                    KindergartenName = x.KindergartenName,
                    Teacher = x.Teacher

                });

            return View(result);
        }

		[HttpGet]
		public IActionResult Create()
		{
			KindergartenCreateUpdateViewModel result = new();

			return View("CreateUpdate", result);
		}

		[HttpPost]
		public async Task<IActionResult> Create(KindergartenCreateUpdateViewModel vm)
		{
			var dto = new KindergartenDto()
			{
				Id = vm.Id,
				GroupName = vm.GroupName,
				ChildrenCount = vm.ChildrenCount,
				KindergartenName = vm.KindergartenName,
				Teacher = vm.Teacher,
				CreatedAt = vm.CreatedAt,
				UpdatedAt = vm.UpdatedAt,
			};

			var result = await _kindergartenServices.Create(dto);

			if (result == null)
			{
				return RedirectToAction(nameof(Index));
			}

			return RedirectToAction(nameof(Index), vm);
		}

		[HttpGet]
		public async Task<IActionResult> Update(Guid id)
		{
			var kindergarten = await _kindergartenServices.DetailAsync(id);
			if (kindergarten == null)
			{
				return NotFound();
			}
			var vm = new KindergartenCreateUpdateViewModel();
			
			vm.Id = kindergarten.Id;
			vm.GroupName = kindergarten.GroupName;
			vm.ChildrenCount = kindergarten.ChildrenCount;
			vm.KindergartenName = kindergarten.KindergartenName;
			vm.Teacher = kindergarten.Teacher;
			vm.CreatedAt = kindergarten.CreatedAt;
			vm.UpdatedAt = kindergarten.UpdatedAt;
			
			return View("CreateUpdate", vm);
		}


		[HttpPost]
		public async Task<IActionResult> Update(KindergartenCreateUpdateViewModel vm)
		{
			var dto = new KindergartenDto()
			{
				Id = vm.Id,
				GroupName = vm.GroupName,
				ChildrenCount = vm.ChildrenCount,
				KindergartenName = vm.KindergartenName,
				Teacher = vm.Teacher,
				CreatedAt = vm.CreatedAt,
				UpdatedAt = vm.UpdatedAt,
			};
			var result = await _kindergartenServices.Update(dto);
			if (result == null)
			{
				return RedirectToAction(nameof(Index));
			}
			return RedirectToAction(nameof(Index), vm);
		}


		[HttpGet]
        public async Task<IActionResult> Details(Guid id)
		{
			var kindergarten = await _kindergartenServices.DetailAsync(id);

			if (kindergarten == null)
			{
				return View("Error");
			}

			var vm = new KindergartenDetailsViewModel();

			vm.Id = kindergarten.Id;
			vm.GroupName = kindergarten.GroupName;
            vm.ChildrenCount = kindergarten.ChildrenCount;
            vm.KindergartenName = kindergarten.KindergartenName;
            vm.Teacher = kindergarten.Teacher;
            vm.CreatedAt = kindergarten.CreatedAt;
            vm.UpdatedAt = kindergarten.UpdatedAt;

			return View(vm);
		}
	}
}
