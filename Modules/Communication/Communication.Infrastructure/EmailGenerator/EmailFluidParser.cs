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
            :base()
        {
            _randomNumberStrategy = new RandomNumberStrategy();
            _randomStrategy = new RandomStrategy();
            _incrementStrategy = new IncrementStrategy();
            _randomUniqueStrategy = new RandomUniqueStrategy();
        }

        [FluidVariable("user")]
        public UserData? UserData { get; set; }

        [FluidVariable("sender")]
        public UserData? Sender { get; set;  }

        [FluidVariable("receiver")]
        public UserData? Receiver { get; set; }

        [FluidVariable("recipients")]
        public List<UserData> Receivers { get; set; } = new List<UserData>();

        [FluidVariable]
        public int SEQ { get; set; } = 0;

        [FluidFunction]
        public FluidValue RandomNumber(FunctionArguments arguments, FluidContext context) => _randomNumberStrategy.Method.Invoke(arguments,context);

        [FluidFunction]
        public FluidValue Random(FunctionArguments arguments, FluidContext context) => _randomStrategy.Method.Invoke(arguments,context);

        [FluidFunction]
        public FluidValue RandomUnique(FunctionArguments arguments, FluidContext context) => _randomUniqueStrategy.Method.Invoke(arguments,context);

        [FluidFunction]
        public FluidValue Increment(FunctionArguments arguments, FluidContext context) => _incrementStrategy.Method.Invoke(arguments,context);

    }
}
