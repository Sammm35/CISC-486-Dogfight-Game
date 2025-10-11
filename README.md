# FSM explanation

## State 1: Attack state (starting state)
NPC will aim at and shoot the player.

### Transitions:
- **1 → 2**: If the NPC flies too close to the player.  
- **1 → 2**: If the attack phase has lasted too long (random time from 6–16 seconds).  
- **1 → 3**: If there is an item close by and the attack phase has lasted too long (random 4–14 seconds).  
- **1 → 4**: If the NPC is too far away from the center.  
- **1 → 4**: If the NPC is too close to hitting the ground (taking into account distance as well as pitch).

---

## State 2: Strafe state
NPC will evasively strafe away from the player.

### Transitions:
- **2 → 3**: If there is a close item and the strafe phase has lasted a random time from 0–5 seconds.  
- **2 → 1**: If the strafe phase has lasted too long (random 2–10 seconds).

---

## State 3: Item collection state
NPC will move towards an item.

### Transitions:
- **3 → 1**: If the item no longer exists (collected or despawned).  
- **3 → 2**: If at least 6 seconds have passed since the phase started and the player is aiming close to the NPC.

---

## State 4: Avoid border state
NPC will move back towards the center of the map to avoid the border.  

**Note:** Only state 1 can transition to this state because it's the only state where the NPC can pathfind close to the border.

### Transitions:
- **4 → 1**: Random time from 2–5 seconds.  

---

# CISC 486 Game Proposal

## Game Title
Dogfight

## Core gameplay

### Core ideas
- The game is 1 vs. 1 with the option of playing against either an AI or another player.  
- Each player will be in control of a fighter jet which can shoot bright lasers. Each plane starts with a limited number of lives and every time a player successfully lands a shot that hits the opponent’s plane, the opponent loses 1 life. The goal is to eliminate the opposing player by firing lasers at them until they lose all of their lives.

### Specifics
- The players will be contained in a non-infinite open area to prevent players from just running away.  
- The player will constantly be moving forwards and can turn by holding down the arrow key that corresponds to the direction they want to turn.  
- The number of lives each player has will be displayed somewhere on the screen.  
- Powerups will occasionally spawn which can be collected by flying into them with your plane. This can include extra lives or other powerups that will disappear after a little while.  
  - E.g. faster turning speed, different firing modes and whatever else seems reasonable to code in once I get there.  
- My current plan is for the game to be a 3rd person view of the plane with a key bind that, when held down, allows the player to briefly switch to a 2nd person view allowing them to see behind.  
- If the player crashes by either colliding into each other or into the world border, they will lose several lives.

## Game type
Arena Battle

## Player Setup
- Local (singleplayer) to play against an AI opponent.  
- Online multiplayer to play against another player.

## AI Enemies
- The idea is to have the AI enemy mimic decisions and movements that a real player would do. This includes 4 main behaviours:  
  1. Attempting to aim at and shoot at the enemy plane.  
  2. Steer clear from the world borders to not crash.  
  3. Fly into powerups.  
  4. Attempt to be somewhat evasive if the player is aiming at them.  
- The AI will prioritize which behaviour it should take depending on certain variables in the current world state. E.g. current position, enemy position, direction aimed, distance from powerup…  
- Additionally, I might decide to throw in a randomly generated number into the variables to allow the AI to be a bit less predictable.

## Scripted events
- Powerups spawning occasionally throughout the world. Maybe every 30 seconds, this number is heavily subject to change.  
- Some kind of end screen when the game has been won/lost.

## Environment
- The game will take place in the sky, on a sunny day, with a nice green field below it.  
- Some kind of cage will be placed around for the walls and ceiling border. I don’t want to get complicated at all for this part since it’s just me working on this.

## Basic Planning Factors

### Assets
- I really don’t need a whole lot of assets (grass, sun, planes, laser). The Unity store certainly has plenty of free assets for these needs.

### Team information
- Individual, Group 15, Sam Tylman #20349914
