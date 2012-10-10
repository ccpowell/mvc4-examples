using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DRCOG.Domain.Interfaces;

namespace DRCOG.Domain.Factories
{
    //public class AddOnFactoryElement<T> : IAddOnElement
    //    where T : new()
    //{
    //    public object New()
    //    {
    //        return new T();
    //    }
    //}

    //public class AddOnFactory<K, T> where K : IComparable
    //{
    //    Dictionary<K, Func<T>> elements = new Dictionary<K, Func<T>>();

    //    public void Add<V>(K key) where V : T, new()
    //    {
    //        elements.Add(key, () => new V());
    //    }

    //    public T Create(K key)
    //    {
    //        if (elements.ContainsKey(key))
    //        {
    //            return elements[key]();
    //        }
    //        throw new ArgumentException();
    //    }
    //}

    public class GenericAddOnFactory<I, C>
        where C : I
    {
        public static Func<object[], I> Dispensor { get; set; }

        public static I CreateInstance(params object[] args)
        {
            if (Dispensor != null)
                return Dispensor(args);
            return (I)Activator.CreateInstance(typeof(C), args);
        }
    }
}
