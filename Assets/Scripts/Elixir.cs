using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

namespace UnityRoyale
{
    public class Elixir : MonoBehaviour
    {
        private float elixir;
        private float lerpTimer;

        public float elixirRegenerationSpeed = 1;
        public float maxElixir = 100;
        public float elixirIncreasedChipSpeed = 2f;
        public float elixirDecreasedChipSpeed = 2f;
        public Image frontElixirBar, backElixirBar;

        [Tooltip("The Elixir amount that the player can actually use for placing the Cards.")]
        public static float usableElixir;

        [Tooltip("Field to manage the start and stop filling of ElixirBar")]
        bool toStopFillElixirBar = false;

        private void OnEnable()
        {
            ActionManager.ReduceElixir += ReduceElixir;
            ActionManager.GameOverCall += HandleFillingOfElixirBar;
        }
        private void OnDisable()
        {
            ActionManager.ReduceElixir -= ReduceElixir;
            ActionManager.GameOverCall -= HandleFillingOfElixirBar;
        }

        // Start is called before the first frame update
        void Start()
        {
            //elixir = maxElixir;

            //Set the Elixir bar to be filled 20% at start of the gameplay
            backElixirBar.fillAmount = 0.4f;
            frontElixirBar.fillAmount = backElixirBar.fillAmount - 0.1f;
            elixir = backElixirBar.fillAmount * 100.0f;
            UpdateUsableElixir();
        }

        // Update is called once per frame
        void Update()
        {
            if (!toStopFillElixirBar)
            {
                elixir = Mathf.Clamp(elixir, 0, maxElixir);
                UpdateElixirUI();

                IncreaseElixir(10f);

                //To manage the increase of FrontElixirBar Fill Amount w.r.t the BackElixirBar Fill Amount
                if ((backElixirBar.fillAmount - frontElixirBar.fillAmount) > 0.1f)
                {
                    frontElixirBar.fillAmount += 0.1f;
                    UpdateUsableElixir();
                }

                /*if (Input.GetKeyDown(KeyCode.A))
                {
                    ReduceElixir(10f);
                }
                if (Input.GetKeyDown(KeyCode.S))
                {
                    IncreaseElixir(10f);
                }*/
            }
        }


        public void UpdateElixirUI()
        {
            float fillBack = backElixirBar.fillAmount;
            float hFraction = elixir / maxElixir;

            if (fillBack < hFraction)//When Elixir is Increased
            {
                lerpTimer += Time.deltaTime * elixirRegenerationSpeed;
                float percentComplete = lerpTimer / elixirDecreasedChipSpeed;
                percentComplete = percentComplete * percentComplete;
                backElixirBar.fillAmount = Mathf.Lerp(fillBack, hFraction, percentComplete);
            }
        }


        public void ReduceElixir(float Damage)
        {
            //elixir -= Damage;
            //lerpTimer = 0f;

            frontElixirBar.fillAmount = (usableElixir - Damage) / 100.0f;
            backElixirBar.fillAmount = (usableElixir - Damage) / 100.0f;
            UpdateUsableElixir();
        }

        public void IncreaseElixir(float healAmount)
        {
            elixir += healAmount;
            lerpTimer = 0f;
        }

        /// <summary>
        /// Method to update the usableElixir field
        /// </summary>
        void UpdateUsableElixir()
        {
            usableElixir = frontElixirBar.fillAmount * 100.0f;
        }

        /// <summary>
        /// Method to handle the Stopping of filling of ElixirBar on GameOver
        /// </summary>
        void HandleFillingOfElixirBar()
        {
            toStopFillElixirBar = true;
        }
    }
}



//#region OLD CODE Elixir Decrease
////----START---Original Code Don't Delete------
///*if (fillBack > hFraction)//When Elixir is Reduced
//{
//    print("Elixir is reduced");
//    frontElixirBar.fillAmount = hFraction;
//    lerpTimer += Time.deltaTime;
//    float percentComplete = lerpTimer / elixirIncreasedChipSpeed;
//    percentComplete = percentComplete * percentComplete;
//    backElixirBar.fillAmount = Mathf.Lerp(fillBack, hFraction, percentComplete);
//}*/
////----END---Original Code Don't Delete------
//#endregion

//#region OLD CODE Elixir Increase
////----START---Original Code Don't Delete------
///*if (fillFront < hFraction)//When Elixir is Increased
//{
//    print("Elixir is Increased");
//    backElixirBar.fillAmount = hFraction;
//    lerpTimer += Time.deltaTime;
//    float percentComplete = lerpTimer / elixirDecreasedChipSpeed;
//    percentComplete = percentComplete * percentComplete;
//    frontElixirBar.fillAmount = Mathf.Lerp(fillFront, backElixirBar.fillAmount, percentComplete);
//}*/
////----END---Original Code Don't Delete------
//#endregion