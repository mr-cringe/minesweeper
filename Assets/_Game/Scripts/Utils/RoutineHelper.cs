using System;
using System.Collections;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Minesweeper.Utils
{
    public class RoutineDummy : MonoBehaviour
    {
        public event Action OnDestroyEvent;

        private void OnDestroy()
        {
            OnDestroyEvent?.Invoke();
        }
    }

    public class RoutineHelper
    {
        private static RoutineDummy _instance;
        private static RoutineDummy Instance => _instance ??= CreateDummy();

        private static RoutineDummy CreateDummy()
        {
            var routineHelper = new GameObject(nameof(RoutineHelper));
            Object.DontDestroyOnLoad(routineHelper);
            var dummy = routineHelper.AddComponent<RoutineDummy>();
            dummy.OnDestroyEvent += () => _instance = null;
            return dummy;
        }

        public static Coroutine Start(IEnumerator routine)
        {
            return Instance.StartCoroutine(routine);
        }

        public static void Stop(Coroutine coroutine)
        {
            Instance.StopCoroutine(coroutine);
        }

        public static void StopAll()
        {
            Instance.StopAllCoroutines();
        }
    }
}