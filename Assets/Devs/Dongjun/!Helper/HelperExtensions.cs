using System;
using UnityEngine;

namespace Dongjun.Helper
{
    public static class EnumHelper
    {
        public static int Count<T>() where T : Enum
        {
            return Enum.GetValues(typeof(T)).Length;
        }
    }

    public static class General
    {
        public static bool IsPrefab(this GameObject go)
        {
            return !go.scene.IsValid();
        }
        public static void GetComponent<T>(this GameObject go, ref T variable) where T : Component
        {
            variable = go.GetComponent<T>();
        }

        public static void ClearChildren(this GameObject target)
        {
            for (int i = 0; i < target.transform.childCount; i++)
            {
                GameObject.Destroy(target.transform.GetChild(i).gameObject);
            }
        }
        public static void ClearChildren(this Transform target)
        {
            for (int i = 0; i < target.childCount; i++)
            {
                GameObject.Destroy(target.GetChild(i).gameObject);
            }
        }
    }

    public static class ArrayExtensions
    {
        public static T GetClosest<T>(this Collider2D[] cols, Transform pivot) where T : class
        {
            if (cols == null || cols.Length == 0)
                return null;

            T result = null;

            float dist = -1;
            for (int i = 0; i < cols.Length; i++)
            {
                float curDist = Vector2.Distance(cols[i].transform.position, pivot.position);

                if (dist == -1 || dist > curDist)
                {
                    T cur = cols[i].GetComponent<T>();
                    if (cur == null)
                        continue;

                    dist = curDist;
                    result = cur;
                }
            }

            return result;
        }
        public static GameObject GetClosest(this Collider2D[] cols, Transform pivot)
        {
            if (cols == null || cols.Length == 0)
                return null;

            GameObject result = null;

            float dist = -1;
            for (int i = 0; i < cols.Length; i++)
            {
                float curDist = Vector2.Distance(cols[i].transform.position, pivot.position);

                if (dist == -1 || dist > curDist)
                {
                    GameObject cur = cols[i].gameObject;
                    if (cur == null)
                        continue;

                    dist = curDist;
                    result = cur;
                }
            }

            return result;
        }
    }

    public static class AnimationHelper
    {
        public static float CurrentClipTime(this Animator animator)
        {
            return animator.GetCurrentAnimatorStateInfo(0).length * (1 - animator.GetCurrentAnimatorStateInfo(0).normalizedTime);
        }
        public static float CurrentClipLength(this Animator animator)
        {
            return animator.GetCurrentAnimatorStateInfo(0).length;
        }

        public static bool CheckCurrentClipName(this Animator animator, string nameToCompare)
        {
            return animator.GetCurrentAnimatorStateInfo(0).IsName(nameToCompare);
        }
        public static float GetNormalizedTime(this Animator animator)
        {
            return animator.GetCurrentAnimatorStateInfo(0).normalizedTime;
        }

        public static void SetDuration(this Animator animator, float duration, string animName = null)
        {
            if ((animName != null && !animator.CheckCurrentClipName(animName)) || duration <= 0)
            {
                animator.speed = 1;
                return;
            }

            animator.speed = animator.GetCurrentAnimatorClipInfo(0)[0].clip.length / duration;
        }
        public static void SetDuration(this Animator animator, float duration, float maxDuration, string animName = null)
        {
            if ((animName != null && !animator.CheckCurrentClipName(animName)) || duration <= 0)
            {
                animator.speed = 1;
                return;
            }

            duration = maxDuration <= 0 || duration < maxDuration ? duration : maxDuration;
            animator.speed = animator.GetCurrentAnimatorClipInfo(0)[0].clip.length / duration;
        }
        public static void ResetSpeed(this Animator animator)
        {
            animator.speed = 1;
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

        public static Vector2 Clamp(this Vector2 target, 
            float? minX = null, 
            float? maxX = null, 
            float? minY = null,
            float? maxY = null)
        {
            target.x = Mathf.Clamp(target.x, minX ?? float.MinValue, maxX ?? float.MaxValue);
            target.y = Mathf.Clamp(target.y, minY ?? float.MinValue, maxY ?? float.MaxValue);
            return target;
        }
        public static Vector2 Clamp(this Vector2 target, Vector2? min, Vector2? max)
        {
            target.x = Mathf.Clamp(target.x, min == null ? float.MinValue : min.Value.x, max == null ? float.MinValue : max.Value.x);
            target.y = Mathf.Clamp(target.y, min == null ? float.MinValue : min.Value.y, max == null ? float.MinValue : max.Value.y);
            return target;
        }
        public static Vector3 Clamp(this Vector3 target,
            float? minX = null,
            float? maxX = null,
            float? minY = null,
            float? maxY = null,
            float? minZ = null,
            float? maxZ = null)
        {
            target.x = Mathf.Clamp(target.x, minX ?? float.MinValue, maxX ?? float.MaxValue);
            target.y = Mathf.Clamp(target.y, minY ?? float.MinValue, maxY ?? float.MaxValue);
            target.y = Mathf.Clamp(target.z, minZ ?? float.MinValue, maxZ ?? float.MaxValue);
            return target;
        }
        public static Vector3 Clamp(this Vector3 target, Vector3? min, Vector3? max)
        {
            target.x = Mathf.Clamp(target.x, min == null ? float.MinValue : min.Value.x, max == null ? float.MinValue : max.Value.x);
            target.y = Mathf.Clamp(target.y, min == null ? float.MinValue : min.Value.y, max == null ? float.MinValue : max.Value.y);
            target.z = Mathf.Clamp(target.z, min == null ? float.MinValue : min.Value.z, max == null ? float.MinValue : max.Value.z);
            return target;
        }
    }
}
