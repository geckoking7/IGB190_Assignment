using UnityEngine;

public interface IPooledObject
{
    void Cleanup();
    void Setup();
}
