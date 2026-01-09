namespace DormitoryManagementSystem.Models;

public class EvictionNotification
{
    public int Id { get; set; }
    public int UserId { get; set; }
    public UserInfo User { get; set; }
    public string Reason { get; set; } = ""; // Причина виселення
    public DateTime EvictionDate { get; set; }
    public DateTime BlockDate { get; set; } // Дата блокування (EvictionDate + 7 днів)
    public bool IsRead { get; set; } = false;
}