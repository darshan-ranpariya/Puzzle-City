using UnityEngine;

public class Item : MonoBehaviour
{
    public ItemColor color;
    public ItemType type;
    [UnityEngine.Serialization.FormerlySerializedAs("spriteRenderer")]
    public SpriteRenderer graphic;
    public ItemSpriteCollection spriteCollection;

    public bool isActive;

    private void Start()
    {
        SetItemTypeAndSprite();
    }

    private void LateUpdate()
    {
        transform.rotation = Quaternion.identity;
    }

    public void SetItemTypeAndSprite()
    {
        type = RandomEnum<ItemType>.Get();
        color = RandomEnum<ItemColor>.Get();
        graphic.sprite = spriteCollection.GetSprite(type, color);
        isActive = true;
    }

    public void ActivateItem()
    {
        SetItemTypeAndSprite();
        isActive = true;
        graphic.gameObject.SetActive(true);
        SetGraphicScale(Vector3.one);
    }

    public void HideSprite()
    {
        isActive = false;
        SetGraphicScale(Vector3.zero);
        graphic.gameObject.SetActive(false);
    }

    void SetGraphicScale(Vector3 size)
    {
        iTween.ScaleTo(gameObject, iTween.Hash("scale", size, "time", .2f, "easetype", iTween.EaseType.easeInBack));
    }

}

public enum ItemType
{
    Hexa,
    Penta
}

public enum ItemColor
{
    Red,
    Blue,
    Yellow
}
