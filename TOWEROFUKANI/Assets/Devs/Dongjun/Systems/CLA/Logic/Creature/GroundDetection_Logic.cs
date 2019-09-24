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

    public LayerMask GroundLayers;
    public LayerMask OneWayLayer;

    public BoxCollider2D IW_Solid;

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
    public static void DetectGround(
        bool canDetect,
        Rigidbody2D rb2D,
        Transform tf,
        GroundDetectionData detectionData,
        ref bool isGrounded,
        ref GroundInfo groundInfo)
    {
        #region Set Up
        groundInfo.Reset();
        isGrounded = false;

        if (!canDetect)
        {
            if (detectionData.IW_Solid != null)
            {
                detectionData.IW_Solid.size = new Vector2(detectionData.Size.x, detectionData.Size.y);
                detectionData.IW_Solid.offset = new Vector2(0, 0);
            }
            return;
        }
        else
        {
            if (detectionData.IW_Solid != null)
            {
                detectionData.IW_Solid.size = new Vector2(detectionData.Size.x, detectionData.Size.y - detectionData.InnerSnapDist);
                detectionData.IW_Solid.offset = new Vector2(0, detectionData.InnerSnapDist * 0.5f);
            }
        }
        #endregion

        #region Box Cast
        Vector2 castPos = new Vector2(tf.position.x, tf.position.y + detectionData.Size.y);
        float offset = detectionData.OutterSnapDist + detectionData.OffsetAmount;
        float castDist = detectionData.Size.y + (-rb2D.velocity.y * Time.fixedDeltaTime > offset ? -rb2D.velocity.y * Time.fixedDeltaTime : offset);

        RaycastHit2D[] hits = Physics2D.BoxCastAll(castPos, detectionData.Size, 0f, Vector2.down, castDist, detectionData.GroundLayers);
        if (hits == null) return;
        #endregion

        #region Get Highest Hit Point
        for (int i = 0; i < hits.Length; i++)
        {
            if (detectionData.IgnoreGrounds.Contains(hits[i].collider) || 
                hits[i].point.y > tf.position.y - (detectionData.Size.y * 0.5f) + detectionData.InnerSnapDist)
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
        rb2D.velocity = new Vector2(rb2D.velocity.x, 0);
        tf.position = new Vector2(tf.position.x, groundInfo.HitPointY + (detectionData.Size.y * 0.5f) + detectionData.OffsetAmount);
        #endregion
    }

    public static void ExecuteOnGroundMethod(
        ICanDetectGround canDetectGround, 
        bool isGrounded, 
        ref GroundDetectionData data)
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

    public static void FallThrough(
        ref bool input_FallThrough,
        bool isGrounded,
        Rigidbody2D rb2D,
        Transform tf,
        Collider2D oneWayCollider,
        GroundDetectionData detectionData)
    {
        #region Overlap Box
        Vector2 detectPos = tf.position;
        Vector2 detectSize = detectionData.Size;
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

        if (!input_FallThrough)
            return;

        input_FallThrough = false;

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