using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.FileProviders;
using Shop.ApplicationServices.Services;
using Shop.Core.ServiceInterface;
using Shop.Data;

namespace Shop
{
	public class Program
	{
		public static void Main(string[] args)
		{
			var builder = WebApplication.CreateBuilder(args);

			// Add services to the container.
			builder.Services.AddControllersWithViews();

			builder.Services.AddScoped<ISpaceshipServices, SpaceshipServices>();

			builder.Services.AddScoped<IFileServices, FileServices>();

			builder.Services.AddScoped<IKindergartenServices, KindergartenServices>();

			builder.Services.AddScoped<IRealEstateServices, RealEstateServices>();
			builder.Services.AddScoped<IWeatherForecastServices, WeatherForecastServices>();
			builder.Services.AddScoped<IChuckNorrisServices, ChuckNorrisServices>();

			builder.Services.AddDbContext<ShopContext>(options =>
				options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));


			var app = builder.Build();

			// Configure the HTTP request pipeline.
			if (!app.Environment.IsDevelopment())
			{
				app.UseExceptionHandler("/Home/Error");
				// The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
				app.UseHsts();
			}

			app.UseHttpsRedirection();
			app.UseStaticFiles();

			app.UseStaticFiles(new StaticFileOptions
			{
				FileProvider = new PhysicalFileProvider
				(Path.Combine(builder.Environment.ContentRootPath, "multipleFileUpload")),
				RequestPath = "/multipleFileUpload"
			});

			app.UseRouting();

			app.UseAuthorization();

			app.MapControllerRoute(
				name: "default",
				pattern: "{controller=Home}/{action=Index}/{id?}");

			app.Run();
		}
	}
}