using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameView : MonoBehaviour
{
    readonly float[] HEARTSIZE = { 0f, 0.5f, 1f };
    public Image[] Hearts;
    public GameObject RetryCover;

    void Awake()
    {
        RetryCover.SetActive(false);
    }

    void Update()
    {
        if (GameEngine.Instance.Player.Niwatori == null)
        {
            return;
        }

        for (var i = 0; i < Hearts.Length; i++)
        {
            var index = Mathf.Max(Mathf.Min(GameEngine.Instance.Player.Niwatori.Hp - ((HEARTSIZE.Length - 1) * i), HEARTSIZE.Length - 1), 0);
            var size = HEARTSIZE[index];
            Hearts[i].transform.localScale = new Vector3(size, size);
        }

        if (GameEngine.Instance.Player.Niwatori.IsDead)
        {
            RetryCover.SetActive(true);
        }
        else if(!GameEngine.Instance.Player.Niwatori.IsDead && RetryCover.activeInHierarchy)
        {
            RetryCover.SetActive(false);
        }
    }

    public void OnClickRetry()
    {
        GameEngine.Instance.Init();
    }
}
