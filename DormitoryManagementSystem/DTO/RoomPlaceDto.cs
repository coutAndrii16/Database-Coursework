using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DormitoryManagementSystem.DTO;

public class RoomPlaceDto
{
    public int Id { get; set; }
    public int RoomId { get; set; }
    public string DisplayNumber { get; set; } = string.Empty;
}