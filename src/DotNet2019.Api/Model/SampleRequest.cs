using System.ComponentModel.DataAnnotations;

namespace DotNet2019.Api.Model
{
    /// <summary>
    /// Using data annotations as Fluent Validation package is not working with 3.0 preview yet.
    /// </summary>
    public class SampleRequest
    {
        [Required]
        [Range(1, 200)]
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
    }
}
