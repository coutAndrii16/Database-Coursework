using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace DormitoryManagementSystem.Models
{
    public class RoomPlace
    {
        public int Id { get; set; }
        public int RoomId { get; set; }

        public Room Room { get; set; }
        public int? PlaceNumber { get; set; }

        // Навігація до проживаючого (1:1)
        public UserInfo? Student { get; set; }
    }
}
