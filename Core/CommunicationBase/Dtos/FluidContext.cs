using Fluid;


namespace CommunicationBase.Dtos
{
    public record FluidContext(TemplateContext Context, IDictionary<string,object> Model);
}
