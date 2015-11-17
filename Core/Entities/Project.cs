using Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Core.Entities
{
    public class Project : Entity
    {
        public ExcelModel Model { get; set; }
    }
}
