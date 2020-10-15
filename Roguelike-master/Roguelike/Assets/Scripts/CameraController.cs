using UnityEngine;

public class CameraController : MonoBehaviour
{
	public GameObject followTarget;
	public Camera _camera;

	public void LateUpdate()
	{
		if ( _camera && followTarget )
		{
			Vector2 newPosition = new Vector2( followTarget.transform.position.x, followTarget.transform.position.y );
			float nextX = Mathf.Round( Game.PPU * newPosition.x );
			float nextY = Mathf.Round( Game.PPU * newPosition.y );
			_camera.transform.position = new Vector3( nextX / Game.PPU, nextY / Game.PPU, _camera.transform.position.z );
		}
	}
}
