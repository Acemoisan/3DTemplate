/*
 *  Copyright © 2022 Omuhu Inc. - All Rights Reserved
 *  Unauthorized copying of this file, via any medium is strictly prohibited
 *  Proprietary and confidential
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "DialogCharacter", menuName = "Scriptable Objects/Dialog/Character")]
public class DialogCharacterSO : ScriptableObject
{
    public string displayName;
    public Sprite portrait;
}
