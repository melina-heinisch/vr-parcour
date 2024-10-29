## VR Parcour Game
A video demonstration can be found [here](https://youtu.be/3ZPCepzHZYY?si=h4tKZsqJTyHpxGA2).
## Implemented Optional Requirements:

https://lectures.hci.informatik.uni-wuerzburg.de/ss23/3dui/modules/additional-resources/project-requirements-summer-page.html
- O3: The player should be able to save their performance by entering their name and seeing previous players’ performances.	(25P)
- O4: The player should be able to pick up one tool to help them to finish. – e.g. grappling hook, teleportation gun. (25P)

## Supported user interactions
- walking
- running
- rotating
- jumping
- teleportation
- open/close help menu
- interaction with ui, i.e. navigation through help menu and scoreboard with virtual keyboard

## Start the application
Open the project in Unity 2021.3.11f1. Navigate to `Assets/3DUI/scenes/final-scene.unity` and press play in the Unity editor.

## Implementation

### M8: Time-Challenge Game Mode
There is a level that must be completed within a specific time frame. If a player falls off a platform or runs out of time, they lose the game. Upon reaching the goal, the player can review their own performance.

### M9: Minimalistic virtual body
The minimalistic virtual body comprises two hand models. On the left wrist, there is a watch that displays the remaining time.

### M10: Techniques to simulate the feeling of walking, running and jumping
#### Walking / Running (like [this](https://youtu.be/o3fobq9Ncqw?feature=shared))

Users can activate the ability to walk or run by simply pressing either of the grip buttons. In order to start moving, the user has to swing the controllers up and down. To promote user engagement and reduce the risk of visually induced motion sickness, the controller movement along the x and y axes is used to control the movement acceleration. This is intended to resemble an "Arm Swinging Steering" mechanism, like "Walking in Place."
Slow arm movement speed results in walking while faster speeds will result in running.

#### Jumping:

To jump use one trigger button. Double jump is possible by pressing both trigger button / double pressing on one trigger.

### M11: Automatic restart when losing / winning
If the player loses the game, the level restarts. If the player wins, they have 10 seconds to decide whether they want to save their performance (as per O3). After the expiration of these 10 seconds, the level restarts.

### M12: Help menu
The player can use the menu button on the left controller to display a help menu inside the virtual environment that gives an overview of all interactions and how to realise them. It can be opened at any time and place. If opened, the game will pause and resume on closing the help menu. Additionally, all objects that are between the player and the help menu and are occluding it will be hidden to ensure that the help menu is always fully visible. The content of the help menu is easily extendible for designers in case new interactions/game mechanics are added.

### O3: Save the performance
At the goal, the player has the option to save their performance. If they choose to do so, a keyboard with a QWERTZ layout appears, allowing them to enter a name by using the raycast on the right hand and the right trigger button. Once the name is confirmed, if it ranks within the top 10 scores, it will appear in the highscore list. All entries are saved persistently on the machine the game is running on.
The keyboard can be moved by using the thumbstick of the left controller to give the users the option to reposition the keyboard. Furthermore the keyboard is implemented to be easily used in other contexts as well and is not hard-linked to the result panel.

### O4: pick up one tool: teleportation gun
To pick up the teleportation gun, press the right grip button. Using a curved raycast, the player can choose the target in vista space scope. Teleportation is only possible within the currently visible area. Hold down the right trigger button to display pre-travel information with an Orientation Widget. Upon releasing the trigger button, the player will teleport with fast motion to the selected target.

## Performance
The framerate is constantly above 70 FPS, see here: https://gitlab2.informatik.uni-wuerzburg.de/hci/students/3dui/ss23/ss23-3dui-project-team-1/-/blob/310fcb124d80372720e1d4cd1aac2f0c07045534/Assets/video/FPS_Test.mp4  
The FPS display can be activated with the secondary button on the left controller.

## Precision
To give the player full control, the movement has to be activated with the grip button and even small movement is possible.  
The player has 3 different options to rotate: the direction of the controller, Snap Rotation and Smooth Rotation on right thumbstick. The player can switch between snap and smooth rotation by pressing the thumbstick. Snap Rotation is set as default. Rotation through the controller is included as during testing, some players found it easier to navigate using it than turning their head/body.

## Safety
In our small user study with 5 participants (including external without HCI background), no motion-sickness occured. Even one participant who is susceptible to motion-sickness, didn't experience motion-sickness when playing our parcour game.

## Usability
To increase the usability, we provide a detailed help menu. In our small user study, the participants learned the interactions after a brief practice phase and handled them well.  
We provide visual, audio and or haptic feedback, e.g. outline for teleportation gun when grabbable, sound when jumping / teleporting / start and goal barrier, vibration of controller by teleporting / walking through walls

## Readability
We checked the UI with this [Contrast Checker](https://webaim.org/resources/contrastchecker/). We have the following contrast ratios:
- 21:1 (black:white) on HelpMenu, ScoreBoard, ResultPanel, Keyboard, Time Remaining, Game Over Panel, Watch
- 6.8:1 (blue:white) on ResultPanel

## User Experience
Some feedback from our small user study: it is fun, challenging (even though some passages of the level may be harder, users said they perceived it as motivating to overcome these challenges), easy to learn


## Sources

- [Blue Button](https://www.pngall.com/button-png/) by Rojal <sup>1</sup>
- [Progress Bar Circle](https://drive.google.com/file/d/16xcdTMNiWnT3nFQF9Vq_QZq91_m6JgT7/view) from [this Video](https://youtu.be/2MSMmPWedyg) - no license information available
- [Rounded Square](https://www.flaticon.com/free-icon/square-of-rounded-corners_44773) and [Black Circle](https://www.flaticon.com/free-icon/black-circle_14) by Freepik - no license information available
- [Skybox](https://assetstore.unity.com/packages/2d/textures-materials/sky/skybox-series-free-103633)<sup>2</sup> by "Avionx"
- [Letter in Circle (R and L)](https://www.flaticon.com/packs/alphabet-a-to-z-52) <sup>4</sup> by shohanur.rahman13 on Flaticon
- [Space Background Sound](https://filmmusic.io/song/10077-space-exploration)<sup>1</sup> "Space Exploratioon" by WinnieTheMoog"
- [Start Sound](https://mixkit.co/free-sound-effects/goal/)<sup>5</sup> "Achievement completed" by Mixit
- [Winning Sound](https://mixkit.co/free-sound-effects/win/)<sup>5</sup> "Small Win" by Mixit
- [Losing Sound](https://pixabay.com/de/sound-effects/negative-beeps-6008/)<sup>6</sup> "Negative Beep" by Pixabay
-  [Jumping Sound](https://pixabay.com/de/sound-effects/cartoon-jump-6462/)<sup>6</sup> "Cartoon jump" by Pixabay
- [Easter Egg Sound](https://pixabay.com/de/sound-effects/cat-call-meow-102607/)<sup>6</sup> "Cat Call (Meow)" by Pixabay
- [Time Penalty Sound](https://mixkit.co/free-sound-effects/wrong/)<sup>6</sup> "Tech break fail" by Mixit
- [Keyboard Press Sound](https://mixkit.co/free-sound-effects/keyboard/)<sup>6</sup> "Hard single key press in a laptop" by Mixit
- [Quick Outline](https://assetstore.unity.com/packages/tools/particles-effects/quick-outline-115488)<sup>2</sup> by "Chris Nolet"

<sup>1</sup> [Creative Commons Attribution-NonCommercial 4.0 International](https://creativecommons.org/licenses/by-nc/4.0/)  
<sup>2</sup> Unity Asset Store - Extension Asset: One license required for each individual user  
<sup>3</sup> Free License of Freepik  
<sup>4</sup> Free License of Flaticon  
<sup>5</sup> Mixkit Sound Effects Free License  
<sup>6</sup> Pixabay Free License  
