using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;


public class Unit
{

    public Vector3Int pos;
    public Vector3Int futurePos;
    public Vector3Int pastPos;
    public List<Vector3Int> new_movement_pos_list;
    public List<Vector3Int> possible_movement_pos_list;

    public Tile tile;

    public bool selected;

    public Unit()
    {
        selected = false;

    }
    public Vector3Int randPos(int min_pos,int max_pos)
    {
        return new Vector3Int(Random.Range(min_pos, max_pos), Random.Range(min_pos, max_pos), 0);
    }
}

