using UnityEngine;
using UnityStandardAssets.CrossPlatformInput;

[RequireComponent(typeof(Animator))]

public class UnityChanController : MonoBehaviour
{
	[SerializeField] private float mSpeedScale = 5.0f;
	private Animator animator;

	void Start () {
		animator = GetComponent<Animator>();
	}
	
	void Update () {
		float vertical = CrossPlatformInputManager.GetAxis("Vertical");
		float horizontal = CrossPlatformInputManager.GetAxis("Horizontal");
		Vector3 addPos = new Vector3(horizontal, vertical, 0.0f) * mSpeedScale;
		transform.position += addPos * Time.deltaTime;
	}

}
