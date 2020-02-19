using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Projectile : PoolingObj
{
    #region Var: Inspector
    [Header("Detection: Tag")]
    [SerializeField] protected string[] ignoreTags;

    [Header("Detection: Creature")]
    [SerializeField] protected Rigidbody2D creatureDetectRB;

    [Header("Detection: Wall")]
    [SerializeField] protected Rigidbody2D wallDetectRB;

    [Header("Visual")]
    [SerializeField] protected bool rotateToMovingDir = true;

    [Header("Effects")]
    [SerializeField] protected PoolingObj particle_Hit;
    [SerializeField] protected float particle_HitOffset;
    #endregion

    #region Var: Data
    protected Vector2 velocity;
    protected ProjectileData projectileData;
    protected AttackData attackData;
    #endregion

    #region Method: Init PoolingObj
    public override void ResetOnSpawn()
    {
        velocity = Vector2.zero;
        projectileData.travelDist.Reset();

        if (creatureDetectRB != null) creatureDetectRB.transform.localPosition = Vector2.zero;
        if (wallDetectRB != null) wallDetectRB.transform.localPosition = Vector2.zero;
    }
    #endregion

    #region Method: Init Data
    public void InitData(Vector2 startDir, ProjectileData projectileData, AttackData attackData = new AttackData())
    {
        // Init Data
        this.projectileData = projectileData;
        this.attackData = attackData;

        // Init Velocity
        velocity = projectileData.moveSpeed.Value * startDir;
    }
    #endregion

    #region Method: Unity
    protected virtual void FixedUpdate()
    {
        Move();
    }
    #endregion

    #region Method: Move
    protected virtual void Move()
    {
        // Set Velocity
        velocity.y -= projectileData.gravity.Value * Time.fixedDeltaTime;

        // Move
        transform.Translate(velocity * Time.fixedDeltaTime, Space.World);

        // Update Travle Dist
        float dist = velocity.magnitude * Time.fixedDeltaTime;
        projectileData.travelDist.ModFlat += dist;

        // Detect Object
        if (!DetectCreature(dist)) DetectWall(dist);

        // On Maxt Distance
        if (projectileData.travelDist.Value >= projectileData.travelDist.Max)
            OnMaxDist();

        // Rotate Towards Moving Dir
        if (rotateToMovingDir) transform.right = velocity.normalized;
    }
    protected virtual void OnMaxDist()
    {
        ObjPoolingManager.Sleep(this);
    }
    #endregion

    #region Method: Detect Object
    protected bool IsVaildTag(Component target)
    {
        for (int j = 0; j < ignoreTags.Length; j++)
            if (target.CompareTag(ignoreTags[j]))
                return false;

        return true;
    }
    protected virtual bool DamageCreature(GameObject hit)
    {
        return false;
    }
    protected virtual bool DetectCreature(float dist)
    {
        if (creatureDetectRB == null)
            return false;

        // Check Creature
        List<RaycastHit2D> creatureHits = new List<RaycastHit2D>();
        creatureDetectRB.transform.position = transform.position - (Vector3)(velocity.normalized * dist);
        creatureDetectRB.Cast(velocity.normalized, creatureHits, dist);

        // Get Closest Creature
        creatureHits = creatureHits.OrderByDescending(o => Vector2.SqrMagnitude((Vector2)creatureDetectRB.transform.position - o.point)).ToList();
        for (int i = creatureHits.Count - 1; i >= 0; i--)
        {
            if (!IsVaildTag(creatureHits[i].collider))
                continue;

            if (!DamageCreature(creatureHits[i].collider.gameObject))
                continue;

            OnHit(creatureHits[i].point);
            return true;
        }

        return false;
    }
    protected virtual bool DetectWall(float dist)
    {
        if (wallDetectRB == null)
            return false;

        // Check Wall
        List<RaycastHit2D> wallHits = new List<RaycastHit2D>();
        wallDetectRB.transform.position = transform.position - (Vector3)(velocity.normalized * dist);
        wallDetectRB.Cast(velocity.normalized, wallHits, dist);

        // Get Closest Wall
        wallHits = wallHits.OrderByDescending(o => Vector2.SqrMagnitude((Vector2)wallDetectRB.transform.position - o.point)).ToList();
        for (int i = wallHits.Count - 1; i >= 0; i--)
        {
            if (!IsVaildTag(wallHits[i].collider))
                continue;

            OnHit(wallHits[i].point);
            return true;
        }

        return false;
    }

    protected virtual void OnHit(Vector2 hitPos)
    {
        // Sleep
        this.Sleep();

        // Spawn Hit Effect
        if (particle_Hit == null) return;
        Transform hitParticle = particle_Hit.Spawn(hitPos, Quaternion.identity).transform;
        hitParticle.right = -velocity.normalized;
        hitParticle.position -= (Vector3)velocity.normalized * particle_HitOffset;
    }
    #endregion
}
