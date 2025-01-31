using UnityEngine;

public class Movement : MonoBehaviour
{

    public Rigidbody2D topOfSpring;
    public float moveSpeed = 10f; // Adjust for smooth movement

    private bool isDragging = false;
    // Start is called once before the first execution of Update after the MonoBehaviour is created

    void Update()
    {
        if (Input.GetMouseButtonDown(0)) // Left Click
        {
            isDragging = true;
        }
        if (Input.GetMouseButtonUp(0)) // Release Click
        {
            isDragging = false;
        }
    }

    void FixedUpdate()
    {
        if (isDragging && topOfSpring != null)
        {
            // Convert mouse position to world space
            Vector2 mouseWorldPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);

            // Move the top segment toward the mouse smoothly
            Vector2 newPosition = Vector2.Lerp(topOfSpring.position, mouseWorldPosition, Time.fixedDeltaTime * moveSpeed);

            // Apply force instead of teleporting for physics-based movement
            Vector2 forceDirection = (newPosition - topOfSpring.position) * moveSpeed;
            topOfSpring.linearVelocity = forceDirection;
        }
    }

    void RotateSpringSegments()
    {
        // Find all the spring segments connected to the parent GameObject
        SpringSegment[] segments = FindObjectsOfType<SpringSegment>();

        foreach (SpringSegment segment in segments)
        {
            if (segment.connectedSegment != null)
            {
                // Calculate the direction to the connected segment
                Vector2 direction = segment.connectedSegment.position - segment.transform.position;

                // Convert the direction into an angle and apply rotation
                float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
                segment.transform.rotation = Quaternion.Lerp(segment.transform.rotation, Quaternion.Euler(0, 0, angle), Time.deltaTime * 5f);
            }
        }
    }
}
