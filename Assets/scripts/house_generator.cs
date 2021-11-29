using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class house_generator : MonoBehaviour
{
    // Start is called before the first frame update
    public Transform parent;
    public GameObject wall;

    void Awake()
    {
        construct_house();
    }
    void Start()
    {
        
    }

    void construct_house()
    {
        //each object i make is a wall for a house
        float length = Random.Range(30f, 80f);
        float width = Random.Range(30f, 80f);
        float l_f_width = Random.Range(0f, width - 12);
        GameObject back_wall = Instantiate(wall, new Vector3(0, 1, 0.5f * (length - 1)) + transform.position, Quaternion.Euler(90,0,0), transform);
        back_wall.transform.localScale = new Vector3(width, 1, 5);
        GameObject L_side_wall = Instantiate(wall, new Vector3(-0.5f * (width - 1) , 1, 0) + transform.position, Quaternion.Euler(90, 0, 90), transform);
        L_side_wall.transform.localScale = new Vector3((length - 2), 1, 5);
        GameObject R_side_wall = Instantiate(wall, new Vector3(0.5f * (width - 1), 1, 0) + transform.position, Quaternion.Euler(90, 0, 90), transform);
        R_side_wall.transform.localScale = new Vector3((length - 2), 1, 5);
        GameObject L_front_wall = Instantiate(wall, new Vector3(-0.5f * (width - l_f_width), 1, -0.5f * (length - 1)) + transform.position, Quaternion.Euler(90,0,0), transform);
        L_front_wall.transform.localScale = new Vector3(l_f_width, 1, 5);
        GameObject R_front_wall = Instantiate(wall, new Vector3(0.5f * (l_f_width + 10), 1, -0.5f * (length - 1)) + transform.position, Quaternion.Euler(90,0,0), transform);
        R_front_wall.transform.localScale = new Vector3((width - l_f_width - 10), 1, 5);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
