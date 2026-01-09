using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DormitoryManagementSystem.DTO;

public class RoomDto
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public int Floor { get; set; }
    public int PlacesCount { get; set; }
    public int OccupiedCount { get; set; }
    public string DisplayName { get; set; } = string.Empty;
}