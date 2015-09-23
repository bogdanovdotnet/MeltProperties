using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Models
{
    public class PhaseComposition
    {
        public List<PhaseModel> Phases { get; set; }

        public PhaseComposition()
        {
            this.Phases = new List<PhaseModel>();
        }
    }
}
