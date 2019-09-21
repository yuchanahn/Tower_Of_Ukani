using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public struct GroundDetectionData
{
    [HideInInspector]
    public Vector2 size;

    public List<Collider2D> ignoreGrounds;

    public float snapInnerDist;
    public float snapOutterDist;
    public float offsetAmount;

    public LayerMask groundLayers;
    public LayerMask oneWayLayer;
}

public struct GroundInfo
{
    public Collider2D collider;
    public GameObject gameObject;
    public float hitPointY;

    public void Reset()
    {
        collider = null;
        gameObject = null;
        hitPointY = 0;
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
        groundInfo.Reset();
        isGrounded = false;

        if (!canDetect)
            return;

        #region Box Cast
        Vector2 castPos = new Vector2(tf.position.x, tf.position.y + detectionData.size.y);
        float offset = detectionData.snapOutterDist + detectionData.offsetAmount;
        float castDist = detectionData.size.y + ((-rb2D.velocity.y * Time.fixedDeltaTime > offset) ? -rb2D.velocity.y * Time.fixedDeltaTime : offset);

        RaycastHit2D[] hits = Physics2D.BoxCastAll(castPos, detectionData.size, 0f, Vector2.down, castDist, detectionData.groundLayers);

        if (hits == null)
            return;
        #endregion

        #region Get Highest Hit Point
        for (int i = 0; i < hits.Length; i++)
        {
            if (detectionData.ignoreGrounds.Contains(hits[i].collider) || 
                hits[i].point.y > tf.position.y - (detectionData.size.y * 0.5f) + detectionData.snapInnerDist)
                continue;

            if (groundInfo.collider == null || groundInfo.hitPointY < hits[i].point.y)
            {
                groundInfo.collider = hits[i].collider;
                groundInfo.gameObject = hits[i].collider.gameObject;
                groundInfo.hitPointY = hits[i].point.y;
            }
        }

        if (groundInfo.collider == null)
            return;
        #endregion

        #region Is Grounded
        // Snap To Highest Point
        tf.position = new Vector3(tf.position.x, groundInfo.hitPointY + (detectionData.size.y * 0.5f) + detectionData.offsetAmount, tf.position.z);
        
        // Is Grounded
        isGrounded = true;
        #endregion
    }

    public static void ExecuteOnGroundMethod(
        ICanDetectGround canDetectGround, 
        bool isGrounded, 
        ref bool groundEnter, 
        ref bool groundExit)
    {
        if (isGrounded)
        {
            if (!groundEnter)
            {
                groundEnter = true;
                groundExit = false;
                canDetectGround.OnGroundEnter();
            }

            canDetectGround.OnGroundStay();
        }
        else if (!groundExit)
        {
            groundEnter = false;
            groundExit = true;

            canDetectGround.OnGroundExit();
        }
    }

    public static void FallThrough(
        ref bool input_FallThrough,
        bool isGrounded,
        Transform tf,
        Collider2D oneWayCollider,
        GroundDetectionData detectionData,
        GroundInfo groundInfo)
    {
        #region Overlap Box
        Vector2 detectPos = tf.position;
        detectPos.y -= detectionData.snapOutterDist * 0.5f;

        Vector2 detectSize = detectionData.size;
        detectSize.y += detectionData.snapOutterDist;

        Collider2D[] overlaps = Physics2D.OverlapBoxAll(detectPos, detectSize, 0f, detectionData.oneWayLayer);
        #endregion

        #region Enable Collsion
        if (overlaps != null && detectionData.ignoreGrounds != null)
        {
            for (int i = detectionData.ignoreGrounds.Count - 1; i >= 0; i--)
            {
                if (Array.Exists(overlaps, col => col == detectionData.ignoreGrounds[i])) continue;

                Physics2D.IgnoreCollision(oneWayCollider, detectionData.ignoreGrounds[i], false);
                detectionData.ignoreGrounds.Remove(detectionData.ignoreGrounds[i]);
            }
        }
        else
        {
            if (overlaps != null)
            {
                for (int i = 0; i < detectionData.ignoreGrounds.Count; i++)
                    Physics2D.IgnoreCollision(oneWayCollider, detectionData.ignoreGrounds[i], false);
            }
            detectionData.ignoreGrounds.Clear();
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
            if (detectionData.ignoreGrounds.Contains(overlaps[i]))
                continue;

            Physics2D.IgnoreCollision(oneWayCollider, overlaps[i], true);
            detectionData.ignoreGrounds.Add(overlaps[i]);
        }
        #endregion
    }

    public static void FallThrough(
        bool isGrounded,
        Transform tf,
        Collider2D oneWayCollider,
        GroundDetectionData detectionData,
        GroundInfo groundInfo)
    {
        #region Overlap Box
        Vector2 detectPos = tf.position;
        detectPos.y -= detectionData.snapOutterDist * 0.5f;

        Vector2 detectSize = detectionData.size;
        detectSize.y += detectionData.snapOutterDist;

        Collider2D[] overlaps = Physics2D.OverlapBoxAll(detectPos, detectSize, 0f, detectionData.oneWayLayer);
        #endregion

        #region Enable Collsion
        if (overlaps != null && detectionData.ignoreGrounds != null)
        {
            for (int i = detectionData.ignoreGrounds.Count - 1; i >= 0; i--)
            {
                if (Array.Exists(overlaps, col => col == detectionData.ignoreGrounds[i])) continue;

                Physics2D.IgnoreCollision(oneWayCollider, detectionData.ignoreGrounds[i], false);
                detectionData.ignoreGrounds.Remove(detectionData.ignoreGrounds[i]);
            }
        }
        else
        {
            if (overlaps != null)
            {
                for (int i = 0; i < detectionData.ignoreGrounds.Count; i++)
                    Physics2D.IgnoreCollision(oneWayCollider, detectionData.ignoreGrounds[i], false);
            }
            detectionData.ignoreGrounds.Clear();
        }
        #endregion

        #region Disable Collision
        if (overlaps == null || !isGrounded)
            return;

        for (int i = 0; i < overlaps.Length; i++)
        {
            if (detectionData.ignoreGrounds.Contains(overlaps[i]))
                continue;

            Physics2D.IgnoreCollision(oneWayCollider, overlaps[i], true);
            detectionData.ignoreGrounds.Add(overlaps[i]);
        }
        #endregion
    }
}