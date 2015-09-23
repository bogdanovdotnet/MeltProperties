using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Core.Entities;

namespace Core.Models
{
    public class PhaseSystemModel
    {
        public List<PhaseModel> Phases { get; set; }

        public string Formula { get; set; }

        public PhaseSystemModel(PhasesSystem system)
        {
            this.Formula = system.Formula;
            this.Phases = new List<PhaseModel>();
            foreach (var phase in system.Phases)
            {
                Phases.Add(new PhaseModel(){ Phase = phase });
            }
        }
    }
}
