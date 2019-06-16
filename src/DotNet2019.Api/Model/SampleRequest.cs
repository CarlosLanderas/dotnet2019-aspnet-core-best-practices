using System.ComponentModel.DataAnnotations;

namespace DotNet2019.Api.Model
{
    public class SampleRequest
    {
        [Required]
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
    }
}
