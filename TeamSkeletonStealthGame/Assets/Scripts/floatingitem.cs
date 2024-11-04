using UnityEngine;

public class FloatingItem : MonoBehaviour
{
    public float floatHeight = 0.5f; 
    public float floatSpeed = 1f;    
    public float rotationSpeed = 50f; 

    private Vector3 originalPosition;

    private void Start()
    {
        
        transform.localScale = new Vector3(0.75f,0.75f,0f);
        originalPosition = transform.position;
    }

    private void Update()
    {
        
        float newY = originalPosition.y + Mathf.Sin(Time.time * floatSpeed) * floatHeight;
        transform.position = new Vector3(originalPosition.x, newY, originalPosition.z);

        
        transform.Rotate(0, rotationSpeed * Time.deltaTime, 0);
    }

    public void SetPosition(Vector3 newPosition)
    {
    originalPosition = newPosition;
    }

}
