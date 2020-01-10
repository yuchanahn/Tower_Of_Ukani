using System;
using UnityEngine;

namespace Dongjun.Helper
{
    public static class General
    {
        public static int EnumCount<T>() where T : Enum
        {
            return Enum.GetValues(typeof(T)).Length;
        }
    }

    public static class ArrayExtensions
    {
        public static int Push<T>(this T[] source, T value)
        {
            var index = Array.IndexOf(source, default);

            if (index != -1)
                source[index] = value;

            return index;
        }

        public static T GetClosest<T>(this Collider2D[] hits, Transform pivot) where T : Component
        {
            if (hits == null || hits.Length == 0)
                return null;

            T result = null;

            float dist = -1;
            for (int i = 0; i < hits.Length; i++)
            {
                float curDist = Vector2.Distance(hits[i].transform.position, pivot.position);

                if (dist == -1 || dist > curDist)
                {
                    T cur = hits[i].GetComponent<T>();
                    if (cur == null)
                        continue;

                    dist = curDist;
                    result = cur;
                }
            }

            return result;
        }
    }

    public static class VectorHelper
    {
        public static Vector2 Change(this Vector2 target, float? x = null, float? y = null)
        {
            if (x.HasValue) target.x = x.Value;
            if (y.HasValue) target.y = y.Value;
            return target;
        }
        public static Vector3 Change(this Vector3 target, float? x = null, float? y = null, float? z = null)
        {
            if (x.HasValue) target.x = x.Value;
            if (y.HasValue) target.y = y.Value;
            if (z.HasValue) target.z = z.Value;
            return target;
        }

        public static Vector2 Add(this Vector2 target, float? x = null, float? y = null)
        {
            if (x.HasValue) target.x += x.Value;
            if (y.HasValue) target.y += y.Value;
            return target;
        }
        public static Vector3 Add(this Vector3 target, float? x = null, float? y = null, float? z = null)
        {
            if (x.HasValue) target.x += x.Value;
            if (y.HasValue) target.y += y.Value;
            if (z.HasValue) target.z += z.Value;
            return target;
        }
    }
}
