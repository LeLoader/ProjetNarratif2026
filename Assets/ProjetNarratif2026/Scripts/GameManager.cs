using UnityEngine;

public abstract class GameManager : MonoBehaviour
{
    abstract public void Initialize();
    abstract public void Deinitialize();
}
