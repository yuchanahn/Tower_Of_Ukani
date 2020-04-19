using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeighPlatform : WeighObject
{
    public float minYPos;
    public float maxYPos;

    Rigidbody2D _rg;

    [SerializeField]
    int canLiftUp = 0;
    [SerializeField]
    float speed = 0;

    [SerializeField]
    float maxVel = 5f;
    [SerializeField]
    float minVel = -0f;


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
        float force = (canLiftUp + 1 - GetDownForce());

        // Debug.Log(force);

        float result = force * speed;

        
        result = Mathf.Min(result, maxVel);
        result = Mathf.Max(result, minVel);

        if (transform.position.y >= maxYPos) result = Mathf.Min(result, 0);
        else if (transform.position.y <= minYPos) result = Mathf.Max(result, 0);
        
        _rg.velocity = new Vector2(_rg.velocity.x, result);

        ClampYPos();
    }

    void ClampYPos()
    {
        float y = transform.position.y;
        y = Mathf.Clamp(y, minYPos, maxYPos);

        transform.position = new Vector3(transform.position.x, y, transform.position.z);
    }
}
