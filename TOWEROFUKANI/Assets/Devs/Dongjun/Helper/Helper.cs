using UnityEngine;

namespace Dongjun.Helper
{
    public static class General
    {
        public static bool IsNull(this GameObject go)
        {
            try
            {
                string temp = go.name;
            }
            catch
            {
                return true;
            }

            return false;
        }

        public static T GetClosest<T>(this Collider2D[] hits, Transform pivot)
            where T : MonoBehaviour
        {
            if (hits is null || hits.Length == 0)
                return null;

            T result = null;

            float dist = -1;
            for (int i = 0; i < hits.Length; i++)
            {
                if (dist == -1 || dist > Vector2.Distance(hits[i].transform.position, pivot.position))
                    result = hits[i].GetComponent<T>();
            }

            return result;
        }
    }
}
