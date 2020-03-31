using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using MyTools.Helpers;

namespace MyTools.Triggers.Base
{
    public class Trigger<T> : ImprovedBehaviour
    {
        public event Action<T> OnEnter = delegate { };
        public event Action<T> OnExit = delegate { };

        protected void Enter(T obj) { OnEnter(obj); }
        protected void Exit(T obj) { OnExit(obj); }

        private void OnTriggerEnter(Collider col) { EnterEvent(col); }
        private void OnTriggerExit(Collider col) { ExitEvent(col); }

        protected virtual void EnterEvent(Collider col) { }
        protected virtual void ExitEvent(Collider col) { }

        public void SetActive(bool state) => gameObject.SetActive(state);
    }
}