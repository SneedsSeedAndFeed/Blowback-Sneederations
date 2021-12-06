using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class terrain_generator : MonoBehaviour
{
    public Terrain t;

    public GameObject[] pebbles;
    public GameObject[] boulders;
    public GameObject[] trees;
    public GameObject house;
    //T stands for terrain, intelligent i know.

    private TerrainData t_data;
    public Transform house_parent;
    public Transform nature_stuff;

    private void Awake()
    {
        t_data = t.terrainData;

        EditTerrain();
        foreach(Transform child in house_parent)
        {
            child.GetComponent<house_generator>().Remove_shit_from_house();
        }
    }
    void Start()
    {
    }


    public static float NextGaussian()
    {
        float v1, v2, s;
        do
        {
            v1 = 2.0f * Random.Range(0f, 1f) - 1.0f;
            v2 = 2.0f * Random.Range(0f, 1f) - 1.0f;
            s = v1 * v1 + v2 * v2;
        } while (s >= 1.0f || s == 0f);

        s = Mathf.Sqrt((-2.0f * Mathf.Log(s)) / s);

        return v1 * s;
    }


    public static float NextGaussian(float mean, float standard_deviation)
    {
        return mean + NextGaussian() * standard_deviation;
    }


    public static float NextGaussian(float mean, float standard_deviation, float min, float max)
    {
        float x;
        x = Mathf.Clamp(NextGaussian(mean, standard_deviation), min, max);
        return x;
    }

    public void EditTerrain()
    {
        //very important !! for some reason x and y are swapped in game, deal with it fucko x goes up and y goes right
        float input_mean = 1f;
        float[,,] map = new float[t_data.alphamapWidth, t_data.alphamapHeight, 8];
        //print(t_data.alphamapHeight);
        //print(t_data.alphamapWidth);
        float height_convert = 200.0f / t_data.alphamapHeight;
        float width_convert = 300.0f / t_data.alphamapWidth;
        for (int y = 0; y < t_data.alphamapHeight; y++)
        {
            for (int x = 0; x < t_data.alphamapWidth; x++)
            {
                if (Random.Range(0f, 100f) <= 0.02)
                {
                    Vector3 position = new Vector3(x*width_convert, 0.1f, y*height_convert);
                    Instantiate(pebbles[Random.Range(0,8)], position, Quaternion.Euler(90,0, Random.Range(0f, 90f)), nature_stuff);
                }
                if (Random.Range(0f, 100f) <= 11 && x % 10 == 0 && y % 10 == 0)
                {
                    Vector3 position = new Vector3(x * width_convert, 0.1f, y * height_convert);
                    Instantiate(trees[Random.Range(0, 8)], position, Quaternion.Euler(90, 0, Random.Range(0f, 90f)), nature_stuff);
                }
                if (Random.Range(0f, 100f) <= 0.002f)
                {
                    if(x > 10/width_convert && y > 10/height_convert  && x <  t_data.alphamapWidth - 10/width_convert && y < t_data.alphamapHeight - 10/height_convert)
                    {
                        Vector3 position = new Vector3(x * width_convert, 20f, y * height_convert);
                        Instantiate(boulders[Random.Range(0, 3)], position, Quaternion.Euler(90, 0, Random.Range(0f, 90f)), nature_stuff);
                    }
                }
                if (Random.Range(0f, 100f) <= 0.1f)
                {
                    if (x > 40/width_convert && y > 40/height_convert && x < t_data.alphamapWidth - 40/width_convert  && y < t_data.alphamapHeight - 40/height_convert)
                    {
                        Vector3 position = new Vector3(x * width_convert, 0, y * height_convert);
                        Instantiate(house, position, Quaternion.identity, house_parent);
                    }
                }
                float green_value = NextGaussian(input_mean, 0.25f, 0f, 1f);
                float dirt_value = 1 - green_value;
                //map[x, y, 0] = 0;
                map[x, y, 1] = green_value;
                map[x, y, 7] = dirt_value;
                /*
                for (int y_ = y; y_ < y + 5 && y_ < t_data.alphamapHeight; y_++)
                {
                    for(int x_ = x; x_ < x + 5 && x_ < t_data.alphamapWidth; x_++)
                    {
                        map[x_, y_, 0] = green_value;
                        map[x_, y_, 3] = dirt_value;

                    }
                }*/
                if (y > 1 && x < t_data.alphamapWidth - 2)
                {
                    //input_mean = (green_value + map[x + 2, y - 2, 0]) /(2f);
                    input_mean = (map[x, y-1, 1] + map[x + 2, y - 1, 1] + map[x+1,y-1,1]*Mathf.Sqrt(2)) / (Mathf.Sqrt(2)+2);

                }
            }
        }
        t_data.SetAlphamaps(0, 0, map);

    }
}
