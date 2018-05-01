using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace MonobitEngine
{
	/// <summary>
	/// MonobitNetworkプラグインで使うためのメインクラスです
	/// </summary>
	public sealed class MonobitNetwork : MonobitEngineBase.MonobitNetwork
    {
        /// <summary>
        /// Monobitサーバーに接続します。
        /// </summary>
        /// <param name="gameVersion">クライアントのバージョン番号</param>
        /// <returns>true:接続成功、false:接続失敗</returns>
        /// <remarks>ユーザーはゲームバージョンで個々に分断される</remarks>
        public static bool ConnectServer(string gameVersion, Hashtable customAuthData = null)
        {
            // プレハブ生成処理ルーチンの登録
            MonobitEngineBase.MonobitNetwork.InstantiateWithResources = MonobitNetwork.OnInstantiateWithResources;
            MonobitEngineBase.MonobitNetwork.InstantiateWithAssetBundle = MonobitNetwork.OnInstantiateWithAssetBundle;

            // Monobitサーバーへの接続処理
            return MonobitEngineBase.MonobitNetwork.ConnectServerBase(gameVersion, customAuthData);
        }

		/// <summary>
		/// ルーム内の全クライアントが、ホストと同じゲームシーンをロードすべきかを決めます。
		/// </summary>
		/// <remarks>
		/// 読み込むゲームシーンを同期するためには、ホストはMonobitNetwork.LoadLevelを使っている必要があります。
		/// そうであれば、全てのクライアントは、更新や入室の際に新しいシーンを読み込むことになります。
		/// </remarks>
		public static bool autoSyncScene
        {
            get { return GetAutomaticallySyncScene(); }
            set
            {
                MonobitNetwork.SetAutomaticallySyncScene(value, ActiveSceneBuildIndex, ActiveSceneName);
            }
        }

		/// <summary>
		/// 現在動作しているシーンのファイル名を取得します。
		/// </summary>
		public static string ActiveSceneName
        {
            get
            {
#if UNITY_MIN_5_3
                UnityEngine.SceneManagement.Scene s = UnityEngine.SceneManagement.SceneManager.GetActiveScene();
                return s.name;
#else
                return Application.loadedLevelName;
#endif
            }
        }

		/// <summary>
		/// 現在動作しているシーンのインデックスを取得します。
		/// </summary>
		public static int ActiveSceneBuildIndex
        {
            get
            {
#if UNITY_MIN_5_3
                return UnityEngine.SceneManagement.SceneManager.GetActiveScene().buildIndex;
#else
                return Application.loadedLevel;
#endif
            }
        }

        /// <summary>
        /// Resources.Load() を実行してプレハブを生成します。
        /// </summary>
        /// <param name="prefabName">プレハブ名</param>
        /// <returns>新しく生成されたインスタンスを返します。</returns>
        public static GameObject OnInstantiateWithResources(string prefabName)
        {
            return (GameObject)Resources.Load(prefabName, typeof(GameObject));
        }

        /// <summary>
        /// AssetBundle.LoadAsset() を実行してプレハブを生成します。
        /// </summary>
        /// <param name="assetBundlePath">アセットバンドルのパス名</param>
        /// <param name="assetBundleName">アセットバンドル名</param>
        /// <returns>新しく生成されたインスタンスを返します。</returns>
        public static GameObject[] OnInstantiateWithAssetBundle(string assetBundlePath, string assetBundleName)
        {
#if UNITY_5 || UNITY_2017
            List<GameObject> goList = new List<GameObject>();

            try
            {
                // アセットバンドルに含まれるリストの取得
                AssetBundle bundle = AssetBundle.LoadFromFile(assetBundlePath + "/" + assetBundleName);
                string[] bundleNameArray = bundle.GetAllAssetNames();
                foreach (string bundleName in bundleNameArray)
                {
                    // プレハブの読み込み
                    GameObject prefab = bundle.LoadAsset<GameObject>(bundleName);
                    goList.Add(GameObject.Instantiate(prefab));
                }
                bundle.Unload(false);
            }
            catch (System.Exception)
            {
                Debug.LogError("AssetBundle.LoadAsset() failed.");
                return null;
            }

            return goList.ToArray();
#else
            // AssetBundle.Load に必要な機能が備わっていないので、エラーを出力する
            Debug.LogError("AssetBundle.LoadAsset() is not defined.(perhaps you are using Unity4.x)");
            return null;
#endif
        }
    }
}
