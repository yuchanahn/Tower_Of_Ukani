using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public struct GroundDetectionData
{
    [Header("Layers")]
    public LayerMask GroundLayers;
    public LayerMask OneWayLayer;

    [Header("Collider")]
    public BoxCollider2D IW_Solid;

    [Header("Detection Value")]
    public float InnerSnapDist;
    public float OutterSnapDist;
    public float OffsetAmount;

    [HideInInspector] public Vector2 Size;
    [HideInInspector] public List<Collider2D> IgnoreGrounds;
    [HideInInspector] public bool isGrounded;

    [HideInInspector] public bool OnGroundEnter_Executed;
    [HideInInspector] public bool OnGroundExit_Executed;

    [HideInInspector] public RaycastHit2D[] hitGrounds;
    [HideInInspector] public Rigidbody2D groundRB;
}

public interface ICanDetectGround
{
    void OnGroundEnter();
    void OnGroundStay();
    void OnGroundExit();
}

public static class GroundDetection_Logic
{
    public static Vector2 detectDir = Vector2.down;

    public static void Reset_IW_Solid_Col_Size(this ref GroundDetectionData data)
    {
        data.IW_Solid.offset = Vector2.zero;
        data.IW_Solid.size = data.Size;
    }

    public static void DetectGround(this ref GroundDetectionData data, bool canDetect, Rigidbody2D rb2D, Transform tf)
    {
        #region Set Up
        data.isGrounded = false;
        data.hitGrounds = null;
        data.groundRB = null;

        Vector2 scaledSize = data.Size * tf.localScale;

        if (data.IW_Solid != null)
        {
            data.IW_Solid.size = !canDetect ? data.Size : new Vector2(data.Size.x, data.Size.y - (data.InnerSnapDist / tf.localScale.y));
            data.IW_Solid.offset = !canDetect ? new Vector2(0, 0) : new Vector2(0, (data.InnerSnapDist / tf.localScale.y) * 0.5f);
        }

        if (!canDetect)
        {
            data.Reset_IW_Solid_Col_Size();
            return;
        }
        #endregion

        #region Box Cast
        Vector2 castPos = new Vector2(tf.position.x, tf.position.y + scaledSize.y);
        float offset = data.OutterSnapDist + data.OffsetAmount;

        float deltaYDist = -rb2D.velocity.y * Time.fixedDeltaTime;
        float castDist = scaledSize.y + (deltaYDist > offset ? deltaYDist : offset);

        RaycastHit2D[] hits = Physics2D.BoxCastAll(castPos, scaledSize, 0f, detectDir, castDist, data.GroundLayers);
        if (hits.Length == 0)
            return;
        #endregion

        #region Get Highest Hit Point
        float? hitPointY = null;
        GameObject hitGround = null;

        for (int i = 0; i < hits.Length; i++)
        {
            if (data.IgnoreGrounds.Contains(hits[i].collider) || hits[i].point.y > tf.position.y - (scaledSize.y * 0.5f) + data.InnerSnapDist)
                continue;

            if (hitPointY is null || hitPointY < hits[i].point.y)
            {
                hitPointY = hits[i].point.y;
                hitGround = hits[i].collider.gameObject;
            }
        }

        if (hitPointY is null)
            return;
        #endregion

        #region Is Grounded
        // Set Data
        data.isGrounded = true;
        data.hitGrounds = Physics2D.BoxCastAll(tf.position, scaledSize, 0f, detectDir, data.OutterSnapDist, data.GroundLayers);
        data.groundRB = hitGround.GetComponent<Rigidbody2D>();

        // Snap To Highest Point
        tf.position = new Vector2(tf.position.x, hitPointY.Value + (scaledSize.y * 0.5f) + data.OffsetAmount);
        rb2D.velocity = new Vector2(rb2D.velocity.x, 0);
        #endregion
    }
    public static void ExecuteOnGroundMethod(this ref GroundDetectionData data, ICanDetectGround canDetectGround)
    {
        if (data.isGrounded)
        {
            if (!data.OnGroundEnter_Executed)
            {
                // On Ground Enter
                canDetectGround.OnGroundEnter();
                data.OnGroundEnter_Executed = true;
                data.OnGroundExit_Executed = false;
            }

            // On Ground Stay
            canDetectGround.OnGroundStay();
        }
        else if (!data.OnGroundExit_Executed)
        {
            // On Ground Exit
            canDetectGround.OnGroundExit();
            data.OnGroundEnter_Executed = false;
            data.OnGroundExit_Executed = true;
        }
    }

    public static void FallThrough(this GroundDetectionData data, ref bool input, Rigidbody2D rb2D, Transform tf, Collider2D oneWayCollider)
    {
        #region Overlap Box
        Vector2 detectPos = tf.position;
        Vector2 detectSize = data.Size * tf.localScale;
        if (rb2D.velocity.x != 0)
        {
            detectPos.x += (rb2D.velocity.x) / 2;
            detectSize.x += Mathf.Abs(rb2D.velocity.x);
        }
        detectPos.y -= data.OutterSnapDist * 0.5f;
        detectSize.y += data.OutterSnapDist;

        Collider2D[] overlaps = Physics2D.OverlapBoxAll(detectPos, detectSize, 0f, data.OneWayLayer);
        #endregion

        #region Enable Collsion
        if (overlaps != null && data.IgnoreGrounds != null)
        {
            for (int i = data.IgnoreGrounds.Count - 1; i >= 0; i--)
            {
                if (Array.Exists(overlaps, col => col == data.IgnoreGrounds[i])) continue;

                Physics2D.IgnoreCollision(oneWayCollider, data.IgnoreGrounds[i], false);
                data.IgnoreGrounds.RemoveAt(i);
            }
        }
        else
        {
            if (overlaps != null)
            {
                for (int i = 0; i < data.IgnoreGrounds.Count; i++)
                    Physics2D.IgnoreCollision(oneWayCollider, data.IgnoreGrounds[i], false);
            }
            data.IgnoreGrounds.Clear();
        }
        #endregion

        #region Disable Collision

        if (!input)
            return;

        input = false;

        if (overlaps is null || !data.isGrounded)
            return;

        for (int i = 0; i < overlaps.Length; i++)
        {
            if (data.IgnoreGrounds.Contains(overlaps[i]))
                continue;

            Physics2D.IgnoreCollision(oneWayCollider, overlaps[i], true);
            data.IgnoreGrounds.Add(overlaps[i]);
        }
        #endregion
    }

    public static void FollowMovingPlatform(this GroundDetectionData data, Rigidbody2D rb2D)
    {
        if (!data.isGrounded || data.groundRB == null)
            return;

        rb2D.velocity = new Vector2(rb2D.velocity.x + data.groundRB.velocity.x, data.groundRB.velocity.y);
    }
}