EndlessRunnerGame is a 3D game where a player is constantly moving forward while collecting points and avoiding obstacles. The level repeats itself infinitely, gradually increasing difficulty, until the player collides with the obstacle. 
This game is made with Unity 2021.3.6f1 for Android platforms. Tested on Android 8, 12 with all display resolutions. All models were downloaded from 3D stocks for free. 

Program description
After the game starts, user controls the player (cat) by swiping left and right to change the traffic lane, up to jump over the barrier and down to slip under the barrier. The longer the game lasts, the more points the player gets. Points are made up of playing time and the number of collected coins, which in this case are sushi eaten by the cat. As soon as the player crashes into an obstacle, the game ends. The user can repeat the game or quit. 

Program structure
The scripts for the game are written in C# programming language. The program is divided into six parts, which are C# classes that represent and control behavior of individual parts of the game, such as: level generation, camera control, player control, event handler and some objects control..
Detailed functions are listed below. 

- GameManager.cs
The class that controls the game process: whether the game has started, what is the current score, whether the player has lost. 

- LevelGenerator.cs
This class generates a road with random obstacles to create a unique level. Actually, the road does not lengthen infinitely. At a particular time, there is only a constant number of road parts. As soon as the player passes them and they disappear from the screen, these parts are removed. 

- PlayerController.cs
This class controls player's movement. The speed increases with each update of the screen and the new position is calculated. The script contains a swipe controller for mobile devices, which is used by default, but it is possible to control the player with keyboard arrows. If the player collides with an obstacle, he stops. 

- CameraController.cs
The main camera's class, which ensures that the camera follows the player. 

- Coin.cs
This class defines behavior of the coin. When a player encounters the coin, it disappears and the score increases. 

- Events.cs
Functions of this class are called in response to events, that are triggerred by button click. 

What can be added to the game to improve it:
- More diverse obstacles and environment
- Hight score
- Smoother player movement - for now it simply "teleports" to a new position
- Sound effects and music 

Links and resourses:
- [Official Unity documentation and manual: ](https://docs.unity3d.com/Manual)
- [3D models: ](https://free3d.com ; https://sketchfab.com)
