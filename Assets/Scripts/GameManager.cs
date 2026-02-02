using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] SOGlobalMetrics globalMetricsSO;
    public GlobalMetrics globalMetrics;
    public static GameManager instance;

    [SerializeField] Curve timeBetweenNPC;
    int npcCount = 0;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
        globalMetrics = globalMetricsSO.globalMetrics;
    }

    private void Start()
    {
        Metric metric = globalMetrics.metrics.Find((metric) => metric.type == EMetricType.INDOCTRINATED);
        metric.OnMetricReachedExtreme += DilemmaManager.instance.OnMetricReachedExtreme;
        CharacterBuilderManager.OnCharactersCreationFinished += OnCharactersCreationFinished;
    }

    private void OnCharactersCreationFinished(int npcCount)
    {
        this.npcCount += npcCount;
        SetTimer().OnTimerElapsed += () => {
            CharacterBuilderManager.Instance.AssignAnActionToRandomCharacter(ActionDataDrop.GetActionGoToPc());
        };
    }

    // Call after all NPC from dilema have spawned
    public Timer SetTimer()
    {
        Timer timer = gameObject.AddComponent<Timer>();
        timer.Internal_Start(timeBetweenNPC.curve.Evaluate(npcCount), true);
        return timer;
    }
}
