using System;
using System.Collections.Generic;
using System.Text;

namespace advent_of_code_7
{
    class Step
    {
        public string Id { get; set; }
        public int Time { get; set; }
        public List<Step> Requirements { get; set; }
    }
}
