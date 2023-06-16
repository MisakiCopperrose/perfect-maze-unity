using System;
using System.Collections.Generic;
using UnityEngine;

public class MazeCellObject : MonoBehaviour
{
#if UNITY_EDITOR
    private static List<Stack<MazeCellObject>> _pools;
#endif
    [NonSerialized] 
    private Stack<MazeCellObject> _pool;

    private Stack<MazeCellObject> Pool
    {
        get
        {
            _pool ??= new();
#if UNITY_EDITOR
            Pools.Add(_pool);
#endif
            return _pool;
        }
        set => _pool = value;
    }

    public MazeCellObject GetInstance()
    {
        if (!Pool.TryPop(out var instance))
        {
            instance = Instantiate(this);
            instance.Pool = Pool;

            return instance;
        }

        instance.gameObject.SetActive(true);

        return instance;
    }

    public void Recycle()
    {
        Pool.Push(this);
        gameObject.SetActive(false);
    }

#if UNITY_EDITOR
    private static List<Stack<MazeCellObject>> Pools => _pools ??= new();

    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
    private static void ClearPools()
    {
        foreach (var cellObject in Pools)
        {
            cellObject.Clear();
        }
    }
#endif
}