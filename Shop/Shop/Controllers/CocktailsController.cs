using Microsoft.AspNetCore.Mvc;
using Shop.Core.Dto.Cocktails;
using Shop.Core.ServiceInterface;
using Shop.Models.Cocktails;


//namespace Shop.Controllers
//{
//    public class CocktailsController : Controller
//    {
//        private readonly ICocktailsServices _cocktailsServices;

//        public CocktailsController
//            (
//            ICocktailsServices cocktailsServices
//            )
//        {
//            _cocktailsServices = cocktailsServices;
//        }
//        public async Task<IActionResult> Index()
//        {
//            //CocktailsDto dto = new();

//            var dtoList = await _cocktailsServices.CocktailsResult();

//            if (dtoList == null || !dtoList.Any())
//            {
//                ModelState.AddModelError(string.Empty, "No cocktails found.");
//                return View("Error");
//            }

//            //var vmList = dtoList.Select(dto => new CocktailsSearchViewModel
//            //{
//            //    strDrink = dto.strDrink,
//            //    strDrinkAlternate = dto.strDrinkAlternate,
//            //    strTags = dto.strTags,
//            //    strCategory = dto.strCategory,
//            //    strIBA = dto.strIBA,
//            //    strAlcoholic = dto.strAlcoholic,
//            //    strGlass = dto.strGlass,
//            //    strInstructions = dto.strInstructions,
//            //    strDrinkThumb = dto.strDrinkThumb,
//            //    strIngredient1 = dto.strIngredient1,
//            //    strIngredient2 = dto.strIngredient2,
//            //    strIngredient3 = dto.strIngredient3,
//            //    strIngredient4 = dto.strIngredient4,
//            //    strIngredient5 = dto.strIngredient5,
//            //    strIngredient6 = dto.strIngredient6,
//            //    strIngredient7 = dto.strIngredient7,
//            //    strIngredient8 = dto.strIngredient8,
//            //    strIngredient9 = dto.strIngredient9,
//            //    strIngredient10 = dto.strIngredient10,
//            //    strIngredient11 = dto.strIngredient11,
//            //    strIngredient12 = dto.strIngredient12,
//            //    strIngredient13 = dto.strIngredient13,
//            //    strIngredient14 = dto.strIngredient14,
//            //    strIngredient15 = dto.strIngredient15,
//            //    strMeasure1 = dto.strMeasure1,
//            //    strMeasure2 = dto.strMeasure2,
//            //    strMeasure3 = dto.strMeasure3,
//            //    strMeasure4 = dto.strMeasure4,
//            //    strMeasure5 = dto.strMeasure5,
//            //    strMeasure6 = dto.strMeasure6,
//            //    strMeasure7 = dto.strMeasure7,
//            //    strMeasure8 = dto.strMeasure8,
//            //    strMeasure9 = dto.strMeasure9,
//            //    strMeasure10 = dto.strMeasure10,
//            //    strMeasure11 = dto.strMeasure11,
//            //    strMeasure12 = dto.strMeasure12,
//            //    strMeasure13 = dto.strMeasure13,
//            //    strMeasure14 = dto.strMeasure14,
//            //    strMeasure15 = dto.strMeasure15,
//            //}).ToList();


//            return View(dtoList);
//        }
//    }
//}

//teine
//namespace Shop.Controllers
//{
//    public class CocktailsController : Controller
//    {
//        private readonly ICocktailsServices _cocktailsServices;

//        public CocktailsController(ICocktailsServices cocktailsServices)
//        {
//            _cocktailsServices = cocktailsServices;
//        }

//        // This action will handle the search form submission and call the service
//        [HttpPost]
//        public async Task<IActionResult> SearchCocktails(CocktailsIndexViewModel viewModel)
//        {
//            if (!string.IsNullOrEmpty(viewModel.strDrink))
//            {
//                var cocktails = await _cocktailsServices.CocktailsResult(viewModel.strDrink);

