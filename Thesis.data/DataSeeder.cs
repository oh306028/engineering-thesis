using Microsoft.EntityFrameworkCore;
using Thesis.data.Data;

namespace Thesis.data
{
    public static class DataSeeder
    {
        public static async Task SeedAsync(AppDbContext dbContext)
        {
            if (!dbContext.Subjects.Any())
            {
                var math = new Subject { Name = "Matematyka" };
                var logic = new Subject { Name = "Logika" };
                var polish = new Subject { Name = "Język Polski" };
                var english = new Subject { Name = "Język Angielski" };
                var naturalHistory = new Subject { Name = "Przyroda" };
                dbContext.Subjects.AddRange(math, logic, polish, english, naturalHistory);
                await dbContext.SaveChangesAsync();

            }

            if (!dbContext.NotificationMessages.Any())
            {
                var notificationMessages = new List<NotificationMessage>() {

                    new NotificationMessage() {Name = "Nowe zadanie domowe!" },
                    new NotificationMessage() {Name = "Mam problem z zadaniem" },
                    new NotificationMessage() {Name = "Potrzebuję porozmawiać" },
                    new NotificationMessage() {Name = "Potrzebuję spotkania" },
                    new NotificationMessage() {Name = "Potrzebuję pomocy" },
                    new NotificationMessage() {Name = "Dziękuję za pomoc" },
                    new NotificationMessage() {Name = "Mam problem z platformą" },
                    new NotificationMessage() {Name = "Ktoś mi dokucza" },


                };

                dbContext.NotificationMessages.AddRange(notificationMessages);
                await dbContext.SaveChangesAsync();
            }

            if (!dbContext.LearningPaths.Any())
            {
                var maths = dbContext.Subjects.First(p => p.Name == "Matematyka");
                var polish = dbContext.Subjects.First(p => p.Name == "Język Polski");

                var young = new LearningPath()
                {
                    Type = 1,
                    Level = 1,
                    Name = "Młody Podróżnik",
                    Subject = maths

                };

                var experienced = new LearningPath()
                {
                    Type = 1,
                    Level = 2,
                    Name = "Doświadczony Hobbit",
                    Subject = polish

                };

                var hard = new LearningPath()
                {
                    Type = 1,
                    Level = 3,
                    Name = "Prastary Olbrzym",
                    Subject = maths

                };

                var review = new LearningPath()
                {
                    Type = 0,
                    Level = 1,
                    Name = "Podróżnik w czasie"

                };

                dbContext.LearningPaths.AddRange(young, experienced, hard, review);
                await dbContext.SaveChangesAsync();
            }

            await SeedMathsExercises(dbContext);
            await SeedPolishExercises(dbContext);


            if (!dbContext.Badges.Any())
            {
                var badges = new List<Badge>
                {
                    new Badge { Name = "Mistrz Nauki", Emote = "🌟" },
                    new Badge { Name = "Ognisty Start", Emote = "🔥" },
                    new Badge { Name = "Złoty Medal", Emote = "🥇", LearningPath = dbContext.LearningPaths.First(p => p.Name == "Młody Podróżnik") },
                    new Badge { Name = "Srebrny Medal", Emote = "🥈", LearningPath = dbContext.LearningPaths.First(p => p.Name == "Doświadczony Hobbit") },
                    new Badge { Name = "Brązowy Medal", Emote = "🥉", LearningPath = dbContext.LearningPaths.First(p => p.Name == "Prastary Olbrzym") },
                    new Badge { Name = "Trofeum Mistrza", Emote = "🏆" },
                    new Badge { Name = "Puchar Umysłu", Emote = "🏅" },
                    new Badge { Name = "Wirtuoz Wiedzy", Emote = "🎓" },
                    new Badge { Name = "Ekspert Logiki", Emote = "🧠" },
                    new Badge { Name = "Odkrywca Przyrody", Emote = "🌿" },
                    new Badge { Name = "Lingwistyczny As", Emote = "📚" },
                    new Badge { Name = "Mistrz Języka", Emote = "📝" },
                    new Badge { Name = "Gwiazda Matematyki", Emote = "✨" },
                    new Badge { Name = "Turbo Uczeń", Emote = "⚡" },
                    new Badge { Name = "Pionier Wiedzy", Emote = "🚀" },
                    new Badge { Name = "Super Czytelnik", Emote = "📖" },
                    new Badge { Name = "Kreatywny Umysł", Emote = "🎨" },
                    new Badge { Name = "Wielki Strateg", Emote = "♟️" },
                    new Badge { Name = "Mistrz Gier", Emote = "🎮" },
                    new Badge { Name = "Legendarne Osiągnięcie", Emote = "🏹" }
                };

                dbContext.Badges.AddRange(badges);
                await dbContext.SaveChangesAsync();

            }

            if (!dbContext.Achievements.Any())
            {
                var achievements = new List<Achievement>
            {
                new Achievement
                {
                    Name = "Rakietowy Start",
                    Description = "Zdobądź pierwsze 100 punktów w systemie.",
                    Badge = dbContext.Badges.First(b => b.Name == "Ognisty Start")
                },
                new Achievement
                {
                    Name = "Prawdziwy Mistrz",
                    Description = "Rozwiąż 100 zadań w systemie.",
                    Badge = dbContext.Badges.First(b => b.Name == "Mistrz Nauki")
                },
                new Achievement
                {
                    Name = "Mózgowiec",
                    Description = "Rozwiąż 1000 zadań w systemie.",
                    Badge = dbContext.Badges.First(b => b.Name == "Ekspert Logiki")
                },
                new Achievement
                {
                    Name = "Badacz Natury",
                    Description = "Ukończ całą ścieżkę z przyrody.",
                    Badge = dbContext.Badges.First(b => b.Name == "Odkrywca Przyrody")
                },
                new Achievement
                {
                    Name = "Słownikowy Wojownik",
                    Description = "Pokonaj 100 zadań ze słownictwa.",
                    Badge = dbContext.Badges.First(b => b.Name == "Lingwistyczny As")
                },
                new Achievement
                {
                    Name = "Arcypisarz",
                    Description = "Napisz 10 wzorowych wypracowań.",
                    Badge = dbContext.Badges.First(b => b.Name == "Mistrz Języka")
                },
                new Achievement
                {
                    Name = "Mag Matematyki",
                    Description = "Zakończ 30 lekcji matematyki.",
                    Badge = dbContext.Badges.First(b => b.Name == "Gwiazda Matematyki")
                },
                new Achievement
                {
                    Name = "Naukowy Wirtuoz",
                    Description = "Zakończ trzy różne ścieżki z poziomu Nauka.",
                    Badge = dbContext.Badges.First(b => b.Name == "Wirtuoz Wiedzy")
                },
                new Achievement
                {
                    Name = "Strategiczny Geniusz",
                    Description = "Rozwiąż 20 zadań logicznych.",
                    Badge = dbContext.Badges.First(b => b.Name == "Wielki Strateg")
                },
                new Achievement
                {
                    Name = "Stabilny Gracz",
                    Description = "Loguj się codziennie przez 14 dni.",
                    Badge = dbContext.Badges.First(b => b.Name == "Turbo Uczeń")
                },
            };

                dbContext.Achievements.AddRange(achievements);
                await dbContext.SaveChangesAsync();
            }







            if (!dbContext.AccountLevels.Any())
            {
                var firstLevel = new AccountLevel() { Level = 1, MinPoints = 0, MaxPoints = 49 };
                var secondLevel = new AccountLevel() { Level = 2, MinPoints = 50, MaxPoints = 99 };
                var thirdLevel = new AccountLevel() { Level = 3, MinPoints = 100, MaxPoints = 199 };
                var fourthLevel = new AccountLevel() { Level = 4, MinPoints = 200, MaxPoints = 399 };
                var fifthLevel = new AccountLevel() { Level = 5, MinPoints = 400, MaxPoints = 999 };
                var sixthLevel = new AccountLevel() { Level = 6, MinPoints = 1000, MaxPoints = 1999 };
                var seventhLevel = new AccountLevel() { Level = 7, MinPoints = 2000, MaxPoints = 4999 };

                dbContext.AccountLevels.AddRange(firstLevel, secondLevel, thirdLevel, fourthLevel, fifthLevel, sixthLevel, seventhLevel);
                await dbContext.SaveChangesAsync();
            }
                



        }

