using EditorAttributes;
using Palmmedia.ReportGenerator.Core.Parser.Analysis;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    [SerializeField] SOGlobalMetrics globalMetricsSO;
    public GlobalMetrics globalMetrics;

    [SerializeField] Curve timeBetweenNPC;
    int npcCount = 0;

    #region World Objectives

    [SerializeField, ReadOnly] List<SuperWorldObjective> goalSuperWorldObjectives = new();
    [SerializeField, ReadOnly] List<SuperWorldObjective> realitySuperWorldObjectives = new();

    #endregion World Objectives

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

        ComputeGoalWorldObjective();
        ComputeRealityWorldObjective();
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
        SetTimer().OnTimerElapsed += () =>
        {

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
        ComputeRealityWorldObjective();
        ComputeGoalWorldObjective();

        foreach(SuperWorldObjective goalSuperWorldObjective in goalSuperWorldObjectives)
        {
            foreach(SuperWorldObjective realitySuperWorldObjective in realitySuperWorldObjectives)
            {
                if (realitySuperWorldObjective.type != goalSuperWorldObjective.type)
                    continue;

                EMetricType currentType = realitySuperWorldObjective.type;
                List<Tuple<EMetricState, SuperWorldObjective.EComparisonResult>> results = goalSuperWorldObjective.Compare(realitySuperWorldObjective);
                foreach(var result in results)
                {
                    
                }
            }
        }


    }

    private void ComputeRealityWorldObjective()
    {
        List<SuperWorldObjective> realitySuperWorldObjectives = new()
        {
            new SuperWorldObjective(EMetricType.INDOCTRINATED, new()),
            new SuperWorldObjective(EMetricType.VIOLENCE, new()),
        };

        foreach (BehaviorController controller in CharacterBuilderManager.Instance.GetCharacters())
        {
            foreach (KeyValuePair<EMetricType, EMetricState> metric in controller.metrics)
            {
                realitySuperWorldObjectives.Find((swo) => swo.type == metric.Key).IncrementCount(metric.Value);
            }
        }
    }

    private void ComputeGoalWorldObjective()
    {
        List<BehaviorController> controllers = CharacterBuilderManager.Instance.GetCharacters();
        foreach (Metric metric in globalMetrics.metrics)
        {
            List<WorldObjective> lwo = new()
            {
                new WorldObjective(EMetricState.POSITIVE, Mathf.RoundToInt(controllers.Count / (float)metric.Positive)),
                new WorldObjective(EMetricState.NEUTRAL, Mathf.RoundToInt(controllers.Count / (float)metric.Neutral)),
                new WorldObjective(EMetricState.NEGATIVE, Mathf.RoundToInt(controllers.Count / (float)metric.Negative))
            };

            SuperWorldObjective swo = new(metric.type, lwo);
        }
    }

    [Serializable]
    class WorldObjective
    {
        public EMetricState state;
        public int count;

        public WorldObjective(EMetricState state, int count)
        {
            this.state = state;
            this.count = count;
        }
    }

    class SuperWorldObjective
    {
        public SuperWorldObjective(EMetricType type, List<WorldObjective> worldObjectives)
        {
            this.type = type;
            this.worldObjectives = worldObjectives;
        }

        public EMetricType type;
        public List<WorldObjective> worldObjectives;

        public void Fix(int npcCount)
        {
            int total = 0;
            foreach (WorldObjective wo in worldObjectives)
            {
                total += wo.count;
            }

            if (total < npcCount)
            {
                WorldObjective wo = worldObjectives.Find((w) => w.state == FindHighestProportion());
                wo.count++;
            }
            else if (total > npcCount)
            {
                WorldObjective wo = worldObjectives.Find((w) => w.state == FindHighestProportion());
                wo.count--;
            }
        }

        private EMetricState FindHighestProportion()
        {
            int currentHighestCount = 0;
            EMetricState highestProportionState = EMetricState.NEUTRAL;
            foreach (WorldObjective wo in worldObjectives)
            {
                if (wo.count > currentHighestCount)
                {
                    currentHighestCount = wo.count;
                    highestProportionState = wo.state;
                }
            }
            return highestProportionState;
        }

        public void IncrementCount(EMetricState state)
        {
            worldObjectives.Find((wo) => wo.state == state).count++;
        }

        public List<Tuple<EMetricState, EComparisonResult>> Compare(SuperWorldObjective reality)
        {
            List<Tuple<EMetricState, EComparisonResult>> comparisonResult = new();
            foreach (WorldObjective wantedWorldObjective in worldObjectives)
            { 
                foreach (WorldObjective realityWorldObjective in reality.worldObjectives)
                {
                    if (realityWorldObjective.state != wantedWorldObjective.state)
                        continue;

                    EMetricState state = realityWorldObjective.state;

                    if (realityWorldObjective.count < wantedWorldObjective.count)
                    {
                        comparisonResult.Add(new Tuple<EMetricState, EComparisonResult>(state, EComparisonResult.NEED_MORE));
                    }
                    else if (realityWorldObjective.count > wantedWorldObjective.count)
                    {
                        comparisonResult.Add(new Tuple<EMetricState, EComparisonResult>(state, EComparisonResult.NEED_LESS));
                    }
                }
            }
            return comparisonResult;
        }

        public enum EComparisonResult
        {
            GOOD = 0,
            NEED_MORE = 1,
            NEED_LESS = 2,
        }
    }
}
