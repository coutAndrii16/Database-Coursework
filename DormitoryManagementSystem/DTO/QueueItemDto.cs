namespace DormitoryManagementSystem.DTO;
public class QueueItemDto
{
    public int QueueNumber { get; set; }
    public string UserName { get; set; } = string.Empty;
    public string RoomInfo { get; set; } = string.Empty;
    public string ReservationTime { get; set; } = string.Empty;
}