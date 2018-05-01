using UnityEngine;
using UnityStandardAssets.CrossPlatformInput;

public class CubeController : MonoBehaviour
{
	[SerializeField] private float mSpeedScale = 5.0f;

	void Update ()
	{
		float vertical = CrossPlatformInputManager.GetAxis("Vertical");
		float horizontal = CrossPlatformInputManager.GetAxis("Horizontal");
		Vector3 addPos = new Vector3(horizontal, vertical, 0.0f) * mSpeedScale;
		transform.position += addPos * Time.deltaTime;

		if (CrossPlatformInputManager.GetButton("Jump"))
		{
			GetComponent<Renderer>().material.color = Color.red;
			;
		}
		else
		{
			GetComponent<Renderer>().material.color = Color.white;
		}
	}
}
