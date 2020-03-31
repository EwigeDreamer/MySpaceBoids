namespace MyTools.Extensions.Vectors
{
    using UnityEngine;
    public static class VectorEx
    {
        public static Vector2 ToV2_xz(this Vector3 v) { return new Vector2(v.x, v.z); }
        public static Vector2 ToV2_xy(this Vector3 v) { return new Vector2(v.x, v.y); }
        public static Vector2 ToV2_zy(this Vector3 v) { return new Vector2(v.z, v.y); }
        public static Vector2 ToV2_yx(this Vector3 v) { return new Vector2(v.y, v.x); }

        public static Vector3 ToV3_0yz(this Vector3 v) { v.x = 0f; return v; }
        public static Vector3 ToV3_x0z(this Vector3 v) { v.y = 0f; return v; }
        public static Vector3 ToV3_xy0(this Vector3 v) { v.z = 0f; return v; }
        public static Vector3 ToV3_xz0(this Vector3 v) { v.y = v.z; v.z = 0f; return v; }

        public static Vector3 ToV3_xy0(this Vector2 v) { return new Vector3(v.x, v.y, 0f); }
        public static Vector3 ToV3_x0y(this Vector2 v) { return new Vector3(v.x, 0f, v.y); }
        public static Vector3 ToV3_0yx(this Vector2 v) { return new Vector3(0f, v.y, v.x); }
        public static Vector3 ToV3_yx0(this Vector2 v) { return new Vector3(v.y, v.x, 0f); }

        public static Vector3 Rotate(this Vector3 v, Quaternion rot) { return rot * v; }

        public static Vector3 SetX(this Vector3 v, float x) { v.x = x; return v; }
        public static Vector3 SetY(this Vector3 v, float y) { v.y = y; return v; }
        public static Vector3 SetZ(this Vector3 v, float z) { v.z = z; return v; }
    }
}