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
