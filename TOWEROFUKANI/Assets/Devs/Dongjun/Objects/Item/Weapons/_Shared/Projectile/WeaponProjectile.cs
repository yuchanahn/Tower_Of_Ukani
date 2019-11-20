using UnityEngine;

public class WeaponProjectile : PoolingObj
{
    [Header("Object Detection")]
    [SerializeField] protected Vector2 detectSize;
    [SerializeField] protected LayerMask detectLayers;

    [Header("Effects")]
    [SerializeField] protected PoolingObj particle_Hit;
    [SerializeField] protected float particle_HitOffset;

    protected Vector2 velocity;
    protected WeaponProjectileData projectileData;

    public override void ResetOnSpawn()
    {
        projectileData.curTravelDist = 0;
    }

    #region Method: Unity
    protected virtual void FixedUpdate()
    {
        Move();
        DetectObject();
        OnMaxDist();
    }
    #endregion

    #region Method: Move
    protected virtual void Move()
    {
        velocity.y -= projectileData.gravity.Value;
        transform.Translate(velocity * Time.fixedDeltaTime, Space.World);
        projectileData.curTravelDist += velocity.magnitude * Time.fixedDeltaTime;

        transform.right = velocity.normalized;
    }
    protected virtual void OnMaxDist()
    {
        if (projectileData.curTravelDist >= projectileData.maxTravelDist.Value)
            ObjPoolingManager.Sleep(this);
    }
    #endregion

    #region Method: Hit
    protected virtual void DetectObject()
    {
        Vector2 pos = (transform.position + (transform.right * detectSize.x * 0.5f)) - (transform.right * (projectileData.moveSpeed.Value * Time.fixedDeltaTime));
        float rot = transform.rotation.eulerAngles.z;

        RaycastHit2D[] hits =
            Physics2D.BoxCastAll(pos, detectSize, rot, transform.right, projectileData.moveSpeed.Value * Time.fixedDeltaTime, detectLayers);

        Vector2 hitPos = transform.position;

        if (hits.Length != 0)
        {
            bool hasHit = false;

            for (int i = 0; i < hits.Length; i++)
            {
                if (hits[i].collider.CompareTag("Player"))
                    continue;

                hasHit = true;
                hitPos = hits[i].point;

                IDamage mob = hits[i].collider.GetComponent<IDamage>();
                if (mob != null)
                {
                    mob.Hit(projectileData.attackData.Damage);

                    PlayerStats.DamageDealt = projectileData.attackData.Damage;
                    ItemEffectManager.Trigger(PlayerActions.Hit);

                    OnHit(hitPos);
                    return;
                }
            }

            if (hasHit)
                OnHit(hitPos);
        }
    }
    protected virtual void OnHit(Vector2 hitPos)
    {
        // Sleep
        ObjPoolingManager.Sleep(this);

        // Spawn Effect
        Transform hitParticle = particle_Hit.Spawn(hitPos, Quaternion.identity).transform;
        hitParticle.right = -transform.right;
        hitParticle.position -= transform.right * particle_HitOffset;
    }
    #endregion

    #region Method: SetData
    public void SetData(WeaponProjectileData projectileData)
    {
        this.projectileData = projectileData;
        velocity = projectileData.moveSpeed.Value * transform.right;
    }
    #endregion
}
