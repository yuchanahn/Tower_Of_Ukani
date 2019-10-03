using UnityEngine;

public class Bullet : PoolingObj
{
    [Header("Projectile Stats")]
    [SerializeField] protected int damage = 1;
    [SerializeField] protected float moveSpeed = 50f;
    [SerializeField] protected float maxDist = 20f;

    [Header("Object Detection")]
    [SerializeField] protected Vector2 detectSize;
    [SerializeField] protected LayerMask detectLayers;

    [Header("Effects")]
    [SerializeField] protected PoolingObj particle_Hit;
    [SerializeField] protected float particle_HitOffset;

    protected float curTravelDist = 0;

    public override void ResetOnActive()
    {
        curTravelDist = 0;
    }

    protected virtual void FixedUpdate()
    {
        Move();
        OnMaxDist();
        DetectObject();
    }

    protected virtual void Move()
    {
        transform.Translate(moveSpeed * Time.fixedDeltaTime * transform.right, Space.World);
    }
    protected virtual void OnMaxDist()
    {
        curTravelDist += moveSpeed * Time.fixedDeltaTime;
        if (curTravelDist >= maxDist)
            ObjPoolingManager.Sleep(this);
    }
    protected virtual void DetectObject()
    {
        Vector2 pos = (transform.position + (transform.right * detectSize.x * 0.5f)) - (transform.right * (moveSpeed * Time.fixedDeltaTime));
        float rot = transform.rotation.eulerAngles.z;

        RaycastHit2D[] hits = 
            Physics2D.BoxCastAll(pos, detectSize, rot, transform.right, moveSpeed * Time.fixedDeltaTime, detectLayers);

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
                    mob?.Hit(damage);
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
        GameObject hitParticle = ObjPoolingManager.Spawn(particle_Hit, hitPos, Quaternion.identity);
        hitParticle.transform.right = -transform.right;
        hitParticle.transform.position -= transform.right * particle_HitOffset;
    }
}
