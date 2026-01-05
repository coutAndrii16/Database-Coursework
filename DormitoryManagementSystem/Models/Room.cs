using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace DormitoryManagementSystem.Models
{
    public class Room
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int Floor { get; set; }
        public int PlacesCount { get; set; }
        public int DormitoryId { get; set; }
        public Dormitory Dormitory { get; set; }
        public string? Comments { get; set; }
        public ICollection<RoomPlace> Places { get; set; }
    }
}
