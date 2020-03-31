namespace MyTools.Extensions.Quaternions
{
    using UnityEngine;
    public static class QuaternionEx
    {
        public static Quaternion InverseQt(this Quaternion q) { return Quaternion.Inverse(q); }
        public static Quaternion AddQt(this Quaternion q, Quaternion toAdd) { return toAdd * q; }
        public static Quaternion GetTowardsQt(this Quaternion q, Quaternion towards) { return DifferenceQt(q, towards); }

        public static Quaternion DifferenceQt(Quaternion a, Quaternion b) { return b * Quaternion.Inverse(a); }
    }
}