public float horizontalThreshold = 0.05f;
public float verticalThreshold = 0.1f;

void UpdateCamera()
{
        Vector3 viewPos = Camera.main.WorldToViewportPoint(transform.position);
        
        bool dirty = false;
        
        if(viewPos.x > 1.0f - horizontalTheshold)
        {
                viewPos.x = 1.0f - horizontalThreshold;
                dirty = true;
        }
        
        if(viewPos.x < horizontalThreshold)
        {
                viewPos.x = horizontalThreshold;
                dirty = true;
        }
        
        if(viewPos.y > 1.0f - vericalThreshold)
        {
                viewPos.y = 1.0f - verticalThreshold;
                dirty = true;
        }
        
        if(dirty)
        {
                Vector3 offset = Transform.position - Camera.main.ViewportToWorldPoint(viewPos);
                mainCamera.transform.position += offset;
        }
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
