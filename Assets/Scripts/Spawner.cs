using UnityEngine;
using System.Collections;

/// <summary>
/// NPC spawner
/// </summary>
public class Spawner : MonoBehaviour 
{
	public GameObject ToSpawn;		
	public int SpawnCount = 50;	
	private bool m_IsSpawning = false;
	
	int m_CurrentSpawnCount;

	private void Start()
	{
		m_CurrentSpawnCount = transform.GetChild(0).childCount;
	}

	public void Update()
	{
		// ホスト以外は処理をしない
		if (!MonobitEngine.MonobitNetwork.isHost)
		{
			return;
		}
		
		if(m_IsSpawning && transform.GetChild(0).childCount < m_CurrentSpawnCount + SpawnCount)
		{
			for(int i = 0 ; i < 50 ; i++) // try 50 times. Brute force approach, randomly try to spawn and make sure its in the Spawnable zone. 
			{
				Vector3 position = transform.position + Random.insideUnitSphere * 20;
				position.y = 0;
							
				RaycastHit hitInfo;
				if(Physics.Raycast(position + new Vector3(0,1,0), Vector3.down,out hitInfo,10) && hitInfo.collider.tag == "Spawnable")
				{
					GameObject newNPC = MonobitEngine.MonobitNetwork.InstantiateSceneObject(ToSpawn.name, position, Quaternion.Euler(0, Random.value * 180, 0), 0, null) as GameObject;

					
					// NPC_Spawnerの子としてNPC生成 & NPCをアクティブにする処理。
					// この処理はMonobitでホストではないユーザに対しても、PrefabがInstantiateされたあとで適用する必要があるので、NPC_Patrol.csに移植する。
//					newNPC.transform.parent = transform.GetChild(0).transform;									
//					newNPC.SetActive(true);
//					m_CurrentSpawnCount++;
					
					return;
				}
			}
		}
	}
		
	public void BeginSpawning()
	{
		m_IsSpawning = true;		
	}
}
