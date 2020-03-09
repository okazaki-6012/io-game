using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float Speed = 1f;
    public float Duration = 1f;

    float createdTime_;

    void Start()
    {
        createdTime_ = Time.time;
    }

    void Update()
    {
        transform.Translate(Vector3.forward * Speed);

        if (createdTime_ + Duration < Time.time)
        {
            Destroy(gameObject);
        }
    }
}
