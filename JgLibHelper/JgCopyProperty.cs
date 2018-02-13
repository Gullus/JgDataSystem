using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

namespace JgLibHelper
{
    public class JgCopyProperty<T>
    {
        private static readonly Action<T, T>[] _CopyProps = Prepare();

        private static Action<T, T>[] Prepare()
        {
            Type typeSchnitt = typeof(T);
            var types = new List<Type>(typeSchnitt.GetInterfaces());
            types.Add(typeSchnitt);

            ParameterExpression source = Expression.Parameter(typeSchnitt, "source");
            ParameterExpression target = Expression.Parameter(typeSchnitt, "target");

            var erg = new List<Action<T, T>>();

            foreach (var type in types)
            {
                var copyProps = from prop in type.GetProperties(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic)
                                where prop.CanRead && prop.CanWrite
                                let getExpr = Expression.Property(source, prop)
                                let setExpr = Expression.Call(target, prop.GetSetMethod(true), getExpr)
                                select Expression.Lambda<Action<T, T>>(setExpr, source, target).Compile();

                erg.AddRange(copyProps.ToList());
            }

            return erg.ToArray();
        }

        public JgCopyProperty()
        { }

        public T CopyProperties(T source, T target)
        {
            foreach (Action<T, T> copyProp in _CopyProps)
                copyProp(source, target);
            return target;
        }
    }
}
