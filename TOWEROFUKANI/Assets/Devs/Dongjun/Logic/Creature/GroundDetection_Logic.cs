using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public struct GroundDetectionData
{
    [HideInInspector] public Vector2 Size;
    [HideInInspector] public List<Collider2D> IgnoreGrounds;

    [HideInInspector] public bool OnGroundEnter_Executed;
    [HideInInspector] public bool OnGroundExit_Executed;

    [Header("Layers")]
    public LayerMask GroundLayers;
    public LayerMask OneWayLayer;

    [Header("Collider")]
    public BoxCollider2D IW_Solid;

    [Header("Detection Value")]
    public float InnerSnapDist;
    public float OutterSnapDist;
    public float OffsetAmount;
}

public struct GroundInfo
{
    public Collider2D Col;
    public GameObject GO;
    public float HitPointY;

    public void Reset()
    {
        Col = null;
        GO = null;
        HitPointY = 0;
    }
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

    public static void DetectGround(bool canDetect, Rigidbody2D rb2D, Transform tf, GroundDetectionData data, ref bool isGrounded, ref GroundInfo groundInfo)
    {
        #region Set Up
        groundInfo.Reset();
        isGrounded = false;

        Vector2 scaledSize = data.Size * tf.localScale;

        if (data.IW_Solid != null)
        {
            data.IW_Solid.size = !canDetect ? data.Size : new Vector2(data.Size.x, data.Size.y - (data.InnerSnapDist / tf.localScale.y));
            data.IW_Solid.offset = !canDetect ? new Vector2(0, 0) : new Vector2(0, (data.InnerSnapDist / tf.localScale.y) * 0.5f);
        }

        if (!canDetect) return;
        #endregion

        #region Box Cast
        Vector2 castPos = new Vector2(tf.position.x, tf.position.y + scaledSize.y);
        float offset = data.OutterSnapDist + data.OffsetAmount;

        float deltaYDist = -rb2D.velocity.y * Time.fixedDeltaTime;
        float castDist = scaledSize.y + (deltaYDist > offset ? deltaYDist : offset);

        RaycastHit2D[] hits = Physics2D.BoxCastAll(castPos, scaledSize, 0f, detectDir, castDist, data.GroundLayers);
        if (hits == null) return;
        #endregion

        #region Get Highest Hit Point
        for (int i = 0; i < hits.Length; i++)
        {
            if (data.IgnoreGrounds.Contains(hits[i].collider) ||
                hits[i].point.y > tf.position.y - (scaledSize.y * 0.5f) + data.InnerSnapDist)
                continue;

            if (groundInfo.Col == null || groundInfo.HitPointY < hits[i].point.y)
            {
                groundInfo.Col = hits[i].collider;
                groundInfo.GO = hits[i].collider.gameObject;
                groundInfo.HitPointY = hits[i].point.y;
            }
        }
        if (groundInfo.Col == null) return;
        #endregion

        #region Is Grounded
        isGrounded = true;

        // Snap To Highest Point
        if (rb2D.velocity.y <= 0)
        {
            rb2D.velocity = new Vector2(rb2D.velocity.x, 0);
            tf.position = new Vector2(tf.position.x, groundInfo.HitPointY + (scaledSize.y * 0.5f) + data.OffsetAmount);
        }

        Rigidbody2D groundRB = groundInfo.GO.GetComponent<Rigidbody2D>();
        if (groundRB) rb2D.velocity = new Vector2(rb2D.velocity.x, groundRB.velocity.y);
        #endregion
    }
    public static void ExecuteOnGroundMethod(ICanDetectGround canDetectGround, bool isGrounded, ref GroundDetectionData data)
    {
        if (isGrounded)
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

    public static void FallThrough(ref bool input, bool isGrounded, Rigidbody2D rb2D, Transform tf, Collider2D oneWayCollider, GroundDetectionData detectionData)
    {
        #region Overlap Box
        Vector2 detectPos = tf.position;
        Vector2 detectSize = detectionData.Size * tf.localScale;
        if (rb2D.velocity.x != 0)
        {
            detectPos.x += (rb2D.velocity.x) / 2;
            detectSize.x += Mathf.Abs(rb2D.velocity.x);
        }
        detectPos.y -= detectionData.OutterSnapDist * 0.5f;
        detectSize.y += detectionData.OutterSnapDist;

        Collider2D[] overlaps = Physics2D.OverlapBoxAll(detectPos, detectSize, 0f, detectionData.OneWayLayer);
        #endregion

        #region Enable Collsion
        if (overlaps != null && detectionData.IgnoreGrounds != null)
        {
            for (int i = detectionData.IgnoreGrounds.Count - 1; i >= 0; i--)
            {
                if (Array.Exists(overlaps, col => col == detectionData.IgnoreGrounds[i])) continue;

                Physics2D.IgnoreCollision(oneWayCollider, detectionData.IgnoreGrounds[i], false);
                detectionData.IgnoreGrounds.Remove(detectionData.IgnoreGrounds[i]);
            }
        }
        else
        {
            if (overlaps != null)
            {
                for (int i = 0; i < detectionData.IgnoreGrounds.Count; i++)
                    Physics2D.IgnoreCollision(oneWayCollider, detectionData.IgnoreGrounds[i], false);
            }
            detectionData.IgnoreGrounds.Clear();
        }
        #endregion

        #region Disable Collision

        if (!input)
            return;

        input = false;

        if (overlaps == null || !isGrounded)
            return;

        for (int i = 0; i < overlaps.Length; i++)
        {
            if (detectionData.IgnoreGrounds.Contains(overlaps[i]))
                continue;

            Physics2D.IgnoreCollision(oneWayCollider, overlaps[i], true);
            detectionData.IgnoreGrounds.Add(overlaps[i]);
        }
        #endregion
    }
}