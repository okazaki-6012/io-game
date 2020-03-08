using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    /// <summary>
    /// 移動可能な制限距離
    /// </summary>
    public float MoveDistance = 19;

    public Niwatori Niwatori;

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

        var move = Niwatori.transform.TransformDirection(Vector3.forward);
        if (Vector3.Distance(Vector3.zero, Niwatori.transform.position + move) < MoveDistance)
        {
            Niwatori.SetMovePosition(Niwatori.transform.position + move, Input.GetKey(KeyCode.Space));
        }
    }
}
