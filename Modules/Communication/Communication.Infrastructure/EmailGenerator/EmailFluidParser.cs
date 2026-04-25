using Base;
using Communication.Infrastructure.EmailGenerator.Strategies;
using CommunicationBase;
using CommunicationBase.Attributes;
using CommunicationBase.Dtos;
using Fluid;
using Fluid.Values;

namespace Communication.Infrastructure.EmailGenerator
{
    public class EmailFluidParser : FluidParserModel
    {
        private readonly RandomNumberStrategy _randomNumberStrategy;
        private readonly RandomStrategy _randomStrategy;
        private readonly IncrementStrategy _incrementStrategy;
        private readonly RandomUniqueStrategy _randomUniqueStrategy;

        public EmailFluidParser()
            : base()
        {
            _randomNumberStrategy = new RandomNumberStrategy();
            _randomStrategy = new RandomStrategy();
            _incrementStrategy = new IncrementStrategy();
            _randomUniqueStrategy = new RandomUniqueStrategy();
        }

        [FluidVariable("605")]
        public UserData? UserData { get; set; }

        [FluidVariable("606")]
        public UserData? Sender { get; set; }

        [FluidVariable("607")]
        public UserData? Receiver { get; set; }

        [FluidVariable("608")]
        public List<UserData> Receivers { get; set; } = new List<UserData>();

        [FluidVariable("609")]
        public int SEQ { get; set; } = 0;

        [FluidFunction(title: "601")]
        public FluidValue RandomNumber(FunctionArguments arguments, FluidContext context) => _randomNumberStrategy.Method.Invoke(arguments, context);

        [FluidFunction(title: "602")]
        public FluidValue Random(FunctionArguments arguments, FluidContext context) => _randomStrategy.Method.Invoke(arguments, context);

        [FluidFunction(title: "603")]
        public FluidValue RandomUnique(FunctionArguments arguments, FluidContext context) => _randomUniqueStrategy.Method.Invoke(arguments, context);

        [FluidFunction(title: "604")]
        public FluidValue Increment(FunctionArguments arguments, FluidContext context) => _incrementStrategy.Method.Invoke(arguments, context);

    }
}
