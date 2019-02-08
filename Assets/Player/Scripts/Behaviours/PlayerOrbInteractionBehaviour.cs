﻿using UnityEngine;

namespace Sangaku
{
    [RequireComponent(typeof(Collider))]
    public class PlayerOrbInteractionBehaviour : BaseBehaviour
    {
        [SerializeField] bool isEnabled = true;
        [SerializeField] Transform orbCatchPoint;
        [SerializeField] float catchDuration;

        [SerializeField] UnityFloatEvent OnOrbCatch;
        [SerializeField] UnityVoidEvent OnCatchEnd;

        SphereCollider catchCollider;
        [HideInInspector]
        public OrbController caughtOrb;

        ManaBehaviour mana;

        protected override void CustomSetup()
        {
            caughtOrb = null;
            mana = Entity.gameObject.GetComponent<ManaBehaviour>();
        }

        ManaBehaviour orbMana;
        /// <summary>
        /// Checks if there is no Orb already, and if not catches the given one. Called by the Orb class.
        /// </summary>
        /// <param name="_orb">the Orb to catch.</param>
        public void CatchOrb(OrbController _orb)
        {
            if (!isEnabled)
                return;
            if (!caughtOrb)
            {
                caughtOrb = _orb;
                orbMana = caughtOrb.GetComponent<ManaBehaviour>();
                mana.AddMana(orbMana.GetMana());
                print(name + " received " + orbMana.GetMana() + " mana from " + caughtOrb.name +"!");
                caughtOrb.SM.GoToCaughtState(orbCatchPoint);
                OnOrbCatch.Invoke(catchDuration);
            }
        }

        /// <summary>
        /// Frees the current caught Orb if there is one.
        /// </summary>
        public void FreeOrb()
        {
            if (!isEnabled)
                return;
            caughtOrb.SM.GoToFreeState();
            caughtOrb = null;
            OnCatchEnd.Invoke();
        }

        /// <summary>
        /// Returns the orb caught if there is one.
        /// </summary>
        /// <returns></returns>
        public OrbController GetOrb()
        {
            if (!isEnabled)
                return null;
            if (caughtOrb)
                return caughtOrb;
            else
                return null;
        }

        public override void Enable(bool _value)
        {
            isEnabled = _value;
        }

    } 
}