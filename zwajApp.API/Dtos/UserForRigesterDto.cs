using System.ComponentModel.DataAnnotations;

namespace zwajApp.API.Dtos
{
    public class UserForRigesterDto
    {
        [Required]
        public string Username { get; set; }
        [StringLength(8,MinimumLength=4,ErrorMessage="لازم 8 حروف ولا يقل عن 4")]
        public string Password { get; set; }
    }
}