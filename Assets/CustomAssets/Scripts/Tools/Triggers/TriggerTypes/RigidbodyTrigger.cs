using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MyTools.Triggers.Base;

namespace MyTools.Triggers
{
    public class RigidbodyTrigger : Trigger<Rigidbody>
    {
        protected override void EnterEvent(Collider col)
        {
            var rb = col.attachedRigidbody;
            if (rb == null) return;
            Enter(rb);
        }
        protected override void ExitEvent(Collider col)
        {
            var rb = col.attachedRigidbody;
            if (rb == null) return;
            Exit(rb);
        }
    }
}