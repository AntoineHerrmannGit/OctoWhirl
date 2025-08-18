namespace OctoWhirl.Core.Tools.Maths.Generators
{
    public class LogBrownianGenerator : BrownianGenerator
    {
        private double _state;
        private readonly double _initialState;

        public LogBrownianGenerator(double mean = 0, double sigma = 1, double step = 0.01, double initialState = 100, IGenerator<double>? generator = null, int? seed = null)
            : base(mean, sigma, step, generator, seed)
        {
            _initialState = initialState;
            _state = initialState;
        }

        public override double GetNext()
        {
            double currentState = _state;
            _state = base.GetNext();
            return currentState;
        }

        public override void Reset()
        {
            _state = _initialState;
        }
    }
}
