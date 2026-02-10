using AYellowpaper.SerializedCollections;
using System.Collections.Generic;
using UnityEngine;

public class Sapling : MonoBehaviour
{
    [SerializeField] float growthTime;
    [SerializeField] Animation growthAnimation;
    [SerializeField] Object saplingMesh;

    [SerializeField, SerializedDictionary("Mesh", "Weight")] SerializedDictionary<GameObject, int> possibleOutcome;

    private bool checkIsAnimationPlaying;

    public void Water()
    {
        Timer.SetTimer(gameObject, growthTime).OnTimerElapsed += () => 
        {
            checkIsAnimationPlaying = true;
            growthAnimation.Play();
        };
    }

    public void Update()
    {
        if (checkIsAnimationPlaying && !growthAnimation.isPlaying)
        {
            Grow();
        }
    }

    private void Grow()
    {
        checkIsAnimationPlaying = false;
        int totalWeight = 0;
        foreach (KeyValuePair<GameObject, int> outcome in possibleOutcome)
        {
            totalWeight += outcome.Value;
        }

        foreach (KeyValuePair<GameObject, int> outcome in possibleOutcome)
        {
            if (outcome.Value / (float)totalWeight >= Random.Range(1, totalWeight + 1) / (float)totalWeight)
            {
                Destroy(saplingMesh);
                GameObject newObject = Instantiate(outcome.Key, transform);
                gameObject.name = newObject.GetComponent<MeshFilter>().mesh.name;
                Destroy(this);
                // @TODO Destroy this marche pas?
                // if the game object must be destroyed
                //Destroy(gameObject);
                break;
            }
            else
            {
                totalWeight -= outcome.Value;
                continue;
            }
        }

        Destroy(this);
    }
}
