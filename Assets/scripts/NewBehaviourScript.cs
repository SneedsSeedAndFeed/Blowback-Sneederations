using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/*
public class NewBehaviourScript : MonoBehaviour
{

    public int distance_public = new int();

    IEnumerator FindDist(System.Action<int> callBack)
    {

        for (int x = 0; x < 4; x++)
        {
            if (x == 3)
            {
                yield return x;
                callBack(x);
                break;

            }

        }


    }

    string get_hit_chance()
    {
        int distance = 0;
        StartCoroutine(
            FindDist((int i) => {
                distance = i;
                distance_public = i;
            })

        );
        print(distance);
        print(distance_public);
        float chance = 70 * (100 - distance * distance) / 100;
        chance = Mathf.Floor(chance);
        return chance.ToString() + "%";
        
    }

    void Update()
    {
        print(get_hit_chance());

    }

}*/


