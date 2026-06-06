
#  Slop-Unity-Dungeon-Generator

A gridless dungeon generator for unity, shouldnt be overly difficult to use and i tried to make it easily integratable into anything. It stiches prefabs together via enterance and exits, given a variety of params; depth, required rooms, seed and your list of prefabs.

p.s the prefabs were made using URP lighting.

## How to use:
To begin, make some Room Types, by editing the enum.
Then make some prefabs for the generator! These will need 1 enter and any number of exit nodes with the corresponding enter.cs and exit.cs, and the root of the prefab. should have Room.cs. In Room.cs ensure you have attached all Exit Nodes and the Enterance node. Feel free to look at the included prefabs.
We will also need a start / root prefab that has a enter node to work as a player spawnpoint and  1 or more exits.
Create a empty node in your scene and attach the Generator Script. Take a look through the settings, but importantly ensure to attach the start prefab, and add your prefabs to randomly generate in Random Room Settings.
Now call Generate(); in the Generator.cs script or tick generateOnStart.
The generator will create a new scene for the dungeon.

## How does it work?
The foundation of the system works in a very simple manner. Start with a room and branch out through all of its exits nodes (adding them to a queue to be processed). Deque next exit and Weight room types then rooms and attempt to place the random rooms enterance node on the current exit node, if there is overlap try something else, if there are no possible rooms, wall the exit, finally Que all created exits.
There are of course, other edge cases such as creating a wall when max depth has been reached and some settings like capping depth at the end room.

This creates the basis of the procedural generation, but I wanted to be able to include rooms that MUST be generated. This uses a linear curve over the depth it is supposed to spawn, meaning as we approach the max depth the chance of spawning is higher. If we dont have the rooms at the end, generation must be re-done, this will happen over 25 possible attempts by default (configurable) but with my setup, I havent seen it go past 1 retry.


## Screenshots

![App Screenshot](docs/images/seed:603.png)


## Lessons Learned

- Asynchronous Programming for generation,
- Procedural Generation using Random Number generation and weightings

## Future Improvements
 - Optimise types (a weight could be a byte instead of an int), as this would take up negligble memory per prefab in comparison
 - Groupings to minimise collision checking (Group rooms with same sizes / collisions (to avoid collision retry abuse) taking into account orientation and with cumulative weights of inside prefabs & its own limits)
 - Allow certain types of exits and enterances (Ladder or Large door or Small door) to allow for varied passages. Could also be used in a fairly hacky way to fill gaps (say large door but just fill gap to make small)
 - Regenerate button during debugging
 - Make exit and enter nodes into prefabs to be placed in and a template room prefab, for ease of use.
 - implement backtracking when the end room cannot be placed, instead of regenerating, though the chances are slim with the current system.

## Authors

- [@MothInBox](https://github.com/MothInBox)

