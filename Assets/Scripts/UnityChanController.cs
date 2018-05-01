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
		float horizontal = CrossPlatformInputManager.GetAxis("Horizontal");
		transform.Rotate(0, horizontal * mSpeedScale, 0);
		
		float vertical = CrossPlatformInputManager.GetAxis("Vertical");
		Vector3 velocity = new Vector3(0, 0, vertical) * mSpeedScale;
		velocity = transform.TransformDirection(velocity);
		transform.position += velocity * Time.deltaTime;
	}

}
