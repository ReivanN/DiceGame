using System.Collections;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class DiceRollerWithEffect : MonoBehaviour
{
    public TextMeshProUGUI resultTextPlayer1;
    public TextMeshProUGUI resultTextPlayer2;
    public Button playerDiceButton;
    public float rollDuration = 0.5f;
    public AudioSource playerDiceAudioSource;
    public AudioClip playerDiceAudioClip;
    public Color finalColor = Color.green;

    [SerializeField] public int totalScore; // Сумма двух кубиков

    private RoundManager.RoundPhase currentPhase;

    void OnEnable()
    {
        RoundManager.OnPhaseStart += UpdateState;
    }

    void OnDisable()
    {
        RoundManager.OnPhaseStart -= UpdateState;
    }

    private void UpdateState(RoundManager.RoundPhase phase, int roundNumber)
    {
        currentPhase = phase;
    }

    private void Update()
    {
        if (currentPhase != RoundManager.RoundPhase.Gathering)
        {
            return;
        }
    }

    public IEnumerator RollEffect()
    {
        if (currentPhase != RoundManager.RoundPhase.Gathering)
        {
            playerDiceButton.gameObject.SetActive(false);
            yield return new WaitForEndOfFrame();
        }
        else
        {
            playerDiceButton.gameObject.SetActive(true);
        }

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
    }
}