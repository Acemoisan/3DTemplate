// using UnityEngine;
// using UnityEditor;

// [ExecuteInEditMode]
// public class SceneCameraFollower : MonoBehaviour
// {
//     public Transform target;  // Target object to follow

//     void Update()
//     {
//         if (target == null)
//             return;

//         // This code only executes in the Unity Editor (not in builds of the game)
//         if (Application.isEditor && !Application.isPlaying)
//         {
//             // Get the current scene view camera
//             SceneView sceneView = SceneView.lastActiveSceneView;
//             if (sceneView != null)
//             {
//                 // Set the scene view camera to follow the target's position and rotation
//                 sceneView.LookAt(target.position, target.rotation);
//             }
//         }
//     }
// }
