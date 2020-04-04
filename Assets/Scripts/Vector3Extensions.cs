using UnityEngine;

public static class Vector3Extensions
{
        public static Vector3 ToVector3(this Vector3Int v3) 
                => new Vector3(v3.x, v3.y, v3.z);

        public static Vector3Int ToVector3Int(this Vector3 v3)
        {
                return new Vector3Int((int) v3.x, (int) v3.y, (int) v3.z);
        }
}