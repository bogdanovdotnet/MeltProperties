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

        public float SumLiquid { get; set; }

        public List<OxideResultModel> SolidSumOxides { get; set; }

        public PhaseSystemModel(PhasesSystem system)
        {
            this.Formula = system.Formula;
            this.Phases = new List<PhaseModel>();
            foreach (var phase in system.Phases)
            {
                Phases.Add(new PhaseModel(){ Phase = phase });
            }

            this.SolidSumOxides = new List<OxideResultModel>();
            foreach (var oxide in system.Oxides)
            {
                this.SolidSumOxides.Add(new OxideResultModel()
                                            {
                                                Oxide = oxide
                                            });
            }
        }

        public PhaseSystemModel(PhaseSystemModel model)
        {
            this.Formula = model.Formula;
            this.Phases = new List<PhaseModel>();
            foreach (var phase in model.Phases)
            {
                Phases.Add(new PhaseModel() { Phase = phase.Phase, Percentage = phase.Percentage });
            }

            this.SolidSumOxides = model.SolidSumOxides;
        }

        public void SetSolidSum()
        {
            foreach (var oxide in this.SolidSumOxides)
            {
                float sum = 0;
                foreach (var phase in this.Phases)
                {
                    var curOxide = phase.Phase.Oxides.FirstOrDefault(x => x.Oxide.Formula == oxide.Oxide.Formula);
                    if (curOxide != null)
                    {
                        sum += (phase.Percentage * curOxide.Percentage) / 100;
                    }
                }

                oxide.Percentage = sum;
            }
        }
    }
}
