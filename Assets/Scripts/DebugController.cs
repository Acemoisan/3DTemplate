using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using TMPro;
using UnityEngine.Events;

public class DebugController : MonoBehaviour
{
    [Header("Dependencies")]
    [SerializeField] PlayerInventory playerInventory;
    [SerializeField] PlayerAttributes playerAttributes;
    [SerializeField] PlayerController playerController;
    [SerializeField] PlayerControllerSO _playerControllerSO;
    [SerializeField] SceneLoader sceneLoader;
    [SerializeField] TimeManagerSO currentTime;
    //[SerializeField] SeasonManagerSO currentSeason;
    [SerializeField] ItemDatabase itemDatabase;
    [SerializeField] ItemSpawnerSO itemSpawner;
    [SerializeField] TMP_InputField inputField;
    [SerializeField] GameObject debugMenu;
    [SerializeField] GameObject helpMenu;
    [SerializeField] Transform helpCommandListHolder;
    //[SerializeField] Transform itemListHolder;



    [Header("Text References")]
    [SerializeField] List<GameObject> debugPageTabs;
    [SerializeField] TextMeshProUGUI healthValueRef;
    [SerializeField] TextMeshProUGUI energyValueRef;
    [SerializeField] TextMeshProUGUI speedValueRef;
    [SerializeField] TextMeshProUGUI inGameDayValueRef;
    [SerializeField] TextMeshProUGUI inGameTimeValueRef;
    [SerializeField] TextMeshProUGUI inGameSecondsPerMinuteValueRef;
    //[SerializeField] TextMeshProUGUI chanceOfRainPercentageValueRef;
    //[SerializeField] TextMeshProUGUI seasonTextRef;
    //[SerializeField] TextMeshProUGUI weatherTextRef;
    // [SerializeField] TextMeshProUGUI fishingTextRef;
    // [SerializeField] TextMeshProUGUI combatTextRef;
    // [SerializeField] TextMeshProUGUI waterBendingTextRef;
    // [SerializeField] TextMeshProUGUI fireBendingTextRef;
    // [SerializeField] TextMeshProUGUI earthBendingTextRef;
    // [SerializeField] TextMeshProUGUI airBendingTextRef;


    [Header("Configurables")]
    [SerializeField] Color commandIDTextColor;
    [SerializeField] SceneSO customScene;





    [SerializeField] List<ItemSlot> inventoryKit;
    Dictionary<int, string> itemIndexList = new Dictionary<int, string>();
    [HideInInspector] public Dictionary<string, int> itemNameToID = new Dictionary<string, int>();
    string chatLogFileName = "Command_Logs";




    bool showConsole = false;


    string input = "";


    public static DebugCommand HELP;
    public static DebugCommand DIE;
    public static DebugCommand NEXT_DAY;
    public static DebugCommand<int, int> SPAWN_ITEM;
    public static DebugCommand<string> SPAWN_ANIMAL;
    //public static DebugCommand<string> LOAD_SCENE;
    public static DebugCommand<float> SET_HEALTH;
    public static DebugCommand<float> SET_ENERGY;
    public static DebugCommand<float> SET_MANA;
    public static DebugCommand<int> SET_GOLD;
    public static DebugCommand<int> SET_TIME;
    public static DebugCommand<int> SET_TIME_INC;
    public static DebugCommand<string> TIME_STATE;
    public static DebugCommand<int> SET_WEATHER;
    public static DebugCommand<int> SET_SEASON;
    public static DebugCommand<string, int> SET_ABILITY_LEVEL;

    public List<DebugCommandBase> commandList;


    string commandColorHexCode;

    void Awake()
    {
        commandColorHexCode = UnityEngine.ColorUtility.ToHtmlStringRGB(commandIDTextColor);
        SetCommands();
    }

    void Start()
    {
        SetItemIndex();
    }

