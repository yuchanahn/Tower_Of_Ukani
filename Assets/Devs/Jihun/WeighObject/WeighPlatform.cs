using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeighPlatform : WeighObject
{
    Rigidbody2D _rg;

    [SerializeField]
    int canLiftUp = 0;
    [SerializeField]
    float speed = 0;
    [SerializeField]
    float forceRate = 1;

    [SerializeField]
    float maxVel = 5f;
    [SerializeField]
    float minVel = -5f;


    private void Start()
    {
        _rg = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        GoUp();
    }

    void GoUp()
    {
        float force = (canLiftUp + 1 - (GetDownForce() / forceRate));

        // Debug.Log(force);

        float result = force * speed;
        if (result > maxVel) result = maxVel;
        if (result < minVel) result = minVel;

        _rg.velocity = new Vector2(_rg.velocity.x, result);
    }
}
