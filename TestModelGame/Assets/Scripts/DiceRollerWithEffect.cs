using System.Collections;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class DiceRollerWithEffect : MonoBehaviour
{
    public TextMeshProUGUI resultTextPlayer;
    public Button playerDiceButton;
    public float rollDuration = 0.5f;
    public AudioSource playerDiceAudioSource;
    public AudioClip playerDiceAudioClip;
    public Color finalColor = Color.green;
    [Header("Ходы")]
    private int playerRollsLeft = 10;

    //public TextMeshProUGUI playersRolls;

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

        resultTextPlayer.color = Color.black;
        resultTextPlayer.fontStyle = FontStyles.Normal;
        playerDiceAudioSource.PlayOneShot(playerDiceAudioClip);

        while (elapsed < rollDuration)
        {
            int tempResult = Random.Range(1, 7);
            resultTextPlayer.text = tempResult.ToString();

            elapsed += 0.1f;
            yield return new WaitForSeconds(0.1f);
        }

        int finalResult = Random.Range(1, 7);
        resultTextPlayer.text = finalResult.ToString();

        resultTextPlayer.color = finalColor;
        resultTextPlayer.fontStyle = FontStyles.Bold;

        Debug.Log("Выпало число: " + finalResult);


        yield return new WaitForSeconds(0f);

    }
}
