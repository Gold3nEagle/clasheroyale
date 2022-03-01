using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UnityRoyale
{
    public class DestroyedReamins : MonoBehaviour
    {
        private void OnEnable()
        {
            ActionManager.NavmeshRebuild();
        }

        // Start is called before the first frame update
        void Start()
        {

        }
    }
}
