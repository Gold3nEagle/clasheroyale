using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UnityRoyale
{
    public static class ActionManager
    {
        [Tooltip("Action call to reduce the Elixir from the bar on use of any card")]
        public static Action<float> ReduceElixir;

        [Tooltip("Action call to reduce the Elixir from the bar on use of any card")]
        public static Action GameOverCall;

        [Tooltip("Action call to Rebuild Navmesh after any building has been destroyed")]
        public static Action NavmeshRebuild;

        [Tooltip("Action call when any Small Tower is destroyed on the Play Field")]
        public static Action<Placeable.Faction, Enum.BuildingsPosition> BuildingDestroyedCall;
    }

    public static class FuncManager
    {
        [Tooltip("Func call to manage the Forbidden area for placing the cards in the Play Area")]
        public static Func<Enum.BuildingsPosition> CheckForbiddenArea;
    }
}
