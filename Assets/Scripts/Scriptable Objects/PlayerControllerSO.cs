using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;


[CreateAssetMenu(fileName = "New Manager", menuName = "Scriptable Objects/Controllers/Player Controller SO")]
public class PlayerControllerSO : ScriptableObject
{
        [Header("Player Configurables")]
        [Header("Health")]
        public float _playerMaxHealth;


        [Space(20)]
        [Header("Energy")]
        public float _playerMaxEnergy;



        [Header("Movement Speed")]
		[Tooltip("Move speed of the character in m/s")]
		public float MoveSpeed = 5.0f;
		[Tooltip("Sprint speed of the character in m/s")]
		public float SprintSpeedMultiplier = 1.5f;
        [Tooltip("Sprint speed of the character in m/s")]
		public float MoveSpeedBoost = 6.0f;
        [Tooltip("Aim speed of the camera")]
        public float lookSpeed;
        
        [Tooltip("Acceleration and deceleration")]
		public float SpeedChangeRate = 10.0f;

        [Space(10)]
        [Tooltip("The height the player can jump")]
        public float JumpHeight = 1.2f;

        [Tooltip("The character uses its own gravity value. The engine default is -9.81f")]
        public float Gravity = -15.0f;

        [Space(10)]
        [Tooltip("Time required to pass before being able to jump again. Set to 0f to instantly jump again")]
        public float JumpTimeout = 0.50f;

        [Tooltip("Time required to pass before entering the fall state. Useful for walking down stairs")]
        public float FallTimeout = 0.15f;



        // Preference Keys
        public const string Look_Speed_Key = "Look_Speed";

        public void ChangeLookSpeed(float value) { ValueChanged(Look_Speed_Key, value); }

        private void ValueChanged(string key, float value)
        {
            this.SaveValue(key, value);
            this.LoadValue(key);
        }

        private void SaveValue(string key, float value)
        {
            if (key == Look_Speed_Key) PlayerPrefs.SetFloat(Look_Speed_Key, value);
        }

        public void LoadValues()
        {
            this.lookSpeed = PlayerPrefs.GetFloat(Look_Speed_Key, 200);
        }

        private void LoadValue(string key)
        {
            if (key == Look_Speed_Key) lookSpeed = PlayerPrefs.GetFloat(Look_Speed_Key, 200);
        }
}
