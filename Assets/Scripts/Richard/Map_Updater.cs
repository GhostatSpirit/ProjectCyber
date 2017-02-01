using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class Map_Updater : MonoBehaviour {
    public GameObject map;
    public InputField lat_inupt;
    public InputField lon_inupt;
    public double lat = 0;
    public double lon = 0;

    public void update_map()
    {
        lat = map.GetComponent<Bing_Map>().query_data.latitude = double.Parse(lat_inupt.text);
        lon = map.GetComponent<Bing_Map>().query_data.longitude = double.Parse(lon_inupt.text);
        map.GetComponent<Bing_Map>().refresh();
    }
    
}
