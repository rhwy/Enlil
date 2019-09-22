using System.Collections.Generic;

namespace Enlil
{
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
}