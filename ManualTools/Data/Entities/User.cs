namespace ManualTools.Data.Entities
{
  public class User
  {
    public User()
    {
    }
    public string Id { get; set; } = Guid.NewGuid().ToString();
    public string Name { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public bool EmailConfirmed { get; set; }
    public string BankAccount { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;
    public string PasswordSalt { get; set; } = string.Empty;
    public ICollection<Tool> Tools { get; set; } = new HashSet<Tool>();
    public ICollection<Order> Orders { get; set; } = new HashSet<Order>();
  }
}

