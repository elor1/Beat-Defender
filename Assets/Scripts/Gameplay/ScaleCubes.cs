using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScaleCubes : MonoBehaviour
{
    private const float INITIAL_SCALE = 0.7f;
    private const float SCALE_DECREASE = 3.0f;
    private const float MAX_SCALE = 2.0f;
    private const float RAISE_SCALE = 50.0f;

    private static GameObject _player;
    // Start is called before the first frame update
    void Start()
    {
        
        _player = GameObject.FindGameObjectWithTag("Player");

        ResetScales();
    }

    // Update is called once per frame
    void Update()
    {
        for (int x = 0; x < MapGenerator._width; x++)
        {
            for (int y = 0; y < MapGenerator._height; y++)
            {
                if (MapGenerator._mapGrid[x, y]. type != MapGenerator.TileType.Room)
                {
                    MapGenerator._mapGrid[x, y].obj.transform.localScale = new Vector3(INITIAL_SCALE,
                        Mathf.Clamp(MapGenerator._mapGrid[x, y].obj.transform.localScale.y - (Time.deltaTime * SCALE_DECREASE),
                        INITIAL_SCALE, MAX_SCALE), INITIAL_SCALE);
                }
            }
        }
    }

    /// <summary>
    /// Reset the scale of each wall to its initial value
    /// </summary>
    public static void ResetScales()
    {
        for (int x = 0; x < MapGenerator._width; x++)
        {
            for (int y = 0; y < MapGenerator._height; y++)
            {
                MapGenerator._mapGrid[x, y].obj.transform.localScale = new Vector3(INITIAL_SCALE, INITIAL_SCALE, INITIAL_SCALE);
            }
        }
    }

    /// <summary>
    /// Raise each wall on the map by an amount relating to how far it is from the player
    /// </summary>
    public static void RaiseWalls()
    {
        //Debug.Log("BEAT");
        for (int x = 0; x < MapGenerator._width; x++)
        {
            for (int y = 0; y < MapGenerator._height; y++)
            {
                float distanceFromPlayer = Vector3.Distance(_player.transform.position, MapGenerator._mapGrid[x, y].obj.transform.position);
                float yScale = RAISE_SCALE / ((distanceFromPlayer * distanceFromPlayer) * 0.35f);
                MapGenerator._mapGrid[x, y].obj.transform.localScale = new Vector3(INITIAL_SCALE, Mathf.Clamp(Mathf.Abs(yScale), INITIAL_SCALE, MAX_SCALE), INITIAL_SCALE);
            }
        }
    }
}
