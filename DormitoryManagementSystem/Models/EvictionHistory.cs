namespace DormitoryManagementSystem.Models;
public class EvictionHistory
{
    public int Id { get; set; }

    public int UserId { get; set; }
    public UserInfo User { get; set; }

    public DateTime EvictionDate { get; set; }

    public int? OldRoomPlaceId { get; set; }
}
