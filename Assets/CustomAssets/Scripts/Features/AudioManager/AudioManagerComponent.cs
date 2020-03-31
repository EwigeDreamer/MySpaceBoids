using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace AudioManagerCostyls
{
    public class AudioManagerComponent : MonoBehaviour
    {
        public event Action OnStart = delegate { };
        private void Start()
        {
            OnStart();
        }
    }
}