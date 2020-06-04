using JetBrains.Annotations;
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
    public static List<GameObject> tiles = new List<GameObject>();
    // Start is called before the first frame update
    void Start()
    {
        GenerateGrid();
        Camera.main.transform.position = new Vector3(transform.position.x, transform.position.y, -10);
        //Camera.current.WorldToScreenPoint(new Vector3(transform.position.x, transform.position.y, -10));
    }

    private void GenerateGrid()
    {
        int tileIndex = 0;
        GameObject referenceTile = (GameObject)Instantiate(Resources.Load("placeholderTile"));

        for (int row = 0; row < rows; row++)
        {
            for (int col = 0; col < cols; col++)
            {
                GameObject tile = Instantiate(referenceTile, transform) as GameObject;
                tile.AddComponent<BoxCollider2D>();
                tile.name = tileIndex.ToString();
                tile.layer = 8;

                if (row == 0)
                {
                    tile.GetComponent<BuildingController>().unlockedForBuilding = true;
                    Destroy(tile.transform.GetChild(0).gameObject);
                }

                float posX = col * tileSize;
                float posY = row * -tileSize; // <-- cartesian position system
                tile.transform.position = new Vector2(posX, posY);
                tiles.Add(tile);
                tileIndex++;
            }
        }

        Destroy(referenceTile);
        float gridWidth = cols * tileSize;
        float gridHeight = rows * tileSize;
        transform.position = new Vector2(-(gridWidth / 2 - tileSize / 2), gridHeight / 2 - tileSize / 2);
    }
}
