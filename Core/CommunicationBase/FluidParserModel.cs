using CommunicationBase.Attributes;
using CommunicationBase.Interfaces;
using Fluid;
using Fluid.Values;
using System.Collections.Concurrent;
using System.Reflection;

namespace CommunicationBase;

public abstract class FluidParserModel : IFluidParser
{
    private static readonly ConcurrentDictionary<Type, Metadata> _cache = new();

    private readonly Metadata _metadata;

    protected FluidParserModel()
    {
        _metadata = _cache.GetOrAdd(GetType(), Metadata.Create);
    }

    public Dictionary<string, object?> Variables
    {
        get
        {
            var builder = new FluidModelBuilder();

            foreach (var variable in _metadata.Variables)
            {
                builder.AddVariable(variable.Name, variable.Getter(this));
            }

            Configure(builder);

            return builder.Variables;
        }
    }

    public List<IFluidFunction> Functions
    {
        get
        {
            var builder = new FluidModelBuilder();

            foreach (var metadata in _metadata.Functions)
            {
                builder.AddFunction(metadata.Getter(this));
            }

            Configure(builder);

            return builder.Functions;
        }
    }

    public abstract int GetTranslationKey(string invoker);

    /// <summary>
    /// Allows derived models to register additional variables/functions.
    /// </summary>
    protected virtual void Configure(FluidModelBuilder builder)
    {
    }

    #region Metadata

    private sealed class Metadata
    {
        public List<VariableMetadata> Variables { get; } = [];
        public List<FunctionMetadata> Functions { get; } = [];

        public static Metadata Create(Type type)
        {
            var metadata = new Metadata();

            foreach (var property in type.GetProperties(BindingFlags.Instance | BindingFlags.Public))
            {
                var attribute = property.GetCustomAttribute<FluidVariableAttribute>();

                if (attribute == null)
                    continue;

                metadata.Variables.Add(new VariableMetadata(
                    property.Name,
                    property));
            }

            foreach (var property in type.GetProperties(BindingFlags.Instance | BindingFlags.Public))
            {
                var attribute = property.GetCustomAttribute<FluidFunctionAttribute>();

                if (attribute == null)
                    continue;

                if (!typeof(IFluidFunction).IsAssignableFrom(property.PropertyType))
                {
                    throw new InvalidOperationException(
                        $"{property.Name} must implement {nameof(IFluidFunction)}.");
                }

                metadata.Functions.Add(new FunctionMetadata(property));
            }

            return metadata;
        }

        private static void ValidateMethod(MethodInfo method)
        {
            if (method.ReturnType != typeof(FluidValue))
                throw new InvalidOperationException(
                    $"{method.Name} must return FluidValue.");

            var parameters = method.GetParameters();

            if (parameters.Length != 2 ||
                parameters[0].ParameterType != typeof(FunctionArguments) ||
                parameters[1].ParameterType != typeof(TemplateContext))
            {
                throw new InvalidOperationException(
                    $"{method.Name} has an invalid Fluid function signature.");
            }
        }
    }

    private sealed record VariableMetadata(string Name, PropertyInfo Property)
    {
        public object? Getter(object instance)
            => Property.GetValue(instance);
    }

    private sealed record FunctionMetadata(Func<object, IFluidFunction> Getter)
    {
        public FunctionMetadata(PropertyInfo property)
            : this(instance => (IFluidFunction)property.GetValue(instance)!)
        {
        }

        public FunctionMetadata(FieldInfo field)
            : this(instance => (IFluidFunction)field.GetValue(instance)!)
        {
        }
    }

    #endregion
}

public sealed class FluidModelBuilder
{
    internal Dictionary<string, object?> Variables { get; } = new();
    internal List<IFluidFunction> Functions { get; } = new();

    public FluidModelBuilder AddVariable(string name, object? value)
    {
        Variables[name] = value;
        return this;
    }

    public FluidModelBuilder AddFunction(IFluidFunction function)
    {
        Functions.Add(function);
        return this;
    }
}