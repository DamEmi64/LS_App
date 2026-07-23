using CommunicationBase.Dtos;
using CommunicationBase.Interfaces;
using Fluid;
using Fluid.Values;
using System.Dynamic;

namespace CommunicationBase
{
    public static class FluidGenerator
    {

        public static FluidContext GenerateContext(IEnumerable<IFluidParser> parsers, Dictionary<string, object>? properties = null)
        {
            var model = new ExpandoObject();

            if (model is IDictionary<string, object> modelObj)
            {
                var variables = parsers.SelectMany(x => x.Variables);
                var functions = parsers.SelectMany(x => x.Functions);

                foreach (var prop in properties ?? new Dictionary<string, object>())
                {
                    modelObj.Add(prop.Key, prop.Value);
                }

                foreach (var variable in variables)
                {
                    modelObj.Add(variable.Key, variable.Value ?? new object());
                }

                var fluidProperties = modelObj.ToDictionary(StringComparer.OrdinalIgnoreCase);

                var context = new TemplateContext(model);

                var fluidContext = new FluidContext
                (
                    Model: fluidProperties,
                    Context: context
                );

                foreach (var function in functions)
                {
                    context.SetValue(function.Invoker, new FunctionValue((args, ctx) =>
                    {
                        return function.Method.Invoke(args, fluidContext);
                    }));
                }

                return fluidContext;
            }

            throw new InvalidCastException("Failed to create context model.");
        }

        public static ValueTask<string> GenerateAsync(string template, FluidContext context)
        {
            var parser = new FluidParser(new FluidParserOptions { AllowFunctions = true });
            parser.TryParse(template, out var parsedTemplate, out var error);

            if (parsedTemplate is null)
            {
                throw new InvalidOperationException($"Failed to parse template: {error}");
            }

            return parsedTemplate.RenderAsync(context.Context);
        }

        public static string Generate(string template, FluidContext context)
        {
            var parser = new FluidParser(new FluidParserOptions { AllowFunctions = true });
            parser.TryParse(template, out var parsedTemplate, out var error);

            if (parsedTemplate is null)
            {
                throw new InvalidOperationException($"Failed to parse template: {error}");
            }

            return parsedTemplate.Render(context.Context);
        }
    }
}