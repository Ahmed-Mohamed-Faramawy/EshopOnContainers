using System.ComponentModel.DataAnnotations;


public class RegisterInputModel
{
    public string CardHolderName { get; set; }
    public string SecurityNumber { get; set; }
    public string Username { get; set; }
    public string Password { get; set; }
    public string CardNumber { get; set; }
    public int CardType { get; set; }
}