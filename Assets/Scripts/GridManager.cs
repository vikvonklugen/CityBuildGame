using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class GridManager : MonoBehaviour
{

    [SerializeField]
    private int rows = 6;
    [SerializeField]
    private int cols = 6;

    [SerializeField]
    private float tileSize = 1;

    public float TileSize
    {
        get
        {
            return tileSize;
        }

        private set
        {
            tileSize = value;
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        GenerateGrid();
        Camera.main.transform.position = new Vector3(transform.position.x, transform.position.y, -10);
        //Camera.current.WorldToScreenPoint(new Vector3(transform.position.x, transform.position.y, -10));
    }

    private void GenerateGrid ()
    {
        GameObject referenceTile = (GameObject)Instantiate(Resources.Load("placeholderTile"));

        for (int row = 0; row < rows; row++)
        {
            for (int col = 0; col < cols; col++)
            {

                GameObject tile = Instantiate(referenceTile, transform) as GameObject;
                float posX = col * TileSize;
                float posY = row * -TileSize; // <-- cartesian position system
                tile.transform.position = new Vector2(posX, posY);
            }
        }

        Destroy(referenceTile);

        float gridWidth = cols * TileSize;
        float gridHeight = rows * TileSize;
        transform.position = new Vector2(gridWidth / 2 + TileSize / 2, gridHeight / 2 - TileSize / 2);
    }

    // Update is called once per frame
    void Update()
    {

    }
}
