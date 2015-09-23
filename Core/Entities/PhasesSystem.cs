using System.Collections.Generic;

namespace Core.Entities
{
    public class PhasesSystem : Entity
    {
        public string Formula { get; set; }

        public List<Oxide> Oxides { get; set; }

        public List<Phase> Phases { get; set; }

        public PhasesSystem()
        {
            this.Oxides = new List<Oxide>();
            this.Phases = new List<Phase>();
        }

        //public override string ToString()
        //{
        //    var phaseString = string.Empty;
        //    var isFirst = true;
        //    foreach (var oxide in Oxides)
        //    {
        //        if (!isFirst)
        //        {
        //            phaseString += " - ";
        //        }
        //        phaseString += oxide.ToString();
        //        isFirst = false;
        //    }
        //    return phaseString;
        //}
    }
}