    private void SetCommands()
    {        
        HELP = new DebugCommand("help", "Shows all available commands", "help", () =>
        {
            ShowHelp();
        });

        DIE = new DebugCommand("die", "Kills Player", "die", () =>
        {
            playerAttributes.Kill();
        });

        NEXT_DAY = new DebugCommand("next_day", "Skips to next day", "next_day", () =>
        {
            currentTime.SkipToNextMorning();
        });

        SPAWN_ITEM = new DebugCommand<int, int>("spawn_item", "Spawns item(s) based on given Index and Count", "spawn_item <color=orange>itemIndex</color> <color=yellow>amount</color>", (int itemIndex, int count) =>
        {
            ItemSO itemToSpawn;
            //use index and get dictionary value pair from indexlist
           // string itemName = itemIndexList[itemIndex];

           string itemName = itemIndexList[itemIndex];

            itemToSpawn = itemDatabase.GetMatchingItem(itemName);
            itemSpawner.SpawnItem(itemToSpawn, transform.parent.position, count);
        });

        // LOAD_SCENE = new DebugCommand<string>("load_scene", "Loads scene based on given Name", "load_scene <color=orange>sceneName</color>", (string sceneName) =>
        // {
        //     SceneLoaderManager.instance.ManuallyLoadScene(sceneName);
        // });

        TIME_STATE = new DebugCommand<string>("time", "Sets Time State ", "time <color=yellow>pause/resume</color>", (string state) =>
        {
            switch (state)
            {
                case "pause":
                    currentTime.PauseTime(true);
                    break;
                case "resume":
                    currentTime.PauseTime(false);
                    break;
                default:
                    Debug.Log("Invalid Time State");
                    break;
            }
        });

        SET_TIME = new DebugCommand<int>("set_time", "Sets Hour of Day (24HR)", "set_time <color=yellow>XX</color>", (int time) =>
        {
            currentTime.SetTime(time);
            //playerMusicManager.PlayAnyTrack(MusicManager.instance.GetTrackToPlay());
        });

        SET_TIME_INC = new DebugCommand<int>("set_time_inc", "Sets Time Increment - (Seconds per in game 10 min) Default - 8", "set_time_inc <color=yellow>time</color>", (int increment) =>
        {
            currentTime.SetTimeIncrement(increment);
        });

        SET_HEALTH = new DebugCommand<float>("set_health", "Add or Subtract Health", "set_health <color=yellow>amount</color>", (float health) =>
        {
            playerAttributes.IncreaseHealth(health);
        });

        SET_ENERGY = new DebugCommand<float>("set_energy", "Add or Subtract Energy", "set_energy <color=yellow>amount</color>", (float energy) =>
        {
            playerAttributes.IncreaseEnergy(energy);
        });


        SET_GOLD = new DebugCommand<int>("set_gold", "Gives set amount of Gold to player", "set_gold <color=yellow>amount</color>", (int gold) =>
        {
            playerInventory.AddCurrency(gold);
        });

        // SET_WEATHER = new DebugCommand<int>("set_weather", "Sets Weather based on Index (Sun-0, Wind-1, Fog-2, Rain-3, Snow-4. Storm-5)", "set_weather <color=yellow>index</color>", (int weatherIndex) =>
        // {
        //     WeatherManager.instance.SetWeather(weatherIndex);
        // });

        // SET_SEASON = new DebugCommand<int>("set_season", "Sets Season based on Index (Spring-0, Summer-1, Fall-2, Winter-3) ", "set_season <color=yellow>index</color>", (int seasonIndex) =>
        // {
        //     currentSeason.SetSeason(seasonIndex);
        // });



        commandList = new List<DebugCommandBase>()
        {
            HELP,
            DIE,
            NEXT_DAY,
            SPAWN_ITEM,
            //LOAD_SCENE,
            SET_TIME,
            SET_TIME_INC,
            TIME_STATE,
            SET_HEALTH,
            SET_ENERGY,
            SET_GOLD,
            //SET_WEATHER,
            //SET_SEASON,
            
        };
    }

