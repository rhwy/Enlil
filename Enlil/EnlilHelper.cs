using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using System.Runtime.CompilerServices;
using Enlil.Domain;

namespace Enlil
{
    public static class EnlilHelper
    {
        public static BuildContext BuildAssemblyForPath => BuildContext.Default;
        public static GetTypeFilterForAttributeName TypesForAttribute = name => (context) =>
            new ReflectionAction<IEnumerable<Type>,Type>(
                AssemblyHelper.GetTypesByAttributeName(context.ResultingAssembly, name));

        public static GetMethodFilterForAttributeName MethodTypesForAttribute = name => (context) =>
            new ReflectionAction<IEnumerable<MethodOnType>,MethodOnType>(
                AssemblyHelper.GetTypesByAttributeNameOnMethod(context.ResultingAssembly, name));
        
        public static RunAction<T> Each<T>(Action<T> a) => (T e) => a(e);

    }

    public static class Reflectionextensions
    {
        public static void Each<T>(this ReflectionAction<IEnumerable<T>,T> reflectionAction, Action<T> action)
        {
            foreach (var element in reflectionAction.Element)
            {
                action(element);
            }
        }
    }

    public class ReflectionAction<T,U> where T : IEnumerable<U>
    {
        public T Element { get; }

        public ReflectionAction(T element)
        {
            Element = element;
        }

        public static ReflectionAction<T,U> operator | (
            ReflectionAction<T,U> reflectionAction, RunAction<U> action)
        {
            if (reflectionAction.Element is IEnumerable<U> elements)
            {
                foreach (var element in elements)
                {
                    action(element);
                }
            }

            return reflectionAction;
        }

    }
    public delegate TypeFilter GetTypeFilterForAttributeName(string name);
    public delegate MethodFilter GetMethodFilterForAttributeName(string name);
    
    public delegate ReflectionAction<IEnumerable<Type>,Type> TypeFilter(BuildContext context);
    public delegate ReflectionAction<IEnumerable<MethodOnType>,MethodOnType> MethodFilter(BuildContext context);

    public delegate void RunAction<T>(T element);
    public delegate RunAction<T> EachAction<T>(Action<T> action);
}