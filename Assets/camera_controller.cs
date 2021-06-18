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
        if (Input.GetMouseButton(1) || Input.GetMouseButton(2)) // if rmb or mmb are pressed you can move the camera
        {
            transform.position += new Vector3(Input.GetAxisRaw("Mouse X") * Time.deltaTime * speed,
                                       Input.GetAxisRaw("Mouse Y") * Time.deltaTime * speed, 0.0f);
        }

        myCamera.fieldOfView += zoomSpeed * Input.GetAxis("Mouse ScrollWheel");//the fov and orthographicsize(basically the zoom of the camera) are changed with scroll wheel
        myCamera.orthographicSize += zoomSpeed * Input.GetAxis("Mouse ScrollWheel");

        myCamera.orthographicSize = Mathf.Clamp(myCamera.orthographicSize, 4, 12);
        //maximum and minimum zoom

        float xpos = transform.position.x;
        float ypos = transform.position.y;

        xpos = Mathf.Clamp(xpos, -25 + myCamera.orthographicSize + 1.1f * (myCamera.orthographicSize - 5),
            29 - myCamera.orthographicSize - 1.1f * (myCamera.orthographicSize - 5));

       
        ypos = Mathf.Clamp(ypos, -28 + myCamera.orthographicSize,
            20 - myCamera.orthographicSize);

        //these two are to stop the camera from moving outside the area of the map, not made for random maps yet,
        //however it looks complicated, this is because it is made such that zooming wont change the amount you can see outside the map

        transform.position = new Vector3(xpos, ypos, transform.position.z);

    }
}
