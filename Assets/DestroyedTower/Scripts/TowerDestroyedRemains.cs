using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UnityRoyale
{
    /// <summary>
    /// Class to manage the activation of Destroyed reamins after the tower is destroyed 
    /// </summary>
    public class TowerDestroyedRemains : MonoBehaviour
    {
        [Tooltip("Reference of Destroyed Reamins of the Tower")]
        [SerializeField] GameObject destroyedReaminsPrefab;

        // Start is called before the first frame update
        void Start()
        {
        }

        public void ActivateDestroyedRemains()
        {
            destroyedReaminsPrefab.SetActive(true);
        }
    }
}
