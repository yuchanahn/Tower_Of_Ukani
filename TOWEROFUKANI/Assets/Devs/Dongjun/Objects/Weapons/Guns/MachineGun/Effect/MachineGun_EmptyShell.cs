using UnityEngine;

public class MachineGun_EmptyShell : PlayerDropObj
{
    [SerializeField] private float forceMin;
    [SerializeField] private float forceMax;

    public override void ResetOnActive()
    {
        base.ResetOnActive();

        rb2D.AddForce((Vector3.up - transform.right).normalized * Random.Range(forceMin, forceMax), ForceMode2D.Impulse);
        rb2D.AddTorque(Mathf.Sign(transform.right.x) * Random.Range(forceMin, forceMax), ForceMode2D.Impulse);
    }
}
