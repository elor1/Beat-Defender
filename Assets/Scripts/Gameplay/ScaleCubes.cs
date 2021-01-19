using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScaleCubes : MonoBehaviour
{
    private const float INITIAL_SCALE = 0.7f; //Starting/min scale of cubes
    private const float SCALE_DECREASE = 3.0f; //Amount cubes scale decreases each frame
    private const float MAX_SCALE = 3.0f; //Maximum scale cubes can have
    private const float RAISE_SCALE = 100.0f; //Amount cubes are scaled up each beat
    private const float MIN_HUE = 0.5f; //Minimum hue value a cube's colour can be

    private static GameObject _player; //Player game object
    private static float _maxDistanceToPlayer; //The distance between the player and the furthest cube

    // Start is called before the first frame update
    void Start()
    {
        
        _player = GameObject.FindGameObjectWithTag("Player");
        _maxDistanceToPlayer = 0.0f;

        ResetScales();
    }

    // Update is called once per frame
    void Update()
    {
        for (int x = 0; x < MapGenerator.Width; x++)
        {
            for (int y = 0; y < MapGenerator.Height; y++)
            {
                if (MapGenerator.MapGrid[x, y]. Type != MapGenerator.TileType.Room)
                {
                    //Decrease cube scales each frame
                    MapGenerator.MapGrid[x, y].Obj.transform.localScale = new Vector3(INITIAL_SCALE,
                        Mathf.Clamp(MapGenerator.MapGrid[x, y].Obj.transform.localScale.y - (Time.deltaTime * SCALE_DECREASE),
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
        for (int x = 0; x < MapGenerator.Width; x++)
        {
            for (int y = 0; y < MapGenerator.Height; y++)
            {
                MapGenerator.MapGrid[x, y].Obj.transform.localScale = new Vector3(INITIAL_SCALE, INITIAL_SCALE, INITIAL_SCALE);
            }
        }
    }

    /// <summary>
    /// Raise each wall on the map by an amount relating to how far it is from the player
    /// </summary>
    public static void RaiseWalls()
    {
        _maxDistanceToPlayer = 0.0f;
        for (int x = 0; x < MapGenerator.Width; x++)
        {
            for (int y = 0; y < MapGenerator.Height; y++)
            {
                float distanceFromPlayer = Vector3.Distance(_player.transform.position, MapGenerator.MapGrid[x, y].Obj.transform.position);
                if (distanceFromPlayer > _maxDistanceToPlayer)
                {
                    _maxDistanceToPlayer = distanceFromPlayer;
                }

                float yScale = RAISE_SCALE / ((distanceFromPlayer * distanceFromPlayer) * 0.35f);
                MapGenerator.MapGrid[x, y].Obj.transform.localScale = new Vector3(INITIAL_SCALE, Mathf.Clamp(Mathf.Abs(yScale), INITIAL_SCALE, MAX_SCALE), INITIAL_SCALE);
            }
        }
    }

    /// <summary>
    /// Colour each cube
    /// </summary>
    private void ColourCubes()
    {
        for (int x = 0; x < MapGenerator.Width; x++)
        {
            for (int y = 0; y < MapGenerator.Height; y++)
            {
                float distanceFromPlayer = Vector3.Distance(_player.transform.position, MapGenerator.MapGrid[x, y].Obj.transform.position);

                //Hue is determined by scale
                float hue = Mathf.Clamp(MapGenerator.MapGrid[x, y].Obj.transform.localScale.y / MAX_SCALE, MIN_HUE, 1.0f);

                //Saturation is determined by current audio amplitude & distance from player
                float saturation = Mathf.Clamp(AudioAnalyser.CurrentAmplitude, 0.0f, 1.0f);
                saturation = Mathf.Clamp(saturation * (1.0f / (distanceFromPlayer * (distanceFromPlayer / 3.0f) / _maxDistanceToPlayer)), 0.0f, 1.0f);

                //Set cube colour
                Color cubeColour = Color.HSVToRGB(hue, saturation, 1.0f);
                Renderer cubeRenderer = MapGenerator.MapGrid[x, y].Obj.GetComponent<Renderer>();
                cubeRenderer.material.color = cubeColour;
            }
        }
    }
}
