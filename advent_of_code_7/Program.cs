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
            List<Step> AllSteps = new List<Step>();
            List<Step> AvailableSteps = new List<Step>();
            List<Step> UnavailableSteps = new List<Step>(AllSteps);
            string stepLetterOrder = null;
            foreach (string row in inputText)
            {
                AddStep(row);
                
            }
            foreach (string row in inputText)
            {
                AddRequirement(row);
            }
            
            GetAvailableSteps();
            

            while (AvailableSteps.Count > 0)
            {
                AvailableSteps.Sort((x, y) => string.Compare(x.Id, y.Id));
                Step step = AvailableSteps.First();
                step.Completed = true;
                UnavailableSteps.Remove(step);
                AddLetterOrder(step.Id);
                AvailableSteps.RemoveAt(0);
                UpdateAvailableSteps(step);

            }

            void AddStep(string row) {
                
                string stepId = row.Substring(5, 1);
                
                Step step = new Step();
                step.Id = stepId;
                if (AllSteps == null || !AllSteps.Any(x => (x.Id == step.Id)))
                {
                    AllSteps.Add(step);
                    UnavailableSteps.Add(step);
                } 
            }

            void AddRequirement(string row)
            {
                string stepId = row.Substring(36, 1);
                string requiredStepId = row.Substring(5, 1);
                if (AllSteps.Any(x => x.Id == stepId))
                {
                    Step step = AllSteps.Find(x => x.Id == stepId);
                    Step reqStep = AllSteps.Find(x => x.Id == requiredStepId);
                    if (step.Requirements == null)
                    {
                        step.Requirements = new List<Step>();
                        step.Requirements.Add(reqStep);
                    }
                    else
                    {
                        step.Requirements.Add(reqStep);
                    }
                }
            }

            void GetAvailableSteps()
            {
                foreach (Step step in AllSteps)
                {
                    if (step.Requirements == null && !AvailableSteps.Any(x => x.Id == step.Id))
                    {
                        AvailableSteps.Add(step);
                        UnavailableSteps.Remove(step);
                    }
                }
            }

            void UpdateAvailableSteps(Step completedStep)
            {
                foreach (Step step in UnavailableSteps)
                {
                    if (step.Requirements.Any(x => x.Id == completedStep.Id))
                    {
                        Step reqStep = step.Requirements.Find(x => x.Id == completedStep.Id);
                        step.Requirements.Remove(reqStep);
                        
                    }
                    if (step.Requirements.Count == 0 && !AvailableSteps.Any(x => x.Id == step.Id))
                    {
                        AvailableSteps.Add(step);
                    }
                }
            }

            void AddLetterOrder(string stepId)
            {
                stepLetterOrder += stepId;
            }

            Console.ReadKey();
        }
    }
}
