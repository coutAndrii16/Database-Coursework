using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using DormitoryManagementSystem.Models;

namespace DormitoryManagementSystem.Services
{
    public class ShowerSlotGeneratorService
    {
        private readonly DatabaseContext _dbContext;

        public ShowerSlotGeneratorService(DatabaseContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task GenerateSlotsForTodayAsync()
        {
            var today = DateTime.Today;

            var timeRanges = new List<(TimeSpan start, TimeSpan end)>
            {
                (new TimeSpan(6, 0, 0),  new TimeSpan(7, 0, 0)),
                (new TimeSpan(9, 0, 0),  new TimeSpan(10, 0, 0)),
                (new TimeSpan(12, 0, 0), new TimeSpan(13, 0, 0)),
                (new TimeSpan(15, 0, 0), new TimeSpan(16, 0, 0)),
                (new TimeSpan(18, 0, 0), new TimeSpan(19, 0, 0)),
                (new TimeSpan(21, 0, 0), new TimeSpan(22, 0, 0))
            };

            var genderCapacities = new Dictionary<string, int>
            {
                { "Чоловіча", 4 },
                { "Жіноча", 4 }
            };

            foreach (var gender in genderCapacities.Keys)
            {
                int maxReservations = genderCapacities[gender];
                foreach (var (start, end) in timeRanges)
                {
                    var slot = new ShowerSlot
                    {
                        Date = today,
                        StartTime = start,
                        EndTime = end,
                        MaxReservations = maxReservations,
                        Gender = gender,                // записуємо “Чоловік” або “Жінка”
                                                        // тимчасово для тестових "Чоловіча" або "Жіноча"
                        Reservations = new List<ShowerReservation>()
                    };
                    _dbContext.ShowerSlots.Add(slot);
                }
            }

            await _dbContext.SaveChangesAsync();
        }
    }
}
