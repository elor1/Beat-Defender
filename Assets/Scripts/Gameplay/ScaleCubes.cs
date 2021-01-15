using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScaleCubes : MonoBehaviour
{
    private const float INITIAL_SCALE = 0.7f;
    private const float SCALE_DECREASE = 3.0f;
    private const float MAX_SCALE = 3.0f;
    private const float RAISE_SCALE = 100.0f;
    private const float MIN_HUE = 0.5f;

    private static GameObject _player;

    private static float maxDistanceToPlayer = 0.0f;
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

        ColourCubes();
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
        maxDistanceToPlayer = 0.0f;
        for (int x = 0; x < MapGenerator._width; x++)
        {
            for (int y = 0; y < MapGenerator._height; y++)
            {
                float distanceFromPlayer = Vector3.Distance(_player.transform.position, MapGenerator._mapGrid[x, y].obj.transform.position);
                if (distanceFromPlayer > maxDistanceToPlayer)
                {
                    maxDistanceToPlayer = distanceFromPlayer;
                }

                float yScale = RAISE_SCALE / ((distanceFromPlayer * distanceFromPlayer) * 0.35f);
                MapGenerator._mapGrid[x, y].obj.transform.localScale = new Vector3(INITIAL_SCALE, Mathf.Clamp(Mathf.Abs(yScale), INITIAL_SCALE, MAX_SCALE), INITIAL_SCALE);
            }
        }
    }

    private void ColourCubes()
    {
        for (int x = 0; x < MapGenerator._width; x++)
        {
            for (int y = 0; y < MapGenerator._height; y++)
            {
                float distanceFromPlayer = Vector3.Distance(_player.transform.position, MapGenerator._mapGrid[x, y].obj.transform.position);
                float hue = Mathf.Clamp(MapGenerator._mapGrid[x, y].obj.transform.localScale.y / MAX_SCALE, MIN_HUE, 1.0f);
                float saturation = Mathf.Clamp(AudioAnalyser._currentAmplitude, 0.0f, 1.0f);
                saturation = Mathf.Clamp(saturation * (1.0f / (distanceFromPlayer * (distanceFromPlayer / 3.0f) / maxDistanceToPlayer)), 0.0f, 1.0f);

                Color cubeColour = Color.HSVToRGB(hue, saturation, 1.0f);
                Renderer cubeRenderer = MapGenerator._mapGrid[x, y].obj.GetComponent<Renderer>();
                cubeRenderer.material.color = cubeColour;
            }
        }
    }
}
