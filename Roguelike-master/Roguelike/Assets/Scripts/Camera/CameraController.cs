using UnityEngine;

public class CameraController : MonoBehaviour {
    public static Transform followTarget;
    public Camera _camera;

    private float Step { get { return MoveAcrossBoardSpeed * Time.deltaTime; } }
    private const float MoveAcrossBoardSpeed = 10.0f;

    private void Start() {
        _camera.transform.position = followTarget.transform.position - Vector3.forward * 10;
    }

    public static void SetFollowTarget( Transform transform ) {
        followTarget = transform;
    }

    public void LateUpdate() {
        if ( _camera && followTarget ) {
            Vector2 stepDestination = new Vector2( followTarget.position.x, followTarget.position.y );
            Vector3 positionAfterMoving = Vector3.MoveTowards( transform.position, stepDestination, Step );
            _camera.transform.position = new Vector3( positionAfterMoving.x, positionAfterMoving.y, _camera.transform.position.z );
        }
    }
}