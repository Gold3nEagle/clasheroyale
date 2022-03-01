using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UnityRoyale
{
    /// <summary>
    /// Class to manage the Playable Area, where the player can place their Unit of the selecetd card.
    /// </summary>
    public class PlayAreaController : MonoBehaviour
    {
        [Tooltip("Player's no. of small buildings destroyed on the play field")]
        int playerBuildingDestroyed = 0;

        [Tooltip("Opponent's no. of small buildings destroyed on the play field")]
        int opponentBuildingDestroyed = 0;

        [Tooltip("Place/Position small tower building that is destroyed")]
        [SerializeField] Enum.BuildingsPosition destroyedBuildingPlace = Enum.BuildingsPosition.None;

        private void OnEnable()
        {
            FuncManager.CheckForbiddenArea += HandleForbiddedArea;
            ActionManager.BuildingDestroyedCall += GetDestroyedBuildingCall;
        }
        private void OnDisable()
        {
            FuncManager.CheckForbiddenArea -= HandleForbiddedArea;
            ActionManager.BuildingDestroyedCall -= GetDestroyedBuildingCall;
        }

        // Start is called before the first frame update
        void Start()
        {
        }

        void GetDestroyedBuildingCall(Placeable.Faction _faction, Enum.BuildingsPosition _buildingPlace)
        {
            if (_faction.Equals(Placeable.Faction.Player))
            {
                playerBuildingDestroyed++;
                if (playerBuildingDestroyed.Equals(1))
                {
                    if (_buildingPlace.Equals(Enum.BuildingsPosition.Left))//For Building on Left
                        destroyedBuildingPlace = Enum.BuildingsPosition.Left;
                    else//For Building on Right
                        destroyedBuildingPlace = Enum.BuildingsPosition.Right;
                }
                else if (playerBuildingDestroyed.Equals(2))
                    destroyedBuildingPlace = Enum.BuildingsPosition.Center;
            }
            else
            {
                opponentBuildingDestroyed++;
                if (opponentBuildingDestroyed.Equals(1))
                {
                    if (_buildingPlace.Equals(Enum.BuildingsPosition.Left))//For Building on Left
                        destroyedBuildingPlace = Enum.BuildingsPosition.Left;
                    else//For Building on Right
                        destroyedBuildingPlace = Enum.BuildingsPosition.Right;
                }
                else if (opponentBuildingDestroyed.Equals(2))
                    destroyedBuildingPlace = Enum.BuildingsPosition.Center;
            }
        }

        Enum.BuildingsPosition HandleForbiddedArea()
        {
            if (destroyedBuildingPlace.Equals(Enum.BuildingsPosition.Center))
                return Enum.BuildingsPosition.Center;
            else if (destroyedBuildingPlace.Equals(Enum.BuildingsPosition.Left))
                return Enum.BuildingsPosition.Left;
            else if (destroyedBuildingPlace.Equals(Enum.BuildingsPosition.Right))
                return Enum.BuildingsPosition.Right;
            else
                return Enum.BuildingsPosition.None;
        }
    }
}