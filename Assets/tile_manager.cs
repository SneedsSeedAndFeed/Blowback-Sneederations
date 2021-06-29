using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.UIElements;
using System.Linq;



public class tile_manager : MonoBehaviour
{

    Unit player = new Unit();
    Unit enemy = new Unit();


    private Grid grid;
    [SerializeField] private Tilemap highlightMap = null;
    [SerializeField] private Tilemap groundMap = null;
    [SerializeField] private Tilemap brushMap = null;
    [SerializeField] private Tilemap impassibleMap = null;
    [SerializeField] private Tilemap playerMap = null;
    [SerializeField] private Tile hoverTile = null;
    [SerializeField] private Tile redTile = null;
    [SerializeField] private Tile hoverRedTile = null;
    [SerializeField] private Tile greenTile = null;
    [SerializeField] private Tile selectedTile = null;
    [SerializeField] private Tile playerTile = null;
    [SerializeField] private Tile enemyTile = null;


    private Vector3Int previousMousePos = new Vector3Int();
    

    private Vector3Int origin = new Vector3Int(0, 0, 0);
    

    public GameObject end_button;

    public GameObject stats_ui;

    public int distance = new int();






    void x_tile_picker(Vector3Int mousePos, int range, int tilesize, Tile tile, int min, int max, int y)
    {
        for (int x = min; x <= max; x ++)
        {
            Vector3Int pos = new Vector3Int(Mathf.Clamp(mousePos.x + x, -8,8), Mathf.Clamp(mousePos.y + y,-8,8), 0);
            if (!impassibleMap.GetTile(pos))
            {
                highlightMap.SetTile(pos, tile);
            }
        }
    }









    List<Vector3Int> x_tile_picker(List<Vector3Int> new_highlighted_tiles,List<Vector3Int> poslist,Vector3Int highlight_pos, Tile tile, int min, int max, int y)
    {
        for (int x = min; x <= max; x ++)
        {
            Vector3Int new_highlight_pos = new Vector3Int(Mathf.Clamp(highlight_pos.x + x,-8,8), Mathf.Clamp(highlight_pos.y + y,-8,8), 0);
            if (!impassibleMap.GetTile(new_highlight_pos))
            {
                if (!poslist.Contains(new_highlight_pos))
                {
                    new_highlighted_tiles.Add(new_highlight_pos);
                    poslist.Add(new_highlight_pos);
                }
            }
        }
        return new_highlighted_tiles;
    }

    List<Vector3Int> x_tile_picker(List<Vector3Int> new_highlighted_tiles, Vector3Int highlight_pos, int min, int max, int y)
    {
        for (int x = min; x <= max; x++)
        {
            Vector3Int new_highlight_pos = new Vector3Int(Mathf.Clamp(highlight_pos.x + x, -8, 8), Mathf.Clamp(highlight_pos.y + y, -8, 8), 0);
            if (!impassibleMap.GetTile(new_highlight_pos))
            {
                if (!new_highlighted_tiles.Contains(new_highlight_pos))
                {
                    new_highlighted_tiles.Add(new_highlight_pos);
                }
            }
        }
        return new_highlighted_tiles;
    }








