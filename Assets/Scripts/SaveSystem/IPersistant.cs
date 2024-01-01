using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IPersistant
{
    public void LoadData(SaveData data, SceneData sceneData);
    public void SaveData(ref SaveData data, ref SceneData sceneData); //use ref because when we save we want the implementing script to modify the data
}
