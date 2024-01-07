using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class SaveSlotHolder : MonoBehaviour
{
    [SerializeField] SaveSystem saveSystem;
    [SerializeField] UnityEngine.GameObject saveSlotholder;
    [SerializeField] UnityEngine.GameObject fileSlotPrefab;
    // [SerializeField] SaveMenuValuesSO saveMenuValuesSO;
    // [SerializeField] GameObject statsPanel;

    // [Header("Save Values")]
    // [SerializeField] TextMeshProUGUI fileName;
    // [SerializeField] TextMeshProUGUI season;
    // [SerializeField] TextMeshProUGUI day;
    // [SerializeField] TextMeshProUGUI year;
    // [SerializeField] TextMeshProUGUI gold;
    // [SerializeField] TextMeshProUGUI timePlayed;
    // [SerializeField] TextMeshProUGUI dateSaved;
    // [SerializeField] TextMeshProUGUI timeSaved;

    // void Start()
    // {
    //     ResetValues();
    // }

    // void Update()
    // {
    //     fileName.text = saveMenuValuesSO.fileNameText;
    //     season.text = saveMenuValuesSO.season;
    //     day.text = saveMenuValuesSO.day + ",";
    //     year.text = "Year " + saveMenuValuesSO.year;
    //     gold.text = saveMenuValuesSO.gold;
    //     timePlayed.text = saveMenuValuesSO.timePlayed;
    //     dateSaved.text = saveMenuValuesSO.dateSaved;
    //     timeSaved.text = saveMenuValuesSO.timeSaved;
    // }

    // void ResetValues()
    // {
    //     saveMenuValuesSO.fileNameText = "";
    //     saveMenuValuesSO.season = "";
    //     saveMenuValuesSO.day = "";
    //     saveMenuValuesSO.year = "";
    //     saveMenuValuesSO.gold = "";
    //     saveMenuValuesSO.timePlayed = "";
    //     saveMenuValuesSO.dateSaved = "";
    //     saveMenuValuesSO.timeSaved = "";
    // }


    public void LoadAllSaveSLots()
    {
        foreach (Transform slot in saveSlotholder.transform)
        {
            Destroy(slot.gameObject);
        }

        for (int slot = 0; slot < saveSystem.savedFiles.files.Count; slot++)
        {
            InstantiateSaveSlot(slot);
        }

        // if(saveSystem.saveFile.files.Count == 0)
        // {
        //     statsPanel.SetActive(false);
        // }
    }

    void InstantiateSaveSlot(int index)
    {
        UnityEngine.GameObject newSlot = Instantiate(fileSlotPrefab, saveSlotholder.transform);
        newSlot.GetComponent<SaveSlot>().LoadFile(index);
    }
}
