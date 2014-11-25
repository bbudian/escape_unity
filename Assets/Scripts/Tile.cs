using UnityEngine;
using System.Collections;

public class Tile : MonoBehaviour
{
    public enum TileLayers { Passable, DOT, Kill, Impassable,OffMap }

    TileLayers layer;
    Vector2 size;
    Vector2 position;
    public string name;
    public int spriteID;

    public int SpriteID
    {
        get { return spriteID; }
        set { spriteID = value; }
    }

    public string Name
    {
        get { return name; }
        set { name = value; }
    }

    public Vector2 Position
    {
        get { return position; }
        set { position = value; }
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

    public void SetSprite(Sprite spriteImage)
    {
        if (gameObject.GetComponent<SpriteRenderer>() == null)
        {
            gameObject.AddComponent<SpriteRenderer>();
        }
        gameObject.GetComponent<SpriteRenderer>().sprite = spriteImage;
    }

    float GetWidth()
    {
        return size.x;
    }

    float GetHeight()
    {
        return size.y;
    }

    // Use this for initialization
    void Start()
    {
        //size = new Vector2(32, 32); //Pixel size of sprite on texture
    }

    // Update is called once per frame
    void Update()
    {

    }

    void OnCollisionEnter2D(Collision2D coll)
    {
    }

    void AddSpriteRenderer()
    {
        if (gameObject.GetComponent<SpriteRenderer>() == null)
        {
            gameObject.AddComponent<SpriteRenderer>();
            gameObject.GetComponent<SpriteRenderer>().sortingLayerName = "Ground";
        }                           
    }

    public void AddCollisionRect(string name,float sizeX = .32f,float sizeY = .32f, float centerX = 0, float centerY = 0)
    {
        Debug.Log(sizeX + ", " + sizeY + ", " + centerX + ", " + centerY);
        BoxCollider2D coll = gameObject.AddComponent<BoxCollider2D>();
        coll.name = name;
        coll.size = new Vector2(sizeX,sizeY);
        coll.center = new Vector2(centerX, centerY);
        Debug.Log("Added Collision Rectangle with size " + coll.size.ToString());
    }
}
