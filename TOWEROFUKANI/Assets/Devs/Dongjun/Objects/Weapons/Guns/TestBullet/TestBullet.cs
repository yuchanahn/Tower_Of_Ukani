using UnityEngine;

public class TestBullet : PoolingObj
{
    [Header("Projectile Stats")]
    [SerializeField] private int damage = 1;
    [SerializeField] private float flySpeed = 50f;
    [SerializeField] private float maxDist = 20f;

    [Header("Object Detection")]
    [SerializeField] private Vector2 detectPos;
    [SerializeField] private Vector2 detectSize;
    [SerializeField] private LayerMask detectLayers;

    [Header("Effects")]
    [SerializeField] private PoolingObj particle_Hit;

    private float curDist = 0;

    public override void ResetOnActive()
    {
        curDist = 0;
    }

    private void FixedUpdate()
    {
        transform.Translate(flySpeed * Time.fixedDeltaTime * transform.right, Space.World);
        DestoryOnMaxDist();
        DetectObject();
    }

    private void DetectObject()
    {
        Vector2 pos = (Vector2)transform.position + (transform.right * detectPos) - (Vector2)(transform.right * (flySpeed * Time.fixedDeltaTime));
        float rot = transform.rotation.eulerAngles.z;

        RaycastHit2D[] hits = 
            Physics2D.BoxCastAll(pos, detectSize, rot, transform.right, flySpeed * Time.fixedDeltaTime, detectLayers);

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
                    BulletHit(hitPos);
                    return;
                }
            }

            if (hasHit)
                BulletHit(hitPos);
        }
    }
    private void DestoryOnMaxDist()
    {
        curDist += flySpeed * Time.fixedDeltaTime;

        if (curDist >= maxDist)
            ObjPoolingManager.Sleep(this);
    }

    private void BulletHit(Vector2 hitPos)
    {
        ObjPoolingManager.Activate(particle_Hit, hitPos, Quaternion.identity).transform.right = -transform.right;
        ObjPoolingManager.Sleep(this);
    }
}
