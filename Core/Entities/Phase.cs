using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using NServiceKit.DataAnnotations;

namespace Core.Entities
{
    public class Phase : Entity
    {
        [Index(Unique = true)]
        public string Formula { get; set; }

        public List<PhaseOxideContent> Oxides { get; set; }

        public Phase()
        {
            this.Oxides = new List<PhaseOxideContent>();
        }
    }
}
