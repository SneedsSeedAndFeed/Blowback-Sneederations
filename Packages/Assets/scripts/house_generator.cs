using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class house_generator : MonoBehaviour
{
    // Start is called before the first frame update
    public Transform parent;
    public GameObject wall;
    public GameObject floor;
    public GameObject roof;
    private GameObject floor_;
    private GameObject roof_;
    public Transform player;
    public Transform nature_parent;
    private float width;
    private float length;

    void Awake()
    {
        construct_house();
    }
    void Start()
    {
        player = GameObject.Find("player").transform;

    }

    void construct_house()
    {
        
        //each object i make is a wall for a house
        length = Random.Range(30f, 80f);
        width = Random.Range(30f, 80f);
        float l_f_width = Random.Range(0f, width - 12);

        Quaternion norm_rot_x = Quaternion.Euler(90, 0, 0);
        Quaternion norm_rot_z = Quaternion.Euler(90, 90, 0);


        GameObject back_wall = Instantiate(wall, new Vector3(0, 1, 0.5f * (length - 1)) + transform.position, norm_rot_x, transform);
        back_wall.transform.localScale = new Vector3(width, 1, 5);

        GameObject L_side_wall = Instantiate(wall, new Vector3(-0.5f * (width - 1) , 1, 0) + transform.position, norm_rot_z, transform);
        L_side_wall.transform.localScale = new Vector3((length - 2), 1, 5);

        GameObject R_side_wall = Instantiate(wall, new Vector3(0.5f * (width - 1), 1, 0) + transform.position, norm_rot_z, transform);
        R_side_wall.transform.localScale = new Vector3((length - 2), 1, 5);

        GameObject L_front_wall = Instantiate(wall, new Vector3(-0.5f * (width - l_f_width), 1, -0.5f * (length - 1)) + transform.position, norm_rot_x, transform);
        L_front_wall.transform.localScale = new Vector3(l_f_width, 1, 5);

        GameObject R_front_wall = Instantiate(wall, new Vector3(0.5f * (l_f_width + 10), 1, -0.5f * (length - 1)) + transform.position, norm_rot_x, transform);
        R_front_wall.transform.localScale = new Vector3((width - l_f_width - 10), 1, 5);

        floor_ = Instantiate(floor, transform.position, norm_rot_x, transform);
        floor_.transform.localScale = new Vector3(width/19.7f, length/19.7f, 0);

        roof_ = Instantiate(roof, transform.position, norm_rot_x, transform);
        roof_.transform.localScale = new Vector3(width/6f, length/6f, 0);
    }

    public void Remove_shit_from_house()
    {
        //do not change the name of nature stuff without changing the name of this fucker
        nature_parent = GameObject.Find("nature_stuff").transform;
        float x = transform.position.x;
        float z = transform.position.z;
        //print(width);
        print(transform.parent.childCount +"bruh");
        List<Transform> destroy_list = new List<Transform>();
        foreach (Transform child_house in transform.parent)
        {
            float c_h_x = child_house.position.x;
            float c_h_z = child_house.position.z;
            float c_h_width = child_house.GetComponent<house_generator>().width;
            float c_h_length = child_house.GetComponent<house_generator>().length;
            if (z + length / 2 + c_h_length / 2 >= c_h_z && z <= c_h_z && ((x - width / 2 - c_h_width / 2 <= c_h_x && x >= c_h_x) || (x + width / 2 + c_h_width / 2 >= c_h_x && x <= c_h_x)))
            {
                if (transform != child_house)
                {
                    destroy_list.Add(child_house);
                    Destroy(child_house.gameObject);
                }
            }

        }

        foreach(Transform dead_house in destroy_list)
        {
            dead_house.parent = null;
        }
        print(transform.parent.childCount+ "FUCK");
        foreach (Transform child in nature_parent)
        {
            float c_x = child.position.x;
            float c_z = child.position.z;
            //is the position of a nature object inside the house
            if (c_x > x - width/2 - 5 && c_x < x + width / 2 + 5 && c_z > z - length/2 -5 && c_z < z + length/2 + 5)
            {
                Destroy(child.gameObject);
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        roof_removal();
    }

    void roof_removal()
    {
        if (player.position.x < transform.position.x + width / 2 && player.position.x > transform.position.x - width / 2
            && player.position.z < transform.position.z + length / 2 && player.position.z > transform.position.z - length / 2)
        {
            print("go get fucked you bastard");
            roof_.SetActive(false);
        }
        else
        {
            roof_.SetActive(true);
        }
    }
}
