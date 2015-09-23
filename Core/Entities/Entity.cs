using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using NServiceKit.DataAnnotations;

namespace Core.Entities
{
    public class Entity
    {
        [AutoIncrement]
        public int Id { get; set; }
    }
}
