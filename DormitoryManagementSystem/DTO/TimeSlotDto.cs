namespace DormitoryManagementSystem.DTO;
public class TimeSlotDto
{
    public TimeSpan StartTime { get; set; }
    public TimeSpan EndTime { get; set; }
    public string TimeRange { get; set; } = string.Empty;
}
