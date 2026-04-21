using CommunicationBase.Dtos;
using System;
using System.Collections.Generic;
using System.Text;

namespace CommunicationBase
{
    public static class CommunicationExtension
    {
        public static T GetProperty<T>(this FluidContext context, string name)
        {
            if (context.Model.TryGetValue(name, out var value))
            {
                if (value is T typedValue)
                {
                    return typedValue;
                }
            }
            throw new KeyNotFoundException($"Property '{name}' not found in context.");
        }

        public static void SetProperty<T>(this FluidContext context, string key, T value)
        {
            ArgumentNullException.ThrowIfNull(value);

            if (context.Model.ContainsKey(key))
            {
                context.Model[key] = value;
            }
            else
            {
                context.Model.Add(key, value);
            }
        }
    }
}
