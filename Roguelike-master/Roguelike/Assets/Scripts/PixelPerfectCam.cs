using UnityEngine;

public class PixelPerfectCam : MonoBehaviour {
	
	public GameObject followTarget;
	
	public Camera _camera;
	
	private float _pixelLockedPPU = 16.0f;
	
	public void FixedUpdate(){
		
		if(_camera && followTarget){
			Vector2 newPosition = new Vector2(followTarget.transform.position.x, followTarget.transform.position.y);
			float nextX = Mathf.Round(_pixelLockedPPU * newPosition.x);
			float nextY = Mathf.Round(_pixelLockedPPU * newPosition.y);
			_camera.transform.position = new Vector3(nextX/_pixelLockedPPU, nextY/_pixelLockedPPU, _camera.transform.position.z);
		}
		
	}
}
