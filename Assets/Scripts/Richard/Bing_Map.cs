using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class Bing_Map : MonoBehaviour {
    RawImage rawImage;
    public Bing_Map_Location query_data;
    public int zoom = 13;
    public int size = 512;
    public string bing_map_key = "AkgiM7JUZs4i22iTTzRUZfBfLr3N_GbeMShMEk0f2W6AeQfbMlV-5OfwvS8zIcyo";

    // Use this for initialization
    void Start()
    {
        rawImage = GetComponent<RawImage>();
        refresh();
    }

    public void refresh()
    {
        StartCoroutine(get());
    }

    public IEnumerator get()
    {
        string url = "http://dev.virtualearth.net/REST/v1/Imagery/Map/Road";
        string center_location = string.Format("{0},{1}", query_data.latitude, query_data.longitude);
        string u_zoom = zoom.ToString();
        string map_size = string.Format("mapSize={0},{1}", query_data.size_X, query_data.size_Y);
        string key = string.Format("&key={0}", bing_map_key).Replace(" ","");
        var final_url = new WWW(url + "/" + center_location + "/" + u_zoom + "?" + map_size + key);
        yield return final_url;
        rawImage.texture = final_url.textureNonReadable;
        rawImage.SetNativeSize();
    }
	
    [System.Serializable]
    public class Bing_Map_Location
    {
        public double latitude = 35.7022121;
        public double longitude = 139.7722703;
        public int size_X = 600;
        public int size_Y = 600;
    }

}
