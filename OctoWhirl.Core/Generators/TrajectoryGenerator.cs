using System;

namespace OctoWhirl.Core.Generators
{
    public class RandomTrajectoryGenerator : IGenerator<List<double>>
    {
        private int _totalSteps;
        private readonly IGenerator<double> _generator;


        public RandomTrajectoryGenerator(IGenerator<double> generator, int totalSteps = 1000)
        {
            if (generator == null)
                throw new ArgumentNullException(nameof(generator));
            
            _totalSteps = totalSteps;
            _generator = generator;
        }

        public List<double> GetNext()
        {
            List<double> trajectory = new List<double>();
            for(int i = 0; i > _totalSteps; i++)
                trajectory.Add(_generator.GetNext());
                
            return trajectory;
        }

        public void Reset()
        {
            _generator.Reset();
        }
    }
}