    void highlight_tile_in_range_without_obstacle(Vector3Int mousePos, int range, int tilesize, Tile tile)
    {
        
        for (int y = -tilesize * range; y <= tilesize * range; y += tilesize)
        {
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







    void highlight_tile_in_range(Vector3Int mousePos, List<Vector3Int> poslist, int range, float tilesize, Tile tile)
    {

        List<Vector3Int> highlighted_tiles = new List<Vector3Int>();
        highlighted_tiles.Add(mousePos);

        for(int i = 0; i < range; i++)
        {
            List<Vector3Int> new_highlighted_tiles = new List<Vector3Int>();
            foreach (Vector3Int highlight_pos in highlighted_tiles)
            {
                for (int y = -1; y <= 1; y++)
                {
                    if (y % 2 == 0)
                    {
                        new_highlighted_tiles = x_tile_picker(new_highlighted_tiles, poslist, highlight_pos, tile, -1, 1, y);
                    }
                    else if(highlight_pos.y % 2 != 0)
                    {
                        new_highlighted_tiles = x_tile_picker(new_highlighted_tiles, poslist, highlight_pos, tile, 0, 1, y);
                    }
                    else
                    {
                        new_highlighted_tiles = x_tile_picker(new_highlighted_tiles, poslist, highlight_pos, tile, -1, 0, y);
                    }
                }
            }
            highlighted_tiles = highlighted_tiles.Concat(new_highlighted_tiles).ToList();
        }
            
    }

    int FindDist(Vector3Int shooterPos, Vector3Int target, int range)
    {

        List<Vector3Int> highlighted_tiles = new List<Vector3Int>();
        highlighted_tiles.Add(shooterPos);

        for (int i = 0; i < range; i++)
        {
            List<Vector3Int> new_highlighted_tiles = new List<Vector3Int>();
            foreach (Vector3Int highlight_pos in highlighted_tiles)
            {
                for (int y = -1; y <= 1; y++)
                {
                    if (y % 2 == 0)
                    {
                        new_highlighted_tiles = x_tile_picker(new_highlighted_tiles, highlight_pos, -1, 1, y);
                    }
                    else if (highlight_pos.y % 2 != 0)
                    {
                        new_highlighted_tiles = x_tile_picker(new_highlighted_tiles, highlight_pos, 0, 1, y);
                    }
                    else
                    {
                        new_highlighted_tiles = x_tile_picker(new_highlighted_tiles, highlight_pos, -1, 0, y);
                    }
                }
            }
            highlighted_tiles = highlighted_tiles.Concat(new_highlighted_tiles).ToList();
            if (highlighted_tiles.Contains(enemy.pos))
            {
                return i + 1;
            }
        }
        return 0;
        /*
        yield return -1;
        callBack(-1);*/

    }





    void remove_prev_mousePos(Vector3Int prevMousePos)
    {
        bool can_be_removed = new bool();
        can_be_removed = false;
        if (!player.selected)
        {
            foreach (Vector3Int i in player.new_movement_pos_list)
            {
                if (i == prevMousePos)
                {
                    can_be_removed = true;
                }
            }
            if (!can_be_removed)
            {
                highlightMap.SetTile(prevMousePos, null);
            }
        }
    }


    float get_hit_chance()
    {
        distance = 0;
        distance = FindDist(player.pos, enemy.pos, 10);
        print(distance);
        //print(distance);
        if (distance == 0)
        {
            return 0;
        }
        else
        {
            float chance = 70 * (100 - distance*distance) / 100;
            chance = Mathf.Floor(chance);
            return chance;
        }
    }




    void player_movement_control(Vector3Int mousePos)
    {
        if (Input.GetMouseButton(0))
        {
            stats_ui.GetComponent<panel_stats_display>().remove_everything();
            if (Input.GetMouseButtonDown(0))
            {
                if (playerMap.GetTile(mousePos) == player.tile)
                {
                    player.pos= mousePos;
                    highlight_tile_in_range(mousePos, player.possible_movement_pos_list, 3, 1f, hoverTile);
                    foreach(Vector3Int i in player.possible_movement_pos_list)
                    {
                        highlightMap.SetTile(i, hoverTile);
                    }
                    player.possible_movement_pos_list.Remove(enemy.pos);

                    player.selected = true;
                    highlightMap.SetTile(mousePos, selectedTile);
                    highlightMap.SetTile(enemy.pos, hoverRedTile);
                }
                else if (player.selected)
                {
                    if (player.possible_movement_pos_list.Contains(mousePos))
                    {
                        highlightMap.SetTile(mousePos, selectedTile);
                        if (player.futurePos != mousePos)
                        {
                            player.new_movement_pos_list.Remove(player.futurePos);
                        }
                        player.pastPos = player.pos;
                        player.futurePos = mousePos;
                        player.new_movement_pos_list.Add(player.futurePos);
                        player.selected = false;
                    }
                    else if (mousePos == enemy.pos)
                    {
                        highlightMap.SetTile(mousePos, redTile);
                    }
                    else
                    {
                        player.selected = false;
                    }
                    foreach (Vector3Int i in player.possible_movement_pos_list)
                    {
                        if (highlightMap.GetTile(i) == hoverTile || i == player.pos)
                        {
                            highlightMap.SetTile(i, null);
                        }
                    }
                    highlightMap.SetTile(player.futurePos, greenTile);
                    player.possible_movement_pos_list.Clear();
                    

                }
            }
            if (!player.selected)
            {
                if (mousePos == player.futurePos)
                {
                    highlightMap.SetTile(mousePos, greenTile);
                }
                else
                {
                    highlightMap.SetTile(mousePos, selectedTile);
                }
                if(highlightMap.GetTile(enemy.pos) == hoverRedTile)
                {
                    highlightMap.SetTile(enemy.pos, null);
                }
            }
        }
        else
        {
            if (!player.selected)
            {

                if (mousePos == player.futurePos)
                {
                    highlightMap.SetTile(mousePos, greenTile);
                }
                else
                {
                    highlightMap.SetTile(mousePos, hoverTile);
                }
            }
            else
            {
                if (mousePos == enemy.pos)
                {

                    stats_ui.GetComponent<panel_stats_display>().display_everything("enemy", get_hit_chance().ToString() + "%", "100%");
                }
                else
                {
                    stats_ui.GetComponent<panel_stats_display>().remove_everything();
                }
            }
            //simply hovering your mouse over a tile will make it piss yellow
        }
    }

   






    void endButton(Vector3Int mousePos)
    {
        if (end_button.GetComponent<end_button_controller>().buttonPressed)
        {
           player.pos = player.futurePos;
            player.new_movement_pos_list.Clear();
            playerMap.SetTile(player.pastPos, null);
            playerMap.SetTile(player.pos, player.tile);
            highlight_tile_in_range_without_obstacle(mousePos, 40, 1, null);
        }
    }





    





    // Start is called before the first frame update
    void Start()
    {
        player.pos = player.randPos(-8, 8);
        enemy.pos = enemy.randPos(-8, 8);
        while (player.pos == enemy.pos || !groundMap.GetTile(player.pos) || !groundMap.GetTile(enemy.pos))
        {
            player.pos = player.randPos(-8,8);
            player.pos = player.randPos(-8, 8);
        }
        player.futurePos = player.pos;
        enemy.futurePos = enemy.pos;
        player.tile = playerTile;
        enemy.tile = enemyTile;
        playerMap.SetTile(player.pos, player.tile);
        playerMap.SetTile(enemy.pos, enemy.tile);
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
        //keeps the recorded mouse position within the grid (so we dont end up drawing  tiles in places that we arent allowed to)
        endButton(mousePos);
        //pressing the end button causes every chosen action to occur and eventually the ai will do their move here too.

        remove_prev_mousePos(previousMousePos);
        //this little fucking thing removes the highlighted tile from where the mouse was previously, but to stop it from removing tiles
        //that we actually dont want removed in this way we gotta check each one to make sure that the previous
        //mouse position wasnt on a tile that we want to "keep"

        player_movement_control(mousePos);
        //

        previousMousePos = mousePos;
        
        
    }



    Vector3Int GetMousePosition()
    {
        Vector2 mouseWorldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        return grid.WorldToCell(mouseWorldPos);
    }
}
