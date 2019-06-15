using FluentValidation;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

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
