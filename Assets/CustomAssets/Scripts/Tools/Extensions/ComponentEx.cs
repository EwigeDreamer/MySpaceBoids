namespace MyTools.Extensions.Components
{
    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;
    using System;
    using System.Linq;

    #region BehaviourEnabled
    public static class EnabledBehaviourEx
    {
        public static TObj SetEnabled<TObj>(this TObj component, bool state) where TObj : Behaviour
        {
            if (component == null) throw new ArgumentNullException(nameof(component));
            component.enabled = state;
            return component;
        }
        public static TColl SetEnabled<TColl, TObj>(this TColl collection, bool state)
            where TColl : IList<TObj>
            where TObj : Behaviour
        {
            if (collection == null) throw new ArgumentNullException(nameof(collection));
            var count = collection.Count;
            for (int i = 0; i < count; ++i) collection[i].enabled = state;
            return collection;
        }
        public static TColl SetEnabledOne<TColl, TObj>(this TColl collection, int index, bool state)
            where TColl : IList<TObj>
            where TObj : Behaviour
        {
            if (collection == null) throw new ArgumentNullException(nameof(collection));
            var count = collection.Count;
            for (int i = 0; i < count; ++i) collection[i].enabled = !state;
            if (index > -1 && index < count) collection[index].enabled = state;
            return collection;
        }
        public static TColl SetEnabledOne<TColl, TObj>(this TColl collection, TObj one, bool state)
            where TColl : IList<TObj>
            where TObj : Behaviour
        {
            if (collection == null) throw new ArgumentNullException(nameof(collection));
            var count = collection.Count;
            for (int i = 0; i < count; ++i) collection[i].enabled = collection[i] == one ? state : !state;
            return collection;
        }
    }
    #endregion

    #region ColliderEnabled
    public static class EnabledColliderEx
    {
        public static TObj SetEnabled<TObj>(this TObj collider, bool state) where TObj : Collider
        {
            if (collider == null) throw new ArgumentNullException(nameof(collider));
            collider.enabled = state;
            return collider;
        }
        public static TColl SetEnabled<TColl, TObj>(this TColl collection, bool state)
            where TColl : IList<TObj>
            where TObj : Collider
        {
            if (collection == null) throw new ArgumentNullException(nameof(collection));
            var count = collection.Count;
            for (int i = 0; i < count; ++i) collection[i].enabled = state;
            return collection;
        }
        public static TColl SetEnabledOne<TColl, TObj>(this TColl collection, int index, bool state)
            where TColl : IList<TObj>
            where TObj : Collider
        {
            if (collection == null) throw new ArgumentNullException(nameof(collection));
            var count = collection.Count;
            for (int i = 0; i < count; ++i) collection[i].enabled = !state;
            if (index > -1 && index < count) collection[index].enabled = state;
            return collection;
        }
        public static TColl SetEnabledOne<TColl, TObj>(this TColl collection, TObj one, bool state)
            where TColl : IList<TObj>
            where TObj : Collider
        {
            if (collection == null) throw new ArgumentNullException(nameof(collection));
            var count = collection.Count;
            for (int i = 0; i < count; ++i) collection[i].enabled = collection[i] == one ? state : !state;
            return collection;
        }
    }
    #endregion

    #region RendererEnabled
    public static class EnabledRendererEx
    {
        public static TObj SetEnabled<TObj>(this TObj renderer, bool state) where TObj : Renderer
        {
            if (renderer == null) throw new ArgumentNullException(nameof(renderer));
            renderer.enabled = state;
            return renderer;
        }
        public static TColl SetEnabled<TColl, TObj>(this TColl collection, bool state)
            where TColl : IList<TObj>
            where TObj : Renderer
        {
            if (collection == null) throw new ArgumentNullException(nameof(collection));
            var count = collection.Count;
            for (int i = 0; i < count; ++i) collection[i].enabled = state;
            return collection;
        }
        public static TColl SetEnabledOne<TColl, TObj>(this TColl collection, int index, bool state)
            where TColl : IList<TObj>
            where TObj : Renderer
        {
            if (collection == null) throw new ArgumentNullException(nameof(collection));
            var count = collection.Count;
            for (int i = 0; i < count; ++i) collection[i].enabled = !state;
            if (index > -1 && index < count) collection[index].enabled = state;
            return collection;
        }
        public static TColl SetEnabledOne<TColl, TObj>(this TColl collection, TObj one, bool state)
            where TColl : IList<TObj>
            where TObj : Renderer
        {
            if (collection == null) throw new ArgumentNullException(nameof(collection));
            var count = collection.Count;
            for (int i = 0; i < count; ++i) collection[i].enabled = collection[i] == one ? state : !state;
            return collection;
        }
    }
    #endregion

    #region RigidbodyKinematic
    public static class RigidbodyEx
    {
        public static Rigidbody SetKinematic(this Rigidbody rigidbody, bool state)
        {
            if (rigidbody == null) throw new ArgumentNullException(nameof(rigidbody));
            rigidbody.isKinematic = state;
            return rigidbody;
        }
        public static TColl SetKinematic<TColl>(this TColl collection, bool state)
            where TColl : IList<Rigidbody>
        {
            if (collection == null) throw new ArgumentNullException(nameof(collection));
            var count = collection.Count;
            for (int i = 0; i < count; ++i) collection[i].isKinematic = state;
            return collection;
        }
        public static TColl SetKinematicOne<TColl>(this TColl collection, int index, bool state)
            where TColl : IList<Rigidbody>
        {
            if (collection == null) throw new ArgumentNullException(nameof(collection));
            var count = collection.Count;
            for (int i = 0; i < count; ++i) collection[i].isKinematic = !state;
            if (index > -1 && index < count) collection[index].isKinematic = state;
            return collection;
        }
        public static TColl SetKinematicOne<TColl>(this TColl collection, Rigidbody one, bool state)
            where TColl : IList<Rigidbody>
        {
            if (collection == null) throw new ArgumentNullException(nameof(collection));
            var count = collection.Count;
            for (int i = 0; i < count; ++i) collection[i].isKinematic = collection[i] == one ? state : !state;
            return collection;
        }
    }
    #endregion
}
