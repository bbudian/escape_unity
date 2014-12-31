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

    void CreateEmptyBaseLayer(int rows, int cols)
    {
        baseLayerMap = new GameObject[rows * cols];
        float currX = 0.0f;
        float currY = 0.0f;
        mapWidth = mapHeight = 0.0f;
        float width = 0, height = 0;
        string tileID = "terrain_atlas_33";
        int id = 33;
        for (int row = 0; row < rows; row++)
        {
            for (int col = 0; col < cols; col++)
            {
                GameObject tileObject = new GameObject("Tile");
                tileObject.transform.position = new Vector3(currX, currY, 0);
                tileObject.AddComponent<Tile>();
                tileObject.GetComponent<Tile>().Position = new Vector2(currX, currY);

                Sprite spr = tileSpriteMap[tileID];
                tileObject.GetComponent<Tile>().Layer = Tile.TileLayers.Passable;
                tileObject.GetComponent<Tile>().SetSprite(spr);
                tileObject.GetComponent<SpriteRenderer>().sortingLayerName = "BaseGround";
                tileObject.GetComponent<SpriteRenderer>().material = Resources.Load<Material>("Materials/SpriteMat");
                width = tileWidth;
                mapWidth += width;
                height = tileHeight;
                mapHeight += height;
                tileObject.GetComponent<Tile>().Size = new Vector2(tileWidth, tileHeight);
                baseLayerMap[GetTile(row, col)] = tileObject;
                tileObject.GetComponent<Tile>().Name = tileID;
                tileObject.GetComponent<Tile>().SpriteID = id;
                currX += width;
            }
            currX = 0.0f;
            currY -= height;
        }

    }

    void BaseLayerPass1(int rows, int cols, float tileWidth, float tileHeight)
    {
        //-- PASS 1 -- Populate array with empty cells

        //Populate array with "empty" cells
        baseLayerMap = new GameObject[rows * cols];

        float currX = 0.0f;
        float currY = 0.0f;
        string tileID = "terrain_atlas_33";
        int id = 33;
        for (int row = 0; row < rows; row++)
        {
            for (int col = 0; col < cols; col++)
            {
                //Create new tile
                GameObject tileObject = new GameObject("Tile");
                tileObject.transform.position = new Vector3(currX, currY, 0);

                //Get Sprite from map
                Sprite spr = tileSpriteMap[tileID];

                //Set Tile Data
                Tile tile = tileObject.AddComponent<Tile>();
                tile.Position = new Vector2(currX, currY);
                tile.Layer = Tile.TileLayers.Passable;
                tile.SetSprite(spr);
                tile.Size = new Vector2(tileWidth, tileHeight);
                tile.Name = tileID;
                tile.SpriteID = id;

                //Set Sorting layer and material for the Sprite renderer
                SpriteRenderer spRend = tileObject.GetComponent<SpriteRenderer>();
                spRend.sortingLayerName = "BaseGround";
                spRend.material = Resources.Load<Material>("Materials/SpriteMat");

                //Place tile onto tile map
                baseLayerMap[GetTile(row, col)] = tileObject;

                //Move to next column
                currX += tileWidth;
            }

            //Move to beginning of next row
            currX = 0.0f;
            currY -= tileHeight;
        }
    }

    void BaseLayerPass2(int numRows, int numCols)
    {
        //-- PASS 2 -- Generate ground tiles

        //Initialization
        int start = 0, min = 0, max = 0, length = 0;

        //Algorithm ignores all edges
        for (int row = 1; row < numRows - 1; row++)
        {
            //This allows for 3 rows to be the same to allow proper tile placement
            if (row % 3 == 0)
            {
                //Create min and max for random length range
               min = (int)(numCols * 0.5);
               max = (int)(numCols * 0.9);

                //Calculate length
                length = Random.Range(min, max + 1);
                Debug.Log("min: " + min + " max: " + max + " length: " + length);

                //Calculate starting column
                start = Random.Range(1, (numCols - length));
            }
            
            for (int col = start; col < (length + start); col++)
            {
                //Store local copy of tile 
                GameObject tileObj = baseLayerMap[GetTile(row, col)];
                Tile tile = tileObj.GetComponent<Tile>();

                //Set tile to ground sprite
                tile.Name = "terrain_atlas_112";
                tile.SpriteID = 112;
                tile.SetSprite(tileSpriteMap[tile.Name]);
            }
        }
    }

    void InitializeTileMap(int rows, int cols, float tileWidth, float tileHeight)
    {
        string emptyCell = "terrain_atlas_33";

        //Create Base layer (2 Pass)
        BaseLayerPass1(rows, cols, tileWidth, tileHeight);
        BaseLayerPass2(rows, cols);

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

                Tile tile = tileObject.AddComponent<Tile>();

                int tileInt = 235;

                //Place Walls

                //Checking Base Layer cells
                if (baseLayerMap[GetTile(row, col)].GetComponent<Tile>().name == emptyCell)
                    tileInt = 33;
                // If the cell to the right AND the cell below are empty this is an upward left corner
                else if (col + 1 < numCols && row + 1 < numRows && CheckBaseLayerName(row, col + 1, emptyCell) && CheckBaseLayerName(row + 1, col, emptyCell))
                    tileInt = 3;
                // If the cell to the left and down are both empty current is a left facing corner
                else if (col - 1 >= 0 && row + 1 < numRows && CheckBaseLayerName(row, col - 1, emptyCell) && CheckBaseLayerName(row + 1, col, emptyCell))
                    tileInt = 4;
                // If the cell to the left and Up are both empty current is a left facing corner
                else if (col - 1 >= 0 && row - 1 >= 0 && CheckBaseLayerName(row, col - 1, emptyCell) && CheckBaseLayerName(row - 1, col, emptyCell))
                    tileInt = 36;
                else if (col + 1 < numCols && row - 1 >= 0 && CheckBaseLayerName(row, col + 1, emptyCell) && CheckBaseLayerName(row - 1, col, emptyCell))
                    tileInt = 35;
                // If the cell below this cell is empty make current cell an upward facing wall
                else if (row + 1 < numRows && CheckBaseLayerName(row + 1, col, emptyCell))
                    tileInt = 1;
                // If the cell to the left is empty make current cell a right facing wall
                else if (col - 1 >= 0 && CheckBaseLayerName(row, col - 1, emptyCell))
                    tileInt = 34;
                // If the cell down and to the left one is empty make current cell a right upward facing corner
                else if (row + 1 < numRows && col - 1 > 0 && CheckBaseLayerName(row + 1, col - 1, emptyCell))
                    tileInt = 2;
                // If the cell above is empty make current cell a downward facing wall
                else if (row - 1 >= 0 && CheckBaseLayerName(row - 1, col, emptyCell))
                    tileInt = 65;
                // If the cell up one and to the left one is empty make current cell a downward corner
                else if (row - 1 >= 0 && col - 1 > 0 && CheckBaseLayerName(row - 1, col - 1, emptyCell))
                    tileInt = 66;
                // If the cell to the right is an ampty cell make current cell right facing wall
                else if (col + 1 < numCols && CheckBaseLayerName(row, col + 1, emptyCell))
                    tileInt = 32;
                // If the cell up and to the right one is empty make left downfacing corner
                else if (row - 1 >= 0 && col + 1 < numCols && CheckBaseLayerName(row - 1, col + 1, emptyCell))
                    tileInt = 64;
                // If the cell down and to the right one is empty make left  down facing corner
                else if (row + 1 < numRows && col + 1 < numCols && CheckBaseLayerName(row + 1, col + 1, emptyCell))
                    tileInt = 0;

                if (row - 1 >= 0 && CheckTileID(row - 1, col, 36))
                    tileInt = 100;
                if (row - 1 >= 0 && CheckTileID(row - 1, col, 35))
                    tileInt = 99;
                if (row - 1 >= 0 && CheckTileID(row - 1, col, 65))
                    tileInt = 129;
                if (row - 1 >= 0 && CheckTileID(row - 1, col, 66))
                    tileInt = 130;
                if (row - 1 >= 0 && CheckTileID(row - 1, col, 64))
                    tileInt = 128;

                if (tileInt == 0 || tileInt == 1 || tileInt == 2 || tileInt == 3 || tileInt == 4 || tileInt == 32 || tileInt == 34 || tileInt == 35 || tileInt == 36 ||
                   tileInt == 64 || tileInt == 65 || tileInt == 66 || tileInt == 99 || tileInt == 100)
                    tile.Layer = Tile.TileLayers.Impassable;

                string tileID = "terrain_atlas_" + tileInt;
                Sprite spr = tileSpriteMap[tileID];
                if (spr == null)
                {
                    Debug.Log("name was incorrect");
                    continue;
                }

                if (tile == null)
                    Debug.LogError("Tile Component is invalid");
                else
                {
                    tile.Position = new Vector2(currX, currY);
                    tile.SetSprite(spr);
                    tile.Size = new Vector2(tileWidth, tileHeight);
                    tile.SpriteID = tileInt;
                    tile.Name = tileID;
                    if (tile.Layer == Tile.TileLayers.Impassable)
                        AddCollisionRect(ref tileObject, tileInt);
                }

                SpriteRenderer spRend = tileObject.GetComponent<SpriteRenderer>();
                if (spRend == null)
                    Debug.LogError("Tile Sprite Renderer is not valid");
                else
                {
                    spRend.sortingLayerName = "Ground";
                    spRend.material = Resources.Load<Material>("Materials/SpriteMat");
                }
                

                tileMap[GetTile(row, col)] = tileObject;

                width = tileWidth;
                height = tileHeight;

                currX += width;
            }
            currX = 0.0f;
            currY -= height;
        }
    }

    void AddCollisionRect(ref GameObject obj, int id)
    {
        Tile tile = obj.GetComponent<Tile>();

        switch (id)
        {
            //1,3,4,36,35,34,2,65,66,32,64,0
            case 0:
                tile.AddCollisionRect("corner_wall_0", 0.12f, 0.135f, 0.1f, -0.09f);
                break;
            case 1:
                tile.AddCollisionRect("wall_1", 0.32f, 0.1f, 0.1f, -0.05f);
                break;
            case 2:
                tile.AddCollisionRect("corner_wall_2", 0.12f, 0.135f, -0.1f, -0.09f);
                break;
            case 3:
                tile.AddCollisionRect("corner_wall_3");
                break;
            case 4:
                tile.AddCollisionRect("corner_wall_4");
                break;
            case 32:
                tile.AddCollisionRect("wall_32", 0.19f, 0.32f, 0.07f);
                break;
            case 34:
                tile.AddCollisionRect("wall_32", 0.19f, 0.32f, -0.07f);
                break;
            case 35:
                tile.AddCollisionRect("corner_wall_35");
                break;
            case 36:
                tile.AddCollisionRect("corner_wall_36");
                break;
            case 64:
                tile.AddCollisionRect("corner_wall_64", 0.14f, 0.32f, .09f);
                break;
            case 65:
                tile.AddCollisionRect("wall_65");
                break;
            case 66:
                tile.AddCollisionRect("corner_wall_66", 0.14f, 0.32f, -.09f);
                break;
            case 99:
                tile.AddCollisionRect("corner_wall_99_0", 0.32f, 0.13f, 0f, .1f);
                tile.AddCollisionRect("corner_wall_99_1", 0.18f, 0.195f, 0.07f, -.06f);
                break;
            case 100:
                tile.AddCollisionRect("corner_wall_100_0", 0.32f, 0.13f, 0f, .1f);
                tile.AddCollisionRect("corner_wall_100_1", 0.18f, 0.195f, -0.07f, -.06f);
                break;
            case 128:
                tile.AddCollisionRect("corner_wall_128", 0.129f, 0.12f, .095f, 0.1f);
                break;
            case 129:
                tile.AddCollisionRect("wall_129", 0.32f, 0.1f, 0f, 0.11f);
                break;
            case 130:
                tile.AddCollisionRect("corner_wall_130", 0.115f, 0.12f, -.1f, 0.1f);
                break;
        }
        //obj.GetComponent<Tile>().AddCollisionRect("rock");
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

    bool CheckBaseLayerName(int row, int col, string name)
    {
        return baseLayerMap[GetTile(row, col)].GetComponent<Tile>().Name == name;
    }

    bool CheckBaseLayerID(int row, int col, int id)
    {
        return baseLayerMap[GetTile(row, col)].GetComponent<Tile>().SpriteID == id;
    }

    bool CheckTileID(int row, int col, int id)
    {
        return tileMap[GetTile(row, col)].GetComponent<Tile>().SpriteID == id;
    }
}