        private static async Task SeedMathsExercises(AppDbContext dbContext)
        {
            if (!dbContext.Exercises.Any())
            {
                var young = dbContext.LearningPaths.First(p => p.Name == "Młody Podróżnik");
                var math = dbContext.Subjects.First(p => p.Name == "Matematyka");

                var exercises = new List<Exercise>()
                {
                    new Exercise() { Level = 1, Content = "Jaki jest wynik mnożenia 5 x 7?", Answer = new Answer() { CorrectNumber = 35 }, Subject = math },
                    new Exercise() { Level = 1, Content = "Jaki jest wynik mnożenia 8 x 12?", Answer = new Answer() { CorrectNumber = 96 }, Subject = math },
                    new Exercise() { Level = 1, Content = "Jaki jest wynik mnożenia 9 x 9?", Answer = new Answer() { CorrectNumber = 81 }, Subject = math },
                    new Exercise() { Level = 1, Content = "Jaki jest wynik mnożenia 6 x 14?", Answer = new Answer() { CorrectNumber = 84 }, Subject = math },
                    new Exercise() { Level = 1, Content = "Jaki jest wynik mnożenia 7 x 11?", Answer = new Answer() { CorrectNumber = 77 }, Subject = math },
                    new Exercise() { Level = 1, Content = "Jaki jest wynik mnożenia 4 x 25?", Answer = new Answer() { CorrectNumber = 100 }, Subject = math },
                    new Exercise() { Level = 1, Content = "Jaki jest wynik mnożenia 3 x 33?", Answer = new Answer() { CorrectNumber = 99 }, Subject = math },
                    new Exercise() { Level = 1, Content = "Jaki jest wynik mnożenia 10 x 10?", Answer = new Answer() { CorrectNumber = 100 }, Subject = math },
                    new Exercise() { Level = 1, Content = "Jaki jest wynik mnożenia 2 x 48?", Answer = new Answer() { CorrectNumber = 96 }, Subject = math },
                    new Exercise() { Level = 1, Content = "Jaki jest wynik mnożenia 12 x 8?", Answer = new Answer() { CorrectNumber = 96 }, Subject = math },
                    new Exercise() { Level = 1, Content = "Jaki jest wynik mnożenia 7 x 13?", Answer = new Answer() { CorrectNumber = 91 }, Subject = math },
                    new Exercise() { Level = 1, Content = "Jaki jest wynik mnożenia 9 x 10?", Answer = new Answer() { CorrectNumber = 90 }, Subject = math },
                    new Exercise() { Level = 1, Content = "Jaki jest wynik mnożenia 11 x 9?", Answer = new Answer() { CorrectNumber = 99 }, Subject = math },
                    new Exercise() { Level = 1, Content = "Jaki jest wynik mnożenia 6 x 15?", Answer = new Answer() { CorrectNumber = 90 }, Subject = math },
                    new Exercise() { Level = 1, Content = "Jaki jest wynik mnożenia 8 x 12?", Answer = new Answer() { CorrectNumber = 96 }, Subject = math },
                };

                var learningPathEx = exercises.Select(e => new LearningPathExercises() { Exercise = e, LearningPath = young }).ToList();

                dbContext.Exercises.AddRange(exercises);
                dbContext.LearningPathExercises.AddRange(learningPathEx);

                await dbContext.SaveChangesAsync();
            }
        }

