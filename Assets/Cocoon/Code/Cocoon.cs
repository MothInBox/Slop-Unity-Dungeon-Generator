using UnityEngine;
using UnityEngine.SceneManagement;

public class Cocoon : MonoBehaviour
{
    [Header("Dungeon Generation Settings")]
    [Tooltip("The seed used to generate the dungeon. If 0, a random seed will be generated.")]
    [SerializeField]public long seed = 0; 
    [Tooltip("The MAXIMUM depth of the dungeon. Higher values will create a larger and more complex dungeon. read setEndRoomToDepth tooltip to understand why this is \"true\". Values of 0 to 255.")]
    [SerializeField][Range(0, 255)]public byte trueDepth = 0;
    [Tooltip("If true, the end room will be set to the depth specified in trueDepth. If false, the end room will be placed at a random depth up to trueDepth.")]
    [SerializeField]public bool setEndRoomToDepth = false; 

    [Header("Room Customization")]
    [Tooltip("The type of room. This can be used customize generation behavior slightly. See RoomType.cs to edit, add or remove room types.")]
    [SerializeField] RoomSettings roomSettings;

    [Header("Additional Settings")]
    [Tooltip("If true, the dungeon will be generated when the scene starts. Otherwise call the Generate() Method in this class from another script.")]
    [SerializeField]public bool generateOnStart = false;
    [Tooltip("Moves player to start room and scene after generation")]
    [SerializeField]public bool movePlayerToStartRoom = true;








    void Start()
    {
        
    }
}
