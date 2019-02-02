﻿using UnityEngine;
using UnityEngine.Events;

namespace Sangaku
{
    /// <summary>
    /// Behaviour che gestisce l'input del player
    /// </summary>
    public class PlayerInputBehaviour : BaseBehaviour
    {
        #region Events
        /// <summary>
        /// Evento lanciato alla pressione del bottone di dash
        /// </summary>
        [SerializeField] UnityVector3Event OnDashPressed;
        /// <summary>
        /// Evento lanciato alla pressione del bottone di shot
        /// </summary>
        [SerializeField] UnityEvent OnShotPressed;
        /// <summary>
        /// Evento lanciato alla pressione del bottone di parry
        /// </summary>
        [SerializeField] UnityEvent OnParryPressed;
        /// <summary>
        /// Evento lanciato al cambio di direzione dell'asse di input
        /// </summary>
        [SerializeField] UnityVector3Event OnDirectionUpdate;
        #endregion

        #region Inputs KeyCode
        /// <summary>
        /// Tasto che corrisponde allo shot
        /// </summary>
        [SerializeField] KeyCode shotInput = KeyCode.Mouse0;
        /// <summary>
        /// Tasto che corrisponde al parry
        /// </summary>
        [SerializeField] KeyCode parryInput = KeyCode.Mouse1;
        /// <summary>
        /// Tasto che corrisponde al dash
        /// </summary>
        [SerializeField] KeyCode dashInput = KeyCode.Space;
        #endregion

        /// <summary>
        /// Enumerativo che indica il tipo di movimento che si vuole
        /// </summary>
        public enum DirectionType { Global, Camera };
        public DirectionType InputDirection;

        /// <summary>
        /// Direzione in cui viene mosso l'asse di input della direzione
        /// </summary>
        Vector3 _moveDirection;
        /// <summary>
        /// Propery che lancia un evento al cambio di direzione dell'input
        /// </summary>
        Vector3 MoveDirection
        {
            get { return _moveDirection; }
            set
            {
                if (_moveDirection != value)
                {
                    _moveDirection = value;
                    OnDirectionUpdate.Invoke(_moveDirection);
                }
            }
        }
        /// <summary>
        /// Riferimento alla camera
        /// </summary>
        Camera cam;
        bool canShoot = true;
        bool canDash = true;
        bool canMove = true;
        bool canParry = true;

        protected override void CustomSetup()
        {
            cam = Camera.main;
        }

        /// <summary>
        /// Update
        /// </summary>
        void Update()
        {
            if (IsSetupped)
                ReadInput(); 
        }

        /// <summary>
        /// Variabile temporanea per il calcolo del movimento in base alla camera
        /// </summary>
        Vector3 cameraBasedDirection;
        /// <summary>
        /// Funzione che si occupa di leggere gli input
        /// </summary>
        void ReadInput()
        {
            //Move Input
            if (canMove)
            {
                if (InputDirection == DirectionType.Global)
                {
                    MoveDirection = new Vector3(Input.GetAxisRaw("Horizontal"), 0f, Input.GetAxisRaw("Vertical")).normalized;
                }
                else if (InputDirection == DirectionType.Camera && cam != null)
                {
                    MoveDirection = new Vector3(Input.GetAxisRaw("Horizontal"), 0f, Input.GetAxisRaw("Vertical")).normalized;
                    cameraBasedDirection = cam.transform.TransformDirection(MoveDirection);
                    MoveDirection = new Vector3(cameraBasedDirection.x, MoveDirection.y, cameraBasedDirection.z).normalized;
                }
            }

            //Shoot Input
            if (canShoot && Input.GetKeyDown(shotInput))
            {
                OnShotPressed.Invoke();
            }

            //ParryInput
            if (canParry && Input.GetKeyDown(parryInput))
            {
                OnParryPressed.Invoke();
            }

            //DashInput
            if (canDash && Input.GetKeyDown(dashInput))
            {
                OnDashPressed.Invoke(MoveDirection);
            }
        }

        public void ToggleShootInput(bool _value)
        {
            canShoot = _value;
        }
        public void ToggleMovementInput(bool _value)
        {
            canMove = _value;
            MoveDirection = Vector3.zero;
        }
        public void ToggleDashInput(bool _value)
        {
            canDash = _value;
        }
        public void ToggleParryInput(bool _value)
        {
            canParry = _value;
        }
    }
}
