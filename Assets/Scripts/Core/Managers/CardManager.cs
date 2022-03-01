using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using DG.Tweening;
using System;

namespace UnityRoyale
{
    public class CardManager : MonoBehaviour
    {
        public Camera mainCamera; //public reference
        public LayerMask playingFieldMask;
        public GameObject cardPrefab;
        public DeckData playersDeck;
        /*public MeshRenderer forbiddenAreaRenderer;*/

        [Space(20)]
        [Tooltip("Reference of Center, Left and Right ForbiddenArea for the player")]
        [SerializeField] List<GameObject> forbiddenAreaList;
        [Tooltip("Reference of Opponent side player's Placement area, that enables after either of the Small Towers are destroyed")]
        [SerializeField] List<GameObject> playersPlacementAreaList;

        public UnityAction<CardData, Vector3, Placeable.Faction> OnCardUsed;

        [Header("UI Elements")]
        public RectTransform backupCardTransform; //the smaller card that sits in the deck
        public RectTransform cardsDashboard; //the UI panel that contains the actual playable cards
        public RectTransform cardsPanel; //the UI panel that contains all cards, the deck, and the dashboard (center aligned)

        private Card[] cards;
        private bool cardIsActive = false; //when true, a card is being dragged over the play field
        private GameObject previewHolder;
        private Vector3 inputCreationOffset = new Vector3(0f, 0f, 1f); //offsets the creation of units so that they are not under the player's finger

        private void Awake()
        {
            previewHolder = new GameObject("PreviewHolder");
            cards = new Card[4]; //4 is the length of the dashboard
        }

        public void LoadDeck()
        {
            DeckLoader newDeckLoaderComp = gameObject.AddComponent<DeckLoader>();
            newDeckLoaderComp.OnDeckLoaded += DeckLoaded;
            newDeckLoaderComp.LoadDeck(playersDeck);
        }

        //...

        private void DeckLoaded()
        {
            Debug.Log("Player's deck loaded");

            //setup initial cards
            _ = StartCoroutine(AddCardToDeck(.1f));
            for (int i = 0; i < cards.Length; i++)
            {
                _ = StartCoroutine(PromoteCardFromDeck(i, .4f + i));
                _ = StartCoroutine(AddCardToDeck(.8f + i));
            }
        }

        //moves the preview card from the deck to the active card dashboard
        private IEnumerator PromoteCardFromDeck(int position, float delay = 0f)
        {
            Debug.Log("Player Promoting Card from Deck");

            yield return new WaitForSeconds(delay);

            backupCardTransform.SetParent(cardsDashboard, true);
            //move and scale into position
            backupCardTransform.DOAnchorPos(new Vector2(160f * (position + 1) + 15f, -50f), .1f + (.05f * position)).SetEase(Ease.OutQuad);
            backupCardTransform.localScale = new Vector3(.8f, .9f, .9f);

            //store a reference to the Card component in the array
            Card cardScript = backupCardTransform.GetComponent<Card>();
            cardScript.cardId = position;
            cards[position] = cardScript;

            //setup listeners on Card events
            cardScript.OnTapDownAction += CardTapped;
            cardScript.OnDragAction += CardDragged;
            cardScript.OnTapReleaseAction += CardReleased;
        }

        //adds a new card to the deck on the left, ready to be used
        private IEnumerator AddCardToDeck(float delay = 0f) //TODO: pass in the CardData dynamically
        {
            Debug.Log("Player Add Card to Deck");

            yield return new WaitForSeconds(delay);

            //create new card
            backupCardTransform = Instantiate<GameObject>(cardPrefab, cardsPanel).GetComponent<RectTransform>();
            backupCardTransform.localScale = Vector3.one * 0.7f;

            //send it to the bottom left corner
            backupCardTransform.anchoredPosition = new Vector2(80f, -300f);
            backupCardTransform.DOAnchorPos(new Vector2(70f, 70f), .2f).SetEase(Ease.OutQuad);

            //populate CardData on the Card script
            Card cardScript = backupCardTransform.GetComponent<Card>();
            cardScript.InitialiseWithData(playersDeck.GetNextCardFromDeck());
        }

        private void CardTapped(int cardId)
        {
            cards[cardId].GetComponent<RectTransform>().SetAsLastSibling();
            /*forbiddenAreaRenderer.enabled = true;*/

            //var destroyedTowerPlace = FuncManager.CheckForbiddenArea();

            //if (destroyedTowerPlace.Equals(Enum.BuildingsPosition.Center))
            //{
            //    ManageForbiddenArea(Enum.BuildingsPosition.Center);
            //    playersPlacementAreaList.ForEach(item =>
            //    {
            //        if (item.activeSelf.Equals(false))
            //            item.SetActive(true);
            //    });
            //}
            //else if (destroyedTowerPlace.Equals(Enum.BuildingsPosition.Left))
            //{
            //    ManageForbiddenArea(Enum.BuildingsPosition.Left);
            //    playersPlacementAreaList[0].SetActive(true);//Enable the Player's placement area on lefts side on Opponent's side
            //}
            //else if (destroyedTowerPlace.Equals(Enum.BuildingsPosition.Right))
            //{
            //    ManageForbiddenArea(Enum.BuildingsPosition.Right);
            //    playersPlacementAreaList[1].SetActive(true);//Enable the Player's placement area on Right side on Opponent's side
            //}
            //else//When no towers are destroyed
            //    ManageForbiddenArea(Enum.BuildingsPosition.None);
        }

