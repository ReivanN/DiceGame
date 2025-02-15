using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.Events;

public class Dice : MonoBehaviour
{
    public TextMeshProUGUI resultTextPlayer1;
    public TextMeshProUGUI resultTextPlayer2;
    public Button playerDiceButton;
    public float rollDuration = 0.5f;
    public AudioSource playerDiceAudioSource;
    public AudioClip playerDiceAudioClip;
    public Color finalColor = Color.green;

    [SerializeField] public int totalScore;
    [SerializeField] public TextMeshProUGUI totalScoreText;
    private int attemptsLeft = 2;
    private RoundManager.RoundPhase currentPhase;

    private UnityAction OnGatheringEnd;

    void OnEnable()
    {
        RoundManager.OnPhaseStart += UpdateState;
        OnGatheringEnd += UpdatStateAfterRoll;
    }

    void OnDisable()
    {
        RoundManager.OnPhaseStart -= UpdateState;
        OnGatheringEnd -= UpdatStateAfterRoll;
    }

    private void UpdateState(RoundManager.RoundPhase phase, int roundNumber)
    {
        currentPhase = phase;
        attemptsLeft = 2;
        playerDiceButton.gameObject.SetActive(currentPhase == RoundManager.RoundPhase.Gathering);
    }

    private void UpdatStateAfterRoll()
    {
        totalScoreText.text = totalScore.ToString();
    }

    private void Update()
    {
        if (currentPhase != RoundManager.RoundPhase.Gathering || attemptsLeft <= 0)
        {
            playerDiceButton.gameObject.SetActive(false);
            return;
        }
        else
        {
            playerDiceButton.gameObject.SetActive(true);
        }
    }

    public void DiceP()
    {
        if (attemptsLeft > 0)
        {
            StartCoroutine(RollEffect());
        }
    }

    public IEnumerator RollEffect()
    {
        attemptsLeft--;
        float elapsed = 0f;

        resultTextPlayer1.color = Color.black;
        resultTextPlayer2.color = Color.black;
        resultTextPlayer1.fontStyle = FontStyles.Normal;
        resultTextPlayer2.fontStyle = FontStyles.Normal;

        playerDiceAudioSource.PlayOneShot(playerDiceAudioClip);

        while (elapsed < rollDuration)
        {
            int tempResult1 = Random.Range(1, 7);
            int tempResult2 = Random.Range(1, 7);

            resultTextPlayer1.text = tempResult1.ToString();
            resultTextPlayer2.text = tempResult2.ToString();

            elapsed += 0.1f;
            yield return new WaitForSeconds(0.1f);
        }

        int finalResult1 = Random.Range(1, 7);
        int finalResult2 = Random.Range(1, 7);

        resultTextPlayer1.text = finalResult1.ToString();
        resultTextPlayer2.text = finalResult2.ToString();

        resultTextPlayer1.color = finalColor;
        resultTextPlayer2.color = finalColor;
        resultTextPlayer1.fontStyle = FontStyles.Bold;
        resultTextPlayer2.fontStyle = FontStyles.Bold;

        totalScore = finalResult1 + finalResult2;

        Debug.Log($"Final result: {finalResult1} + {finalResult2} = {totalScore}");
        
        if (attemptsLeft <= 0)
        {
            RoundManager.Instance.SkipPhase();
            OnGatheringEnd?.Invoke();
        }
    }
}