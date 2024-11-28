﻿using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Shop.ApplicationServices.Services;
using Shop.Core.Domain;
using Shop.Core.Dto;
using Shop.Core.ServiceInterface;
using Shop.Data;
using Shop.Models.Kindergartens;
using Shop.Models.RealEstates;

namespace Shop.Controllers
{
	public class RealEstatesController : Controller
	{
		public readonly ShopContext _context;
		private readonly IRealEstateServices _realEstateServices;
		private readonly IFileServices _fileServices;

		public RealEstatesController
			(
			ShopContext context,
			IRealEstateServices realEstateServices,
			IFileServices fileServices
			)
		{
			_context = context;
			_realEstateServices = realEstateServices;
			_fileServices = fileServices;
		}

		public IActionResult Index()
		{
			var result = _context.RealEstates
				.Select(x => new RealEstateIndexViewModel
				{
					Id = x.Id,
					Size = x.Size,
					Location = x.Location,
					RoomNumber = x.RoomNumber,
					BuildingType = x.BuildingType

				});

			return View(result);
		}

		[HttpGet]
		public IActionResult Create()
		{
			RealEstateCreateUpdateViewModel realEstates = new();

			return View("CreateUpdate", realEstates);
		}

		[HttpPost]
		public async Task<IActionResult> Create(RealEstateCreateUpdateViewModel vm)
		{
			var dto = new RealEstateDto()
			{
				Id = vm.Id,
				Size = vm.Size,
				Location = vm.Location,
				RoomNumber = vm.RoomNumber,
				BuildingType = vm.BuildingType,
				CreatedAt = vm.CreatedAt,
				ModifiedAt = vm.ModifiedAt,
				Files = vm.Files, 
				Image = vm.Image
					.Select(x => new FileToDatabaseDto
					{
						Id = x.ImageId,
						ImageData = x.ImageData,
						ImageTitle = x.ImageTitle,
						realEstateId = x.RealEstateId
					}).ToArray()
			};

			var result = await _realEstateServices.Create(dto);

			if (result == null)
			{
				return RedirectToAction(nameof(Index));
			}

			return RedirectToAction(nameof(Index), vm);
		}

        [HttpGet]
        public async Task<IActionResult> Details(Guid id)
        {
            var realEstates = await _realEstateServices.GetAsync(id);

            if (realEstates == null)
            {
                return View("Error");
            }

			var photos = await _context.FileToDatabases
				.Where(x => x.RealEstateId == id)
				.Select(y => new RealEstateImageViewModel
				{
					RealEstateId = y.Id,
					ImageId = y.Id,
					ImageData = y.ImageData,
					ImageTitle = y.ImageTitle,
					Image = string.Format("data:image/gif;base64,{0}", Convert.ToBase64String(y.ImageData))
				}).ToArrayAsync();

            var vm = new RealEstatesDetailsViewModel();

            vm.Id = realEstates.Id;
            vm.Size = realEstates.Size;
            vm.Location = realEstates.Location;
            vm.RoomNumber = realEstates.RoomNumber;
            vm.BuildingType = realEstates.BuildingType;
            vm.CreatedAt = realEstates.CreatedAt;
            vm.ModifiedAt = realEstates.ModifiedAt;
			vm.Image.AddRange(photos);

            return View(vm);
        }

        [HttpGet]
		public async Task<IActionResult> Update(Guid id)
		{
			var realEstates = await _realEstateServices.GetAsync(id);
			if (realEstates == null)
			{
				return NotFound();
			}

            var photos = await _context.FileToDatabases
                .Where(x => x.RealEstateId == id)
                .Select(y => new RealEstateImageViewModel
                {
                    RealEstateId = y.Id,
                    ImageId = y.Id,
                    ImageData = y.ImageData,
                    ImageTitle = y.ImageTitle,
                    Image = string.Format("data:image/gif;base64,{0}", Convert.ToBase64String(y.ImageData))
                }).ToArrayAsync();

            var vm = new RealEstateCreateUpdateViewModel();

			vm.Id = realEstates.Id;
			vm.Size = realEstates.Size;
			vm.Location = realEstates.Location;
			vm.RoomNumber =	realEstates.RoomNumber;
			vm.BuildingType = realEstates.BuildingType;
			vm.CreatedAt = realEstates.CreatedAt;
			vm.ModifiedAt = realEstates.ModifiedAt;
			vm.Image.AddRange(photos);

			return View("CreateUpdate", vm);
		}


		[HttpPut]
		public async Task<IActionResult> Update(RealEstateCreateUpdateViewModel vm)
		{
			var dto = new RealEstateDto()
			{
                Id = vm.Id,
                Size = vm.Size,
                Location = vm.Location,
                RoomNumber = vm.RoomNumber,
                BuildingType = vm.BuildingType,
                CreatedAt = vm.CreatedAt,
                ModifiedAt = vm.ModifiedAt,
				Files = vm.Files,
				Image = vm.Image
					.Select(x => new FileToDatabaseDto
					{
						Id = x.ImageId,
						ImageData = x.ImageData,
						ImageTitle = x.ImageTitle,
						realEstateId = x.RealEstateId,
					}).ToArray()
            };
			var result = await _realEstateServices.Update(dto);
			if (result == null)
			{
				return RedirectToAction(nameof(Index));
			}
			return RedirectToAction(nameof(Index), vm);
		}

        [HttpGet]
        public async Task<IActionResult> Delete(Guid id)
		{
			var realEstates = await _realEstateServices.GetAsync(id);
			
			if (realEstates == null)
            {
                return NotFound();
            }

			var photos = await _context.FileToDatabases
			   .Where(x => x.RealEstateId == id)
			   .Select(y => new RealEstateImageViewModel
			   {
				   RealEstateId = y.Id,
				   ImageId = y.Id,
				   ImageData = y.ImageData,
				   ImageTitle = y.ImageTitle,
				   Image = string.Format("data:image/gif;base64,{0}", Convert.ToBase64String(y.ImageData))
			   }).ToArrayAsync();

			var vm = new RealEstateDeleteViewModel();
            vm.Id = realEstates.Id;
            vm.Size = realEstates.Size;
            vm.Location = realEstates.Location;
            vm.RoomNumber = realEstates.RoomNumber;
            vm.BuildingType = realEstates.BuildingType;
            vm.CreatedAt = realEstates.CreatedAt;
            vm.ModifiedAt = realEstates.ModifiedAt;
			vm.Image.AddRange(photos);

            return View(vm);
        }
        [HttpDelete]
        public async Task<IActionResult> DeleteConfirmation(Guid id)
        {
            var realEstates = await _realEstateServices.Delete(id);
            if (realEstates == null)
            {
                return RedirectToAction(nameof(Index));
            }
            return RedirectToAction(nameof(Index));
        }

		[HttpDelete]
		public async Task<IActionResult> RemoveImage(RealEstateImageViewModel vm)
		{
			var dto = new FileToDatabaseDto()
			{
				Id = vm.ImageId
			};

			var image = await _fileServices.RemoveImageFromDatabase(dto);

			if (image == null)
			{
				return RedirectToAction(nameof(Index));
			}

			return RedirectToAction(nameof(Index));
		}
    }
}
