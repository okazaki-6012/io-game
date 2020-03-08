using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class GameEngine : MonoBehaviour
{
    public Camera TrackingCamera;
    public PlayerController PlayerController;

    public GameObject Niwatori;
    public GameObject Hiyoko;

    public static GameEngine Instance { get; private set; }

    WsClient client_;
    List<Niwatori> niwatoriList_ = new List<Niwatori>();
    List<UserData> users_ = new List<UserData>();
    int frameCount_;
    bool gameStarted_ = false;

    Stack<Message> messages_ = new Stack<Message>();

    void Awake()
    {
        Niwatori.SetActive(false);
        Hiyoko.SetActive(false);

        Instance = this;
    }

    void Start()
    {
        client_ = new WsClient("ws://localhost:3000");
        client_.OnMessage = onMessage;
    }

    void OnApplicationQuit()
    {
        client_.Dispose();
    }

    void Update()
    {
        if (messages_.Count > 0)
        {
            var msg = messages_.Pop();
            switch (msg.Type)
            {
                case "join":
                    client_.SendMessage("gameStart", "Niwako");
                    break;
                case "gameStart":
                    {
                        var data = JsonUtility.FromJson<GameStartMessage>(msg.Data);
                        users_ = data.Users;
                        foreach (var user in users_)
                        {
                            var obj = createNiwatori(user);
                            niwatoriList_.Add(obj);

                            // プレイヤーの操作するオブジェクトの初期化
                            if (user.Id == data.Player.Id)
                            {
                                TrackingCamera.transform.SetParent(obj.transform, false);
                                PlayerController.Niwatori = obj;
                            }
                        }
                        gameStarted_ = true;
                    }
                    break;
                case "updateUser":
                    {
                        if (gameStarted_)
                        {
                            var data = JsonUtility.FromJson<UpdateUserMessage>(msg.Data);
                            var niwatori = FindNiwatori(data.User.Id);
                            if (niwatori != null)
                            {
                                niwatori.SetMovePosition(new Vector3(data.User.X, 0, data.User.Y), data.User.IsDash);
                                niwatori.SetQuaternion(data.User.Angle);
                            }
                            else
                            {
                                users_.Add(data.User);
                                niwatoriList_.Add(createNiwatori(data.User));
                            }
                        }
                    }
                    break;
                case "exitUser":
                    {
                        var data = JsonUtility.FromJson<exitUserMessage>(msg.Data);
                        var user = users_.First(u => u.WsName == data.WsName);
                        var niwatori = niwatoriList_.First(v => v.UId == user.Id);
                        niwatoriList_.Remove(niwatori);
                        Destroy(niwatori.gameObject);
                    }
                    break;
            }
        }

        if (gameStarted_)
        {
            updateServerUser();
        }
    }

    /// <summary>
    /// WebSocketからの処理を受け取る
    /// </summary>
    /// <param name="msg"></param>
    void onMessage(Message msg)
    {
        messages_.Push(msg);
    }

    /// <summary>
    /// サーバーのキャラクター情報を更新する
    /// </summary>
    void updateServerUser()
    {
        frameCount_++;
        if (frameCount_ % 3 == 0)
        {
            var msg = new UpdateUserMessage();
            var c = FindUser(PlayerController.Niwatori.UId);
            c.X = PlayerController.Niwatori.transform.position.x;
            c.Y = PlayerController.Niwatori.transform.position.z;
            c.Angle = PlayerController.Niwatori.transform.eulerAngles.y;
            c.Hp = PlayerController.Niwatori.Hp;
            c.Power = PlayerController.Niwatori.Power;
            c.IsDash = PlayerController.Niwatori.IsDash;
            msg.User = c;
            client_.SendMessage("updateUser", msg);
        }
    }

    public UserData FindUser(int id)
    {
        return users_.First(u => u.Id == id);
    }

    public Niwatori FindNiwatori(int id)
    {
        return niwatoriList_.FirstOrDefault(v => v.UId == id);
    }

    Niwatori createNiwatori(UserData u)
    {
        var obj = Instantiate(Niwatori);
        var pos = new Vector3(u.X, 0, u.Y);
        obj.transform.position = pos;
        obj.SetActive(true);
        var niwatori = obj.GetComponent<Niwatori>();
        niwatori.UId = u.Id;
        niwatori.Hp = u.Hp;
        niwatori.Power = u.Power;
        niwatori.SetMovePosition(pos, u.IsDash);
        niwatori.SetQuaternion(u.Angle);
        return niwatori;
    }
}

[Serializable]
class GameStartMessage
{
    public List<UserData> Users;
    public UserData Player;
}

[Serializable]
class exitUserMessage
{
    public string WsName;
}

[Serializable]
struct UpdateUserMessage
{
    public UserData User;
}

/// <summary>
/// キャラクター情報
/// </summary>
[Serializable]
public struct UserData
{
    public int Id;
    public string WsName;
    public string Name;
    public int Hp;
    public int Power;
    public float Angle;
    public float X;
    public float Y;
    public bool IsDash;
}