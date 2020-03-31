using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MyTools.Triggers.Base;

namespace MyTools.Triggers
{
    public class ComponentInParentTrigger<TComp> : Trigger<TComp> where TComp : Component
    {
        protected override void EnterEvent(Collider col)
        {
            var c = col.GetComponentInParent<TComp>();
            if (c == null) return;
            Enter(c);
        }
        protected override void ExitEvent(Collider col)
        {
            var c = col.GetComponentInParent<TComp>();
            if (c == null) return;
            Exit(c);
        }
    }
}