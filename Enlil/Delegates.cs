using System;
using System.Collections.Generic;
using Enlil.Domain;

namespace Enlil
{
    public delegate RunAction<T> EachAction<T>(Action<T> action);
    public delegate MethodFilter GetMethodFilterForAttributeName(string name);
    public delegate TypeFilter GetTypeFilterForAttributeName(string name);
    public delegate ReflectionAction<IEnumerable<MethodOnType>,MethodOnType> MethodFilter(BuildContext context);
    public delegate void RunAction<T>(T element);
    public delegate ReflectionAction<IEnumerable<Type>,Type> TypeFilter(BuildContext context);
}