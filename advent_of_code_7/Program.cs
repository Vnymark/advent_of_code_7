using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace advent_of_code_7
{
    class Program
    {
        static void Main(string[] args)
        {
            var inputPath = @"../../../input.txt";
            List<string> inputText = File.ReadLines(inputPath).ToList();
            List<Step> AvailableSteps = new List<Step>();
            foreach (string row in inputText)
            {
                AddStep(row);
                
            }

            void AddStep(string row) {
                string stepId = row.Substring(36, 1);
                string requirement = row.Substring(5, 1);
                Step step = new Step();
                step.StepId = stepId;
                if (AvailableSteps == null || !AvailableSteps.Any(x => (x.StepId == step.StepId)))
                {
                    step.Requirements = new List<string>();
                    AvailableSteps.Add(step);
                } else
                {
                    Step existingStep = AvailableSteps.Find(x => (x.StepId == stepId));
                    existingStep.Requirements.Add(requirement);
                }
            }

            Console.ReadKey();
        }
    }
}
