using UnityEngine;

public class this_camera : MonoBehaviour
{
    static public Transform target;      // The point the camera will orbit around
    public float distance = 5.0f; // Distance from the target point
    public float xSpeed = 120.0f; // Speed of horizontal rotation
    public float ySpeed = 120.0f; // Speed of vertical rotation

    public float yMinLimit = -20f; // Minimum vertical angle (to prevent flipping)
    public float yMaxLimit = 80f;  // Maximum vertical angle

    public float distanceMin = 2f;  // Minimum distance the camera can zoom
    public float distanceMax = 15f; // Maximum distance the camera can zoom

    private float x = 0.0f;
    private float y = 0.0f;

    void Start()
    {
        // Initialize camera angles based on current rotation
        Vector3 angles = transform.eulerAngles;
        x = angles.y;
        y = angles.x;

        // Ensure target is set
        if (!target)
        {
            //Debug.LogError("No target set for camera orbit.");
        }
    }

    void LateUpdate()
    {
        if (target)
        {
            // Mouse button 0 (left click) is used to rotate the camera
            if (Input.GetMouseButton(0))
            {
                x += Input.GetAxis("Mouse X") * xSpeed * distance * 0.02f;
                y -= Input.GetAxis("Mouse Y") * ySpeed * 0.02f;

                // Clamp the vertical angle to stay within bounds
                y = ClampAngle(y, yMinLimit, yMaxLimit); // Make sure this calls the method defined below
            }

            // Mouse scroll wheel is used to zoom the camera in and out
            distance = Mathf.Clamp(distance - Input.GetAxis("Mouse ScrollWheel") * 5, distanceMin, distanceMax);

            // Calculate the camera rotation and position
            Quaternion rotation = Quaternion.Euler(y, x, 0);
            Vector3 negDistance = new Vector3(0.0f, 0.0f, -distance);
            Vector3 position = rotation * negDistance + target.position;

            // Apply the rotation and position to the camera
            transform.rotation = rotation;
            transform.position = position;
        }
    }

    // Helper function to clamp the vertical angle
    float ClampAngle(float angle, float min, float max) // This is the ClampAngle method
    {
        if (angle < -360F)
            angle += 360F;
        if (angle > 360F)
            angle -= 360F;
        return Mathf.Clamp(angle, min, max);
    }
}