//                if (cocktails != null && cocktails.Any())
//                {
//                    // Map the first cocktail (or handle multiple if necessary)
//                    var cocktailDto = cocktails.FirstOrDefault();

//                    var searchResult = new CocktailsSearchViewModel
//                    {
//                        idDrink = cocktailDto?.idDrink,
//                        strDrink = cocktailDto?.strDrink,
//                        strTags = cocktailDto?.strTags,
//                        strCategory = cocktailDto?.strCategory,
//                        strIBA = cocktailDto?.strIBA,
//                        strAlcoholic = cocktailDto?.strAlcoholic,
//                        strGlass = cocktailDto?.strGlass,
//                        strInstructions = cocktailDto?.strInstructions,
//                        strDrinkThumb = cocktailDto?.strDrinkThumb,
//                        Ingredients = GetIngredientsAndMeasures(cocktailDto)
//                    };

//                    return View("CocktailDetails", searchResult); // Redirect to the details page
//                }
//            }

//            return View(viewModel); // In case no cocktail is found or if the search string is empty
//        }

//        // This function returns a list of ingredients with their measures
//        private List<string> GetIngredientsAndMeasures(CocktailsDto cocktail)
//        {
//            var ingredients = new List<string>();

//            for (int i = 1; i <= 15; i++)
//            {
//                var ingredientProperty = cocktail.GetType().GetProperty($"strIngredient{i}");
//                var measureProperty = cocktail.GetType().GetProperty($"strMeasure{i}");

//                var ingredient = ingredientProperty?.GetValue(cocktail);
//                var measure = measureProperty?.GetValue(cocktail);

//                if (ingredient != null && !string.IsNullOrEmpty(ingredient.ToString()))
//                {
//                    ingredients.Add($"{ingredient} - {measure ?? "To taste"}");
//                }
//            }

//            return ingredients;
//        }
//    }
//}

namespace Shop.Controllers
{
    public class CocktailsController : Controller
    {
        private readonly ICocktailsServices _cocktailsService;

        public CocktailsController(ICocktailsServices cocktailsService)
        {
            _cocktailsService = cocktailsService;
        }

        public async Task<IActionResult> CocktailDetails(string id)
        {
            // Fetch the cocktail data by ID
            var cocktail = await _cocktailsService.GetCocktailById(id);

            // If no cocktail is found, return 404
            if (cocktail == null)
            {
                return NotFound();
            }

            // Initialize the ViewModel
            var model = new CocktailsSearchViewModel
            {
                idDrink = cocktail.idDrink,
                strDrink = cocktail.strDrink,
                strTags = cocktail.strTags,
                strCategory = cocktail.strCategory,
                strIBA = cocktail.strIBA,
                strAlcoholic = cocktail.strAlcoholic,
                strGlass = cocktail.strGlass,
                strInstructions = cocktail.strInstructions,
                strDrinkThumb = cocktail.strDrinkThumb,
                Ingredients = new List<string>()
            };

            // Loop through the ingredients and measures (1 to 15)
            for (int i = 1; i <= 15; i++)
            {
                // Dynamically get the ingredient and measure properties
                var ingredient = cocktail.GetType().GetProperty($"strIngredient{i}")?.GetValue(cocktail, null);
                var measure = cocktail.GetType().GetProperty($"strMeasure{i}")?.GetValue(cocktail, null);

                // Add the ingredient and measure to the Ingredients list
                if (ingredient != null && !string.IsNullOrEmpty(ingredient.ToString()))
                {
                    if (measure == null || string.IsNullOrEmpty(measure.ToString()))
                    {
                        model.Ingredients.Add(ingredient.ToString());
                    }
                    else
                    {
                        model.Ingredients.Add($"{ingredient} - {measure}");
                    }
                }
            }

            // Return the populated ViewModel to the view
            return View(model);
        }
    }

}