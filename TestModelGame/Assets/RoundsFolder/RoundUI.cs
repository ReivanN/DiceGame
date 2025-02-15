using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class RoundUI : MonoBehaviour
{
    public TextMeshProUGUI phaseText;
    public TextMeshProUGUI roundText;
    public TextMeshProUGUI TimeDuration;
    public float roundTimer;
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
        switch (phase)
        {
            case RoundManager.RoundPhase.Gathering:
                phaseText.text = "Phase: Gathering";
                roundTimer = FindAnyObjectByType<RoundManager>().gatheringTime;
                TimeDuration.text = "Time" + roundTimer.ToString();
                StartCoroutine(UpdateTimer());
                break;
            case RoundManager.RoundPhase.Preparation:
                phaseText.text = "Phase: Preparation";
                roundTimer = FindAnyObjectByType<RoundManager>().preparationDuration;
                TimeDuration.text ="Time" + roundTimer.ToString();
                StartCoroutine(UpdateTimer());
                break;
            case RoundManager.RoundPhase.Battle:
                phaseText.text = "Phase: Battle";
                roundTimer = FindAnyObjectByType<RoundManager>().battleDuration;
                TimeDuration.text = "Time" + roundTimer.ToString();
                StartCoroutine(UpdateTimer());
                break;
            case RoundManager.RoundPhase.Scoring:
                phaseText.text = "Phase: Scoring";
                roundTimer = FindAnyObjectByType<RoundManager>().scoringDuration;
                TimeDuration.text = "Time" + roundTimer.ToString();
                StartCoroutine(UpdateTimer());
                break;
        }
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
