using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

namespace UnityRoyale
{
    public class Building : ThinkingPlaceable
    {
        //Inspector references
        [Header("Timelines")]
        public PlayableDirector constructionTimeline;
        public PlayableDirector destructionTimeline;

        [Space(20)]
        [Tooltip("Reference of Ballista on the building to be rotated along with the weapon for attack")]
        [SerializeField] Transform buildingWeapon;
        [Tooltip("Reference of Cannon on Calstle")]
        [SerializeField] GameObject castleCannon;

        Quaternion newRotation;
        ScoreManager scoreManager;

        [Tooltip("The position/place of building placed on the Play field")]
        [SerializeField] Enum.BuildingsPosition buildingPlaced;

        private void Awake()
        {
            audioSource = GetComponent<AudioSource>();
            scoreManager = FindObjectOfType<ScoreManager>();
        }

        public void Activate(Faction pFaction, PlaceableData pData)
        {
            pType = pData.pType;
            faction = pFaction;
            hitPoints = pData.hitPoints;
            targetType = pData.targetType;
            attackRange = pData.attackRange;
            attackRatio = pData.attackRatio;
            attackAudioClip = pData.attackClip;
            dieAudioClip = pData.dieClip;
            //TODO: add more as necessary

            state = States.Idle;
            constructionTimeline.Play();
        }

        public override void SetTarget(ThinkingPlaceable t)
        {
            base.SetTarget(t);
        }

        //Unit moves towards the target
        public override void Seek()
        {
            if (target == null)
                return;

            base.Seek();
        }

        //Unit has gotten to its target. This function puts it in "attack mode", but doesn't delive any damage (see DealBlow)
        public override void StartAttack()
        {
            base.StartAttack();
        }

        //Starts the attack animation, and is repeated according to the Unit's attackRatio
        public override void DealBlow()
        {
            base.DealBlow();

            newRotation = Quaternion.LookRotation(transform.position - target.transform.position, Vector3.forward);

            newRotation.x = 0.0f;
            newRotation.z = 0.0f;

            if (faction.Equals(Faction.Player))
            {
                if (transform.position.x >= 0)
                    newRotation.y *= (-1.0f);
            }
            else if (faction.Equals(Faction.Opponent))
            {
                if (transform.position.x < 0)
                    newRotation.y *= (-1.0f);
            }

            buildingWeapon.rotation = Quaternion.Slerp(transform.rotation, newRotation, Time.deltaTime * 8);
            FireProjectile();
        }


        protected override void Die()
        {
            base.Die();
            //audioSource.PlayOneShot(dieAudioClip, 1f);

            Debug.Log("Building is dead", gameObject);
            if (pType.Equals(PlaceableType.Building))
            {
                destructionTimeline.Play();
                ActionManager.BuildingDestroyedCall(faction, buildingPlaced);
            }
            else if (pType.Equals(PlaceableType.Castle))
            {
                castleCannon.SetActive(false);
            }

            //Change Score
            if (gameObject.CompareTag("Blue"))
            {
                scoreManager.ChangeScore(GameTeams.teamRed);
            }
            else if (gameObject.CompareTag("Red"))
            {
                scoreManager.ChangeScore(GameTeams.teamBlue);
            }
        }
    }
}