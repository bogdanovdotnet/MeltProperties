using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Core.Entities;

namespace Core.Models
{
    public class BaseCompositionModel
    {
        public BaseCompositionModel()
        {
            this.Composition = new Oxide();
            this.List = new List<Oxide>();
        }

        public BaseCompositionModel(IEnumerable<Oxide> list)
        {
            this.Composition = new Oxide();
            this.List = new List<Oxide>(list);
        }

        public BaseCompositionModel(Oxide oxide)
        {
            this.List = new List<Oxide>();
            this.Composition = new Oxide();
            this.Composition.Formula = oxide.Formula;
        }

        public List<Oxide> List { get; set; }

        public Oxide Composition { get; set; }

        public float Percentage { get; set; }
    }
}