    void SetItemIndex()
    {
        // itemIndexList = new Dictionary<int, string>();
        // itemNameToID = new Dictionary<string, int>();


        // for (int i = 0; i < itemDatabase.items.Count; i++)
        // {
        //     Transform textObject = Instantiate(itemListHolder.GetChild(0), itemListHolder) as Transform;
        //     TextMeshProUGUI text = itemListHolder.GetChild(i).GetComponent<TextMeshProUGUI>();

        //     itemIndexList.Add(i, itemDatabase.items[i].GetItemName());
        //     if(itemNameToID.ContainsKey(itemDatabase.items[i].GetItemName()) == false)
        //     {
        //         itemNameToID.Add(itemDatabase.items[i].GetItemName(), i);
        //     } 

        //     //itemListHolder.GetChild(i).gameObject.SetActive(true);

        //     string label = "id:" + i + "- <color=#" + commandColorHexCode + ">" + itemDatabase.items[i].GetItemName() + "</color>";
        
        //     //itemListHolder.GetChild(i).GetComponent<TextMeshProUGUI>().text = label;  
        //     text.text = label; 
        // }
    }


    public void ChangeDebugPageTab(int pageIndex)
    {
        for(int page = 0; page < debugPageTabs.Count; page++)
        {
            debugPageTabs[page].SetActive(false);
        }

        debugPageTabs[pageIndex].SetActive(true);
    }



    #region QUICK COMMANDS

    public void COM_PrintAllItemNamesToTextFile()
    {
        // string itemListString = "";
        // foreach (ItemSO item in itemDatabase.items)
        // {
        //     itemListString += item.GetItemName() + "\n";
        // }

        // ToTextFile.CreateNewTextFile("Databases/Item Names", itemListString, true);
    }
    public void COM_GivePlayer1000Gold()
    {
        playerInventory.AddCurrency(1000);
    }

    public void COM_NextDay()
    {
        currentTime.SkipToNextMorning();
    }

    public void COM_TeleportToCustomScene()
    {
        //sceneLoader.LoadScene(townScene);
    }


    public void COM_IncreaseEnergyBy50()
    {
        playerAttributes.IncreaseEnergy(50);
    }

    public void DecreaseEnergyBy50()
    {
        playerAttributes.ReduceEnergy(50);
    }

    public void COM_GiveInventoryKit()
    {
        foreach(ItemSlot slot in inventoryKit)
        {
            playerInventory.AddItem(slot.item, slot.count);
        }
    }
    public void COM_RefreshPlayerStats()
    {
        playerAttributes.RefreshStats();
    } 

    public void COM_KILL()
    {
        playerAttributes.Kill();
    }  
    #endregion

    IEnumerator UpdateDebugValueMenu()
    {
        while(debugMenu.activeSelf == true)
        {        
            healthValueRef.text = playerAttributes.GetPlayerHealth().ToString();
            energyValueRef.text = playerAttributes.GetPlayerEnergy().ToString();
            speedValueRef.text = _playerControllerSO.MoveSpeed.ToString();
            inGameDayValueRef.text = TimeManager.instance.dayOfTheWeekString +  " " + TimeManager.instance.GetMonthIndex().ToString() + "/" + TimeManager.instance.GetDayOfTheMonthIndex().ToString() + "/" + TimeManager.instance.GetYear().ToString();
            
            if(TimeManager.instance.TimePaused() == true)
            {
                inGameTimeValueRef.text = "(P)" + TimeManager.instance.GetHour().ToString() + ":" + TimeManager.instance.GetMinute().ToString() + "." + TimeManager.instance.GetSecond().ToString();
            }
            else if(TimeManager.instance.TimePaused() == false)
            {
                inGameTimeValueRef.text = TimeManager.instance.GetHour().ToString() + ":" + TimeManager.instance.GetMinute().ToString() + "." + TimeManager.instance.GetSecond().ToString();
            }
            
            inGameSecondsPerMinuteValueRef.text = TimeManager.instance.GetSecondsPerTenMin().ToString();
            //chanceOfRainPercentageValueRef.text = WeatherManager.instance.GetChanceOfRainPercentage().ToString();
            //seasonTextRef.text = SeasonManager.instance.GetSeasonString();
            //weatherTextRef.text = WeatherManager.instance.GetWeather().ToString();
            
            yield return new WaitForSeconds(0.5f);
        }
    }

