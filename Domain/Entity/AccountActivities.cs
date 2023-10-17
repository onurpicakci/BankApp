namespace Domain.Entity;

public class AccountActivities
{
    public int Id { get; set; }
    public string AccountNumber { get; set; }
    public string Description { get; set; }
    public string ProcessName { get; set; }
    public DateTime OperationDate { get; set; }
}