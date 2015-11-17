using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Core.Models
{
    public class OxidesByTemperatureModel
    {
        public int Temperature { get; set; }

        public List<OxideResultModel> OxidesResult { get; set; }

        public OxidesByTemperatureModel()
        {
            this.OxidesResult = new List<OxideResultModel>();
        }
    }
}
