using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.UIElements;
using System.Linq;

public class tile_manager : MonoBehaviour
{
    private Grid grid;
    [SerializeField] private Tilemap interactiveMap = null;
    [SerializeField] private Tilemap groundMap = null;
    [SerializeField] private Tilemap brushMap = null;
    [SerializeField] private Tilemap impassibleMap = null;
    [SerializeField] private Tilemap playerMap = null;
    [SerializeField] private Tile hoverTile = null;
    [SerializeField] private Tile selectedTile = null;
    [SerializeField] private Tile playerTile = null;


    private Vector3Int previousMousePos = new Vector3Int();
    private Vector3Int playerPos = new Vector3Int();

    private bool characterselected = false;
    
    void x_tile_picker(Vector3Int mousePos, int range, float tilesize, Tile tile, float min, float max, float y)
    {
        for (float x = min; x <= max; x += tilesize)
        {
            Vector3 vector3pos = new Vector3(mousePos.x + x, mousePos.y + y, 0);
            if (!impassibleMap.GetTile(Vector3Int.RoundToInt(vector3pos)))
            {
                interactiveMap.SetTile(Vector3Int.RoundToInt(vector3pos), tile);
            }
        }
    }

    List<Vector3Int> x_tile_picker(List<Vector3Int> new_highlighted_tiles,Vector3Int highlight_pos, Tile tile, int min, int max, int y)
    {
        for (int x = min; x <= max; x ++)
        {
            Vector3Int new_highlight_pos = new Vector3Int(highlight_pos.x + x, highlight_pos.y + y, 0);
            if (!impassibleMap.GetTile(new_highlight_pos))
            {
                interactiveMap.SetTile(new_highlight_pos, tile);
                new_highlighted_tiles.Add(new_highlight_pos);
            }
        }
        return new_highlighted_tiles;
    }



    void highlight_tile_in_range_without_obstacle(Vector3Int mousePos, int range, float tilesize, Tile tile)
    {
        for (float y = -tilesize * range; y <= tilesize * range; y += tilesize)
        {
            print(y);
            if (mousePos.y % 2 == 0)
            {
                if ((y) / tilesize % 2 == 0)
                {
                    x_tile_picker(mousePos, range, tilesize, tile, -range + Mathf.Abs(y / 2), range - Mathf.Abs(y / 2), y);
                }
                else
                {
                    x_tile_picker(mousePos, range, tilesize, tile, -range + (Mathf.Abs(y) - 1) / 2, range - (Mathf.Abs(y) + 1) / 2, y);
                }
            }
            else
            {
                if ((y) / tilesize % 2 == 0)
                {
                    x_tile_picker(mousePos, range, tilesize, tile, -range + Mathf.Abs(y / 2), range - Mathf.Abs(y / 2), y);
                }
                else
                {
                    x_tile_picker(mousePos, range, tilesize, tile, -range + (Mathf.Abs(y) - 1) / 2 + tilesize, range - (Mathf.Abs(y) + 1) / 2 + tilesize, y);
                }
            }
        }
    }

