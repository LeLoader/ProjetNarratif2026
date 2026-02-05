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

        CharacterBuilderManager.OnCharactersCreationFinished += OnCharactersCreationFinished;
    }

    private void Start()
    {
        Metric metric = globalMetrics.metrics.Find((metric) => metric.type == EMetricType.INDOCTRINATED);
        metric.OnMetricReachedExtreme += DilemmaManager.instance.OnMetricReachedExtreme;
    }

    private void OnCharactersCreationFinished(int npcCount)
    {
        this.npcCount += npcCount;
        Timer.SetTimer(gameObject, timeBetweenNPC.curve.Evaluate(npcCount), true).OnTimerElapsed += () =>
        {

            CharacterBuilderManager.Instance.AssignAnActionToRandomCharacter(ActionDataDrop.GetActionGoToPc());
        };

        UpdateWorldObjective();
    }

    private void UpdateWorldObjective()
    {
        ComputeGoalWorldObjective();
        ComputeRealityWorldObjective();

        foreach(SuperWorldObjective goalSuperWorldObjective in goalSuperWorldObjectives)
        {
            foreach(SuperWorldObjective realitySuperWorldObjective in realitySuperWorldObjectives)
            {
                if (realitySuperWorldObjective.GetMetricType() != goalSuperWorldObjective.GetMetricType())
                    continue;

                EMetricType currentType = realitySuperWorldObjective.GetMetricType();
                List<Tuple<EMetricState, SuperWorldObjective.EComparisonResult>> results = goalSuperWorldObjective.Compare(realitySuperWorldObjective);
                foreach(var result in results)
                {
                    Debug.Log($"{result.Item2} of {result.Item1} {currentType}");
                }
            }
        }


    }

    private void ComputeRealityWorldObjective()
    {
        realitySuperWorldObjectives = new()
        {
            new SuperWorldObjective(EMetricType.INDOCTRINATED),
            new SuperWorldObjective(EMetricType.VIOLENCE),
        };

        foreach (BehaviorController controller in CharacterBuilderManager.Instance.GetCharacters())
        {
            foreach (KeyValuePair<EMetricType, EMetricState> metric in controller.metrics)
            {
                realitySuperWorldObjectives.Find((swo) => swo.GetMetricType() == metric.Key).IncrementCount(metric.Value);
            }
        }
    }

    private void ComputeGoalWorldObjective()
    {
        goalSuperWorldObjectives = new();
        List<BehaviorController> controllers = CharacterBuilderManager.Instance.GetCharacters();
        foreach (Metric metric in globalMetrics.metrics)
        {
            List<WorldObjective> lwo = new()
            {
                new WorldObjective(EMetricState.POSITIVE, Mathf.RoundToInt(controllers.Count * ((float)metric.Positive) / 100)),
                new WorldObjective(EMetricState.NEUTRAL, Mathf.RoundToInt(controllers.Count * ((float)metric.Neutral) / 100)),
                new WorldObjective(EMetricState.NEGATIVE, Mathf.RoundToInt(controllers.Count * ((float)metric.Negative) / 100))
            };

            SuperWorldObjective swo = new(metric.type, lwo);
            swo.Fix(npcCount);
            goalSuperWorldObjectives.Add(swo);
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

    [Serializable]
    class SuperWorldObjective
    {
        public SuperWorldObjective(EMetricType type)
        {
            this._type = type;
            _worldObjectives = new()
            {
                new WorldObjective(EMetricState.POSITIVE, 0),
                new WorldObjective(EMetricState.NEUTRAL, 0),
                new WorldObjective(EMetricState.NEGATIVE, 0)         
            };
        }

        public SuperWorldObjective(EMetricType type, List<WorldObjective> worldObjectives)
        {
            this._type = type;
            this._worldObjectives = worldObjectives;
        }

        [SerializeField, ReadOnly] private EMetricType _type;
        [SerializeField, ReadOnly] private List<WorldObjective> _worldObjectives;

        public EMetricType GetMetricType() { return _type; }

        public void Fix(int npcCount)
        {
            int total = 0;
            foreach (WorldObjective wo in _worldObjectives)
            {
                total += wo.count;
            }

            if (total < npcCount)
            {
                WorldObjective wo = _worldObjectives.Find((w) => w.state == FindHighestProportion());
                wo.count++;
            }
            else if (total > npcCount)
            {
                WorldObjective wo = _worldObjectives.Find((w) => w.state == FindHighestProportion());
                wo.count--;
            }
        }

        private EMetricState FindHighestProportion()
        {
            int currentHighestCount = 0;
            EMetricState highestProportionState = EMetricState.NEUTRAL;
            foreach (WorldObjective wo in _worldObjectives)
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
            _worldObjectives.Find((wo) => wo.state == state).count++;
        }

        public List<Tuple<EMetricState, EComparisonResult>> Compare(SuperWorldObjective reality)
        {
            List<Tuple<EMetricState, EComparisonResult>> comparisonResult = new();
            foreach (WorldObjective wantedWorldObjective in _worldObjectives)
            { 
                foreach (WorldObjective realityWorldObjective in reality._worldObjectives)
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
