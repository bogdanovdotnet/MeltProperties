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

        public SystemsByTemperaturesModel(SystemsByTemperaturesModel model)
        {
            this.FirstSystem = new PhaseSystemModel(model.FirstSystem);
            this.SecondSystem = new PhaseSystemModel(model.SecondSystem);
            this.Temperature = model.Temperature;
        }

        public void SetSolidSum()
        {
            this.FirstSystem.SetSolidSum();
            this.SecondSystem.SetSolidSum();
        }
    }
}