    public void ToggleDebug() //called through input event
    {
        StartCoroutine(UpdateDebugValueMenu());
        input = "";
        inputField.text = input;
        ShowHelp();
        inputField.ActivateInputField();
    }

    public void ShowHelp()
    {
        helpMenu.SetActive(true);

        for (int i = 0; i < helpCommandListHolder.transform.childCount; i++)
        {
            helpCommandListHolder.GetChild(i).gameObject.SetActive(false);
        }
        
        for (int i = 0; i < commandList.Count; i++)
        {
            DebugCommandBase command = commandList[i] as DebugCommandBase;
            helpCommandListHolder.GetChild(i).gameObject.SetActive(true);

            string label = "''<color=#" + commandColorHexCode + ">" + command.CommandFormat + "</color>''  -  " + command.CommandDescription;
        
            helpCommandListHolder.GetChild(i).GetComponent<TextMeshProUGUI>().text = label; 
        }
    }

    void UpdateMatchingLabels(string text)
    {
        helpMenu.SetActive(true);

        for (int i = 0; i < helpCommandListHolder.transform.childCount; i++)
        {
            helpCommandListHolder.GetChild(i).gameObject.SetActive(false);
        }

        for (int i = 0; i < commandList.Count; i++)
        {
            if(commandList[i].CommandFormat.Contains(text))
            {
                DebugCommandBase command = commandList[i] as DebugCommandBase;
                helpCommandListHolder.GetChild(i).gameObject.SetActive(true);

                string label = "''<color=#" + commandColorHexCode + ">" + command.CommandFormat + "</color>''  -  " + command.CommandDescription;
            
                helpCommandListHolder.GetChild(i).GetComponent<TextMeshProUGUI>().text = label;   


                //if(SPAWN_ITEM.CommandFormat.Contains(text))
                //{
                    //if text is same as Spawn_Item foarmat. then show the item list
                    //string[] itemNames = itemDatabase.GetItemNames().Split(',');
                                
                    //helpCommandListHolder.GetChild(0).GetComponent<TextMeshProUGUI>().text = itemDatabase.GetItemNames(); 
                //}         
            }
        }

    }


    public void UpdateInput() //called OnValueChanged on textmeshpro input field
    {
        input = inputField.text;
        UpdateMatchingLabels(input);
    }

    public void OnReturn() //called through input event
    {
        HandleInput();
        input = "";
        inputField.text = input;
        inputField.ActivateInputField();
        helpMenu.SetActive(false);
    }



    void HandleInput()
    {
        string[] inputArray = input.Split(' ');

        for (int i = 0; i < commandList.Count; i++)
        {
            DebugCommandBase debugCommand = (DebugCommandBase)commandList[i];
            if(inputArray[0] == debugCommand.CommandID)
            {

                if (commandList[i] as DebugCommand != null)
                {
                    (commandList[i] as DebugCommand).Invoke();
                }
                else if (commandList[i] as DebugCommand<int> != null)
                {
                    (commandList[i] as DebugCommand<int>).Invoke(int.Parse(inputArray[1]));
                }
                else if (commandList[i] as DebugCommand<float> != null)
                {
                    (commandList[i] as DebugCommand<float>).Invoke(float.Parse(inputArray[1]));
                }
                else if (commandList[i] as DebugCommand<string> != null)
                {
                    (commandList[i] as DebugCommand<string>).Invoke(inputArray[1]);
                }
                else if (commandList[i] as DebugCommand<string, int> != null)
                {
                    (commandList[i] as DebugCommand<string, int>).Invoke(inputArray[1], int.Parse(inputArray[2]));
                }
                else if (commandList[i] as DebugCommand<int, int> != null)
                {
                    (commandList[i] as DebugCommand<int, int>).Invoke(int.Parse(inputArray[1]), int.Parse(inputArray[2]));
                }                
                else
                {
                    Debug.Log("Command not found");
                }
            }
        }
    }

}
