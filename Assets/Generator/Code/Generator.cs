using UnityEngine;
using UnityEngine.SceneManagement;

using System.Collections.Generic;


/* <summary>
    @author: MonkeInBOX
    @Tutorial:
        * Rooms will need a Enter and Exit node, depending on if you can Enter or Exit in that room. 
        * These will connect together to create the dungeon layout.
        * Dungeons will be created to a new Scene during runtime, allowing you to generate and swap as needed.
        * End rooms need to be tagged in order to work, depth will be cut off at TrueMaxDepth for regen or the biggest depth when the requirments are met.

        *Generation process:
        1. Create new scene, move generator to it, set as active.
        2. Spawn start prefab, this is the root of the dungeon and where generation will start.
        3. Process rooms in a queue starting with the root and then adding all created children to the queue, for each room process all exits:
            a. If exit is already connected or is a dead end, skip it.
            b. Get candidate room types for the exit based on its allowed types, then weight them and randomly select one.
            c. Get candidate room prefabs for the selected type, then weight them and randomly select one.
            d. Attempt to place the room by aligning it to the exit and checking for overlaps, if it fails remove it from candidates (every possible type and its prefabs) and retry.
            e. If no candidates are left, place a wall on the exit.

*/











public class Generator : MonoBehaviour
{
    //public
    [Header("Generation Settings")]
    [Tooltip("Set seed to generate, if 0 then it will generate a seed before starting.")]
    public int seed = 0;
    [Tooltip("If true, the generator will create a new scene for each generated dungeon. If false, it will generate in the current scene and may cause issues with existing objects. Always set to false if you want to generate multiple dungeons in the same scene.")]
    public bool randomiseSeedAfterGeneration = true;
    [Tooltip("If true, the player will be moved to the dungeon entrance after generation. The player must have the tag 'Player' for this to work.")]
    public bool movePlayerToDungeon = true;
    [Tooltip("Generate the dungeon when the script is loaded.")]
    public bool generateOnStart = true;
    [Tooltip("The maximum depth, this is a HARD limit and will immediently stop generation if reached. Reccomended to give this a high value this with depthEqualToEndRoom.")]
    public int trueMaxDepth = 255;
    [Tooltip("If true, whenever the end room is places, the depth will be capped to its depth value. This should ensure end room generation at the cost of a inconsistent depth.")]
    public bool depthEqualToEndRoom;
    [Header("Debug Settings")]
    public bool DebugMode = true;
    [Tooltip("Enables logging")]
    public bool showGizmos = true;
    [Tooltip("Will hide room gizmos if false. (showing connections)")]
    [Header("Prefab Settings")]
    [Tooltip("The root prefab to begin generation from. It is recommended to use a new room with atleast 1 exit and 1 enter node (as the spawn node for player)")]
    public GameObject startPrefab; // This is the first prefab, it will be used to start the generation as the "root"
    [Tooltip("The prefabs that will be forced into the dungeon, along with how many min and max instances and at what depth they can start appearing. These will be attempted to be placed before any random rooms are placed.")]
    public RequiredPrefabsTypeEntry[] requiredPrefabs; // The prefabs that NEED to be included along with how many min and max instances
    [Tooltip("The prefabs that can be used in the dungeon, categorized by type and with a weight for random selection. At least one prefab needs to be assigned here for each RoomType that is allowed by the exits.")]
    public RandomPrefabsTypeEntry[] randomPrefabs; // The prefabs that can be included, along with their weight for random selection

    //private
    private System.Random rng; //no Unity random as it is static
    private int dungeonCounter = 0;
    private int spawnedRoomCounter = 0;
    private int spawnedWallCounter = 0;
    private GameObject root;
    private int requiredRoomsBiggestDepth = 0;
    private bool isRequiredRoomsMinMet = false;

    //static
    public static bool DebugModeStatic;
    public static bool showGizmosStatic;
    
