using BlazorServerApp.Pages;
using BlazorServerAppDB.Data.Diet;
using BlazorServerAppDB.Data.Exercises;
using Microsoft.EntityFrameworkCore;
using NuGet.Packaging;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BlazorServerApp.Data
{
    public class DietService
    {
        private readonly ShapeShiftDietContext _context;
        public DietService(ShapeShiftDietContext context)
        {
            _context = context;
        }

        public async Task<List<QuestionDiet>> GetQuestionsAsync()
        {
            return await _context.QuestionDiet
                .Where(x => x.IsActive == true)
                .Include(x => x.PossibleAnswersDiet)
                .AsNoTracking()
                .ToListAsync();
        }

        public async Task SaveUserAnswerAsync(UserAnswersDiet answer)
        {
            _context.UserAnswersDiet.Add(answer);
            await _context.SaveChangesAsync();
        }

        public async Task SaveUserAnswerCaloriesAsync(UserAnswersDietCalories answerCalories)
        {
            _context.UserAnswersDietCalories.Add(answerCalories);
            await _context.SaveChangesAsync();
        }

        public async Task ClearUserAnswersDietAsync(string strCurrentUser)
        {
            var userDiet = await _context.UserAnswersDiet
                .Where(ua => ua.UserName == strCurrentUser)
                .ToListAsync();

            var userDietCalories = await _context.UserAnswersDietCalories
                .Where(ua => ua.UserName == strCurrentUser)
                .ToListAsync();

            _context.UserAnswersDiet.RemoveRange(userDiet);
            _context.UserAnswersDietCalories.RemoveRange(userDietCalories);
            await _context.SaveChangesAsync();
        }

        public async Task ClearUserDietPlanAsync(string strCurrentUser)
        {
            var userDietPlan = await _context.UserMealSets
                .Where(ua => ua.UserName == strCurrentUser)
                .ToListAsync();

            _context.UserMealSets.RemoveRange(userDietPlan);
            await _context.SaveChangesAsync();
        }

        public async Task<List<UserMealSets>> GetUserMealSetsAsync(string strCurrentUser)
        {
            return await _context.UserMealSets
                .Where(ues => ues.UserName == strCurrentUser)
                .Include(ues => ues.Breakfast)
                    .ThenInclude(meal => meal.Recipes)
                .Include(ues => ues.Lunch)
                    .ThenInclude(meal => meal.Recipes)
                .Include(ues => ues.Dinner)
                    .ThenInclude(meal => meal.Recipes)
                .OrderBy(ues => ues.MealDay)
                .ToListAsync();
        }

        public async Task<bool> HasUserSubmittedAnswersOrDiet(string userName)
        {
            var hasAnswers = await _context.UserAnswersDiet.AnyAsync(ua => ua.UserName == userName);
            return hasAnswers;
        }

        public async Task<int?> GetUserCaloricRequirement(string userName)
        {
            return await _context.UserAnswersDietCalories
                .Where(u => u.UserName == userName)
                .Select(u => (int?)u.Answer)
                .FirstOrDefaultAsync();
        }

        //Algorytm losowania zestawu dań

        public async Task<List<Meals>> GenerateDietPlanForUser(string strCurrentUser)
        {
            var userAnswers = await _context.UserAnswersDiet
                            .Where(ua => ua.UserName == strCurrentUser)
                            .Include(ua => ua.Question)
                            .Include(ua => ua.ChosenAnswer)
                            .ToListAsync();

            string dietGoals = userAnswers
                .Where(ua => ua.Question.Category == "Goals")
                .Select(ua => MapDietGoals(ua.ChosenAnswer.AnswerText))
                .FirstOrDefault();

            string allergies = userAnswers
                .Where(ua => ua.Question.Category == "Allergy")
                .Select(ua => MapAllergies(ua.ChosenAnswer.AnswerText))
                .FirstOrDefault();

            string dietType = userAnswers
                .Where(ua => ua.Question.Category == "Type")
                .Select(ua => MapDietType(ua.ChosenAnswer.AnswerText))
                .FirstOrDefault();

            string foodType = userAnswers
                .Where(ua => ua.Question.Category == "Food Type")
                .Select(ua => MapFoodType(ua.ChosenAnswer.AnswerText))
                .FirstOrDefault();

            string productType = userAnswers
                .Where(ua => ua.Question.Category == "Product Type")
                .Select(ua => MapProductType(ua.ChosenAnswer.AnswerText))
                .FirstOrDefault();

            string regionTypeFood = userAnswers
                .Where(ua => ua.Question.Category == "Region Type Food")
                .Select(ua => MapRegionTypeFood(ua.ChosenAnswer.AnswerText))
                .FirstOrDefault();

            string excludedProducts = userAnswers
                .Where(ua => ua.Question.Category == "Excluded Products")
                .Select(ua => MapExcludedProducts(ua.ChosenAnswer.AnswerText))
                .FirstOrDefault();

            var meals = await _context.Meals
                .Where(m => (dietGoals == null || m.Goals == dietGoals) &&
                            (allergies == null || !m.Allergy.Equals(allergies)) &&
                            (dietType == null || m.Type == dietType) &&
                            (foodType == null || m.FoodType == foodType) &&
                            (productType == null || m.ProductType == productType) &&
                            (regionTypeFood == null || m.RegionTypeFood == regionTypeFood) &&
                            (excludedProducts == null || !m.ExcludedProducts.Equals(excludedProducts)))
                .ToListAsync();

            int? caloricRequirement = await GetUserCaloricRequirement(strCurrentUser);

            Random rng = new Random();
            int mealDays = 7;  
            List<Meals> userMealSets = new List<Meals>();


            for (int day = 1; day <= mealDays; day++)
            {
                var shuffledMeals = meals.OrderBy(x => rng.Next()).ToList();
                var possibleBreakfasts = shuffledMeals.Where(m => m.MealType == "Breakfast").ToList();
                var possibleLunches = shuffledMeals.Where(m => m.MealType == "Lunch").ToList();
                var possibleDinners = shuffledMeals.Where(m => m.MealType == "Dinner").ToList();

                Meals breakfast = null;
                Meals lunch = null;
                Meals dinner = null;

                foreach (var b in possibleBreakfasts)
                {
                    foreach (var l in possibleLunches)
                    {
                        foreach (var d in possibleDinners)
                        {
                            if ((b.Calories + l.Calories + d.Calories) <= caloricRequirement)
                            {
                                breakfast = b;
                                lunch = l;
                                dinner = d;
                                break;
                            }
                        }
                        if (breakfast != null) break;
                    }
                    if (breakfast != null) break;
                }

                if (breakfast != null && lunch != null && dinner != null)
                {
                    var userMealSet = new UserMealSets
                    {
                        UserName = strCurrentUser,
                        BreakfastId = breakfast.Id,
                        LunchId = lunch.Id,
                        DinnerId = dinner.Id,
                        MealDay = day
                    };

                    _context.UserMealSets.Add(userMealSet);
                }
            }

            await _context.SaveChangesAsync();

            return userMealSets;
        }
        private string MapDietGoals(string answerGoals)
        {
            switch (answerGoals)
            {
                case "Utrata wagi":
                    return "Utrata wagi";
                case "Poprawa samopoczucia":
                    return "Poprawa samopoczucia";
                case "Utrzymanie zdrowia":
                    return "Utrzymanie zdrowia";
                case "Utrzymanie wagi":
                    return "Utrzymanie wagi";
                case "Mix celów":
                    return null;
                default:
                    return answerGoals;
            }
        }

        private string MapAllergies(string answerAllergies)
        {
            switch (answerAllergies)
            {
                case "Gluten":
                    return "Gluten";
                case "Laktoza":
                    return "Laktoza";
                case "Orzechy":
                    return "Orzechy";
                case "Nie mam":
                    return null;
                default:
                    return answerAllergies;
            }
        }

        private string MapDietType(string answerDietType)
        {
            switch (answerDietType)
            {
                case "Wegetariańska":
                    return "Wegetariańska";
                case "Wegańska":
                    return "Wegańska";
                case "Bezglutenowa":
                    return "Bezglutenowa";
                case "Keto":
                    return "Keto";
                case "Brak preferencji":
                    return null;
                default:
                    return answerDietType;
            }
        }

        private string MapFoodType(string answerFoodType)
        {
            switch (answerFoodType)
            {
                case "Mięso":
                    return "Mięso";
                case "Ryby":
                    return "Ryby";
                case "Warzywa":
                    return "Warzywa";
                case "Owoce":
                    return "Owoce";
                case "Produkty zbożowe":
                    return "Produkty zbożowe";
                case "Brak preferencji":
                    return null;
                default:
                    return answerFoodType;
            }
        }

        private string MapProductType(string answerProductType)
        {
            switch (answerProductType)
            {
                case "Istotne":
                    return "Istotne";
                case "Nie istotne":
                    return "Nie istotne";
                default:
                    return answerProductType;
            }
        }

        private string MapRegionTypeFood(string answerRegionTypeFood)
        {
            switch (answerRegionTypeFood)
            {
                case "Kuchnia azjatycka":
                    return "Kuchnia azjatycka";
                case "Kuchnia śródziemnomorska":
                    return "Kuchnia śródziemnomorska";
                case "Kuchnia amerykańska":
                    return "Kuchnia amerykańska";
                case "Brak preferencji":
                    return null;
                default:
                    return answerRegionTypeFood;
            }
        }

        private string MapExcludedProducts(string answerExcludedProducts)
        {
            switch (answerExcludedProducts)
            {
                case "Czerwone mięso":
                    return "Czerwone mięso";
                case "Produkty mleczne":
                    return "Produkty mleczne";
                case "Cukry proste":
                    return "Cukry proste";
                case "Smażone potrawy":
                    return "Smażone potrawy";
                case "Nie wykluczam niczego":
                    return null;
                default:
                    return answerExcludedProducts;
            }
        }
    }
}
