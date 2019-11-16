using UnityEngine;

public class PortalClone : MonoBehaviour
{
    private SpriteRenderer spr;

    private void Awake()
    {
        spr = GetComponent<SpriteRenderer>();
    }

    public void setSprite(Sprite sprite)
    {
        spr.sprite = sprite;
    }

    public void setSortingLayerName(int val)
    {
        spr.sortingLayerID = val;
    }

    public void setSortingOrder(int val)
    {
        spr.sortingOrder = val;
    }

    public void setMaskInteraction(SpriteMaskInteraction SM)
    {
        spr.maskInteraction = SM;
    }
}
