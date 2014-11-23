using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class TileMap : MonoBehaviour
{
    public GameObject[] tileMap;
    public GameObject[] baseLayerMap;
    public Sprite[] tileSprites;
    public Dictionary<string, Sprite> tileSpriteMap;
    int numTiles, numRows, numCols;
    Vector2 mapSize;
    float tileWidth, tileHeight, mapWidth, mapHeight;

    // Use this for initialization
    void Start()
    {
        numRows = numCols = 40;
        tileWidth = tileHeight = .32f;
        numTiles = numRows * numCols;
        LoadTileSprites();
        InitializeTileMap(numRows, numCols, tileWidth, tileHeight);
    }

    void LoadTileSprites()
    {
        tileSprites = Resources.LoadAll<Sprite>("images/terrain_atlas");
        tileSpriteMap = new Dictionary<string, Sprite>();
        if (tileSprites.Length == 0)
            Debug.Log("Failed to load sprites");
        foreach (Sprite sprite in tileSprites)
        {
            tileSpriteMap.Add(sprite.name, sprite);
        }
    }

    // Update is called once per frame
    void Update()
    {
        //TODO: - later
        //    Handle animated tiles
    }

    void CreateBaseLayer(int rows, int cols, float tileWidth, float tileHeight)
    {
        baseLayerMap = new GameObject[numTiles];
        float currX = 0.0f;
        float currY = 0.0f;
        mapWidth = mapHeight = 0.0f;
        float width = 0, height = 0;
        for (int row = 0; row < rows; row++)
        {
            for (int col = 0; col < cols; col++)
            {
                GameObject tileObject = new GameObject("Tile");
                tileObject.transform.position = new Vector3(currX, currY, 0);
                tileObject.AddComponent<Tile>();
                tileObject.GetComponent<Tile>().Position = new Vector2(currX, currY);

                string tileID = "terrain_atlas_112";
                Sprite spr = tileSpriteMap[tileID];
                tileObject.GetComponent<Tile>().Layer = Tile.TileLayers.Passable;
                if (spr == null)
                {
                    Debug.Log("name was incorrect");
                    continue;
                }
                tileObject.GetComponent<Tile>().SetSprite(spr);
                tileObject.GetComponent<SpriteRenderer>().sortingLayerName = "BaseGround";
                tileObject.GetComponent<SpriteRenderer>().material = Resources.Load<Material>("Materials/New Material");
                width = tileWidth;
                mapWidth += width;
                height = tileHeight;
                mapHeight += height;
                tileObject.GetComponent<Tile>().Size = new Vector2(tileWidth, tileHeight);
                baseLayerMap[GetTile(row, col)] = tileObject;
                currX += width;
            }
            currX = 0.0f;
            currY -= height;
        }
    }

    void InitializeTileMap(int rows, int cols, float tileWidth, float tileHeight)
    {
        CreateBaseLayer(rows, cols, tileWidth, tileHeight);

        tileMap = new GameObject[numTiles];
        float currX = 0.0f;
        float currY = 0.0f;
        float width = 0, height = 0;
        for (int row = 0; row < rows; row++)
        {
            for (int col = 0; col < cols; col++)
            {
                GameObject tileObject = new GameObject("Tile");
                tileObject.transform.position = new Vector3(currX, currY, 0);
                tileObject.AddComponent<Tile>();
                tileObject.GetComponent<Tile>().Position = new Vector2(currX, currY);

                int tileInt = Random.Range(0, 3);
                switch (tileInt)
                {
                    case 0:
                        tileInt = 177;
                        break;
                    case 1:
                        tileInt = 112;
                        break;
                    case 2:
                        tileInt = 115;
                        break;
                }
                switch (Random.Range(0, 4))
                {
                    case 0:
                        tileObject.GetComponent<Tile>().Layer = Tile.TileLayers.DOT;
                        break;
                    case 1:
                        tileObject.GetComponent<Tile>().Layer = Tile.TileLayers.Passable;
                        break;
                    case 2:
                        tileObject.GetComponent<Tile>().Layer = Tile.TileLayers.Kill;
                        break;
                    case 3:
                        tileObject.GetComponent<Tile>().Layer = Tile.TileLayers.Impassable;
                        tileInt = 495;
                        break;
                    default:
                        break;
                }
                //Top Left
                if (row >= 0 && row <= 2 && col == 0)
                {
                    tileObject.GetComponent<Tile>().Layer = Tile.TileLayers.Impassable;
                    switch (row)
                    {
                        case 0:
                            tileInt = 36;
                            break;
                        case 1:
                            tileInt = 68;
                            break;
                        case 2:
                            tileInt = 100;
                            break;
                    }
                }
                //Top Right
                else if (row >= 0 && row <= 2 && col == numCols - 1)
                {
                    tileObject.GetComponent<Tile>().Layer = Tile.TileLayers.Impassable;
                    switch (row)
                    {
                        case 0:
                            tileInt = 35;
                            break;
                        case 1:
                            tileInt = 67;
                            break;
                        case 2:
                            tileInt = 99;
                            break;
                    }
                }
                //Top row
                else if (row == 0)
                {
                    tileObject.GetComponent<Tile>().Layer = Tile.TileLayers.Impassable;
                    tileInt = 65;
                }
                else if (row == 1)
                {
                    tileObject.GetComponent<Tile>().Layer = Tile.TileLayers.Impassable;
                    tileInt = 97;
                }
                else if (row == 2)
                {
                    tileObject.GetComponent<Tile>().Layer = Tile.TileLayers.Passable;
                    tileInt = 129;
                }
                //Bottom Left
                else if ((row == numRows - 1) && (col == 0))
                {
                    Debug.Log("Bottom Left");
                    tileObject.GetComponent<Tile>().Layer = Tile.TileLayers.Impassable;
                    tileInt = 4;
                }
                //Bottom Right
                else if ((row == numRows - 1) && (col == numCols - 1))
                {
                    tileObject.GetComponent<Tile>().Layer = Tile.TileLayers.Impassable;
                    tileInt = 3;
                }
                //Left Col
                else if (col == 0)
                {
                    tileObject.GetComponent<Tile>().Layer = Tile.TileLayers.Impassable;
                    tileInt = 34;
                }
                //Bottom row
                else if ((row == numRows - 1))
                {
                    tileObject.GetComponent<Tile>().Layer = Tile.TileLayers.Impassable;
                    tileInt = 1;
                }
                //Right Col
                else if ((col == numCols - 1))
                {
                    tileObject.GetComponent<Tile>().Layer = Tile.TileLayers.Impassable;
                    tileInt = 32;
                }

                string tileID = "terrain_atlas_" + tileInt;
                Sprite spr = tileSpriteMap[tileID];
                if (spr == null)
                {
                    Debug.Log("name was incorrect");
                    continue;
                }
                tileObject.GetComponent<Tile>().SetSprite(spr);
                tileObject.GetComponent<SpriteRenderer>().sortingLayerName = "Ground";
                tileObject.GetComponent<SpriteRenderer>().material = Resources.Load<Material>("Materials/New Material");
                width = tileWidth;
                height = tileHeight;
                tileObject.GetComponent<Tile>().Size = new Vector2(tileWidth, tileHeight);
                if (tileObject.GetComponent<Tile>().Layer == Tile.TileLayers.Impassable)
                    tileObject.GetComponent<Tile>().AddCollisionRect("rock");
                tileMap[GetTile(row, col)] = tileObject;
                currX += width;

            }
            currX = 0.0f;
            currY -= height;
        }

        Debug.Log(mapWidth + " " + mapHeight);
    }

    public Tile.TileLayers CheckPositionOnMap(Vector2 position)
    {
        foreach (GameObject tileObj in tileMap)
        {
            if (tileObj == null)
                continue;
            Tile tile = tileObj.GetComponent<Tile>();
            SpriteRenderer renderer = tileObj.GetComponent<SpriteRenderer>();
            if (renderer == null)
                continue;
            Bounds tileBounds = renderer.bounds;
            if (tileBounds.Contains(new Vector3(position.x, position.y, 0)))
            {
                return tile.Layer;
            }
        }
        return Tile.TileLayers.OffMap;
    }
    public Tile.TileLayers CheckPositionOnMap(Bounds volume)
    {
        foreach (GameObject tileObj in tileMap)
        {
            if (tileObj == null)
                continue;
            Tile tile = tileObj.GetComponent<Tile>();
            SpriteRenderer renderer = tileObj.GetComponent<SpriteRenderer>();
            if (renderer == null)
                continue;
            Bounds tileBounds = renderer.bounds;
            if (tileBounds.Intersects(volume))
            {
                return tile.Layer;
            }
        }
        return Tile.TileLayers.OffMap;
    }

    int GetTile(int row, int col)
    {
        return row * numCols + col;
    }

    Vector2 GetTile(int tile)
    {
        return new Vector2(tile % numCols, tile / numRows);
    }

    public void DisplayMapLayers()
    {
        foreach (GameObject tileObj in tileMap)
        {
            Debug.Log(tileObj.GetComponent<Tile>().Layer);
        }
    }
}
