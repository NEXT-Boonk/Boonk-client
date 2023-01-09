using UnityEngine;

public class Camera : MonoBehaviour
{
    // Rotation around x-axis in degrees
    private float rotationX;
    // Rotation around y-axis in degrees
    private float rotationY;
    
    [SerializeField] private float sensitivity = 500;

    /*
    [SerializeField] private float zoomMin = 3.5f;
    [SerializeField] private float zoomMax = 15f;
    [SerializeField] private float zoomDeaf = 10f;
    [SerializeField] private float zoomDist;

    
    Vector3 dollyDir;
    public float dist;
    public float smooth = 5f;
    */

    // Start is called before the first frame update
    void Start()
    {
        // Mouse settings
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        
        /*
        playerDirection = transform.localPosition.normalized;
        zoomDist = transform.localPosition.magnitude;
        */
    }

    // Update is called once per frame
    void Update()
    {
         // Get mouse input
        float mouseX = Input.GetAxisRaw("Mouse X") * sensitivity * Time.deltaTime;
        float mouseY = Input.GetAxisRaw("Mouse Y") * sensitivity * Time.deltaTime;

        // Dont ask. Quaternions are retarded
        rotationY += mouseX; 
        rotationX -= mouseY;
        rotationX = Mathf.Clamp(rotationX, -90f, 90f);
        
        // Rotate camera
        transform.rotation = Quaternion.Euler(rotationX, rotationY, 0);

        /*
        // Nicks camera zoom
        Vector3 desiredCamPos = transform.parent.TransformPoint(dollyDir * zoomMax);
        RaycastHit hit;

        if(Physics.Linecast (transform.parent.position, desiredCamPos, out hit))
        {
            zoomDist = Mathf.Clamp(hit.distance, zoomMin, zoomMax);
        }
        else
        {
            zoomDist = zoomDeaf;
        }
        transform.localPosition = Vector3.Lerp(transform.localPosition, dollyDir * dist, Time.deltaTime * smooth);
        */
    } 
}
