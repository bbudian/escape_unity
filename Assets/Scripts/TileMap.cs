using UnityEngine;
using System.Collections;

public class TileMap : MonoBehaviour
{

    enum TileLayers { Passable, DOT, Kill, Impassable }
    class Tile
    {
        SpriteRenderer[] sprites;
        TileLayers layer;
        Vector2 size;

        public Tile()
        {
            int defaultNumSprites = 3;

            //Renders in order from lowest to highest (0-bottom -> sprites.size-top)
            sprites = new SpriteRenderer[defaultNumSprites]; 
            for (int sprite = 0; sprite < defaultNumSprites; sprite++)
                sprites[sprite] = null;

            layer = TileLayers.Passable;
            size = new Vector2(32, 32); //Pixel size of sprite on texture
        }

        public Vector2 Size
        {
            get { return size; }
            set { size = value; }
        }

        public TileLayers Layer
        {
            get { return layer; }
            set { layer = value; }
        }

        public void AddSprite(SpriteRenderer spriteImage, int slot)
        {
            sprites[slot] = spriteImage;
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

    Tile[] tileMap;
    int numTiles, numRows, numCols;
    Vector2 mapSize;
    float tileWidth, tileHeight, mapWidth, mapHeight;

    // Use this for initialization
    void Start()
    {
        numRows = numCols = 4;
        tileWidth = tileHeight = 32;
        numTiles = numRows * numCols;
        InitializeTileMap(numRows, numCols, tileWidth, tileHeight);
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
                Sprite spr = Resources.Load<Sprite>("dryCrackedDirtTan");
                if (spr == null)
                    Debug.Log("Failed to load sprite");
                sr.sprite = spr;
                tile.AddSprite(sr,0);
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
