using Core.Entities;

namespace Core.Models
{
    public class SystemsByTemperaturesModel
    {
        public PhaseSystemModel FirstSystem { get; set; }

        public PhaseSystemModel SecondSystem { get; set; }

        public int Temperature { get; set; }

        public SystemsByTemperaturesModel(PhasesSystem firstSystem, PhasesSystem secondSystem, int temperature)
        {
            this.FirstSystem = new PhaseSystemModel(firstSystem);
            this.SecondSystem = new PhaseSystemModel(secondSystem);
            this.Temperature = temperature;
        }
    }
}
