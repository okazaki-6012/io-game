using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float ShotCoolTime = 5;
    float lastShotTime_;

    /// <summary>
    /// 移動可能な制限距離
    /// </summary>
    public float MoveDistance = 19;

    public Niwatori Niwatori { get; private set; }

    public void Init(Niwatori niwatori)
    {
        Niwatori = niwatori;
    }

    void Update()
    {
        if (Niwatori == null)
        {
            return;
        }

        var angle = 0f;
        if (Input.GetKey(KeyCode.D))
        {
            angle = 1;
        }
        if (Input.GetKey(KeyCode.A))
        {
            angle = -1;
        }
        if (angle != 0)
        {
            Niwatori.SetQuaternion(Niwatori.transform.eulerAngles.y + angle);
        }
        if (Input.GetKeyDown(KeyCode.Y))
        {
            GameEngine.Instance.Send(Message.ActionDamge, new ActionDamageMessage { UserId = Niwatori.UserId, Damage = 1 });
        }
        if (Input.GetKeyDown(KeyCode.Space) && lastShotTime_ + ShotCoolTime < Time.time)
        {
            lastShotTime_ = Time.time;
            GameEngine.Instance.Send(Message.ActionShot, new ActionShotMessage { UserId = Niwatori.UserId });
        }

        var move = Niwatori.transform.TransformDirection(Vector3.forward);
        if (Vector3.Distance(Vector3.zero, Niwatori.transform.position + move) < MoveDistance)
        {
            Niwatori.SetMovePosition(Niwatori.transform.position + move, Input.GetKey(KeyCode.W));
        }
    }
}
