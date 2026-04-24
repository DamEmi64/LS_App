using CommunicationBase.Attributes;
using CommunicationBase.Dtos;
using CommunicationBase.Interfaces;
using Fluid.Values;
using System.Reflection;

namespace CommunicationBase
{
    public abstract class FluidParserModel : IFluidParser
    {
        private List<IFluidFunction> _functions = new List<IFluidFunction>();
        private Dictionary<string, object> _variables = new Dictionary<string, object>();

        public FluidParserModel()
        {
            ParseVariables();
            ParseFunctions();
        }

        private void ParseVariables()
        {
            var type = this.GetType();

            var properties = type.GetProperties();

            foreach (var property in properties)
            {
                var attribute = property.GetCustomAttribute<FluidVariableAttribute>();
                if (attribute != null)
                {
                    var name = attribute.Name ?? property.Name;
                    var value = property.GetValue(this);

                    if (value is null)
                        continue;

                    if (_variables.ContainsKey(name))
                    {
                        _variables[name] = value;
                    }
                    else
                    {
                        _variables.Add(name, value);
                    }
                }
            }
        }

        private void ParseFunctions()
        {
            var type = this.GetType();

            var methods = type.GetMethods();

            foreach (var method in methods)
            {
                var attribute = method.GetCustomAttribute<FluidFunctionAttribute>();
                if (attribute != null)
                {
                    var function = new BaseFluidFunction
                    {
                        Invoker = attribute.Invoker ?? method.Name,
                        TitleKey = attribute.Title ?? method.Name,
                        Method = (args,ctx) => (FluidValue)method.Invoke(this, new object[] { args, ctx })!,
                        DescriptionKey = attribute.Description
                    };
                    _functions.Add(function);
                }
            }
        }

        public List<IFluidFunction> Functions => _functions;

        public Dictionary<string, object> Variables => _variables;
    }
}
