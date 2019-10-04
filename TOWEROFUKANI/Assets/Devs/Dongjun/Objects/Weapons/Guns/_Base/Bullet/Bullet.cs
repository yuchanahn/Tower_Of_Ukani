using UnityEngine;

public class Bullet : PoolingObj
{
    [Header("Object Detection")]
    [SerializeField] protected Vector2 detectSize;
    [SerializeField] protected LayerMask detectLayers;

    [Header("Effects")]
    [SerializeField] protected PoolingObj particle_Hit;
    [SerializeField] protected float particle_HitOffset;

    protected BulletData bulletData;
    protected float curTravelDist = 0;


    public override void ResetOnSpawn()
    {
        curTravelDist = 0;
    }

    #region Method: Unity
    protected virtual void FixedUpdate()
    {
        Move();
        OnMaxDist();
        DetectObject();
    }
    #endregion

    #region Method: Move
    protected virtual void Move()
    {
        transform.Translate(bulletData.moveSpeed * Time.fixedDeltaTime * transform.right, Space.World);
    }
    protected virtual void OnMaxDist()
    {
        curTravelDist += bulletData.moveSpeed * Time.fixedDeltaTime;
        if (curTravelDist >= bulletData.maxDist)
            ObjPoolingManager.Sleep(this);
    }
    #endregion

    #region Method: Hit
    protected virtual void DetectObject()
    {
        Vector2 pos = (transform.position + (transform.right * detectSize.x * 0.5f)) - (transform.right * (bulletData.moveSpeed * Time.fixedDeltaTime));
        float rot = transform.rotation.eulerAngles.z;

        RaycastHit2D[] hits = 
            Physics2D.BoxCastAll(pos, detectSize, rot, transform.right, bulletData.moveSpeed * Time.fixedDeltaTime, detectLayers);

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

                var mob = hits[i].collider.GetComponent<IDamage>();
                if (mob != null)
                {
                    mob?.Hit(bulletData.damage);
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
        ObjPoolingManager.Sleep(this);
        Transform hitParticle = ObjPoolingManager.Spawn(particle_Hit, hitPos, Quaternion.identity).transform;
        hitParticle.right = -transform.right;
        hitParticle.position -= transform.right * particle_HitOffset;
    }
    #endregion

    #region Method: Set Value
    public void SetData(BulletData bulletData)
    {
        this.bulletData = bulletData;
    }
    #endregion
}
