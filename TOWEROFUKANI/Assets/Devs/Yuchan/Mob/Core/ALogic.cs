using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ALogic
{
    public static class Detect
    {
        public static Vector2? GetHitPosOrNull(Vector2 ori, Vector2 dir, float dist, LayerMask lyr)
        {
            var c = Physics2D.Raycast(ori, dir, dist, lyr);

            Vector2? Wall = null;

            if (c.collider)
            {
                Wall = c.collider.transform.position;
            }
            return Wall;
        }

        public static Transform GetHitTrOrNull(Vector2 ori, Vector2 dir, float dist, LayerMask lyr)
        {
            var c = Physics2D.Raycast(ori, dir, dist, lyr);

            Transform Wall = null;

            if (c.collider)
            {
                Wall = c.collider.transform;
            }
            return Wall;
        }
    }
}
