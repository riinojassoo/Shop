﻿using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Shop.Core.Dto;
using Shop.Core.ServiceInterface;
using Shop.Data;
using Shop.Models.Kindergartens;

namespace Shop.Controllers
{
    public class KindergartensController : Controller
    {
        public readonly ShopContext _context;
        private readonly IKindergartenServices _kindergartenServices;
		private readonly IFileServices _fileServices;

        public KindergartensController
            (
            ShopContext context,
            IKindergartenServices kindergartenServices,
			IFileServices fileServices
            )
        {
            _context = context;
            _kindergartenServices = kindergartenServices;
			_fileServices = fileServices;
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
				Files = vm.Files,
				Image = vm.Image
						.Select(x => new FileToKindergartenDatabaseDto
						{
							Id = x.ImageId,
							ImageData = x.ImageData,
							ImageTitle = x.ImageTitle,
							KindergartenId = x.KindergartenId,
						}).ToArray()
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

			var photos = await _context.FileToKindergartenDatabases
				.Where(x => x.KindergartenId == id)
				.Select(y => new KindergartenImageViewModel
				{
					KindergartenId = y.Id,
					ImageId = y.Id,
					ImageData = y.ImageData,
					ImageTitle = y.ImageTitle,
					Image = string.Format("data:image/gif;base64,{0}", Convert.ToBase64String(y.ImageData))
				}).ToArrayAsync();

			var vm = new KindergartenCreateUpdateViewModel();
			
			vm.Id = kindergarten.Id;
			vm.GroupName = kindergarten.GroupName;
			vm.ChildrenCount = kindergarten.ChildrenCount;
			vm.KindergartenName = kindergarten.KindergartenName;
			vm.Teacher = kindergarten.Teacher;
			vm.CreatedAt = kindergarten.CreatedAt;
			vm.UpdatedAt = kindergarten.UpdatedAt;
			vm.Image.AddRange(photos);
			
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
				Files = vm.Files,
				Image = vm.Image
					.Select(x => new FileToKindergartenDatabaseDto
					{
						Id = x.ImageId,
						ImageData = x.ImageData,
						ImageTitle = x.ImageTitle,
						KindergartenId = x.KindergartenId,
					}).ToArray()
			};

			var result = await _kindergartenServices.Update(dto);
			if (result == null)
			{
				return RedirectToAction(nameof(Index));
			}
			return RedirectToAction(nameof(Index), vm);
		}

		[HttpGet]
		public async Task<IActionResult> Delete(Guid id)
		{
			var kindergarten = await _kindergartenServices.DetailAsync(id);

			if (kindergarten == null)
			{
				return NotFound();
			}

            var photos = await _context.FileToKindergartenDatabases
			   .Where(x => x.KindergartenId == id)
			   .Select(y => new KindergartenImageViewModel
			   {
				   KindergartenId = y.Id,
				   ImageId = y.Id,
				   ImageData = y.ImageData,
				   ImageTitle = y.ImageTitle,
				   Image = string.Format("data:image/gif;base64,{0}", Convert.ToBase64String(y.ImageData))
			   }).ToArrayAsync();


            var vm = new KindergartenDeleteViewModel();

			vm.Id = kindergarten.Id;
			vm.GroupName = kindergarten.GroupName;
			vm.ChildrenCount = kindergarten.ChildrenCount;
			vm.KindergartenName = kindergarten.KindergartenName;
			vm.Teacher = kindergarten.Teacher;
			vm.CreatedAt = kindergarten.CreatedAt;
			vm.UpdatedAt = kindergarten.UpdatedAt;
			vm.Image.AddRange(photos);

			return View(vm);
		}

		[HttpPost]
		public async Task<IActionResult> DeleteConfirmation(Guid id)
		{
			var kindergarten = await _kindergartenServices.Delete(id);

			if (kindergarten == null)
			{
				return RedirectToAction(nameof(Index));
			}

			return RedirectToAction(nameof(Index));
		}

		[HttpPost]
		public async Task<IActionResult> RemoveImage(KindergartenImageViewModel vm)
		{
			var dto = new FileToKindergartenDatabaseDto()
			{
				Id = vm.ImageId
			};

			var image = await _fileServices.RemoveImageFromKindergartenDatabase(dto);

			if (image == null)
			{
				return RedirectToAction(nameof(Index));
			}

			return RedirectToAction(nameof(Index));
		}


		[HttpGet]
        public async Task<IActionResult> Details(Guid id)
		{
			var kindergarten = await _kindergartenServices.DetailAsync(id);

			if (kindergarten == null)
			{
				return View("Error");
			}

			var photos = await _context.FileToKindergartenDatabases
				.Where(x => x.KindergartenId == id)
				.Select(y => new KindergartenImageViewModel
				{
					KindergartenId = y.Id,
					ImageId = y.Id,
					ImageData = y.ImageData,
					ImageTitle = y.ImageTitle,
					Image = string.Format("data:image/gif;base64,{0}", Convert.ToBase64String(y.ImageData))
				}).ToArrayAsync();

			var vm = new KindergartenDetailsViewModel();

			vm.Id = kindergarten.Id;
			vm.GroupName = kindergarten.GroupName;
            vm.ChildrenCount = kindergarten.ChildrenCount;
            vm.KindergartenName = kindergarten.KindergartenName;
            vm.Teacher = kindergarten.Teacher;
            vm.CreatedAt = kindergarten.CreatedAt;
            vm.UpdatedAt = kindergarten.UpdatedAt;
			vm.Image.AddRange(photos);

			return View(vm);
		}
	}
}
