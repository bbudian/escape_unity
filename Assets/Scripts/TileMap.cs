using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class TileMap : MonoBehaviour
{
    public GameObject[] tileMap;
    public Sprite[] tileSprites;
    public Dictionary<string, Sprite> tileSpriteMap;
    int numTiles, numRows, numCols;
    Vector2 mapSize;
    float tileWidth, tileHeight, mapWidth, mapHeight;

    // Use this for initialization
    void Start()
    {
        numRows = numCols = 40;
        tileWidth = tileHeight = 0.5f;
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

    void InitializeTileMap(int rows, int cols, float tileWidth, float tileHeight)
    {
        tileMap = new GameObject[numTiles];
        float currX = 0.0f;
        float currY = 0.0f;
        mapWidth = mapHeight = 0.0f;
        float width = 0, height = 0;      
        for (int row = 0; row < rows; row++)
        {
            for (int col = 0; col < cols; col++)
            {
                GameObject tileObject = new GameObject("Tile");
                tileObject.transform.position = new Vector3(currX,currY,0);
                tileObject.AddComponent<Tile>();
                tileObject.GetComponent<Tile>().Position = new Vector2(currX, currY);
                
                int tileInt = Random.Range(0, 3);
                switch(tileInt)
                {
                    case 0:
                        tileInt = 112;
                        break;
                    case 1:
                        tileInt = 176;
                        break;
                    case 2:
                        tileInt = 177;
                        break;
                }
                string tileID = "terrain_atlas_" + tileInt;
                Sprite spr = tileSpriteMap[tileID];
                //Debug.Log(spr);
                if (spr == null)
                {
                    Debug.Log("name was incorrect");
                    continue;
                }
                tileObject.GetComponent<Tile>().SetSprite(spr);
                Debug.Log("First" + tileObject.GetComponent<SpriteRenderer>().bounds.size);
                tileObject.GetComponent<SpriteRenderer>().bounds.size.Set(32,32,0);
                Debug.Log(tileObject.GetComponent<SpriteRenderer>().bounds.size);
                width = tileObject.GetComponent<SpriteRenderer>().bounds.size.x;
                mapWidth += width;
                height = tileObject.GetComponent<SpriteRenderer>().bounds.size.y;
                mapHeight += height;
                tileObject.GetComponent<Tile>().Size = new Vector2(32, 32);
                tileMap[GetTile(row, col)] = tileObject;
                currX += width;

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
                        tileObject.GetComponent<SpriteRenderer>().color *= new Vector4(1.0f, 0.25f, 0.25f, 1.0f);
                        break;
                    default:
                        break;
                }
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
            if(renderer == null)
                continue;
            Bounds tileBounds = renderer.bounds;
            if(tileBounds.Contains(new Vector3(position.x,position.y,0)))
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

    public void DisplayMapLayers(){
        foreach (GameObject tileObj in tileMap)
        {
            Debug.Log(tileObj.GetComponent<Tile>().Layer);
        }
    }
}
