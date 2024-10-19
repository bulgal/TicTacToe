using UnityEngine;
using UnityEngine.Events;

public class Board : MonoBehaviour
{
    [Header("Input Settings : ")]
    [SerializeField] private LayerMask boxesLayerMask;
    [SerializeField] private float touchRadius;

    [Header("Mark Sprites : ")]
    [SerializeField] private Sprite spriteX;
    [SerializeField] private Sprite spriteO;

    [Header("Mark Colors : ")]
    [SerializeField] private Color colorX;
    [SerializeField] private Color colorO;

    public UnityAction<MarkEnum,Color> OnEndGameAction;
    public MarkEnum[] boardMatrix;
    private Camera cam;
    private MarkEnum currentMark;
    private bool canPlay;
    private LineRenderer lineRenderer;
    private int marksCount = 0;

    private void Start() {
        cam = Camera.main;
        currentMark = MarkEnum.X;
        boardMatrix = new MarkEnum[9];
        canPlay = true;

        lineRenderer = GetComponent<LineRenderer>();
        lineRenderer.enabled = false;
    }

    private void Update() {
        if (!canPlay || !Input.GetMouseButtonUp(0)) {
            return;
        }

        Collider2D hit = Physics2D.OverlapCircle(cam.ScreenToWorldPoint(Input.mousePosition), touchRadius, boxesLayerMask);
        if (hit) {
            HitBox(hit.GetComponent<Box>());
        }
    }

    private void HitBox(Box box) {
        if (box.isMarked) {
            return;
        }
        marksCount++;

        boardMatrix[box.index] = currentMark;
        box.SetAsMarked(GetSpite(), currentMark, GetColor());

        if (IsGameEnd()) {
            return;
        }
        SwitchPlayer();
    }


    private bool IsGameEnd() {
        bool win = AreBoxesMatched(0, 1, 2) || AreBoxesMatched(3, 4, 5) || AreBoxesMatched(6, 7, 8) ||
        AreBoxesMatched(0, 3, 6) || AreBoxesMatched(1, 4, 7) || AreBoxesMatched(2, 5, 8) ||
        AreBoxesMatched(0, 4, 8) || AreBoxesMatched(2, 4, 6);

        if (win || marksCount == 9) {
            if (OnEndGameAction != null) {
                OnEndGameAction.Invoke(
                    win ? currentMark : MarkEnum.None,
                    win ? GetColor() : Color.black
                );
            }
            canPlay = false;
            return true;
        }

        return false;
    }

    private bool AreBoxesMatched(int firstBox, int secondBox, int lastBox) {
        bool matched =
            boardMatrix[firstBox] == currentMark
            && boardMatrix[secondBox] == currentMark
            && boardMatrix[lastBox] == currentMark;

        if (matched) {
            DrawLine(firstBox, lastBox);
        }

        return matched;
    }

    private void DrawLine(int firstBox, int lastBox) {
        lineRenderer.SetPosition(0, transform.GetChild(firstBox).position);
        lineRenderer.SetPosition(1, transform.GetChild(lastBox).position);

        Color color = GetColor();
        color.a = .3f;
        lineRenderer.startColor = color;
        lineRenderer.endColor = color;

        lineRenderer.enabled = true;
    }

    private void SwitchPlayer() {
        currentMark = (currentMark == MarkEnum.X) ? MarkEnum.O : MarkEnum.X;
    }

    private Color GetColor() {
        return (currentMark == MarkEnum.X) ? colorX : colorO;
    }

    private Sprite GetSpite() {
        return (currentMark == MarkEnum.X) ? spriteX : spriteO;
    }
}