    void Start() //this is simply used to kickstart generation process if flagged, can be called from anywhere however.
    {
        if (seed == 0) seed = System.DateTime.Now.Millisecond; 
        rng = new System.Random(seed);
        DebugModeStatic = DebugMode;
        showGizmosStatic = showGizmos;
        Cache.BuildCache(randomPrefabs);
        if (DebugModeStatic){ ProblemCheck(); }
        if (generateOnStart)
        {
            Generate();
        }
    }

    public void Generate()
    {

        DebugHolder.Log($"Dungeon generation started (seed={seed}, trueMaxDepth={trueMaxDepth}).");

        createNewDungeonScene();
        createStartPrefab();
        try
        {
            StartGeneration();
        }
        catch (System.Exception e)
        {
            DebugHolder.LogError($"Generation failed: {e.Message}");
            ResetGenerator();
            Generate(); //restart generation if it fails due to hitting true max depth
            return;
        }

        if (movePlayerToDungeon) MovePlayerToDungeon();



        if (randomiseSeedAfterGeneration)
        {
            seed = System.DateTime.Now.Millisecond;
            rng = new System.Random(seed);
        }
    }

    //Pre-Generation
    private void createNewDungeonScene()
    {
        //create scene, move to it, set as active
        Scene newScene = SceneManager.CreateScene("Dungeon_" + dungeonCounter++);
        SceneManager.MoveGameObjectToScene(this.gameObject, newScene);
        SceneManager.SetActiveScene(newScene);
    }

    private void createStartPrefab(Vector3 position = default, Quaternion rotation = default)
    {
        if (startPrefab != null)
        {
            root = Instantiate(startPrefab, position, rotation);
        }
        else
        {
            DebugHolder.LogError("Generation aborted: 'startPrefab' is not assigned on Generator.");
        }
    }
   
   private void ResetGenerator()
    {
        if (root != null)
        {
            Destroy(root);
            root = null;
        }
        spawnedRoomCounter = 0;
        spawnedWallCounter = 0;
        requiredRoomsBiggestDepth = 0;
        isRequiredRoomsMinMet = false;
    }

