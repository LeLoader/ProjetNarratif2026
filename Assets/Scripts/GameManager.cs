using EditorAttributes;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    [SerializeField] SOGlobalMetrics globalMetricsSO;
    [SerializeField, ReadOnly] List<WorldObjective> worldObjective = new();
    public GlobalMetrics globalMetrics;

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
        globalMetrics = new();
        globalMetrics.metrics = globalMetricsSO.globalMetrics.metrics.ConvertAll(m => new Metric(m.label, m.type));
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

        UpdateWorldObjective();
    }

    // Call after all NPC from dilema have spawned
    public Timer SetTimer()
    {
        Timer timer = gameObject.AddComponent<Timer>();
        timer.Internal_Start(timeBetweenNPC.curve.Evaluate(npcCount), true);
        return timer;
    }

    private void UpdateWorldObjective()
    {
        List<BehaviorController> controllers = CharacterBuilderManager.Instance.GetCharacters();
        foreach (BehaviorController controller in controllers)
        {
            foreach (KeyValuePair<EMetricType, EMetricState> metric in controller.metrics)
            {
                WorldObjective currentWo = new(metric.Key, metric.Value, 0);
                WorldObjective foundWo = worldObjective.Find((wo) => wo.type == currentWo.type && wo.state == currentWo.state);
                if (foundWo.count >= 1)
                {
                    foundWo.count++;
                }
                else
                {
                    currentWo.count++;
                    worldObjective.Add(currentWo);
                }
            }
        }
        Debug.Log("Updated world obj");

    }

    [Serializable]
    struct WorldObjective
    {
        public EMetricType type;
        public EMetricState state;
        public int count;

        public WorldObjective(EMetricType type, EMetricState state, int count)
        {
            this.type = type;
            this.state = state;
            this.count = count;
        }
    }
}
