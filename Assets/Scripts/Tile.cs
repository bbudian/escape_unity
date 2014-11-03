using UnityEngine;
using System.Collections;

public class Tile : MonoBehaviour
{
    public enum TileLayers { Passable, DOT, Kill, Impassable,OffMap }

    TileLayers layer;
    Vector2 size;
    Vector2 position;

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

    //public SpriteRenderer GetSprite(int slot)
    //{
    //    return sprites[slot];
    //}

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

    void AddSpriteRenderer()
    {
        if (gameObject.GetComponent<SpriteRenderer>() == null)
        {
            gameObject.AddComponent<SpriteRenderer>();
            gameObject.GetComponent<SpriteRenderer>().sortingLayerName = "Ground";
        }
    }
}
