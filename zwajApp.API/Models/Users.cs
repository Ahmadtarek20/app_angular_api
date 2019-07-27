namespace zwajApp.API.Models
{
    public class Users
    {
        public int id {get; set;}
        public string UserName {get; set;}
        public byte[] PasswordHash {get; set;}
        public byte[] PaswordSalt {get; set;}
      
    }
}