using OctoWhirl.Core.Tools.Maths.Generators.Interfaces;

namespace OctoWhirl.Core.Tools.Maths.Generators
{
    public class RandomTrajectoryGenerator : ISimpleGenerator<List<double>>
    {
        private int _totalSteps;
        private readonly ISimpleGenerator<double> _generator;


        public RandomTrajectoryGenerator(ISimpleGenerator<double> generator, int totalSteps = 1000)
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