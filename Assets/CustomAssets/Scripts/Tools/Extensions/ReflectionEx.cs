namespace MyTools.Extensions.Reflection
{
    using System;
    using System.Reflection;
    public static class ReflectionExtensions
    {
        public static void InvokePrivateMethod(this object obj, string methodName, params object[] parameters)
        {
            Type t = obj.GetType();
            MethodInfo m = t.GetMethod(methodName, BindingFlags.NonPublic | BindingFlags.Instance);
            while (m == null && t != typeof(object))
            {
                t = t.BaseType;
                m = t.GetMethod(methodName, BindingFlags.NonPublic | BindingFlags.Instance);
            }
            m?.Invoke(obj, parameters);
        }
    }
}