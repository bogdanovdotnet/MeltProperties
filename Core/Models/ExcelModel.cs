using System.Collections.Generic;
using System.Linq;

using Core.Entities;

namespace Core.Models
{
    public class ExcelModel
    {
        public string NameOfMaterial { get; set; }

        public string FirstSystemName { get; set; }

        public string SecondSystemName { get; set; }

        public string FirstSystemOxide { get; set; }

        public string SecondSystemOxide { get; set; }

        public List<int> Temperatures { get; set; }

        public List<BaseCompositionModel> Oxides { get; set; }

        public List<SystemsByTemperaturesModel> TemperaturesModels { get; set; }

        public List<SystemsByTemperaturesModel> ResultTemperaturesModels { get; set; }

        public float FirstSystemModule { get; set; }

        public float SecondSystemModule { get; set; }

        public List<OxidesByTemperatureModel> OxidesResultModel { get; set; }

        public List<OxidesByTemperatureModel> OxidesInMelt { get; set; }

        public List<OxidesByTemperatureModel> OxidesInMeltRes { get; set; }

        public PhasesSystem FirstSystem { get; set; }

        public PhasesSystem SecondSystem { get; set; }

        public float SumR2O { get; set; }

        public ExcelModel(
            string nameOfMaterial,
            string firstSystemName,
            string secondSystemName,
            string firstSystemOxide,
            string secondSystemOxide,
            List<int> temperatures,
            List<BaseCompositionModel> oxides,
            List<SystemsByTemperaturesModel> temperaturesModels,
            List<SystemsByTemperaturesModel> resultTemperaturesModels,
            float firstSystemModule,
            float secondSystemModule,
            List<OxidesByTemperatureModel> oxidesResultModel,
            PhasesSystem firstSystem,
            PhasesSystem secondSystem,
            float sumR2O)
        {
            this.NameOfMaterial = nameOfMaterial;
            this.FirstSystemName = firstSystemName;
            this.SecondSystemName = secondSystemName;
            this.FirstSystemOxide = firstSystemOxide;
            this.SecondSystemOxide = secondSystemOxide;
            this.Oxides = oxides;
            this.Temperatures = temperatures;
            this.TemperaturesModels = temperaturesModels;
            this.ResultTemperaturesModels = resultTemperaturesModels;
            this.FirstSystemModule = firstSystemModule;
            this.SecondSystemModule = secondSystemModule;
            this.OxidesResultModel = oxidesResultModel;
            this.FirstSystem = firstSystem;
            this.SecondSystem = secondSystem;
            this.SumR2O = sumR2O;
            this.SetMeltsOxides();
        }

        public void SetMeltsOxides()
        {
            this.OxidesInMelt = new List<OxidesByTemperatureModel>();
            foreach (var oxide in this.OxidesResultModel)
            {
                var model = new OxidesByTemperatureModel();
                model.Temperature = oxide.Temperature;
                foreach (var res in oxide.OxidesResult)
                {
                    model.OxidesResult.Add(new OxideResultModel()
                                               {
                                                   Oxide = res.Oxide,
                                                   Percentage = this.Oxides.First(x => x.Composition.Formula == res.Oxide.Formula).Percentage - res.Percentage
                                               });
                }
                var mainOxides = this.OxidesResultModel.First().OxidesResult.Select(x => x.Oxide.Formula);
                foreach (var origOxide in this.Oxides.Where(x => !mainOxides.Contains(x.Composition.Formula)))
                {
                    model.OxidesResult.Add(new OxideResultModel()
                    {
                        Oxide = origOxide.Composition,
                        Percentage = origOxide.Percentage
                    });
                }
                this.OxidesInMelt.Add(model);
            }

            this.SetMeltsOxidesRes();
        }

        private void SetMeltsOxidesRes()
        {
            this.OxidesInMeltRes = new List<OxidesByTemperatureModel>();
            foreach (var oxide in this.OxidesInMelt)
            {
                var model = new OxidesByTemperatureModel();
                model.Temperature = oxide.Temperature;
                var sum = oxide.OxidesResult.Sum(x => x.Percentage);
                foreach (var resultModel in oxide.OxidesResult)
                {
                    var resModelOxide = new OxideResultModel()
                                            {
                                                Oxide = resultModel.Oxide,
                                                Percentage = (resultModel.Percentage * 100) / sum
                                            };
                    model.OxidesResult.Add(resModelOxide);
                }

                this.SetFullPercentage(model.OxidesResult);

                this.OxidesInMeltRes.Add(model);
            }
        }

        private void SetFullPercentage(List<OxideResultModel> list)
        {
            var sum = list.Sum(x => x.Percentage);
            if (sum.Equals(100))
                return;
            list.First().Percentage += 100 - sum;
        }

    }
}
