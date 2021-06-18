using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.UIElements;

public class tile_manager : MonoBehaviour
{
    private Grid grid;
    [SerializeField] private Tilemap interactiveMap = null;
    [SerializeField] private Tilemap groundMap = null;
    [SerializeField] private Tilemap playerMap = null;
    [SerializeField] private Tile hoverTile = null;
    [SerializeField] private Tile selectedTile = null;
    [SerializeField] private Tile playerTile = null;


    private Vector3Int previousMousePos = new Vector3Int();

    // Start is called before the first frame update
    void Start()
    {
        Vector3Int randpos = new Vector3Int(Random.Range(-10, 10), Random.Range(-10, 10), 0);
        playerMap.SetTile(randpos, playerTile);
        //drops player in some random spot
        grid = gameObject.GetComponent<Grid>();
    }

    // Update is called once per frame
    void Update()
    {
        // Mouse over -> highlight tile
        Vector3Int mousePos = GetMousePosition();
        interactiveMap.SetTile(previousMousePos, null);
        if (groundMap.HasTile(mousePos))
        { // Remove old hoverTile
            if (Input.GetMouseButton(0))
            {
                interactiveMap.SetTile(mousePos, selectedTile);
                //if you press lmb, the tile where your mouse is becomes "selected" whatever the fuck that means
            }
            else
            {
                interactiveMap.SetTile(mousePos, hoverTile);
                //simply hovering your mouse over a tile will make it piss yellow
            }
            previousMousePos = mousePos;

        }
        
    }

    Vector3Int GetMousePosition()
    {
        Vector2 mouseWorldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        return grid.WorldToCell(mouseWorldPos);
    }
}
