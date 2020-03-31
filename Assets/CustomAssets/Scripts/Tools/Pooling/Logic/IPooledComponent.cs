using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace MyTools.Pooling
{
    public interface IPooledComponent
    {
        event Action Deactive;
        void OnActivation();
        void OnDeactivation();
    }
}
