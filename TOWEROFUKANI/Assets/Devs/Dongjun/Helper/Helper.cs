using UnityEngine;

namespace Dongjun.Helper
{
    public static class General
    {
        public static bool IsNull(this MonoBehaviour monoBehaviour)
        {
            try
            {
                string temp = monoBehaviour.name;
            }
            catch
            {
                return true;
            }

            return false;
        }
        public static bool IsNull(this GameObject gameObject)
        {
            try
            {
                string temp = gameObject.name;
            }
            catch
            {
                return true;
            }

            return false;
        }

        public static T GetClosest<T>(this Collider2D[] hits, Transform pivot)
            where T : Component
        {
            if (hits == null || hits.Length == 0)
                return null;

            T result = null;

            float dist = -1;
            for (int i = 0; i < hits.Length; i++)
            {
                if (dist == -1 || dist > Vector2.Distance(hits[i].transform.position, pivot.position))
                {
                    T cur = hits[i].GetComponent<T>();
                    if (cur == null) continue;

                    result = cur;
                }
            }

            return result;
        }
    }
}