   //Generation
   private void StartGeneration()
    {
        Queue<Room> roomsToProcess = new Queue<Room>();
        roomsToProcess.Enqueue(root.GetComponent<Room>()); //add root room to processing queue
        while (roomsToProcess.Count > 0)
        {
            Room currentRoom = roomsToProcess.Dequeue();
            Room newRoom = null;
            foreach (Exit exit in currentRoom.exitNodes) //process all exits of the room
            {
                newRoom = null;
                if (currentRoom.GetDepth() >= trueMaxDepth)
                {
                    //redo the generation with new seed
                    DebugHolder.Log($"True max depth of {trueMaxDepth} reached at room '{currentRoom.gameObject.name}' depth {currentRoom.GetDepth()}. Ending generation for this branch.", currentRoom.gameObject);
                    if (!exit.GetIsConnected())
                    {
                        PlaceWall(exit);
                    }
                    if (!depthEqualToEndRoom) throw new System.Exception($"True max depth of {trueMaxDepth} reached at room '{currentRoom.gameObject.name}' depth {currentRoom.GetDepth()}. Ending generation for this branch.");
                    
                }
                if (isRequiredRoomsMinMet && currentRoom.GetDepth() > requiredRoomsBiggestDepth && depthEqualToEndRoom) //if we are at max depth, place walls on all unconnected exits and skip processing
                {
                    if (!exit.GetIsConnected())
                    {
                        PlaceWall(exit);
                    }
                    continue;
                }
                if (!exit.GetIsConnected() || exit.GetIsDeadEnd())
                {
                    //Forced Rooms
                    foreach (RequiredPrefabsTypeEntry required in requiredPrefabs)
                    {
                        DebugHolder.Log($"Evaluating required room of type '{required.roomType}' at depth {currentRoom.GetDepth()} on exit '{exit.gameObject.name}'. Current count: {required.countreached}/{required.countMax}.", exit.gameObject);
                        if (required.countreached >= required.countMax) continue;
                        DebugHolder.Log($"Processing required room of type '{required.roomType}' at depth {currentRoom.GetDepth()} on exit '{exit.gameObject.name}'. Current count: {required.countreached}/{required.countMax}.", exit.gameObject);
                        if (required.depthMax <= currentRoom.GetDepth() && required.countreached < required.countMin)
                        {
                            DebugHolder.Log($"Force placing required room of type '{required.roomType}' at depth {currentRoom.GetDepth()} on exit '{exit.gameObject.name}' because count {required.countreached} has not reached minimum {required.countMin} and depth is at or past max {required.depthMax}.", exit.gameObject);
                            TryPlaceRoom(required.prefabs[0], exit); //Attempt to force the room if at Max Depth or past it if all else fails.
                        }
                        if (required.depthMin <= currentRoom.GetDepth())
                        {
                            //Linear curve the weight upwards depending on what depth is and what the range is
                            required.currentWeight = (int)(((float)(currentRoom.GetDepth() - required.depthMin) / (required.depthMax - required.depthMin)) * 100);
                            DebugHolder.Log($"Calculated weight for required room of type '{required.roomType}' at depth {currentRoom.GetDepth()} on exit '{exit.gameObject.name}': {required.currentWeight} (depthMin={required.depthMin}, depthMax={required.depthMax}).", exit.gameObject);
                            if (required.countreached > required.countMin) {required.currentWeight = 10;}
                            if (rng.Next(0, 100) <= required.currentWeight) //Chance to place depending on how close to max depth we are in the range
                            {
                                DebugHolder.Log($"Attempting to place required room of type '{required.roomType}' at depth {currentRoom.GetDepth()} on exit '{exit.gameObject.name}' with weight {required.currentWeight}.", exit.gameObject);
                                if (newRoom == null)
                                {
                                    newRoom = TryPlaceRoom(required.prefabs[0], exit);
                                }
                                if (newRoom != null)
                                {
                                    requiredRoomsBiggestDepth = Mathf.Max(requiredRoomsBiggestDepth, currentRoom.GetDepth() + 1);
                                    required.countreached++;
                                    continue; //if we successfully placed a required room, skip to next exit
                                }
                                else
                                {
                                    DebugHolder.Log($"Failed to place required room of type '{required.roomType}' at depth {currentRoom.GetDepth()} on exit '{exit.gameObject.name}'.", exit.gameObject);
                                }
                            }
                        }
                    }
                    foreach (RequiredPrefabsTypeEntry required in requiredPrefabs)
                    {
                        if (required.countreached < required.countMin)
                        {
                            DebugHolder.Log($"Failed to meet minimum count for required room of type '{required.roomType}' at depth {currentRoom.GetDepth()}. Current count: {required.countreached}/{required.countMin}.", currentRoom.gameObject);
                            isRequiredRoomsMinMet = false;
                            break;
                        }
                        else
                        {
                            isRequiredRoomsMinMet = true;
                        }
                    }
                    if (newRoom == null)
                    {
                        newRoom = ProcessExit(exit);
                    }
                    if (newRoom != null)
                    {
                        newRoom.SetDepth(currentRoom.GetDepth() + 1);
                        roomsToProcess.Enqueue(newRoom); //add new room to queue
                    }
                }
            }
        }

    }

    //Exit Processing
    private Room ProcessExit(Exit exit)
    {
        List<RoomType> candidateTypes = GetCandidateTypes(exit);

        while (candidateTypes.Count > 0)
        {
            RoomType selectedType = SelectWeightedType(candidateTypes);
            List<GameObject> candidateRooms = GetCandidateRooms(selectedType);

            while (candidateRooms.Count > 0)
            {
                GameObject selectedRoomPrefab = SelectWeightedRoom(candidateRooms);
                Room placedRoom = TryPlaceRoom(selectedRoomPrefab, exit);
                if (placedRoom != null)
                {
                    return placedRoom;
                }

                candidateRooms.Remove(selectedRoomPrefab);
            }

            candidateTypes.Remove(selectedType);
        }

        PlaceWall(exit);
        return null;
    }

