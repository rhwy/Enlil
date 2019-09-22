using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Enlil.Domain;

namespace Enlil
{
    public class AssemblyHelper
    {
        public static WorkingContext CreatWorkingContext(BuildContext buildContext)
        {
            return new WorkingContext(buildContext);
        }

        public static IEnumerable<Type> GetTypesByAttributeName(Assembly asm, string name)
        {
            if (string.IsNullOrEmpty(name)) return Enumerable.Empty<Type>();
            
            name = name.EndsWith("Attribute") ? name : name + "Attribute";

            return asm
                .GetExportedTypes()
                .Where(t => t.CustomAttributes.Any(a => a.AttributeType.Name == name));
        }
        public static IEnumerable<MethodOnType> GetTypesByAttributeNameOnMethod(Assembly asm, string name)
        {
            name = name.EndsWith("Attribute") ? name : name + "Attribute";

            return asm
                .GetExportedTypes()
                .SelectMany(
                    type => type.GetMethods().Select(method => new MethodOnType(type,method)))
                .Where(
                    element => element.Method.CustomAttributes.Any(
                        attrib => attrib.AttributeType.Name == name)); 
        }

        public static IEnumerable<Type> GetTypesImplementingInterface(Assembly asm, string name)
        {
            return asm.GetExportedTypes().Where(
                type => type.GetInterfaces().Any(i => i.Name == name));
        }
    }

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
    public struct WorkingContext
    {
        public BuildContext BuildContext { get; }

        public WorkingContext(BuildContext buildContext)
        {
            BuildContext = buildContext;
        }
    }
}