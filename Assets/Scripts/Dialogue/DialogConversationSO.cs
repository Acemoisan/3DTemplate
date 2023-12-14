/*
 *  Copyright © 2022 Omuhu Inc. - All Rights Reserved
 *  Unauthorized copying of this file, via any medium is strictly prohibited
 *  Proprietary and confidential
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Sentence
{
    public DialogCharacterSO dialogCharacter;

    [TextArea(2, 5)]
    public string text;
}

[CreateAssetMenu(fileName = "DialogConversation", menuName = "Scriptable Objects/Dialog/Conversation")]
public class DialogConversationSO : ScriptableObject
{
    public DialogCharacterSO leftCharacter;
    public DialogCharacterSO rightCharacter;
    public Sentence[] sentences;
}