        private static async Task SeedPolishExercises(AppDbContext dbContext)
        {
            if (!dbContext.Exercises.Include(p => p.Subject).Any(e => e.Subject.Name == "Język Polski"))
            {
                var hobbit = dbContext.LearningPaths.First(p => p.Name == "Doświadczony Hobbit");
                var polish = dbContext.Subjects.First(p => p.Name == "Język Polski");

                var exercises = new List<Exercise>()
        {
            new Exercise()
            {
                Level = 3,
                Content = "Które z poniższych słów jest nazwą własną?",
                Subject = polish,
                Answer = new Answer()
                {
                    CorrectOption = "Warszawa",
                    IncorrectOption1 = "miasto",
                    IncorrectOption2 = "szkoła",
                    IncorrectOption3 = "las"
                }
            },
            new Exercise()
            {
                Level = 4,
                Content = "Które słowo w zdaniu wymaga wielkiej litery?",
                Subject = polish,
                Answer = new Answer()
                {
                    CorrectOption = "Paweł",
                    IncorrectOption1 = "chłopiec",
                    IncorrectOption2 = "pies",
                    IncorrectOption3 = "dom"
                }
            },
            new Exercise()
            {
                Level = 5,
                Content = "Które z poniższych słów to nazwa własna rzeki?",
                Subject = polish,
                Answer = new Answer()
                {
                    CorrectOption = "Wisła",
                    IncorrectOption1 = "rzeka",
                    IncorrectOption2 = "woda",
                    IncorrectOption3 = "strumień"
                }
            },
            new Exercise()
            {
                Level = 3,
                Content = "Które słowo jest poprawnie napisane?",
                Subject = polish,
                Answer = new Answer()
                {
                    CorrectOption = "Kraków",
                    IncorrectOption1 = "krakow",
                    IncorrectOption2 = "Cracow",
                    IncorrectOption3 = "krakw"
                }
            },
            new Exercise()
            {
                Level = 4,
                Content = "Które z poniższych to poprawna nazwa dnia tygodnia?",
                Subject = polish,
                Answer = new Answer()
                {
                    CorrectOption = "Środa",
                    IncorrectOption1 = "sroda",
                    IncorrectOption2 = "sróda",
                    IncorrectOption3 = "środaa"
                }
            },
            new Exercise()
            {
                Level = 6,
                Content = "Które słowo jest nazwą państwa?",
                Subject = polish,
                Answer = new Answer()
                {
                    CorrectOption = "Polska",
                    IncorrectOption1 = "polska",
                    IncorrectOption2 = "miasto",
                    IncorrectOption3 = "kraj"
                }
            },
            new Exercise()
            {
                Level = 5,
                Content = "Które słowo jest poprawną nazwą góry?",
                Subject = polish,
                Answer = new Answer()
                {
                    CorrectOption = "Rysy",
                    IncorrectOption1 = "rysy",
                    IncorrectOption2 = "Gory",
                    IncorrectOption3 = "Szlak"
                }
            },
            new Exercise()
            {
                Level = 4,
                Content = "Które z poniższych słów jest nazwą znanej postaci literackiej?",
                Subject = polish,
                Answer = new Answer()
                {
                    CorrectOption = "Władca Pierścieni",
                    IncorrectOption1 = "książka",
                    IncorrectOption2 = "autor",
                    IncorrectOption3 = "literatura"
                }
            },
            new Exercise()
            {
                Level = 3,
                Content = "Która nazwa jest poprawną nazwą miasta?",
                Subject = polish,
                Answer = new Answer()
                {
                    CorrectOption = "Gdańsk",
                    IncorrectOption1 = "gdansk",
                    IncorrectOption2 = "Gdanskk",
                    IncorrectOption3 = "gdáńsk"
                }
            },
            new Exercise()
            {
                Level = 6,
                Content = "Które słowo jest nazwą znanego państwa w Europie?",
                Subject = polish,
                Answer = new Answer()
                {
                    CorrectOption = "Niemcy",
                    IncorrectOption1 = "niemcy",
                    IncorrectOption2 = "Niemieck",
                    IncorrectOption3 = "Kraj"
                }
            },
            new Exercise()
            {
                Level = 5,
                Content = "Które słowo jest poprawną nazwą postaci z bajki?",
                Subject = polish,
                Answer = new Answer()
                {
                    CorrectOption = "Kopciuszek",
                    IncorrectOption1 = "kopciuszek",
                    IncorrectOption2 = "Cinderella",
                    IncorrectOption3 = "Kopciuzek"
                }
            },
            new Exercise()
            {
                Level = 4,
                Content = "Które słowo jest nazwą własną rzeki w Polsce?",
                Subject = polish,
                Answer = new Answer()
                {
                    CorrectOption = "Odra",
                    IncorrectOption1 = "odra",
                    IncorrectOption2 = "rzeka",
                    IncorrectOption3 = "Strumień"
                }
            },
            new Exercise()
            {
                Level = 3,
                Content = "Które z poniższych słów to poprawna nazwa miasta?",
                Subject = polish,
                Answer = new Answer()
                {
                    CorrectOption = "Poznań",
                    IncorrectOption1 = "poznan",
                    IncorrectOption2 = "Poznan",
                    IncorrectOption3 = "Póznań"
                }
            },
            new Exercise()
            {
                Level = 6,
                Content = "Które słowo jest nazwą znanej góry w Polsce?",
                Subject = polish,
                Answer = new Answer()
                {
                    CorrectOption = "Śnieżka",
                    IncorrectOption1 = "sniezka",
                    IncorrectOption2 = "Góra",
                    IncorrectOption3 = "Sniezka"
                }
            },
            new Exercise()
            {
                Level = 5,
                Content = "Która nazwa jest poprawną nazwą znanego miasta?",
                Subject = polish,
                Answer = new Answer()
                {
                    CorrectOption = "Wrocław",
                    IncorrectOption1 = "wroclaw",
                    IncorrectOption2 = "Wroclaw",
                    IncorrectOption3 = "Wrocławk"
                }
            },
        };

                var learningPathEx = exercises
                    .Select(e => new LearningPathExercises() { Exercise = e, LearningPath = hobbit })
                    .ToList();

                dbContext.Exercises.AddRange(exercises);
                dbContext.LearningPathExercises.AddRange(learningPathEx);

                await dbContext.SaveChangesAsync();
            }
        }

    }
}