        void ManageForbiddenArea(Enum.BuildingsPosition _forbiddedAreaToEnable)
        {
            forbiddenAreaList.ForEach(item => item.SetActive(false));
            forbiddenAreaList[(int)_forbiddedAreaToEnable].SetActive(true);
        }

        private void CardDragged(int cardId, Vector2 dragAmount)
        {
            cards[cardId].transform.Translate(dragAmount);

            #region Code to Manage Enable and Disable of Forbidden area  
            var destroyedTowerPlace = FuncManager.CheckForbiddenArea();

            if (destroyedTowerPlace.Equals(Enum.BuildingsPosition.Center))
            {
                ManageForbiddenArea(Enum.BuildingsPosition.Center);
                playersPlacementAreaList.ForEach(item =>
                {
                    if (item.activeSelf.Equals(false))
                        item.SetActive(true);
                });
            }
            else if (destroyedTowerPlace.Equals(Enum.BuildingsPosition.Left))
            {
                ManageForbiddenArea(Enum.BuildingsPosition.Left);
                playersPlacementAreaList[0].SetActive(true);//Enable the Player's placement area on lefts side on Opponent's side
            }
            else if (destroyedTowerPlace.Equals(Enum.BuildingsPosition.Right))
            {
                ManageForbiddenArea(Enum.BuildingsPosition.Right);
                playersPlacementAreaList[1].SetActive(true);//Enable the Player's placement area on Right side on Opponent's side
            }
            else//When no towers are destroyed
                ManageForbiddenArea(Enum.BuildingsPosition.None);
            #endregion

            //raycasting to check if the card is on the play field
            RaycastHit hit;
            Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);

            bool planeHit = Physics.Raycast(ray, out hit, Mathf.Infinity, playingFieldMask);

            if (planeHit)
            {
                if (!cardIsActive)
                {
                    //Place Player Card Character on the playable Field
                    cardIsActive = true;
                    previewHolder.transform.position = hit.point;
                    cards[cardId].ChangeActiveState(true); //hide card

                    //retrieve arrays from the CardData
                    PlaceableData[] dataToSpawn = cards[cardId].cardData.placeablesData;
                    Vector3[] offsets = cards[cardId].cardData.relativeOffsets;

                    //spawn all the preview Placeables and parent them to the cardPreview
                    for (int i = 0; i < dataToSpawn.Length; i++)
                    {
                        GameObject newPlaceable = GameObject.Instantiate<GameObject>(dataToSpawn[i].associatedPrefab,
                                                                                    hit.point + offsets[i] + inputCreationOffset,
                                                                                    Quaternion.identity,
                                                                                    previewHolder.transform);
                    }
                }
                else
                {
                    //temporary copy has been created, we move it along with the cursor
                    previewHolder.transform.position = hit.point;
                }
            }
            else
            {
                if (cardIsActive)
                {
                    cardIsActive = false;
                    cards[cardId].ChangeActiveState(false); //show card

                    ClearPreviewObjects();
                }
            }
        }

        private void CardReleased(int cardId)
        {
            //raycasting to check if the card is on the play field
            RaycastHit hit;
            Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);

            if (Physics.Raycast(ray, out hit, Mathf.Infinity, playingFieldMask))
            {
                if (OnCardUsed != null)
                    OnCardUsed(cards[cardId].cardData, hit.point + inputCreationOffset, Placeable.Faction.Player); //GameManager picks this up to spawn the actual Placeable

                ClearPreviewObjects();
                Destroy(cards[cardId].gameObject); //remove the card itself

                print("Reduce Elixir as per the Card used in the game");
                ActionManager.ReduceElixir(cards[cardId].cardElixir * 10.0f);

                _ = StartCoroutine(PromoteCardFromDeck(cardId, .2f));
                _ = StartCoroutine(AddCardToDeck(.6f));
            }
            else
            {
                cards[cardId].GetComponent<RectTransform>().DOAnchorPos(new Vector2(220f * (cardId + 1), 0f), .2f).SetEase(Ease.OutQuad);
            }

            /*forbiddenAreaRenderer.enabled = false;*/
            forbiddenAreaList.ForEach(item => item.SetActive(false));
        }

        //happens when the card is put down on the playing field, and while dragging (when moving out of the play field)
        private void ClearPreviewObjects()
        {
            //destroy all the preview Placeables
            for (int i = 0; i < previewHolder.transform.childCount; i++)
            {
                Destroy(previewHolder.transform.GetChild(i).gameObject);
            }
        }
    }
}