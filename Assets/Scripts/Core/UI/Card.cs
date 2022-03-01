using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Events;
using UnityEngine.UI;
using TMPro;

namespace UnityRoyale
{
    public class Card : MonoBehaviour, IDragHandler, IPointerUpHandler, IPointerDownHandler
    {
        public UnityAction<int, Vector2> OnDragAction;
        public UnityAction<int> OnTapDownAction, OnTapReleaseAction;

        [HideInInspector] public int cardId;
        [HideInInspector] public CardData cardData;

        public Image portraitImage; //Inspector-set reference
        private CanvasGroup canvasGroup;

        [Tooltip("Text Componenet Elixir for the Card")]
        [SerializeField] TextMeshProUGUI txtCardElixir;

        [Tooltip("Required Elixir for the Card")]
        [HideInInspector] public float cardElixir;

        private void Awake()
        {
            canvasGroup = GetComponent<CanvasGroup>();
        }


        private void Update()
        {
            if ((Elixir.usableElixir / 10) >= cardElixir)
                ChangeActiveState(false, true);
            else
                ChangeActiveState(true, true);
        }


        //called by CardManager, it feeds CardData so this card can display the placeable's portrait
        public void InitialiseWithData(CardData cData)
        {
            cardData = cData;
            portraitImage.sprite = cardData.cardImage;
            txtCardElixir.text = cardData.requiredElixir.ToString();
            cardElixir = cardData.requiredElixir;
        }

        public void OnPointerDown(PointerEventData pointerEvent)
        {
            /*if (OnTapDownAction != null)
                OnTapDownAction(cardId);*/
            if (canvasGroup.interactable && OnTapDownAction != null)
                OnTapDownAction(cardId);
        }

        public void OnDrag(PointerEventData pointerEvent)
        {
            /*if (OnDragAction != null)
                OnDragAction(cardId, pointerEvent.delta);*/
            if (canvasGroup.interactable && OnDragAction != null)
                OnDragAction(cardId, pointerEvent.delta);
        }

        public void OnPointerUp(PointerEventData pointerEvent)
        {
            /*if (OnTapReleaseAction != null)
                OnTapReleaseAction(cardId);*/
            if (canvasGroup.interactable && OnTapReleaseAction != null)
                OnTapReleaseAction(cardId);
        }

        public void ChangeActiveState(bool isActive, bool _enableDisableCard = false)
        {
            /*canvasGroup.alpha = (isActive) ? .05f : 1f;*/
            canvasGroup.alpha = (isActive) ? .5f : 1f;

            if (_enableDisableCard)
                canvasGroup.interactable = (isActive) ? false : true;
        }
    }
}