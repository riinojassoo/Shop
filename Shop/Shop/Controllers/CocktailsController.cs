using Microsoft.AspNetCore.Mvc;
using Shop.Core.ServiceInterface;
using Shop.Models.Cocktails;


namespace Shop.Controllers
{
    public class CocktailsController : Controller
    {
        private readonly ICocktailsServices _cocktailsServices;

        public CocktailsController
            (
            ICocktailsServices cocktailsServices
            )
        {
            _cocktailsServices = cocktailsServices;
        }
        public IActionResult Index()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> SearchCocktails(CocktailsIndexViewModel model)
        {
            if (string.IsNullOrWhiteSpace(model.strDrink))
            {
                ModelState.AddModelError("strDrink", "Please enter a cocktail name to search.");
                return View("Index", model);
            }

            // Fetch cocktails from the API
            var cocktails = await _cocktailsServices.CocktailsResult(model.strDrink);

            // Map the API results to the view model
            var results = cocktails.Select(c => new CocktailsIndexViewModel
            {
                idDrink = c.idDrink,
                strDrink = c.strDrink,
                strDrinkAlternate = c.strDrinkAlternate,
                strTags = c.strTags,
                strCategory = c.strCategory,
                strIBA = c.strIBA,
                strAlcoholic = c.strAlcoholic,
                strGlass = c.strGlass,
                strInstructions = c.strInstructions,
                strDrinkThumb = c.strDrinkThumb,
                strIngredient1 = c.strIngredient1,
                strIngredient2 = c.strIngredient2,
                strIngredient3 = c.strIngredient3,
                strIngredient4 = c.strIngredient4,
                strIngredient5 = c.strIngredient5,
                strIngredient6 = c.strIngredient6,
                strIngredient7 = c.strIngredient7,
                strIngredient8 = c.strIngredient8,
                strIngredient9 = c.strIngredient9,
                strIngredient10 = c.strIngredient10,
                strIngredient11 = c.strIngredient11,
                strIngredient12 = c.strIngredient12,
                strIngredient13 = c.strIngredient13,
                strIngredient14 = c.strIngredient14,
                strIngredient15 = c.strIngredient15,
                strMeasure1 = c.strMeasure1,
                strMeasure2 = c.strMeasure2,
                strMeasure3 = c.strMeasure3,
                strMeasure4 = c.strMeasure4,
                strMeasure5 = c.strMeasure5,
                strMeasure6 = c.strMeasure6,
                strMeasure7 = c.strMeasure7,
                strMeasure8 = c.strMeasure8,
                strMeasure9 = c.strMeasure9,
                strMeasure10 = c.strMeasure10,
                strMeasure11 = c.strMeasure11,
                strMeasure12 = c.strMeasure12,
                strMeasure13 = c.strMeasure13,
                strMeasure14 = c.strMeasure14,
                strMeasure15 = c.strMeasure15,
            }).ToList();

            // Return the results to a new view or the same view with results
            return View("Cocktail", results);
        }
    }
}