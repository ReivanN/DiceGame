using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;

public class RoundUI : MonoBehaviour
{
    public TextMeshProUGUI phaseText;
    public TextMeshProUGUI roundText;
    public TextMeshProUGUI TimeDuration;
    public float roundTimer;
    private Coroutine timerCoroutine;
    void OnEnable()
    {
        RoundManager.OnPhaseStart += UpdateUI;
    }

    void OnDisable()
    {
        RoundManager.OnPhaseStart -= UpdateUI;
    }

    private void UpdateUI(RoundManager.RoundPhase phase, int roundNumber)
    {
        roundText.text = "Round: " + roundNumber;

        // Получаем время фазы напрямую из RoundManager
        switch (phase)
        {
            case RoundManager.RoundPhase.Gathering:
                phaseText.text = "Phase: Gathering";
                roundTimer = RoundManager.Instance.gatheringTime;
                break;
            case RoundManager.RoundPhase.Preparation:
                phaseText.text = "Phase: Preparation";
                roundTimer = RoundManager.Instance.preparationDuration;
                break;
            case RoundManager.RoundPhase.Battle:
                phaseText.text = "Phase: Battle";
                roundTimer = RoundManager.Instance.battleDuration;
                break;
            case RoundManager.RoundPhase.Scoring:
                phaseText.text = "Phase: Scoring";
                roundTimer = RoundManager.Instance.scoringDuration;
                break;
        }

        TimeDuration.text = "Time: " + Mathf.CeilToInt(roundTimer);

        if (timerCoroutine != null)
        {
            StopCoroutine(timerCoroutine);
        }

        timerCoroutine = StartCoroutine(UpdateTimer());
    }

    private IEnumerator UpdateTimer()
    {
        while (roundTimer > 0)
        {
            roundTimer -= Time.deltaTime;
            TimeDuration.text = "Time: " + Mathf.CeilToInt(roundTimer);
            yield return null;
        }
    }

}
