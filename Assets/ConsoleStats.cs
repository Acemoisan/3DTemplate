using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ConsoleStats : MonoBehaviour
{
    [Title("Text References")]
    [Header("Player Stats")]
    [SerializeField] TextMeshProUGUI healthValueRef; //original: XXXX, current: XXXX / XXXX
    [SerializeField] TextMeshProUGUI energyValueRef;
    [SerializeField] TextMeshProUGUI speedValueRef;
    [SerializeField] TextMeshProUGUI jumpHeightValueRef;
    [SerializeField] TextMeshProUGUI playerWorldPosition;
    [SerializeField] TextMeshProUGUI playerAimWorldPosition;
    [SerializeField] TextMeshProUGUI currentAnimationRef;
    [SerializeField] TextMeshProUGUI currentEffectTypeRef;
    [SerializeField] TextMeshProUGUI interacteeRef;
    [SerializeField] TextMeshProUGUI activeItem;
    [SerializeField] TextMeshProUGUI gravityValueRef;




    [Space(20)]
    [Title("World Stats")]
    [SerializeField] TextMeshProUGUI inGameDayValueRef;
    [SerializeField] TextMeshProUGUI inGameTimeValueRef;
    [SerializeField] TextMeshProUGUI inGameSecondsPerMinuteValueRef;


    [Space(20)]
    [Title("Performance Stats")]
    [SerializeField] TextMeshProUGUI fpsRef;



    string goldHex = "DDAD34";
    string parameterHex = "EF28BF";
    string valueHexCode = "34CADD";
    private float deltaTime = 0.0f;


    void Update()
    {
        PerformanceStats();
    }


    public void UpdatePlayerStats(Transform player, PlayerInventory playerInventory, PlayerAttributes playerAttributes, PlayerController playerController, 
    PlayerAnimation playerAnimation, CameraModeController cameraModeController, PlayerInteraction playerInteraction, PlayerEffectsHandler playerEffectsHandler)
    {
        if(playerEffectsHandler.currentEffect != EffectType.None) currentEffectTypeRef.text = $"<color=#{goldHex}>Current Effect: </color>{playerEffectsHandler.currentEffect} (<color=#{parameterHex}>Duration:</color> {playerEffectsHandler.remainingDuration})";
        else currentEffectTypeRef.text = $"<color=#{goldHex}>Current Effect: </color>None";
        if(playerInteraction.GetInteractee() != null) interacteeRef.text = $"<color=#{goldHex}>Interactee: </color>{playerInteraction.GetInteractee().name}";
        else interacteeRef.text = $"<color=#{goldHex}>Interactee: </color>None";
        playerAimWorldPosition.text = $"<color=#{goldHex}>Aim Pos: </color>{cameraModeController.AimWorldPosition.x}, {cameraModeController.AimWorldPosition.y}, {cameraModeController.AimWorldPosition.z}";
        currentAnimationRef.text = $"<color=#{goldHex}>Current Animation: </color>{playerAnimation.GetAnimator().GetCurrentAnimatorClipInfo(0)[0].clip.name}";
        playerWorldPosition.text = $"<color=#{goldHex}>World Pos: </color>{player.position.x}, {player.position.y}, {player.position.z}";
        healthValueRef.text = $"<color=#{goldHex}>Health: </color><color=#{valueHexCode}>{playerAttributes.GetPlayerHealth()}</color> / {playerAttributes.GetPlayerMaxHealth()}(Max), (OG){playerAttributes.GetPlayerOriginalHealth()}";
        energyValueRef.text = $"<color=#{goldHex}>Energy:</color> (<color=#{parameterHex}>AutoRegen:</color> {playerAttributes.AutoRegenEnergy()}) <color=#{valueHexCode}>{playerAttributes.GetPlayerEnergy()}</color> / {playerAttributes.GetPlayerMaxEnergy()}(Max), (OG){playerAttributes.GetPlayerOriginalEnergy()}";
        speedValueRef.text = $"<color=#{goldHex}>Speed: </color><color=#{valueHexCode}>{playerController.WalkSpeed}</color>, (OG){playerController.OriginalWalkSpeed}";
        jumpHeightValueRef.text = $"<color=#{goldHex}>Jump Height: </color><color=#{valueHexCode}>{playerController.JumpHeight}</color>, (OG){playerController.OriginalJumpHeight}";
        if(playerInventory.GetActiveItem() != null) activeItem.text = $"<color=#{goldHex}>Active Item: </color>{playerInventory.GetActiveItem().name} (<color=#{parameterHex}>Damage:</color> {playerInventory.GetActiveItem().GetToolDamage()})";
        else activeItem.text = $"<color=#{goldHex}>Active Item: </color>None";
        gravityValueRef.text = $"<color=#{goldHex}>Gravity: </color>{playerController.Gravity}";
    }


    public void UpdateWorldStats(TimeManagerSO time)
    {
        inGameDayValueRef.text = $"<color=#{goldHex}>Day: </color>{time.dayOfTheMonthIndex}";
        inGameTimeValueRef.text = $"<color=#{goldHex}>Time: </color>{time.hour}:{time.minute}:{time.second}";
        inGameSecondsPerMinuteValueRef.text = $"<color=#{goldHex}>Seconds Per 10 Min: </color>{time.secondsPerInGameTenMinutes}";
    }

    void PerformanceStats()
    {
        deltaTime += (Time.unscaledDeltaTime - deltaTime) * 0.1f;
        float msec = deltaTime * 1000.0f;
        float fps = 1.0f / deltaTime;
        fpsRef.text = $"{msec:0.0} <color=#{goldHex}>ms</color> ({fps:0.} <color=#{goldHex}>fps</color>)";
    }
}
