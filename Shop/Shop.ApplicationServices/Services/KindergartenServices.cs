﻿using Microsoft.EntityFrameworkCore;
using Shop.Core.Domain;
using Shop.Core.Dto;
using Shop.Core.ServiceInterface;
using Shop.Data;

namespace Shop.ApplicationServices.Services
{
    public class KindergartenServices : IKindergartenServices
    {
		private readonly ShopContext _context;
		private readonly IFileServices _fileServices;

		public KindergartenServices
			(
				ShopContext context,
				IFileServices fileServices
			)
		{
			_context = context;
			_fileServices = fileServices;
		}

		public async Task<Kindergarten> Create(KindergartenDto dto)
		{
			Kindergarten kindergarten = new();

			kindergarten.Id = Guid.NewGuid();
			kindergarten.GroupName = dto.GroupName;
			kindergarten.ChildrenCount = dto.ChildrenCount;
			kindergarten.KindergartenName = dto.KindergartenName;
			kindergarten.Teacher = dto.Teacher;
			kindergarten.CreatedAt = DateTime.Now;
			kindergarten.UpdatedAt = DateTime.Now;

			if (dto.Files != null)
			{
				_fileServices.UploadFilesToKindergartenDatabase(dto, kindergarten);
			}

			await _context.Kindergartens.AddAsync(kindergarten);
			await _context.SaveChangesAsync();

			return kindergarten;
		}



		public async Task<Kindergarten> DetailAsync(Guid id)
		{
			var result = await _context.Kindergartens
				.FirstOrDefaultAsync(x => x.Id == id);

			return result;
		}


		public async Task<Kindergarten> Update(KindergartenDto dto)
		{
			Kindergarten domain = new();

			domain.Id = dto.Id;
			domain.GroupName = dto.GroupName;
			domain.ChildrenCount = dto.ChildrenCount;
			domain.KindergartenName = dto.KindergartenName;
			domain.Teacher = dto.Teacher;
			domain.CreatedAt = dto.CreatedAt;
			domain.UpdatedAt = DateTime.Now;

            if (dto.Files != null)
            {
                _fileServices.UploadFilesToKindergartenDatabase(dto, domain);
            }

            _context.Kindergartens.Update(domain);
			await _context.SaveChangesAsync();

			return domain;
		}

		public async Task<Kindergarten> Delete(Guid id)
		{
			var kindergarten = await _context.Kindergartens
				.FirstOrDefaultAsync(x => x.Id == id);

            var images = await _context.FileToKindergartenDatabases
                .Where(x => x.KindergartenId == id)
                .Select(y => new FileToKindergartenDatabaseDto
                {
                    Id = y.Id,
                    ImageTitle = y.ImageTitle,
                    KindergartenId = y.KindergartenId
                }).ToArrayAsync();

            await _fileServices.RemoveAllImagesFromKindergartenDatabase(images);

            _context.Kindergartens.Remove(kindergarten);
			await _context.SaveChangesAsync();

			return kindergarten;

		}
	}
}
