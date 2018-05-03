using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MonobitEngine;

public class SampleMUNBearpocalypseNetworkControl : MonobitEngine.MonoBehaviour {
    // ルーム名
    private string roomName = "";
    
    // プレイヤーキャラクタ
    private GameObject playerObject = null;
    
    void Start()
    {
        // デフォルトロビーへの自動入室を許可する
        MonobitNetwork.autoJoinLobby = true;
        
        // MUNサーバに接続する
        MonobitNetwork.ConnectServer("SampleMUNBearpocalypse_v0.1");
    }

    void Update()
    {
        // MUNサーバに接続しており、かつルームに入室している場合
        if (MonobitNetwork.isConnect && MonobitNetwork.inRoom)
        {
            // プレイヤーキャラクターが未登場の場合に登場させる
            if (playerObject == null)
            {
                playerObject = MonobitNetwork.Instantiate("Player", Vector3.zero, Quaternion.identity, 0);
            }
        }
    }
    
    // レンダリングやGUIイベントの制御
    void OnGUI()
    {
        // デフォルトのボタンと被らないように段下げを行う
        GUILayout.Space(24);
        
        // MUNサーバに接続している場合
        if (MonobitNetwork.isConnect)
        {
            // ルームに入室していない場合
            if (!MonobitNetwork.inRoom)
            {
                // ボタン入力でサーバから切断・シーンリセット
                if (GUILayout.Button("Disconnect", GUILayout.Width(150)))
                {
                    // サーバから切断する
                    MonobitNetwork.DisconnectServer();
                    
                    // シーンをリロードする（リロードした際に Start() が呼ばれて再接続できる）
                    Application.LoadLevel(Application.loadedLevelName);
                }
                
                
                
                // ボタン入力でルームから退室
                if (GUILayout.Button("Leave Room", GUILayout.Width(150)))
                {
                    MonobitNetwork.LeaveRoom();
                }
                
                
                // ルームを作成して入室
                GUILayout.BeginHorizontal();
                
                // ルーム名の入力
                GUILayout.Label("RoomName: ");
                roomName = GUILayout.TextField(roomName, GUILayout.Width(200));
                
                // ボタン入力でルーム作成
                if (GUILayout.Button("Create Room", GUILayout.Width(150)))
                {
                    MonobitNetwork.CreateRoom(roomName);
                }
                GUILayout.EndHorizontal();
                
                
                // 現在存在するルームからランダムに入室する
                if (GUILayout.Button("Join Random Room", GUILayout.Width(200)))
                {
                    MonobitNetwork.JoinRandomRoom();
                }
                
                
                // ルーム一覧から選択式で入室する
                foreach (RoomData room in MonobitNetwork.GetRoomData())
                {
                    if (GUILayout.Button(string.Format(
                        "Enter Room: {0} ({1}/{2})",
                        room.name,
                        room.playerCount,
                        (room.maxPlayers == 0) ? "-" : room.maxPlayers.ToString() // 入室可能なプレイヤー最大人数。0は無制限。
                    )))
                    {
                        MonobitNetwork.JoinRoom(room.name);
                    }
                }
            }
        }
    }
}
