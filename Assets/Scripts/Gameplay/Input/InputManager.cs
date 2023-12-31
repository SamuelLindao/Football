using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Football.Gameplay.Input
{
    public class InputManager
    {
        public static InputData input;

        public InputActionAsset inputActionProfile;
        public InputActionMap inputActionMap;

        public static Action<int> onSwitchPlayer;

        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.AfterSceneLoad)]
        public static void CreateInstance()
        {
            new InputManager();
        }

        InputManager()
        {
            inputActionProfile = Resources.Load<InputActionAsset>("Input/DefaultActions");

            inputActionMap = inputActionProfile.actionMaps[0];
            inputActionMap["Move"].performed += OnMovePerformed;
            inputActionMap["Move"].canceled += OnMoveCanceled;
            inputActionMap["Through"].performed += OnThroughPerformed;
            inputActionMap["Through"].canceled += OnThroughCanceled;
            inputActionMap["Shoot"].performed += OnShootPerformed;
            inputActionMap["Shoot"].canceled += OnShootCanceled;
            inputActionMap["Pass"].performed += OnPassPerformed;
            inputActionMap["Pass"].canceled += OnPassCanceled;
            inputActionMap["Sprint and Skill"].performed += OnSprintAndSkillPerformed;
            inputActionMap["Sprint and Skill"].canceled += OnSprintAndSkillCanceled;
            inputActionMap["Switch Player"].performed += OnSwitchPlayerPerformed;
            inputActionMap["Switch Player"].canceled += OnSwitchPlayerCanceled;
            inputActionMap.Enable();
        }

        private void OnSwitchPlayerCanceled(InputAction.CallbackContext obj)
        {

        }

        private void OnSwitchPlayerPerformed(InputAction.CallbackContext obj)
        {
            int modifier = (int)obj.ReadValue<float>();

            var value = Mathf.Clamp(input.currentPlayer + modifier, 0, 3);
            if (input.currentPlayer != value)
            {
                input.currentPlayer = value;
                onSwitchPlayer?.Invoke(value);
            }
        }

        private void OnSprintAndSkillCanceled(InputAction.CallbackContext obj)
        {
            input.sprintAndSkill = false;
        }

        private void OnSprintAndSkillPerformed(InputAction.CallbackContext obj)
        {
            input.sprintAndSkill = true;
        }

        private void OnPassCanceled(InputAction.CallbackContext obj)
        {
            input.pass = false;
        }

        private void OnPassPerformed(InputAction.CallbackContext obj)
        {
            input.pass = true;
        }

        private void OnShootCanceled(InputAction.CallbackContext obj)
        {
            input.shoot = false;
        }

        private void OnShootPerformed(InputAction.CallbackContext obj)
        {
            input.shoot = true;
        }

        private void OnThroughCanceled(InputAction.CallbackContext obj)
        {
            input.through = false;
        }

        private void OnThroughPerformed(InputAction.CallbackContext obj)
        {
            input.through = true;
        }

        private void OnMoveCanceled(InputAction.CallbackContext obj)
        {
            input.move = Vector2.zero;
        }

        private void OnMovePerformed(InputAction.CallbackContext obj)
        {
            input.move = obj.ReadValue<Vector2>();
        }
    }
}
