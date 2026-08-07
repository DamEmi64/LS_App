namespace Communication.Infrastructure.Services
{
    public class FluidVariableModel
    {
        public required string Invoker { get; set; }
        public object? Data { get; set; }
        public int Translation { get; set; }
    }
}
