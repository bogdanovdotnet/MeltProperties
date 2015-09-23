using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Core.Entities;

namespace Core.Models
{
    public class OxideСontent : BaseCompositionModel
    {
        public override string ToString()
        {
            return string.Format("{0} - {1}%", this.Composition.Formula, this.Percentage);
        }
    }
}
