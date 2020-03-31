using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MyTools.Triggers.Base;

namespace MyTools.Triggers
{
    public class GameObjectTrigger : Trigger<GameObject>
    {
        protected override void EnterEvent(Collider col)
        {
            Enter(col.gameObject);
        }
        protected override void ExitEvent(Collider col)
        {
            Exit(col.gameObject);
        }
    }
}