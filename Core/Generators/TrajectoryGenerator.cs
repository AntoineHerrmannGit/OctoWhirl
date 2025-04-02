using System;

namespace OctoWhirl.Core.Generators
{
    public class RandomTrajectoryGenerator : IGenerator<List<double>>
    {
        private int _totalSteps;
        private readonly IGenerator _generator;


        public RandomTrajectoryGenerator(int totalSteps = 1000, IGenerator generator)
        {
            if (generator == null)
                throw new ArgumentNullException(nameof(generator));
            
            _totalSteps = totalSteps;
            _generator = generator;
        }

        public List<double> GetNext()
        {
            List<double> trajectory = new List<double>();
            foreach(int i in _totalSteps)
                trajectory.Add(generator.GetNext());
                
            return trajectory;
        }
    }
}