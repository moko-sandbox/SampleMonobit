using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Animator))]  

/// <summary>
/// Makes player lookahead in the direction pointed by controller.
/// Gives a more "responsive" feel
/// </summary>
public class LookAhead : MonobitEngine.MonoBehaviour {
		
	public Transform HeadTransform;   	
	
	Animator m_Animator;

	Vector3 lookAheadPosition;
			
	void Start () 
	{
		m_Animator = GetComponent<Animator>();
		lookAheadPosition = HeadTransform.position + (HeadTransform.forward * 10);
	}
	
	void OnAnimatorIK(int layerIndex)
	{			
		if(!enabled) return; 
			
		if(layerIndex == 0) // do IK pass on base layer only
		{
			if( monobitView.isMine )
			{
				float vertical = m_Animator.GetCurrentAnimatorStateInfo(0).IsName("Locomotion.Idle") ? 10 : 0;
 
				Vector3 lookAheadPosition = HeadTransform.position + (HeadTransform.forward * 10) +
				                            (HeadTransform.up * vertical * Input.GetAxis("Vertical")) + (HeadTransform.right * 20 * Input.GetAxis("Horizontal"));
				m_Animator.SetLookAtPosition(lookAheadPosition);
				m_Animator.SetLookAtWeight(1.0f, 0.1f, 0.9f, 1.0f, 0.7f);
				monobitView.RPC("UpdateLookAhead", MonobitEngine.MonobitTargets.All, lookAheadPosition);
			}
			else
			{
				m_Animator.SetLookAtPosition(lookAheadPosition);
				m_Animator.SetLookAtWeight(1.0f, 0.1f, 0.9f, 1.0f, 0.7f);
			}
		}
	}

	[MunRPC]
	void UpdateLookAhead(Vector3 position)
	{
		lookAheadPosition = position;
	}
}
