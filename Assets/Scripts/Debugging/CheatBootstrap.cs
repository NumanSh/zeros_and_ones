// Compiled only in the Unity Editor and in Development Builds - see CheatManager.cs.
#if UNITY_EDITOR || DEVELOPMENT_BUILD

using UnityEngine;

namespace ZerosAndOnes.Debugging
{
    /// <summary>
    /// Creates the cheat menu automatically when the game starts, in whichever scene it starts in.
    ///
    /// This mirrors <see cref="UI.PauseMenuBootstrap"/>: the project has 40 scenes in Build
    /// Settings, so placing a cheat prefab in each one by hand would be error-prone and would leave
    /// the object saved inside release scenes. Spawned from code it exists only when this file is
    /// compiled, which is only in the Editor and in Development Builds.
    /// </summary>
    public static class CheatBootstrap
    {
        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.AfterSceneLoad)]
        private static void Initialize()
        {
            // The object survives scene loads, so it only has to be created once. Unity's null
            // check also covers a stale static left over by "Enter Play Mode Options".
            if (CheatManager.Instance != null) return;

            var go = new GameObject(CheatManager.ObjectName);
            go.AddComponent<CheatManager>();
            go.AddComponent<CheatConsole>();
        }
    }
}

#endif
