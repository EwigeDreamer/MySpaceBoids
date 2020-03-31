using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MyTools.Factory
{
    public interface IAbstractFactory<TInfo, TObj>
    {
        TObj GetObject(TInfo info);
        bool TryGetObject(TInfo info, out TObj obj);
    }
    public interface IFactory<TObj>
    {
        TObj GetObject();
        bool TryGetObject(out TObj obj);
    }
}
