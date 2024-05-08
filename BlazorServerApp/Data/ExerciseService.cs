using BlazorServerApp.Pages;
using BlazorServerAppDB.Data.Exercises;
using Microsoft.EntityFrameworkCore;
using NuGet.Packaging;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BlazorServerApp.Data
{
    public class ExerciseService
    {

        private readonly ShapeShiftExercisesContext _context;
        public ExerciseService(ShapeShiftExercisesContext context)
        {
            _context = context;
        }

        public async Task<List<QuestionExercises>> GetQuestionsAsync()
        {
            return await _context.QuestionExercises
                .Where(x => x.IsActive == true)
                .Include(x => x.PossibleAnswersExercises)
                .AsNoTracking()
                .ToListAsync();
        }

        public async Task SaveUserAnswerAsync(UserAnswersExercises answer)
        {
            _context.UserAnswersExercises.Add(answer);
            await _context.SaveChangesAsync();
        }

        public async Task<List<UserAnswersExercises>> GetUserAnswersAsync(string strCurrentUser)
        {
            return await _context.UserAnswersExercises
                .Where(ua => ua.UserName == strCurrentUser)
                .Include(ua => ua.Question)
                .AsNoTracking()
                .ToListAsync();
        }

        public async Task<List<UserExerciseSets>> GetUserExerciseSetsAsync(string strCurrentUser)
        {
            return await _context.UserExerciseSets
                .Where(ues => ues.UserName == strCurrentUser)
                .Include(ues => ues.ExerciseSet) 
                .OrderBy(ues => ues.TrainingDay)
                .ToListAsync();
        }

        public async Task ClearUserAnswersAsync(string strCurrentUser)
        {
            var userAnswers = await _context.UserAnswersExercises
                .Where(ua => ua.UserName == strCurrentUser)
                .ToListAsync();

            var userExercises = await _context.UserExerciseSets
                .Where(ua => ua.UserName == strCurrentUser)
                .ToListAsync();

            _context.UserExerciseSets.RemoveRange(userExercises);
            _context.UserAnswersExercises.RemoveRange(userAnswers);
            await _context.SaveChangesAsync();
        }

        public async Task<bool> HasUserSubmittedAnswersOrExercises(string userName)
        {
            var hasAnswers = await _context.UserAnswersExercises.AnyAsync(ua => ua.UserName == userName);
            var hasExercises = await _context.UserExerciseSets.AnyAsync(ue => ue.UserName == userName);
            return hasAnswers && hasExercises;
        }

        //Algorytm losowania zestawu ćwiczeń
        public async Task<List<BlazorServerAppDB.Data.Exercises.Exercises>> GenerateExercisePlanForUser(string strCurrentUser)
        {
            var userAnswers = await _context.UserAnswersExercises
                            .Where(ua => ua.UserName == strCurrentUser)
                            .Include(ua => ua.Question)
                            .Include(ua => ua.ChosenAnswer)
                            .ToListAsync();

            string? exerciseGoal = userAnswers
                .Where(ua => ua.Question.Category == "Goals")
                .Select(ua => MapAnswerToExerciseGoal(ua.ChosenAnswer.AnswerText))
                .FirstOrDefault();

            string? exerciseDifficulty = userAnswers
                .Where(ua => ua.Question.Category == "DifficultyLevel")
                .Select(ua => MapDifficultyLevelToExerciseDifficulty(ua.ChosenAnswer.AnswerText))
                .FirstOrDefault();

            string? exerciseIntensity = userAnswers
                .Where(ua => ua.Question.Category == "IntensityLevel")
                .Select(ua => MapIntensityToExerciseIntensity(ua.ChosenAnswer.AnswerText))
                .FirstOrDefault();

            string? exerciseLocation = userAnswers
                .Where(ua => ua.Question.Category == "Location")
                .Select(ua => MapLocationToExerciseLocation(ua.ChosenAnswer.AnswerText))
                .FirstOrDefault();

            int exerciseMinutes = userAnswers
                .Where(ua => ua.Question.Category == "DurationMinutes")
                .Select(ua => MapDurationToMinutes(ua.ChosenAnswer.AnswerText))
                .FirstOrDefault();

            int trainingDays = userAnswers
                .Where(ua => ua.Question.Category == "Availability")
                .Select(ua => MapTrainingDaysTextToNumber(ua.ChosenAnswer.AnswerText))
                .FirstOrDefault();

            var exercises = await _context.Exercises
                .Where(e => (exerciseGoal == null || e.Goals == exerciseGoal) &&
                    e.DifficultyLevel == exerciseDifficulty &&
                    (exerciseIntensity == null || e.IntensityLevel == exerciseIntensity) &&
                    e.Location == exerciseLocation)
                .ToListAsync();

            List<BlazorServerAppDB.Data.Exercises.Exercises> allSelectedExercises = new List<BlazorServerAppDB.Data.Exercises.Exercises>();

            for (int day = 1; day <= trainingDays; day++)
            {
                List<BlazorServerAppDB.Data.Exercises.Exercises> shuffledExercises = ShuffleExercises(exercises);

                int accumulatedTime = 0;
                int exerciseIndex = 0;

                while (accumulatedTime < exerciseMinutes && exerciseIndex < exercises.Count)
                {
                    var exercise = exercises[exerciseIndex++];
                    int exerciseDuration = exercise.DurationMinutes ?? 0;

                    if (accumulatedTime + exerciseDuration <= exerciseMinutes)
                    {
                        allSelectedExercises.Add(exercise);
                        accumulatedTime += exerciseDuration;

                        var userExerciseSet = new UserExerciseSets
                        {
                            UserName = strCurrentUser,
                            ExerciseSetId = exercise.Id,
                            DateAssigned = DateTime.Now,
                            TrainingDay = day
                        };
                        _context.UserExerciseSets.Add(userExerciseSet);
                    }
                    if (exerciseIndex >= exercises.Count) exerciseIndex = 0;
                }
            }

            await _context.SaveChangesAsync();

            return allSelectedExercises;
        }
        private List<BlazorServerAppDB.Data.Exercises.Exercises> ShuffleExercises(List<BlazorServerAppDB.Data.Exercises.Exercises> exercises)
        {
            Random rng = new Random();
            int n = exercises.Count;
            while (n > 1)
            {
                n--;
                int k = rng.Next(n + 1);
                var value = exercises[k];
                exercises[k] = exercises[n];
                exercises[n] = value;
            }
            return exercises;
        }
        private string MapAnswerToExerciseGoal(string answerGoals)
        {
            switch (answerGoals)
            {
                case "Utrata wagi":
                    return "Utrata wagi";                
                case "Poprawa kondycji fizycznej":
                    return "Poprawa kondycji";
                case "Poprawa samopoczucia":
                    return "Poprawa samopoczucia";
                case "Mix celów":
                    return null;
                default:
                    return answerGoals; 
            }
        }

        private string MapDifficultyLevelToExerciseDifficulty(string answerDifficulty)
        {
            switch (answerDifficulty)
            {
                case "Nie, jestem początkujący":
                    return "Niski";
                case "Tak, ale tylko podstawowe":
                    return "Średni";
                case "Tak, regularnie ćwiczę":
                    return "Zaawansowany";
                default:
                    return answerDifficulty;
            }
        }

        private string MapIntensityToExerciseIntensity(string answerIntensity)
        {
            switch (answerIntensity)
            {
                case "Intensywne":
                    return "Wysoka";
                case "Mniej intensywne":
                    return "Średnia";
                case "Mało wyczerpujące":
                    return "Niska";
                case "Różnorodność - mix typów":
                    return null;
                default:
                    return answerIntensity;
            }
        }

        private string MapLocationToExerciseLocation(string answerLocation)
        {
            switch (answerLocation)
            {
                case "W domu/siłowni":
                    return "Pomieszczenie";
                case "Na świeżym powietrzu":
                    return "Dwór";
                default:
                    return answerLocation;
            }
        }

        private int MapDurationToMinutes(string answerMinutes)
        {
            switch (answerMinutes)
            {
                case "Mniej niż 15 minut":
                    return 15;
                case "15-30 minut":
                    return 30;
                case "Około godziny":
                    return 60;
                default:
                    return 30;
            }
        }

        private int MapTrainingDaysTextToNumber(string answerTraining)
        {
            switch (answerTraining)
            {
                case "1-2 razy w tygodniu":
                    return 2;  
                case "3-4 razy w tygodniu":
                    return 4;
                case "5-6 razy w tygodniu":
                    return 6;
                case "Codziennie":
                    return 7;
                default:
                    return 1;
            }
        }
    }
}
