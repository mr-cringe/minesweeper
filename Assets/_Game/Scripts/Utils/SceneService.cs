using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Minesweeper.Utils
{
    public class SceneService
    {
        private const int GameSceneIndex = 1;

        public void LoadGameScene(Action callback)
        {
            LoadScene(GameSceneIndex, callback);
        }

        private void LoadScene(int index, Action callback)
        {
            var loadOp = SceneManager.LoadSceneAsync(index, LoadSceneMode.Single);
            loadOp.allowSceneActivation = false;
            RoutineHelper.Start(WaitMainMenuLoad(loadOp));

            return;

            IEnumerator WaitMainMenuLoad(AsyncOperation op)
            {
                while (op.progress < .89f)
                {
                    yield return null;
                }

                op.allowSceneActivation = true;

                while (!op.isDone)
                {
                    yield return null;
                }

                callback?.Invoke();
            }
        }
    }
}