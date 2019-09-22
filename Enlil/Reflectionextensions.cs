using System;
using System.Collections.Generic;

namespace Enlil
{
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
}