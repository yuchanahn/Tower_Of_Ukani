using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeighPlatform : WeighObject
{
    Rigidbody2D _rg;

    [SerializeField]
    float upForce = 0;
    [SerializeField]
    float speed = 0;
    [SerializeField]
    float speedRate = 1;

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
        float force = (upForce - GetDownForce());

        float result = force / speedRate * speed;
        if (result > maxVel) result = maxVel;
        if (result < minVel) result = minVel;

        _rg.velocity = new Vector2(_rg.velocity.x, result);
    }
}
