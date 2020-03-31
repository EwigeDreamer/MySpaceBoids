using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MyTools.Triggers.Base;

namespace MyTools.Triggers
{
    public class RigidbodyComponentTrigger<TComp> : Trigger<TComp> where TComp : Component
    {
        protected override void EnterEvent(Collider col)
        {
            var rb = col.attachedRigidbody;
            if (rb == null) return;
            var c = rb.GetComponent<TComp>();
            if (c == null) return;
            Enter(c);
        }
        protected override void ExitEvent(Collider col)
        {
            var rb = col.attachedRigidbody;
            if (rb == null) return;
            var c = rb.GetComponent<TComp>();
            if (c == null) return;
            Exit(c);
        }
    }
}