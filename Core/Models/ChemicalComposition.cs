using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Models
{
    public class ChemicalComposition
    {
        public List<OxideСontent> Components { get; set; }

        public ChemicalComposition()
        {
            this.Components = new List<OxideСontent>();
        }
    }
}
