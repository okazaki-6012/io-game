using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Niwatori : MonoBehaviour
{
    public float RotationSpeed = 100.0f;
    public float WalkSpeed = 1f;
    public float DashSpeed = 3f;

    public Animator Anima;

    public int UId;
    public int Hp;
    public int Power;
    public bool IsDash;

    Quaternion targetQuaternion_;
    Vector3 movePosition_;

    void OnEnable()
    {
        Anima.SetBool("Walk", true);
        Anima.SetBool("Run", false);
    }

    void Update()
    {
        // 向き変更
        if (transform.rotation.y != targetQuaternion_.y)
        {
            transform.rotation = Quaternion.RotateTowards(transform.rotation, targetQuaternion_, RotationSpeed * Time.deltaTime);
        }

        var distance = Vector3.Distance(transform.position, movePosition_);
        if (distance > 0.1f)
        {
            // 1.5f以上の距離があるとダッシュに変化する
            Anima.SetBool("Walk", !IsDash);
            Anima.SetBool("Run", IsDash);
            // 移動処理
            var speed = IsDash ? DashSpeed : WalkSpeed;
            transform.position = Vector3.MoveTowards(transform.position, movePosition_, speed * Time.deltaTime);
        }
        else
        {
            Anima.SetBool("Walk", false);
            Anima.SetBool("Run", false);
        }
    }

    /// <summary>
    /// 行く先の座標を設定する
    /// </summary>
    /// <param name="movePosition"></param>
    public void SetMovePosition(Vector3 movePosition, bool isDash)
    {
        movePosition_ = movePosition;
        IsDash = isDash;
    }

    /// <summary>
    /// 向きの設定
    /// </summary>
    /// <param name="movePosition"></param>
    public void SetQuaternion(float angle)
    {
        targetQuaternion_ = Quaternion.Euler(0, angle, 0);
    }
}
