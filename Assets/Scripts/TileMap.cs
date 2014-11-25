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

        //Get Max Corners

        //// Using exponential Formula y = P0*a^x || y = P0 * pow(a,x);
        //float lowestChance = 1.0f;
        //float maxChance = 100.0f;

        //// Find a using formula : a = (y2/y1)^(1/(x2-x1)) where x = (0,lowestChance) and y = (numCols,100)
        //float a = Mathf.Pow((maxChance / lowestChance), 1.0f / numCols);
        //Debug.Log(a);

        //// Find P0 using formula : P0 = 100/a^numCol
        //float P0 = maxChance / (Mathf.Pow(a, numCols));
        //Debug.Log(P0);

        //float incRateRow = (maxChance - lowestChance) / numRows;


        bool cornerPlaced = false;
        int minWallLength = (int)(numCols * 0.35f);
        int wallLength = 0;
        int currWallLength = 0;
        float perToPlaceCorner = 30;
        int id = -1;

        for (int row = 0; row < rows; row++)
        {
            for (int col = 0; col < cols; col++)
            {
                GameObject tileObject = new GameObject("Tile");
                tileObject.transform.position = new Vector3(currX, currY, 0);
                tileObject.AddComponent<Tile>();
                tileObject.GetComponent<Tile>().Position = new Vector2(currX, currY);

                bool edge = false;

                if (col == 0 || row == 0 || col == numCols - 1 || row == numRows - 1)
                    edge = true;
                //terrain_atlas_112
                string tileID = "terrain_atlas_33";
                id = 33;
                if (!cornerPlaced && !edge)
                {
                    //Determine the chance to place a corner here
                    float chance = Random.Range(0, 100);
                    if (chance <= perToPlaceCorner)
                    {
                        //If the minimum wall length is less than the remaining number of tiles don't place
                        if (numCols - col >= minWallLength)
                        {
                            //Set corner and prepare for new tiles
                            cornerPlaced = true;
                            currWallLength = 0;
                            tileID = "terrain_atlas_112";
                            id = 112;
                            //Debug.Log("\nFirst Corner Placed\n");
                            wallLength = Random.Range(minWallLength, numCols - col);
                        }
                    }
                }
                else if (!edge)
                {
                    currWallLength++;
                    tileID = "terrain_atlas_112";
                    id = 112;

                    if (currWallLength == wallLength)
                    {
                        cornerPlaced = false;
                        currWallLength = 0;
                        //Debug.Log("\nLast Corner Placed\n");
                    }
                }
                //Check to see if row width is at least 4 tiles
                if (row - 1 >= 0 && CheckBaseLayerID(row - 1, col, 112))
                {
                    if (row - 2 >= 0 && CheckBaseLayerID(row - 2, col, 112))
                    {
                        if (row - 3 >= 0 && CheckBaseLayerID(row - 3, col, 112))
                        {
                            if (row - 4 >= 0 && CheckBaseLayerID(row - 4, col, 112))
                            {
                                if (row - 5 >= 0 && CheckBaseLayerID(row - 5, col, 112)) { }
                                else
                                {
                                    tileID = "terrain_atlas_112";
                                    id = 112;
                                }
                            }
                            else
                            {
                                tileID = "terrain_atlas_112";
                                id = 112;
                            }
                        }
                        else
                        {
                            tileID = "terrain_atlas_112";
                            id = 112;
                        }
                    }
                    else
                    {
                        tileID = "terrain_atlas_112";
                        id = 112;
                    }
                }

                if ((row - 1 >= 0 && row - 2 >= 0 && col - 1 >= 0 && col - 2 >= 0) && CheckBaseLayerID(row - 1, col - 1, 33) && (
                    CheckBaseLayerID(row, col - 1, 112) && CheckBaseLayerID(row - 2, col - 1, 112) || CheckBaseLayerID(row - 1, col, 112) && CheckBaseLayerID(row - 1, col - 2, 112)))
                {
                    tileID = "terrain_atlas_112";
                    id = 112;
                    baseLayerMap[GetTile(row - 1, col - 1)].GetComponent<Tile>().SetSprite(tileSpriteMap[tileID]);
                    baseLayerMap[GetTile(row - 1, col - 1)].GetComponent<Tile>().Name = tileID;
                    baseLayerMap[GetTile(row - 1, col - 1)].GetComponent<Tile>().SpriteID = id;
                }

                if (col - 1 >= 0 && col - 2 >= 0 && CheckBaseLayerID(row, col - 1, 112) && CheckBaseLayerID(row, col - 2, 33) && id == 33)
                {
                    baseLayerMap[GetTile(row, col - 1)].GetComponent<Tile>().SetSprite(tileSpriteMap[tileID]);
                    baseLayerMap[GetTile(row, col - 1)].GetComponent<Tile>().Name = tileID;
                    baseLayerMap[GetTile(row, col - 1)].GetComponent<Tile>().SpriteID = id;
                }
                if (row - 1 >= 0 && row - 2 >= 0 && CheckBaseLayerID(row - 1, col, 112) && CheckBaseLayerID(row - 2, col, 33) && id == 33)
                {
                    baseLayerMap[GetTile(row - 1, col)].GetComponent<Tile>().SetSprite(tileSpriteMap[tileID]);
                    baseLayerMap[GetTile(row - 1, col)].GetComponent<Tile>().Name = tileID;
                    baseLayerMap[GetTile(row - 1, col)].GetComponent<Tile>().SpriteID = id;
                }

                //int begRow = row-6;
                //int begCol = col-7;
                //if(row-6 < 0)
                //    begRow = 0;
                //if(begCol < 0)
                //    begCol = 0;
                //int endRow = row;
                //int endCol = col-1;
                //if(endCol < 0)
                //    endCol = 0;
                //for (int r = begRow; r <= row; r++)
                //{
                //    for (int c = begCol; c < col; c++)
                //    {
                //        if (CheckBaseLayerID(r, c, 33))
                //        {
                //            tileID = "terrain_atlas_33";
                //            id = 33;
                //            baseLayerMap[GetTile(r, c)].GetComponent<Tile>().SetSprite(tileSpriteMap[tileID]);
                //            baseLayerMap[GetTile(r, c)].GetComponent<Tile>().Name = tileID;
                //            baseLayerMap[GetTile(r, c)].GetComponent<Tile>().SpriteID = id;
                //        }
                //    }
                //}



                //float chance = Random.Range(0.0f, maxChance);
                //if ( chance <= currChance)
                //{
                //    cornerPlaced = true;
                //    corners[currCorner++] = new Vector2(col, row);
                //}

                //if (cornerPlaced)
                //{
                //    tileID += "_112";
                //    // find current chance using formula above
                //    //currChance = P0 * Mathf.Pow(a,col);
                //    //Debug.Log(currChance);
                //}
                //else
                //    tileID += "_33";
                //}
                //else
                //{
                //    tileID += "_33";
                //}

                //Debug.Log(tileID);

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
                tileObject.GetComponent<Tile>().Name = tileID;
                tileObject.GetComponent<Tile>().SpriteID = id;
                currX += width;
            }
            currX = 0.0f;
            currY -= height;

        }
    }

    void InitializeTileMap(int rows, int cols, float tileWidth, float tileHeight)
    {
        string emptyCell = "terrain_atlas_33";
        CreateBaseLayer(rows, cols, tileWidth, tileHeight);

        //return;

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
                    tileObject.GetComponent<Tile>().Layer = Tile.TileLayers.Impassable;


                //Adjusting all Layer cells
                ////If both cells on either side of current are empty make current empty
                //if ((col - 1 >= 0 && col + 1 < numCols && CheckBaseLayerID(row, col - 1, 33) && CheckBaseLayerID(row, col + 1, 33)) ||
                //    (row - 1 >= 0 && row + 1 < numRows && CheckBaseLayerID(row - 1, col, 33) && CheckBaseLayerID(row + 1, col, 33)))
                //{
                //    tileInt = 33;
                //}


                //int tileInt = Random.Range(0, 3);
                //switch (tileInt)
                //{
                //    case 0:
                //        tileInt = 177;
                //        break;
                //    case 1:
                //        tileInt = 112;
                //        break;
                //    case 2:
                //        tileInt = 115;
                //        break;
                //}
                //switch (Random.Range(0, 4))
                //{
                //    case 0:
                //        tileObject.GetComponent<Tile>().Layer = Tile.TileLayers.DOT;
                //        break;
                //    case 1:
                //        tileObject.GetComponent<Tile>().Layer = Tile.TileLayers.Passable;
                //        break;
                //    case 2:
                //        tileObject.GetComponent<Tile>().Layer = Tile.TileLayers.Kill;
                //        break;
                //    case 3:
                //        tileObject.GetComponent<Tile>().Layer = Tile.TileLayers.Impassable;
                //        tileInt = 495;
                //        break;
                //    default:
                //        break;
                //}
                ////Top Left
                //if (row >= 0 && row <= 2 && col == 0)
                //{
                //    tileObject.GetComponent<Tile>().Layer = Tile.TileLayers.Impassable;
                //    switch (row)
                //    {
                //        case 0:
                //            tileInt = 36;
                //            break;
                //        case 1:
                //            tileInt = 68;
                //            break;
                //        case 2:
                //            tileInt = 100;
                //            break;
                //    }
                //}
                ////Top Right
                //else if (row >= 0 && row <= 2 && col == numCols - 1)
                //{
                //    tileObject.GetComponent<Tile>().Layer = Tile.TileLayers.Impassable;
                //    switch (row)
                //    {
                //        case 0:
                //            tileInt = 35;
                //            break;
                //        case 1:
                //            tileInt = 67;
                //            break;
                //        case 2:
                //            tileInt = 99;
                //            break;
                //    }
                //}
                ////Top row
                //else if (row == 0)
                //{
                //    tileObject.GetComponent<Tile>().Layer = Tile.TileLayers.Impassable;
                //    tileInt = 65;
                //}
                //else if (row == 1)
                //{
                //    tileObject.GetComponent<Tile>().Layer = Tile.TileLayers.Impassable;
                //    tileInt = 97;
                //}
                //else if (row == 2)
                //{
                //    tileObject.GetComponent<Tile>().Layer = Tile.TileLayers.Passable;
                //    tileInt = 129;
                //}
                ////Bottom Left
                //else if ((row == numRows - 1) && (col == 0))
                //{
                //    Debug.Log("Bottom Left");
                //    tileObject.GetComponent<Tile>().Layer = Tile.TileLayers.Impassable;
                //    tileInt = 4;
                //}
                ////Bottom Right
                //else if ((row == numRows - 1) && (col == numCols - 1))
                //{
                //    tileObject.GetComponent<Tile>().Layer = Tile.TileLayers.Impassable;
                //    tileInt = 3;
                //}
                ////Left Col
                //else if (col == 0)
                //{
                //    tileObject.GetComponent<Tile>().Layer = Tile.TileLayers.Impassable;
                //    tileInt = 34;
                //}
                ////Bottom row
                //else if ((row == numRows - 1))
                //{
                //    tileObject.GetComponent<Tile>().Layer = Tile.TileLayers.Impassable;
                //    tileInt = 1;
                //}
                ////Right Col
                //else if ((col == numCols - 1))
                //{
                //    tileObject.GetComponent<Tile>().Layer = Tile.TileLayers.Impassable;
                //    tileInt = 32;
                //}

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
                    AddCollisionRect(ref tileObject, tileInt);

                tileObject.GetComponent<Tile>().SpriteID = tileInt;
                tileObject.GetComponent<Tile>().Name = tileID;
                tileMap[GetTile(row, col)] = tileObject;
                currX += width;

            }
            currX = 0.0f;
            currY -= height;
        }

        Debug.Log(mapWidth + " " + mapHeight);
    }

    void AddCollisionRect(ref GameObject obj, int id)
    {
        Debug.Log(id);
        switch (id)
        {
            //1,3,4,36,35,34,2,65,66,32,64,0
            case 0:
                obj.GetComponent<Tile>().AddCollisionRect("corner_wall_0", 0.12f, 0.135f, 0.1f, -0.09f);
                break;
            case 1:
                obj.GetComponent<Tile>().AddCollisionRect("wall_1", 0.12f, 0.135f, 0.1f, -0.09f);
                break;
            case 2:
                obj.GetComponent<Tile>().AddCollisionRect("corner_wall_2", 0.12f, 0.135f, -0.1f, -0.09f);
                break;
            case 3:
                obj.GetComponent<Tile>().AddCollisionRect("corner_wall_3");
                break;
            case 4:
                obj.GetComponent<Tile>().AddCollisionRect("corner_wall_4");
                break;
            case 32:
                obj.GetComponent<Tile>().AddCollisionRect("wall_32", 0.19f, 0.32f, 0.07f);
                break;
            case 34:
                obj.GetComponent<Tile>().AddCollisionRect("wall_32", 0.19f, 0.32f, -0.07f);
                break;
            case 35:
                obj.GetComponent<Tile>().AddCollisionRect("corner_wall_35");
                break;
            case 36:
                obj.GetComponent<Tile>().AddCollisionRect("corner_wall_36");
                break;
            case 64:
                obj.GetComponent<Tile>().AddCollisionRect("corner_wall_64", 0.14f, 0.32f, .09f);
                break;
            case 65:
                obj.GetComponent<Tile>().AddCollisionRect("wall_65");
                break;
            case 66:
                obj.GetComponent<Tile>().AddCollisionRect("corner_wall_66", 0.14f, 0.32f, -.09f);
                break;
            case 99:
                obj.GetComponent<Tile>().AddCollisionRect("corner_wall_99_0", 0.32f, 0.13f, 0f, .1f);
                obj.GetComponent<Tile>().AddCollisionRect("corner_wall_99_1", 0.18f, 0.195f, 0.07f, -.06f);
                break;
            case 100:
                obj.GetComponent<Tile>().AddCollisionRect("corner_wall_100_0", 0.32f, 0.13f, 0f, .1f);
                obj.GetComponent<Tile>().AddCollisionRect("corner_wall_100_1", 0.18f, 0.195f, -0.07f, -.06f);
                break;
            case 128:
                obj.GetComponent<Tile>().AddCollisionRect("corner_wall_128", 0.129f, 0.12f, .095f,0.1f);
                break;
            case 129:
                obj.GetComponent<Tile>().AddCollisionRect("wall_129", 0.32f, 0.1f, 0f,0.11f);
                break;
            case 130:
                obj.GetComponent<Tile>().AddCollisionRect("corner_wall_130", 0.115f, 0.12f, -.1f,0.1f);
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
