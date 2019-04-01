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
            //var inputPath = @"../../../test.txt";
            List<string> inputText = File.ReadLines(inputPath).ToList();
            List<Step> AllSteps = new List<Step>();
            List<Step> AvailableSteps = new List<Step>();
            List<Step> UnavailableSteps = new List<Step>(AllSteps);
            string stepLetterOrder = null;
            List<Worker> WorkerList = new List<Worker>();
            List<Worker> AvailableWorkers = new List<Worker>(WorkerList);
            List<Step> CompletedSteps = new List<Step>();
            int seconds = 0;

            //Part 1
            HandleDataInput();
            GetAvailableSteps();
            AddStepTime();
            AddWorkers(1);
            BuildSleigh();

            //The last second is added after the work is completed, and therefore removed.
            Console.WriteLine("Seconds it took to assemble to sleigh with one workers: {0}", seconds - 1);
            Console.WriteLine("Order the steps were completed with one worker: {0}", stepLetterOrder);

            //Part 2
            ResetData();
            HandleDataInput();
            GetAvailableSteps();
            AddStepTime();
            AddWorkers(5);
            BuildSleigh();

            //The last second is added after the work is completed, and therefore removed.
            Console.WriteLine("Seconds it took to assemble to sleigh with five workers: {0}", seconds-1);
            Console.WriteLine("Order the steps were completed with five workers: {0}", stepLetterOrder);
            Console.ReadKey();

            //Adding the data
            void HandleDataInput()
            {
                foreach (string row in inputText)
                {
                    AddStep(row);

                }
                foreach (string row in inputText)
                {
                    AddRequirement(row);
                }
            }

            //Go through the row adding both steps if they are missing from the AllSteps list.
            void AddStep(string row) {
                Step step = new Step();
                Step reqStep = new Step();
                step.Id = row.Substring(36, 1);
                reqStep.Id = row.Substring(5, 1);
                if (AllSteps == null || !AllSteps.Any(x => (x.Id == step.Id)))
                {
                    AllSteps.Add(step);
                    UnavailableSteps.Add(step);
                }
                if (!AllSteps.Any(x => (x.Id == reqStep.Id)))
                {
                    AllSteps.Add(reqStep);
                    UnavailableSteps.Add(reqStep);
                }
            }

            //Go through the row and add the the required steps to the different steps.
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

            //Add the time each step takes to complete.
            void AddStepTime()
            {
                AllSteps.Sort((x, y) => string.Compare(x.Id, y.Id));
                int i = 1;
                foreach (Step step in AllSteps)
                {
                    step.Time = 60 + i;
                    i++;
                }
            }

            //Create the amount of workers specified by w.
            void AddWorkers(int w)
            {
                while (WorkerList.Count() < w)
                {
                    Worker worker = new Worker();
                    WorkerList.Add(worker);
                }
            }

            //Reset all the lists and variables for part 2.
            void ResetData()
            {
                AllSteps = new List<Step>();
                AvailableSteps = new List<Step>();
                UnavailableSteps = new List<Step>(AllSteps);
                stepLetterOrder = null;
                WorkerList = new List<Worker>();
                AvailableWorkers = new List<Worker>(WorkerList);
                CompletedSteps = new List<Step>();
                seconds = 0;
            }

            //Get the available steps.
            void GetAvailableSteps()
            {
                foreach (Step step in AllSteps)
                {
                    //Adding the steps to AvailableSteps if they don't already exists.
                    if (step.Requirements == null && !AvailableSteps.Any(x => x.Id == step.Id))
                    {
                        AvailableSteps.Add(step);
                        UnavailableSteps.Remove(step);
                    }
                }
            }

            //When a step is completed, check what steps that step held back from running and add those to AvailableSteps.
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

            //Add the value of the steps completed to a string.
            void AddLetterOrder(string stepId)
            {
                stepLetterOrder += stepId;
            }

            //Building the sleigh, this is where the calculation is done.
            void BuildSleigh()
            {
                //As long as not all steps are completed, run.
                while (CompletedSteps.Count() < AllSteps.Count())
                {
                    //Looping through each worker adding a Step to a Worker or removing a second from what's left of that Workers current Step.
                    foreach (Worker worker in WorkerList)
                    {

                        if (worker.StepTime == 0)
                        {
                            if (worker.Step != null)
                            {
                                AddLetterOrder(worker.Step.Id);
                                UpdateAvailableSteps(worker.Step);
                                CompletedSteps.Add(worker.Step);
                                worker.Step = null;
                            }
                            if (AvailableSteps.Count != 0)
                            {
                                AvailableSteps.Sort((x, y) => string.Compare(x.Id, y.Id));
                                Step step = AvailableSteps.First();
                                UnavailableSteps.Remove(step);
                                worker.Step = step;
                                worker.StepTime = step.Time - 1;
                                AvailableSteps.RemoveAt(0);
                            }
                        }
                        else
                        {
                            worker.StepTime--;
                        }
                    }
                    //To make sure that the free workers always comes after the busy ones, so no second goes lost.
                    WorkerList = WorkerList.OrderByDescending(x => x.StepTime).ToList();
                    seconds++;

                }
            }
        }
    }
}
