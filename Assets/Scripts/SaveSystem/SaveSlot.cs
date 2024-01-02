/*
 *  Copyright � 2022 Omuhu Inc. - All Rights Reserved
 *  Unauthorized copying of this file, via any medium is strictly prohibited
 *  Proprietary and confidential
 */
 
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public class SaveSlot : MonoBehaviour, ISelectHandler, IPointerEnterHandler
{
    [Header("Dependencies")]
    [SerializeField] SaveSystem saveSystem;




    [Header("UI Elements")]
    [SerializeField] TextMeshProUGUI fileNameTextRef;
    [SerializeField] TextMeshProUGUI dayTextRef;
    [SerializeField] TextMeshProUGUI timeTextRef;
    //[SerializeField] Image icon;

    //[SerializeField] SaveMenuValuesSO saveMenuValuesSO;




    [Header("Events")]
    //[SerializeField] UnityEvent ChooseWorldNameEvent;
    //[SerializeField] UnityEvent ChooseCharacterEvent;
    [SerializeField] UnityEvent NextSceneEvent;



    //PRIVATE
    SaveData saveData;
    string worldName;


    public void SetStats(SaveData file) 
    {
        fileNameTextRef.text = file.worldName;
        dayTextRef.text = $"Day: {file.dayOfTheMonthIndex.ToString()}";
        timeTextRef.text = $"{file.hour.ToString()} h  {file.minute.ToString()} m";
        // Sprite foundIcon = Resources.Load<Sprite>("SaveIcons/" + file.iconString);
        // icon.sprite = foundIcon;
    }


    public void LoadFile(int index) //LOADING A FILE BASED ON FILENAME. RETURNING A FILE //LOAD PROFILES BY ID.
    {
        saveSystem.GetWorldDataForSaveSlot(saveSystem.savedFiles.files[index], out SaveData file);
        this.worldName = saveSystem.savedFiles.files[index];
        this.saveData = file;
        SetStats(file);
    }

    public void LoadGame() //BUTTON ONCLICK EVENT
    {
        saveSystem.LoadGame(worldName);
        NextSceneEvent.Invoke();
    }


    public void OnSelect(BaseEventData eventData)
    {
        //UpdateValues();
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        //UpdateValues();
    }

    // public void UpdateValues()
    // {
    //     saveMenuValuesSO.fileNameText = this.saveData.worldName;
    //     saveMenuValuesSO.day = this.saveData.dayOfTheMonthIndex.ToString();
    //     saveMenuValuesSO.gold = this.saveData.players[0].gold.ToString();
    //     saveMenuValuesSO.year = this.saveData.year.ToString();
    //     //saveMenuValuesSO.season = this.saveData.season;
    //     saveMenuValuesSO.timePlayed = this.saveData.realHours.ToString() + "h " + this.saveData.realMinutes.ToString() + "m";
    //     saveMenuValuesSO.dateSaved = this.saveData.dateSaved;
    //     saveMenuValuesSO.timeSaved = this.saveData.timeSaved;
    // }
}
