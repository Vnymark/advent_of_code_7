using System;
using System.Collections.Generic;
using System.Text;

namespace advent_of_code_7
{
    class Step
    {
        public string StepId { get; set; }
        public bool Available { get; set; }
        public bool Completed { get; set; }
        public List<string> Requirements { get; set; }
    }
}
