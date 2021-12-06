using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class camera_control : MonoBehaviour
{
    public Transform playerObject;

    void Start()
    {
    }
    void Update()
    {
        Vector3 raisedPlayerPos = playerObject.position;
        raisedPlayerPos.y = 10;
        transform.position = raisedPlayerPos;



    }
}
