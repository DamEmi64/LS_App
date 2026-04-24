using System;
using System.Collections.Generic;
using System.Text;

namespace Communication.Application.Dtos
{
    public class RulesDto
    {
        public List<FluidDto>? Functions { get; set; }
        public List<FluidDto>? Variables { get; set; }
    }
}
