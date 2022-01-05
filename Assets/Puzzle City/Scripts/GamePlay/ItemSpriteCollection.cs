using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ItemSpriteCollection", menuName = "ItemSpriteCollection")]
public class ItemSpriteCollection : ScriptableObject
{
    public List<ItemSprite> sprites = new List<ItemSprite>();
    public Sprite GetSprite(ItemType t, ItemColor c)
    {
        try
        {
            return sprites.Find((s) => { return s.color == c && s.type == t; }).sprite;
        }
        catch
        {
            return null;
        }
    }
}


[System.Serializable]
public class ItemSprite
{
    public ItemType type;
    public ItemColor color;
    public Sprite sprite;
}

