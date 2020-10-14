using UnityEngine;

namespace TestOne
{
        public float horizontalThreshold = 0.05f;
        public float verticalThreshold = 0.1f;

        void UpdateCamera()
        {
                Vector3 viewPos = Camera.main.WorldToViewportPoint(transform.position);
                Vector3 offset = Transform.position - Camera.main.ViewportToWorldPoint(viewPos);
                mainCamera.transform.position += offset;
        }

        void OnEnable()
        {
                StartCoroutine(PostFixedUpdate());
        }

        IEnumerator PostFixedUpdate()
        {
                while(true)
                {
                        yield return new WaitForFixedUpdate();

                        UpdateCamera();
                }
        }
}

namespace TestTwo
{
        public static Vector3 SuperSmoothLerp(Vector3 followOld, Vector3 targetOld, Vector3 targetNew, float elapsedTime, float lerpAmount)
        {
                Vector3 f = followOld - targetOld + ( targetNew - targetOld ) / ( lerpAmount * elapsedTime );
                
                return targetNew - (targetNew - targetOld) / (lerpAmount * elapsedTime) + f * Mathf.Exp( -lerpAmount * elapsedTime);
        }
}

namespace TestThree
{
 using UnityEngine;
 using System.Collections;
 
 public class SmoothCamera2D : MonoBehaviour {
     
     public float dampTime = 0.15f;
     private Vector3 velocity = Vector3.zero;
     public Transform target;
 
     // Update is called once per frame
     void Update () 
     {
         if (target)
         {
             Vector3 point = camera.WorldToViewportPoint(target.position);
             Vector3 delta = target.position - camera.ViewportToWorldPoint(new Vector3(0.5f, 0.5f, point.z)); //(new Vector3(0.5, 0.5, point.z));
             Vector3 destination = transform.position + delta;
             transform.position = Vector3.SmoothDamp(transform.position, destination, ref velocity, dampTime);
         }
     
     }
 }
}