    void highlight_tile_in_range(Vector3Int mousePos, int range, float tilesize, Tile tile)
    {

        List<Vector3Int> highlighted_tiles = new List<Vector3Int>();
        highlighted_tiles.Add(mousePos);
        /*for (int y = -1; y <= 1; y++)
        {
            if(y % 2 == 0)
            {
                for(int x = -1; x <= 1; x++)
                {
                    Vector3Int highlight_pos = new Vector3Int(mousePos.x + x, mousePos.y + y, 0);
                    if (!impassibleMap.GetTile(highlight_pos))
                    {
                        interactiveMap.SetTile(highlight_pos, tile);
                        highlighted_tiles.Add(highlight_pos);
                    }
                }
            }
        }*/
        for(int i = 0; i < range; i++)
        {
            List<Vector3Int> new_highlighted_tiles = new List<Vector3Int>();
            foreach (Vector3Int highlight_pos in highlighted_tiles)
            {
                for (int y = -1; y <= 1; y++)
                {
                    if (y % 2 == 0)
                    {
                        new_highlighted_tiles = x_tile_picker(new_highlighted_tiles,highlight_pos, tile, -1, 1, y);
                    }
                    else if(highlight_pos.y % 2 != 0)
                    {
                        new_highlighted_tiles = x_tile_picker(new_highlighted_tiles, highlight_pos, tile, 0, 1, y);
                    }
                    else
                    {
                        new_highlighted_tiles = x_tile_picker(new_highlighted_tiles, highlight_pos, tile, -1, 0, y);
                    }
                }
            }
            highlighted_tiles = highlighted_tiles.Concat(new_highlighted_tiles).ToList();
        }
        /*
        for (float y = -tilesize*range; y <= tilesize*range; y+= tilesize)
        {
            print(y);
            if(mousePos.y % 2 == 0)
            {
                if ((y) / tilesize % 2 == 0)
                {
                    x_tile_picker(mousePos, range, tilesize, tile, -range + Mathf.Abs(y / 2), range - Mathf.Abs(y / 2), y);
                }
                else
                {
                    x_tile_picker(mousePos, range, tilesize, tile, -range + (Mathf.Abs(y) - 1) / 2, range - (Mathf.Abs(y) + 1) / 2, y);
                }
            }
            else
            {
                if ((y) / tilesize % 2 == 0)
                {
                    x_tile_picker(mousePos, range, tilesize, tile, -range + Mathf.Abs(y / 2), range - Mathf.Abs(y / 2), y);
                }
                else
                {
                    x_tile_picker(mousePos, range, tilesize, tile, -range + (Mathf.Abs(y) - 1) / 2 + tilesize, range - (Mathf.Abs(y) + 1) / 2 + tilesize, y);
                }
            }
        //print(mousePos.x + Mathf.Cos(60 * i));
        //Vector3 vector3pos = new Vector3(mousePos.x + Mathf.Cos(Mathf.PI/3 * i), mousePos.y + Mathf.Sin(Mathf.PI/3 * i), 0);
        //tilelist[i] = Vector3Int.RoundToInt(vector3pos);
        //interactiveMap.SetTile(Vector3Int.RoundToInt(vector3pos), hoverTile);
        }*/
            
    }

    // Start is called before the first frame update
    void Start()
    {
        Vector3Int randpos = new Vector3Int(Random.Range(-8, 8), Random.Range(-8, 8), 0);
        playerPos = randpos;
        playerMap.SetTile(randpos, playerTile);
        //drops player in some random spot
        grid = gameObject.GetComponent<Grid>();
    }

    // Update is called once per frame
    void Update()
    {
        // Mouse over -> highlight tile
        Vector3Int mousePos = GetMousePosition();
        mousePos.x = Mathf.Clamp(mousePos.x, -8, 8);
        mousePos.y = Mathf.Clamp(mousePos.y, -8, 8);
        if (!characterselected)
        {
            interactiveMap.SetTile(previousMousePos, null);
        }
        if (Input.GetMouseButton(0))
        {
            if (Input.GetMouseButtonDown(0))
            {
                if (playerMap.GetTile(mousePos))
                {
                    playerPos = mousePos;
                    highlight_tile_in_range(mousePos, 3, 1f, hoverTile);
                    characterselected = true;
                    interactiveMap.SetTile(mousePos, selectedTile);
                }
                else if(characterselected)
                {
                    if (interactiveMap.GetTile(mousePos) == hoverTile)
                    {
                        playerMap.SetTile(playerPos, null);
                        playerPos = mousePos;
                        playerMap.SetTile(playerPos, playerTile);
                    }
                    else
                    {
                        characterselected = false;
                    }
                    highlight_tile_in_range_without_obstacle(mousePos, 40, 1f, null);

                }
            }
            if (!characterselected)
            {
                interactiveMap.SetTile(mousePos, selectedTile);
            }
            //if you press lmb, the tile where your mouse is becomes "selected" whatever the fuck that means
        }
        else
        {
            if (!characterselected)
            {
                interactiveMap.SetTile(mousePos, hoverTile);
            }
            //simply hovering your mouse over a tile will make it piss yellow
        }
        previousMousePos = mousePos;

        
        
    }

    Vector3Int GetMousePosition()
    {
        Vector2 mouseWorldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        return grid.WorldToCell(mouseWorldPos);
    }
}
