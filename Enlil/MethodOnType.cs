using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Enlil
{
    public struct MethodOnType
    {
        public MethodOnType(Type type, MethodInfo method)
        {
            Type = type;
            Method = method;
        }

        public Type Type { get; }
        public MethodInfo Method { get; }
        
        public bool HasMethodAttributeArgument<T>(string name, T expectedValue)
        {
            var foundType = this
                .Method
                .CustomAttributes
                .SelectMany(x => x.NamedArguments)
                .FirstOrDefault(x => x.MemberName == "Name" && x.TypedValue.ArgumentType == typeof(T));
            if (foundType != default && foundType.TypedValue.Value is T actualValue)
            {
                return EqualityComparer<T>.Default.Equals(actualValue, expectedValue);
            }

            return false;
        }

        public T InvokeMethod<T>(object instance = null, object[] args = null)
        {
            if (instance == null)
            {
                instance = Activator.CreateInstance(Type, null);
            }

            if (instance.GetType() != Type)
            {
                throw new Exception(
                    $"Actual instance (of type {instance.GetType()}) doesn't match expected type {Type}");
            }
            var result = Method.Invoke(instance, args);
            if (result is T typedResult) return typedResult;
            throw new Exception($"Actual result (of type {result.GetType()}) doesn't match expected type {typeof(T)}");
        }
    }
}