using UnityEngine;
using UnityEngine.SceneManagement;

public class Cocoon : MonoBehaviour
{
    [Header("Dungeon Generation Settings")]
    [Tooltip("The seed used to generate the dungeon. If 0, a random seed will be generated.")]
    [SerializeField]private long seed = 0; 
    [Tooltip("The MAXIMUM depth of the dungeon. Higher values will create a larger and more complex dungeon. read setEndRoomToDepth tooltip to understand why this is \"true\". Values of 0 to 255.")]
    [SerializeField][Range(0, 255)]private byte trueDepth = 0;
    [Tooltip("If true, the end room will be set to the depth specified in trueDepth. If false, the end room will be placed at a random depth up to trueDepth.")]
    [SerializeField]private bool setEndRoomToDepth = false; 

    [Header("Room Customization")]
    [Tooltip("The type of room. This can be used customize generation behavior slightly. See RoomType.cs to edit, add or remove room types.")]
    [SerializeField] private RoomSettings roomSettings;

    [Header("Additional Settings")]
    [Tooltip("The prefix used for naming the generated dungeon scene. The final scene name will be this prefix followed by a unique identifier.")]
    [SerializeField] private string DungeonPrefix = "Cocoon_Dungeon_";
    [Tooltip("If true, the dungeon will be generated when the scene starts. Otherwise call the Generate() Method in this class from another script.")]
    [SerializeField]private bool generateOnStart = false;
    [Tooltip("Moves player to start room and scene after generation")]
    [SerializeField]private bool movePlayerToStartRoom = true;

    private CocoonCache cache;
    private Scene currentDungeon;
    private Room startRoom;
    private Room endRoom;

    void Start()
    {
        //Async dungeon generation
        if (generateOnStart)
        {
            Generate();
        }
    }

    private void activateScene(Scene scene)
    {
        this.currentDungeon = scene;
        SceneManager.SetActiveScene(scene);
    }

    private void movePlayerToStart()
    {
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player != null)
        {
            //Get start room Enter position and rotation
            Entry startEntry = startRoom.getEntry();
            //Detach any possible parents idk if that would cause issues
            player.transform.SetParent(null); 
            //move player to dungeon:
            SceneManager.MoveGameObjectToScene(player, currentDungeon);
            player.transform.SetPositionAndRotation(startEntry.GetTransform().Item2, startEntry.GetTransform().Item1);
            CocoonLogger.LogInfo("Player moved to start room at position: " + player.transform.position + " and rotation: " + player.transform.rotation + " in scene: " + currentDungeon.name, 4, "Cocoon", "Player");
        }
        else
        {
            CocoonLogger.LogWarning("Player object with tag 'Player' not found. Cannot move player to start room.", 2, "Cocoon", "Player");
        }
    }

    private void Generate()
    {
        try
        {
            if (seed == 0)
            {
                seed = CocoonUtility.RandomizeSeed(seed);
                CocoonLogger.LogInfo("Generating random seed.", 4, "Cocoon", "Generation");
            }
            CocoonLogger.LogInfo("Starting Dungeon Generation with seed: " + seed, 3, "Cocoon", "Generation");
            //Create new Scene and activate it
            activateScene(CocoonUtility.createScene(DungeonPrefix + seed));
            //Initialize cache
            cache = new CocoonCache();
            //place start room at 0,0,0
            startRoom = CocoonUtility.placeRoom(roomSettings.getStartRoomPrefab(seed), Vector3.zero, Quaternion.identity);
            if (movePlayerToStartRoom){movePlayerToStart();}
        } catch (System.Exception ex)
        {
            CocoonLogger.LogException(ex, 1, "Cocoon", "Exception");
        }
    }
}
