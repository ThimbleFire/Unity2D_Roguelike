# Roguelike

This is an untitled roguelike project. It features turn based movement and combat. It is being developed for Android.

## TODO

Add a MapEditor. The map editor stores a directory to a list of chunks and enemies suitable for that map. It also stores a path to the next and previous map. 
    Add Area Level to the map editor

When the player character creates their character, it creates a new save file. The map name is saved in this. When the gameplay scene is opened, board manager reads the saved data and loads the character and the map. Rooms create enemies specified in the mapdata. Map data also has a previous and next string which is the path to the previous and next level. When loading a level, the seed is saved so if the player returns to that level, the map layout persists.
However in Diablo 2 fasion, we might conider purging this list of seeds when the game is closed.

## Loading

BoardManager calls Game.LoadSession. This loads the player's game save.
It gets the area level the player was last in.
This area level is used by ResourceRepository to load the correct enemies when BoardManager builds the map.

## Combat

### Block

Enemys with shields have a static 35% chance to block. The player character block rate is determined by their shield. Shield block rates range between 25% and 55%, and can be further enhanced with affixes on the shield.
Blocking can reduce incoming damage up to 100%.
Upon blocking, entities can not block again for 5 turns. This number can be reduced with block recovery affixes.

### Evasion

Evasion results in the attacker missing a melee or physical-ranged attack.

### Magic

Not yet implemented

### Physical Projectiles (ranged)

Not yet implemented

### Melee

Stand adjacent to an aggressor, select them, then hit the sword icon to perform a melee attack.

## Spooky darkness

![image](https://media.discordapp.net/attachments/237335760905306112/1057809237205590118/drag.gif)

## Item Editor

![image](https://cdn.discordapp.com/attachments/237335760905306112/1069889145792626740/image.png)

##Inventory Stats

![image](https://cdn.discordapp.com/attachments/362981245929652228/1070730167011512320/Screenshot_20230202-153819.jpg)
![image](https://cdn.discordapp.com/attachments/362981245929652228/1070730166793424926/Screenshot_20230202-153822.jpg)
