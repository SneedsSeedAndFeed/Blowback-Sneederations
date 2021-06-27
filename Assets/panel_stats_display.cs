using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class panel_stats_display : MonoBehaviour
{
    public TextMesh char_name;
    public TextMesh char_hit_chance;
    public TextMesh char_health_percent;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void display_everything(string name, string hit_chance, string health_percent)
    {
        char_name.text = name;
        char_hit_chance.text = hit_chance;
        char_health_percent.text = health_percent;
    }
    public void remove_everything()
    {
        char_name.text = null;
        char_hit_chance.text = null;
        char_health_percent.text = null;
    }

}
