using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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

            if (!dbContext.LearningPaths.Any())
            {
                var young = new LearningPath()
                {
                    Type = 1,
                    Level = 1,
                    Name = "Młody Podróżnik"
                        
                };

                var experienced = new LearningPath()
                {
                    Type = 1,
                    Level = 2,
                    Name = "Doświadczony Hobbit"

                };

                var hard = new LearningPath()   
                {
                    Type = 1,
                    Level = 3,
                    Name = "Prastary Olbrzym"

                };

                dbContext.LearningPaths.AddRange(young, experienced, hard);
                await dbContext.SaveChangesAsync();
            }



        }
    }
}
