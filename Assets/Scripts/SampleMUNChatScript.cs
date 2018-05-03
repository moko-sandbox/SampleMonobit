using System.Collections;
using System.Collections.Generic;
using System.Reflection.Emit;
using UnityEngine;
using MonobitEngine;

public class SampleMUNChatScript : MonobitEngine.MonoBehaviour {
    // ルーム名
    private string roomName = "";
    
    // チャット発言文
    private string chatWord = "";
    
    // チャット発言ログ
    List<string> chatLog = new List<string>();
    
    // GUI制御
    private void OnGUI()
    {
        // MUNサーバに接続している場合
        if (MonobitNetwork.isConnect)
        {
            // ルームに入室している場合
            if (MonobitNetwork.inRoom)
            {
                // ルーム内のプレイヤー一覧の表示
                GUILayout.BeginHorizontal();
                GUILayout.Label("PlayerList: ");
                foreach (MonobitPlayer player in MonobitNetwork.playerList)
                {
                    GUILayout.Label(player.name + " ");
                }
                GUILayout.EndHorizontal();
                
                
                // ルームからの退室
                if (GUILayout.Button("Leave Room", GUILayout.Width(150)))
                {
                    MonobitNetwork.LeaveRoom();
                    chatLog.Clear();
                }
                
                
                // チャット発言文の入力
                GUILayout.BeginHorizontal();
                GUILayout.Label("Message: ");
                chatWord = GUILayout.TextField(chatWord, GUILayout.Width(400));
                GUILayout.EndHorizontal();
                
                // チャット発言文を送信する
                if (GUILayout.Button("Send", GUILayout.Width(100)))
                {
                    // このScriptが追加されているGameObjectにMonobitViewも追加しておく。このMonobitViewコンポーネントを用いt、MonobitView.RPC()メソッドをコールする。
                    monobitView.RPC("RecvChat", MonobitTargets.All, MonobitNetwork.playerName, chatWord); // ...(1)
                    chatWord = "";
                }
                
                
                // チャットログを表示する
                string msg = "";
                for (int i = 0; i < 10; ++i)
                {
                    msg += ((i < chatLog.Count) ? chatLog[i] : "") + "\r\n";
                }
                GUILayout.TextArea(msg);
            }
            // ルームに入室していない場合
            else
            {
               // ルーム名の入力
                GUILayout.BeginHorizontal();
                GUILayout.Label("RoomName: ");
                roomName = GUILayout.TextField(roomName, GUILayout.Width(200));
                GUILayout.EndHorizontal();
                
                
                // ルームを作成して入室する
                if (GUILayout.Button("Create Room", GUILayout.Width(150)))
                {
                    MonobitNetwork.CreateRoom(roomName); // 自動的に入室もしてくれる
                }
                // ルーム一覧を検索して入室する
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
        // MUNサーバに接続していない場合
        else
        {
            // プレイヤー名の入力
            GUILayout.BeginHorizontal();
            GUILayout.Label("PlayernName: ");
            MonobitNetwork.playerName = GUILayout.TextField((MonobitNetwork.playerName == null) ? "" : MonobitNetwork.playerName, GUILayout.Width(200));
            GUILayout.EndHorizontal();
            
            // デフォルトロビーへの自動入室を許可する
            MonobitNetwork.autoJoinLobby = true;
            
            // MUNサーバに接続する
            if (GUILayout.Button("Connect Serrver", GUILayout.Width(150)))
            {
                MonobitNetwork.ConnectServer("SampleMUNChat_v0.1");
            }
        }
    }
    
    // RPC受信関数
    [MunRPC]
    void RecvChat(string senderName, string senderWord) // ...(1)を受信する。メソッド名、引数値の数や型を対応させる
    {
        chatLog.Add(string.Format("{0}: {1}", senderName, senderWord));
        // 表示は最新10件まで
        if (chatLog.Count > 10)
        {
            chatLog.RemoveAt(0);
        }
    }
}
