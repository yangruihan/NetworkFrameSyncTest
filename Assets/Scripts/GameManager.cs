using UnityEngine;
using System.Collections.Generic;
using UnityEngine.Networking;

public class GameManager : NetworkBehaviour {

    public static GameManager Instance = null;

    public static Player LocalPlayer = null;

    // 游戏计时器
    [SyncVar]
    public float gameTimer;

    void Awake()
    {
        if (Instance != null)
        {
            Debug.LogError("More than one GameManager in scene.");
        }
        else
        {
            Instance = this;
        }
    }

    #region Player tracking
    private const string PLAYER_ID_PREFIX = "Player ";
    private static Dictionary<string, Player> players = new Dictionary<string, Player>();

    public static void RegisterPlayer(string _netID, Player _player)
    {
        string _playerID = PLAYER_ID_PREFIX + _netID;
        players.Add(_playerID, _player);
        _player.transform.name = _playerID;
    }

    public static void UnRegisterPlayer(string _playerID)
    {
        players.Remove(_playerID);
    }

    public static Player GetPlayer(string _playerID)
    {
        return players[_playerID];
    }
    #endregion

    [ServerCallback]
    void Update()
    {
        gameTimer += Time.deltaTime;
    }

    /// <summary>
    /// 每帧分发命令给各客户端
    /// </summary>
    [ServerCallback]
    void FixedUpdate()
    {
        foreach (var player in players.Values)
        {
            player.SendServerCommand();
        }
    }
}
