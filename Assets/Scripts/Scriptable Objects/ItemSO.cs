/*
 *  Copyright © 2022 Omuhu Inc. - All Rights Reserved
 *  Unauthorized copying of this file, via any medium is strictly prohibited
 *  Proprietary and confidential
 */

using UnityEngine;
using UnityEngine.UI;
using Sirenix.OdinInspector;


[System.Serializable]
public abstract class ItemSO : ScriptableObject
{
    //[GUIColor(0.3f, 0.8f, 0.8f, 1f)]
    [Title("General ItemSO Values")]
    [Tooltip("Should be same name as Scriptable Object")] 
    //[BoxGroup("General Item Info")]
    //[LabelWidth(150)]
    [SerializeField] string itemName;


    [Space(10)]
    [TextArea]
    [Tooltip("Limit to 24 characters")] 
    //[BoxGroup("General Item Info")]
    [SerializeField] string itemDescription;


    [Space(10)]
    [PreviewField(50, ObjectFieldAlignment.Left)]
    [HideLabel]
    //[BoxGroup("General Item Info")]
    [SerializeField] Sprite icon;

    
    [Tooltip("Used by Tooltip to show the class of item. And by incubator. To determine if the item is an 'Egg'")] 
    //[BoxGroup("General Item Info")]
    [SerializeField] ItemClassNames itemClass;
    [SerializeField] AnimationString animationString;
    public AnimationString GetAnimationString { get { return animationString; } }
    [Range(1, 5)] [SerializeField] int itemLevel = 1;
    [Range(0, 10000)] [SerializeField] int itemSellValue;
    [SerializeField] bool stackable;


    [Space(30)]
    //[GUIColor(0.3f, 0.8f, 0.8f, 1f)]
    [Header("    Tag - Used by Inventory Buttons to categorize where items can be placed")]
    [Tooltip("TAG MUST BE (Clothing/Ring/Potion/General) - Used by Inventory Buttons to categorize where items can be placed")] 
    [SerializeField] ButtonTag buttonTagEnum;


    //[Space(10)]
    //[GUIColor(0.3f, 0.8f, 0.8f, 1f)]
    //[Header("    Item Action - Currently used for (EatAction)")]
    //public ToolAction onSpecialAction;
    public ItemActionSO ItemAction;


    //[Header("Animation Trigger")]
    //public AnimationSO primaryToolAnimationTriggerName;
    //public AnimationSO secondaryToolAnimationTriggerName;


    // [Header("Item unique ID")]
    // public string uniqueID;
    // [ContextMenu("Generate Unique GUID for ID")]
    // void GenerateUniqueID()
    // {
    //     uniqueID = System.Guid.NewGuid().ToString();
    // }


    public virtual float GetAttackSpeed() { return 0; }
    public string GetItemName() { if(itemName != name) { Debug.Log(itemName + " is not the same name as its Scriptable Object. " + name); } return itemName; }
    public string GetItemDescription() { return itemDescription; }
    public Sprite GetIcon() { if(icon == null) { Debug.LogError("No Sprite attached to " + name); } return icon; }
    public ItemClassNames GetClass() { if(itemClass == ItemClassNames.Null) { Debug.LogError(name + ": Item Class Name is set to NULL"); } return itemClass; }
    public ButtonTag GetButtonTag() { if(buttonTagEnum == ButtonTag.Null) { Debug.LogError(name + ": Button Tag is set to NULL"); } return buttonTagEnum; }
    public bool IsStackable() { return stackable; }
    public int GetItemLevel() { return itemLevel; }
    public int GetItemSellValue() { return itemSellValue; }
    public void SetItemLevel(int level)
    {
        this.itemLevel = level;
    }
}

[System.Serializable]
public enum ItemClassNames
{
    Null,
    Animal,
    Clothing,
    Consumable,
    Crop,
    Egg,
    Furniture,
    General,
    Ingredient,
    Misc,
    Path,
    Potion,
    Product,
    Resource,
    Ring,
    Seed,
    Tool,
    Trash,
    Wand
}

[System.Serializable]
public enum ButtonTag
{
    Null,
    Locked,
    General,
    Clothing,
    Ring,
    Potion,
    Hotbar,
    Misc,
}

[System.Serializable]
public enum AnimationString
{
    NoAnimation,
    Attack,
    SwingTool,
    ThrowItem,
    Block
}
