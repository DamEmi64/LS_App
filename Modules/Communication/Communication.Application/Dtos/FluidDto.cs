namespace Communication.Application.Dtos
{
    public class FluidDto
    {
        public int Id { get; set; }
        public string? Invoker { get; set; }
        public string? Title { get; set; }
        public string? Description { get; set; }
        public static FluidDto ToDto(CommunicationBase.Interfaces.IFluidFunction function)
        {
            var key = function.TitleKey;
            return new FluidDto { Id = int.TryParse(key, out var id) ? id : Random.Shared.Next(), Title = key, Description = function.DescriptionKey, Invoker = function.Invoker };
        }
    }
}
