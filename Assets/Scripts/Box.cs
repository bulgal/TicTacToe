using UnityEngine;

public class Box : MonoBehaviour
{
    public int index;
    public MarkEnum mark;
    public bool isMarked;

    private SpriteRenderer SpriteRenderer;

    private void Awake() {
        SpriteRenderer = GetComponent<SpriteRenderer>();

        index = transform.GetSiblingIndex();
        isMarked = false;
    }

    public void SetAsMarked(Sprite sprite, MarkEnum mark, Color color) {
        isMarked = true;
        this.mark = mark;

        SpriteRenderer.color = color;
        SpriteRenderer.sprite = sprite;

        //Disable the CircleCollider2D (to avoid marking it twice)
        GetComponent<CircleCollider2D>().enabled = false;
    }
}
