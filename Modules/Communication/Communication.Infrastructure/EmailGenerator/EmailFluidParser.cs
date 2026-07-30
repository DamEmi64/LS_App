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

        [FluidVariable]
        public EmailUserData? UserData { get; set; }

        [FluidVariable]
        public EmailUserData? Sender { get; set; }

        [FluidVariable]
        public EmailUserData? Receiver { get; set; }

        [FluidVariable]
        public List<EmailUserData> Receivers { get; set; } = new List<EmailUserData>();

        [FluidVariable]
        public int SEQ { get; set; } = 0;

        [FluidFunction]
        public RandomNumberStrategy RandomNumber => _randomNumberStrategy;

        [FluidFunction]
        public RandomStrategy Random => _randomStrategy;

        [FluidFunction]
        public RandomUniqueStrategy RandomUnique => _randomUniqueStrategy;

        [FluidFunction]
        public IncrementStrategy Increment => _incrementStrategy;

        public override int GetTranslationKey(string invoker)
         => invoker switch
         {
             nameof(UserData) => 605,
             nameof(Sender) => 606,
             nameof(Receiver) => 607,
             nameof(Receivers) => 608,
             nameof(SEQ) => 609,
             _ => throw new NotImplementedException()
         };
    }

    public class EmailUserData : UserData
    {
        public override string ToString()
        {
            return Login ?? string.Empty;
        }

        public static EmailUserData Parse(UserData userData)
        {
            return new EmailUserData
            {
                UserId = userData.UserId,
                Email = userData.Email,
                Login = userData.Login
            };
        }
    }
}
