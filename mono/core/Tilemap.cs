using System.Diagnostics;
using Newtonsoft.Json.Linq;

namespace mono.core
{
    /// <summary>
    /// Permet de charger une tilemap au format JSON.
    /// </summary>
    public class Tilemap
    {
        int height = 0;
        int width = 0;

        public Tilemap(string name, string json)
        {
            // On parse la tilemap
            dynamic map = JObject.Parse(json);
            height = map.height;
            width = map.width;

            // On récupère la couche du terrain
            dynamic layer0 = map.layers[0];
            int[] tiles0 = layer0.data.ToObject(typeof(int[]));
            Debug.Print("width " + width.ToString() + " ; height " + height.ToString() + 
                        " ; map size (imported) " + tiles0.Length.ToString());
        }
    }
}
