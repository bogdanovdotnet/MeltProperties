using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Entities
{
    public class PhaseOxideContent : Entity
    {
        public Oxide Oxide { get; set; }

        public float Percentage { get; set; }
    }
}
