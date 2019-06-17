using System;
using System.Collections.Generic;
using System.Text;

namespace DotNet2019.Api.Infrastructure.Data
{
    internal class User
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public DateTime Created { get; set; }
    }
}
