using System;
using System.Collections.Generic;
using UnityEngine;

namespace Maze.Cell
{
    public class MazeCell : MonoBehaviour
    {
        public MazeCellRotation Rotation { get; set; }
        
#if UNITY_EDITOR
        private static List<Stack<MazeCell>> _pools;
#endif
        [NonSerialized] 
        private Stack<MazeCell> _pool;

        private Stack<MazeCell> Pool
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

        public MazeCell GetInstance()
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
        private static List<Stack<MazeCell>> Pools => _pools ??= new();

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
}