using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class camera_controller : MonoBehaviour
{
    public float speed;
    public float zoomSpeed; 

    private Camera myCamera;
    // Start is called before the first frame update
    void Start()
    {
        myCamera = GetComponent<Camera>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButton(1) || Input.GetMouseButton(2))
        {
            transform.position += new Vector3(Input.GetAxisRaw("Mouse X") * Time.deltaTime * speed,
                                       Input.GetAxisRaw("Mouse Y") * Time.deltaTime * speed, 0.0f);
        }

        myCamera.fieldOfView += zoomSpeed * Input.GetAxis("Mouse ScrollWheel");
        myCamera.orthographicSize += zoomSpeed * Input.GetAxis("Mouse ScrollWheel");

    }
}
