using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hiyoko : MonoBehaviour
{
    public float WalkSpeed = 0.1f;
    public Animator Anima;

    public int Id;

    public Transform Owner;

    Vector3 targetPosition_;
    Quaternion targetQuaternion_;

    void OnEnable()
    {
        Anima.SetBool("Walk", true);
        Anima.SetBool("Run", false);
    }

    void Update()
    {
        if (Owner == null)
        {
            // 回転
            transform.rotation = Quaternion.RotateTowards(transform.rotation, targetQuaternion_, Time.deltaTime);
        }
        else
        {
            // 親がいたら、親の方向を向く
            transform.LookAt(Owner);
        }
    }
}