    private List<RoomType> GetCandidateTypes(Exit exit)
    {
        List<RoomType> candidateTypes = new List<RoomType>();

        foreach (RoomType type in exit.allowedTypePrefabs)
        {
            if (RoomsOfType(type).Length > 0)
            {
                candidateTypes.Add(type);
            }
        }

        return candidateTypes;
    }

    private List<GameObject> GetCandidateRooms(RoomType type)
    {
        GameObject[] rooms = RoomsOfType(type);
        List<GameObject> candidateRooms = new List<GameObject>(rooms.Length);

        for (int i = 0; i < rooms.Length; i++)
        {
            candidateRooms.Add(rooms[i]);
        }

        return candidateRooms;
    }

    private RoomType SelectWeightedType(List<RoomType> candidateTypes)
    {
        int totalWeight = 0;
        foreach (RoomType type in candidateTypes)
        {
            totalWeight += Cache.GetWeightForType(type);
        }

        if (totalWeight <= 0)
        {
            return candidateTypes[rng.Next(0, candidateTypes.Count)];
        }

        int randomValue = rng.Next(0, totalWeight);
        int cumulativeWeight = 0;
        foreach (RoomType type in candidateTypes)
        {
            cumulativeWeight += Cache.GetWeightForType(type);
            if (randomValue < cumulativeWeight)
            {
                return type;
            }
        }

        return candidateTypes[candidateTypes.Count - 1];
    }

    private GameObject SelectWeightedRoom(List<GameObject> candidateRooms)
    {
        int totalWeight = 0;
        foreach (GameObject room in candidateRooms)
        {
            totalWeight += Cache.GetWeightForRoom(room);
        }

        if (totalWeight <= 0)
        {
            return candidateRooms[rng.Next(0, candidateRooms.Count)];
        }

        int randomValue = rng.Next(0, totalWeight);
        int cumulativeWeight = 0;
        foreach (GameObject room in candidateRooms)
        {
            cumulativeWeight += Cache.GetWeightForRoom(room);
            if (randomValue < cumulativeWeight)
            {
                return room;
            }
        }

        return candidateRooms[candidateRooms.Count - 1];
    }

    private Room TryPlaceRoom(GameObject roomPrefab, Exit exit)
    {
        GameObject newRoom = Instantiate(roomPrefab);
        newRoom.name = $"Room_{spawnedRoomCounter++}";
        Room room = newRoom.GetComponent<Room>();

        if (DebugModeStatic)
        {
            DebugHolder.Log($"TryPlaceRoom: trying prefab='{roomPrefab.name}' as room='{newRoom.name}' on exit='{exit.gameObject.name}' (parentRoom='{exit.transform.root.gameObject.name}').", newRoom);
        }

        if (room == null || room.getEnterNode() == null)
        {
            Destroy(newRoom);
            spawnedRoomCounter--;
            return null;
        }

        AlignRoomToExit(room.getEnterNode(), exit);

        Collider roomCollider = newRoom.GetComponent<Collider>();
        if (roomCollider == null)
        {
            if (DebugModeStatic) DebugHolder.LogWarning($"TryPlaceRoom: room '{newRoom.name}' has no Collider, so overlap validation was skipped. Add a collider to this prefab if placement blocking is expected.");
        }

        if (roomCollider != null && checkOverlap(room, exit))
        {
            Destroy(newRoom);
            spawnedRoomCounter--;
            return null;
        }

        ConnectRooms(room.getEnterNode(), exit);
        if (DebugModeStatic)
        {
            DebugHolder.Log($"TryPlaceRoom: placed room='{newRoom.name}' at {newRoom.transform.position}, connected to exit='{exit.gameObject.name}' on parentRoom='{exit.transform.root.gameObject.name}'.", newRoom);
        }
        return room;
    }


    private void ConnectRooms(Enter enter, Exit exit)
    {
        if (enter == null || exit == null)
        {
            DebugHolder.LogError("ConnectRooms failed: missing connector reference (Enter or Exit was null).");
            return;
        }

        enter.Connect(exit);
        exit.Connect(enter);
    }

