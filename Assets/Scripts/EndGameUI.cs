using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class EndGameUI : MonoBehaviour
{
    [Header("UI References : ")]
    [SerializeField] private GameObject uiCanvas;
    [SerializeField] private TMP_Text uiWinnerText;
    [SerializeField] private Button uiRestartButton;

    [Header("Board Reference :")]
    [SerializeField] private Board board;

    private void Start() {
        uiRestartButton.onClick.AddListener(() => SceneManager.LoadScene(0));
        board.OnEndGameAction += OnEndGameEvent;

        uiCanvas.SetActive(false);
    }

    private void OnEndGameEvent(MarkEnum mark, Color color) {
        uiWinnerText.text = (mark == MarkEnum.None) ? "Nobody wins" : mark.ToString() + " Wins!";
        uiWinnerText.color = color;

        uiCanvas.SetActive(true);
    }

    private void OnDestroy() {
        uiRestartButton.onClick.RemoveAllListeners();
        board.OnEndGameAction -= OnEndGameEvent;
    }
}
