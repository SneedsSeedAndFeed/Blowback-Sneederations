using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class player_control : MonoBehaviour
{
    private Vector3 moveDirection;
    public Rigidbody rb;
    public float moveSpeed;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

        ProcessInputs();
    }

    void FixedUpdate()
    {
        Move();
        faceMouse();
    }


    void faceMouse()
    {
        //makes the player face the mouse position
        Vector3 mousePosition = Input.mousePosition;
        mousePosition = Camera.main.ScreenToWorldPoint(mousePosition);
        Vector3 direction = new Vector3(mousePosition.x - transform.position.x, mousePosition.z - transform.position.z, 0);
        //print(mousePosition.x - transform.position.x);
        //print(mousePosition.y);
        //print(mousePosition.z - transform.position.z);
        //print("COKC");
        transform.right = direction;
        //print(transform.rotation.eulerAngles.z);
        transform.rotation = Quaternion.Euler(90, 0, transform.rotation.eulerAngles.z);
    }


    void ProcessInputs()
    {

        //takes input, essentially getaxisraw takes WASD and the arrow keys as "directions"
        float moveX = Input.GetAxisRaw("Horizontal");
        float moveY = Input.GetAxisRaw("Vertical");
        moveDirection = new Vector3(moveX, 0 , moveY).normalized;
    }

    void shot()
    {

    }

    void Move()
    {
        //animate movement and then move
        rb.velocity = new Vector3(moveDirection.x * moveSpeed, 0, moveDirection.z * moveSpeed);
    }

}