    private void AlignRoomToExit(Enter enter, Exit exit)
    {
        Transform roomRoot = enter.transform.root;

        Vector3 enterForward = Vector3.ProjectOnPlane(enter.transform.forward, Vector3.up).normalized;
        Vector3 exitForward = Vector3.ProjectOnPlane(exit.transform.forward, Vector3.up).normalized;
        float yawDelta = Mathf.DeltaAngle(
            Mathf.Atan2(enterForward.x, enterForward.z) * Mathf.Rad2Deg,
            Mathf.Atan2((-exitForward).x, (-exitForward).z) * Mathf.Rad2Deg);

        roomRoot.Rotate(0f, yawDelta, 0f, Space.World);
        roomRoot.position += exit.transform.position - enter.transform.position;
    }

    private void PlaceWall(Exit exit)
    {

        if (exit.allowedWallPrefabs == null || exit.allowedWallPrefabs.Length == 0)        {
            DebugHolder.LogWarning($"PlaceWall skipped: exit '{exit.gameObject.name}' has no allowed wall prefabs configured.");
            return;
        }
        
        
        int wallIndex = rng.Next(0, exit.allowedWallPrefabs.Length);
        GameObject wall = Instantiate(exit.allowedWallPrefabs[wallIndex], exit.transform.position, exit.transform.rotation);
        wall.name = $"wall_{spawnedWallCounter++}";
        GameObject connectionSpot = wall.transform.Find("connectionSpot")?.gameObject;
        if (connectionSpot == null)
        {
            DebugHolder.LogWarning($"PlaceWall failed: selected wall prefab is missing child 'connectionSpot' for exit '{exit.gameObject.name}'.");
            return;
        }
        
        Quaternion rotationDelta = Quaternion.FromToRotation(connectionSpot.transform.forward, wall.transform.forward);
        wall.transform.rotation = rotationDelta * wall.transform.rotation;
        wall.transform.position += wall.transform.position - connectionSpot.transform.position;

        exit.SetDeadEnd(true);
        if (DebugModeStatic)
        {
            DebugHolder.Log($"PlaceWall: sealed exit '{exit.gameObject.name}' on parentRoom='{exit.transform.root.gameObject.name}' using wall='{wall.name}'.", wall);
        }
        return;
    }

    //Misc
    private void ProblemCheck()
    {
        if (startPrefab == null)
        {
            DebugHolder.LogWarning("ProblemCheck: 'startPrefab' is missing. Generation cannot begin without a root room.");
        }
        if (requiredPrefabs == null || requiredPrefabs.Length == 0)
        {
            DebugHolder.LogWarning("ProblemCheck: 'requiredPrefabs' list is empty.");
        }
        if (randomPrefabs == null || randomPrefabs.Length == 0)
        {
            DebugHolder.LogWarning("ProblemCheck: 'randomPrefabs' list is empty.");
        }
    }
    private GameObject[] RoomsOfType(RoomType type)
    {
        foreach (RandomPrefabsTypeEntry entry in randomPrefabs)
        {
            if (entry.roomType == type)
            {
                GameObject[] prefabs = new GameObject[entry.prefabs.Length];
                for (int i = 0; i < entry.prefabs.Length; i++)
                {
                    prefabs[i] = entry.prefabs[i].prefab;
                }
                return prefabs;
            }
        }
        return new GameObject[0]; // Return empty array if type not found
    }
    private void MovePlayerToDungeon()
    {
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player != null && root != null)
        {
            player.transform.position = root.GetComponent<Room>().getEnterNode().transform.position + Vector3.up * 2.5f; // Move player slightly above the floor to avoid falling through
            player.transform.rotation = root.GetComponent<Room>().getEnterNode().transform.rotation;

        }
        else
        {
            DebugHolder.LogWarning("MovePlayerToDungeon skipped: missing Player (tag='Player') or generated start room root.");
        }
    }
    private bool checkOverlap(Room newRoom, Exit ignoreExit)
    {
        if (newRoom == null) return false;
        newRoom.setParent(ignoreExit.transform.root.GetComponent<Room>());
        return newRoom.isOverlapping();
    }
}
