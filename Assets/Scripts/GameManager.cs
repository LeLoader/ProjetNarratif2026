using NUnit.Framework;
using System.Collections;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public SOGlobalMetrics globalMetrics;

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

        Metric metric = globalMetrics.metrics.Find((metric) => metric.type == EMetricType.INDOCTRINATED);
        metric.OnMetricReachedExtreme += DilemaManager.instance.OnMetricReachedExtreme;
    }


    // Call after all NPC from dilema have spawned
    public void SetTimer()
    {
        Timer timer = gameObject.AddComponent<Timer>();
        timer.Internal_Start(timeBetweenNPC.curve.Evaluate(npcCount), true);
    }
}
