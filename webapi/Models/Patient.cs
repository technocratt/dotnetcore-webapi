namespace webapi.Models;

public class Patient : BaseModel
{
    public int Id { get; set; }
    public string Name { get; set; }
    public int Age { get; set; }
}