using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class TileMap : MonoBehaviour
{

    public enum TileLayers { Passable, DOT, Kill, Impassable }

    public class Tile
    {
        public SpriteRenderer[] sprites;
        TileLayers layer;
        Vector2 size;

        public Tile()
        {
            int defaultNumSprites = 3;

            //Renders in order from lowest to highest (0-bottom -> sprites.size-top)
            sprites = new SpriteRenderer[defaultNumSprites];
            for (int sprite = 0; sprite < defaultNumSprites; sprite++)
                sprites[sprite] = new SpriteRenderer();

            layer = TileLayers.Passable;
            size = new Vector2(32, 32); //Pixel size of sprite on texture
        }

        public Vector2 Size
        {
            get { return size; }
            set { size = value; }
        }

        TileLayers Layer
        {
            get { return layer; }
            set { layer = value; }
        }

        public void AddSprite(SpriteRenderer spriteImage, int slot)
        {
            try
            {
                sprites[slot] = spriteImage;
            }
            catch (System.NullReferenceException ex)
            {
                Debug.Log(ex.Message + "MINE");
            }         
        }

        public SpriteRenderer GetSprite(int slot)
        {
            return sprites[slot];
        }

        float GetWidth()
        {
            return size.x;
        }

        float GetHeight()
        {
            return size.y;
        }
    }

    public Tile[] tileMap;
    public Sprite[] tileSprites;
    Dictionary<string, Sprite> tileSpriteMap;
    int numTiles, numRows, numCols;
    Vector2 mapSize;
    float tileWidth, tileHeight, mapWidth, mapHeight;

    // Use this for initialization
    void Start()
    {
        numRows = numCols = 4;
        tileWidth = tileHeight = 32;
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
        tileMap = new Tile[numTiles];
        for (int row = 0; row < rows; row++)
        {
            for (int col = 0; col < cols; col++)
            {
                Tile tile = new Tile();
                SpriteRenderer sr = new SpriteRenderer();
                Sprite spr = tileSpriteMap["dryCrackedDirtTan"];
                if (spr == null)
                {
                    Debug.Log("name was incorrect");
                    continue;
                }
                try
                {
                    sr.sprite = spr;
                }
                catch(System.NullReferenceException ex)
                {
                    Debug.LogException(ex);
                    if(sr.sprite == null)
                        Debug.Log("sr.sprite == null");
                    if (spr == null)
                        Debug.Log("Somehow this was missed");
                }
                tile.AddSprite(sr, 0);
                tileMap[GetTile(row, col)] = tile;
            }
        }
    }

    void CheckPositionOnMap(Vector2 position)
    {

    }

    int GetTile(int row, int col)
    {
        return row * numCols + col;
    }
}
