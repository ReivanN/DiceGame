using UnityEngine;
using System.Collections;

public class RoundManager : MonoBehaviour
{
    public static RoundManager Instance { get; private set; }

    public enum RoundPhase { Gathering, Preparation, Battle, Scoring }
    public RoundPhase currentPhase = RoundPhase.Gathering;

    public int currentRound = 1;
    public float gatheringTime = 30f;
    public float preparationDuration = 30f;
    public float battleDuration = 60f;
    public float scoringDuration = 15f;

    public delegate void PhaseEvent(RoundPhase phase, int roundNumber);
    public static event PhaseEvent OnPhaseStart;

    private bool skipPhase = false; // ���� ��� �������� ����

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        StartCoroutine(RoundRoutine());
    }

    private IEnumerator RoundRoutine()
    {
        while (true)
        {
            yield return StartPhase(RoundPhase.Gathering, gatheringTime);
            yield return StartPhase(RoundPhase.Preparation, preparationDuration);
            yield return StartPhase(RoundPhase.Battle, battleDuration);
            yield return StartPhase(RoundPhase.Scoring, scoringDuration);

            currentRound++; // ����������� ����� ������ ����� Scoring
        }
    }

    private IEnumerator StartPhase(RoundPhase phase, float duration)
    {
        currentPhase = phase;
        OnPhaseStart?.Invoke(currentPhase, currentRound); // �������� ���� � ������ ����

        float timer = 0f;
        skipPhase = false; // ���������� ���� ��������

        while (timer < duration)
        {
            if (skipPhase) break; // ���� ���� ��������� � ����� �������

            timer += Time.deltaTime;
            yield return null;
        }
    }

    public void SkipPhase()
    {
        skipPhase = true;
    }
}
