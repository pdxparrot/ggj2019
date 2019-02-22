# This document is pretty out of date already :(

# Notes

* USE_SPINE in `Project Settings -> Player -> Other Settings -> Configuration -> Scripting Define Symbols` to enable Spine utilities
* UNITY_POST_PROCESSING_STACK_V2 to enable PostProcessingStack v2

# Script Execution Order
* pdxpartyparrot.{project}.Loading.LoadingManager
* pdxpartyparrot.Core.Util.TimeManager
* pdxpartyparrot.Game.State.GameStateManager
* Default Scripts
* pdxpartyparrot.Core.Debug.DebugMenuManager must run last

# Pre-Asset Setup

* Create Assets/csc.rsp
* Copy Art/Core/pdxparrot.png
* Copy Art/Core/progress.png
  * Texture Type: Sprite (2D and UI)
* Create Data/Audio/main.mixer
  * 3 Master child groups
    * Music
      * Expose the Volume parameter and set it to -5db
        * Rename it to MusicVolume
    * SFX
      * Expose the Volume parameter and set it to 0db
        * Rename it to SFXVolume
    * Ambient
      * Expose the Volume parameter and set it to -10db
        * Rename it to AmbientVolume
  * Expose the Master Volume parameter and set it to 0db
    * Rename it to MasterVolume
* Create Data/Physics/Frictionless.physicMaterial
  * Static Friction: 0
  * Dynamic Friction: 0
* Create Data/Physics/Frictionless 2D.physicsMaterial2D
  * Friction: 0
* Create Data/Input/ServerSpectator.inputactions
  * Generate C# Class
    * File: Assets\Scripts\Game\Input\ServerSpectatorControls
      * Need to create containing directory first
    * Class Name: ServerSpectatorControls
    * Namespace: pdxpartyparrot.Game.Input
    * Generate Events
    * Generate Interfaces
  * Action Maps
    * ServerSpectator
      * Actions
        * move forward
          * press and release w
        * move backward
          * press and release s
        * move left
          * press and release a
        * move right
          * press and release d
        * move up
          * press and release space
        * move down
          * press and release left shift
        * look
          * mouse delta

# Project Setup

* Audio Settings
* Editor Settings
  * Version Control Mode: Visible Meta Files
  * Asset Serialization Mode: Force Text
  * Default Behavior Mode: 3D for 3D, 2D for 2D
  * Line Endings: Windows (or maybe Unix would be cleaner?)
* Graphics Settings
  * Set the Render Pipeline Asset if desired (https://github.com/Unity-Technologies/ScriptableRenderPipeline)
    * This will require creating the asset first, which itself may be configured as desired
* Input Settings
* Tags and Layers
  * Add a PostProcessing layer if it doesn't already exist
  * Add a NoPhysics layer
  * Add a Vfx layer
  * Add a Viewer layer
  * Add a Player layer
  * Add a World layer
  * Add a Weather layer
* Physics Settings
  * Set the Default Material to frictionless if desired
  * Only enable the minimum necessary collisions
    * **TODO:** water?
    * Vfx -> Vfx
    * Viewer -> Weather
    * Viewer -> World
    * Player -> Weather
    * Player -> World
    * World -> Weather
* Physics 2D Settings
  * Set the Default Material to frictionless if desired
  * Only enable the minimum necessary collisions
    * **TODO:** water?
    * Vfx -> Vfx
    * Viewer -> Weather
    * Viewer -> World
    * Player -> Weather
    * Player -> World
    * World -> Weather
* Player Settings
  * Set the Company Name (PDX Party Parrot)
  * Set the Product Name
  * Set the Default Icon (Art/Core/pdxparrot.png)
  * Set any desired Splash Images/Logos
  * Color Space: Linear (or Gamma if targeting old mobile/console platforms)
    * Fix up any Grahics API issues that this might cause (generally this means disabling Auto Graphics APIs on certain platforms)
  * Enable Multithreaded Rendering on platforms that support it
  * Enable Static and Dynamic Batching
  * Set the Bundle Identifier
  * Scripting Runtime: .NET 4.x
  * Scripting Backend: IL2CPP
  * API Compatability Level: .NET Standard 2.0
  * Active Input Handling: Both
    * Whenever the new InputSystem handles UI, this can be set to just InputSystem 
  * Minimum Android API: Marshmallow
* Preset Manager
* Quality
* Script Execution Order
* TextMesh Pro
  * Import TMP Essentials if not already done
  * Optionally import TMP Examples & Extras if desired
* Time Settings
* VFX Settings

# Packages

* Update default packages
* Add release packages
  * Asset Bundle Browser
  * Cinemachine
  * Post Processing
  * ProBuilder
* Add preview packages
  * Input System (https://github.com/Unity-Technologies/InputSystem)
  * ProGrids
  * Render-Pipelines.Core
  * HD/Lightweight Render Pipeline (whichever best fits the project)
  * Burst/Entities (if using ECS)
  * Shader Graph
* Add desired assets
  * ConsoleE (optional now that Unity has built-in recompile settings)
  * DOTween (not Pro)
    * Make sure to run the setup
* Add Keijiro Kino
  * Add "jp.keijiro.kino.post-processing": "https://github.com/keijiro/kino.git#upm" to package manifest.json dependencies

# Asset Setup

* Data/Prefabs/Input/EventSystem.prefab
  * Create using default EventSystem that gets added automatically when adding a UI object
* Scripts/Core/com.pdxpartyparrot.Core.asmdef
  * References: Unity.InputSystem, Unity.Postprocessing.Runtime, Unity.TextMeshPro, Kino.Postprocessing
* Scripts/Core/Editor/com.pdxpartyparrot.Core.Editor
  * Editor platform only
  * References: com.pdxpartyparrot.Core.asmdef
* Scripts/Game/com.pdxpartyparrot.Game.asmdef
  * References: com.pdxpartyparrot.Core.asmdef, Unity.InputSystem, Unity.TextMeshPro
* Scripts/Game/Editor/com.pdxpartyparrot.Game.Editor
  * Editor platform only
  * References: com.pdxpartyparrot.Game.asmdef
* Scripts/{project}/com.pdxpartyparrot.{project}.asmdef
  * References: com.pdxpartyparrot.Core.asmdef, com.pdxpartyparrot.Game.asmdef, Unity.InputSystem, Unity.TextMeshPro

## Manager Prefabs Setup

* Managers go in Data/Prefabs/Managers
* AudioManager
  * Create an empty Prefab and add the AudioManager component to it
  * Attach the main mixer to the prefab Mixer
  * Add 4 Audio Sources to the prefab
    * Disable Play on Awake
  * Attach each audio source to an audio source on the AudioManager component
* DebugMenuManager
  * Create an empty Prefab and add the DebugMenuManager component to it
* EngineManager
  * Create an empty Prefab and add the PartyParrotManager component to it
  * Attach the frictionless physics materials
  * Set the UI layer to UI
* InputManager
  * Create an empty Prefab and add the InputManager component to it
  * Attach the EventSystem prefab
* NetworkManager
  * Create an empty Prefab and add the (not Unity) NetworkManager component to it
  * Uncheck Don't Destroy on Load
* ObjectPoolManager
  * Create an empty Prefab and add the ObjectPoolManager component to it
* SceneManager
  * Create an empty Prefab and add the SceneManager component to it
* ViewerManager
  * Create an empty Prefab and add the ViewerManager component to it

## LoadingManager and GameStateManager Prefabs

* Create a new LoadingManager script that overrides Game LoadingManager
* Create a new GameStateManager script that overrides Game GameStateManager
  * Implement the ShowLoadingScreen/UpdateLoadingScreen methods to call the LoadingManager methods
* Add a connection to the GameStateManager in the LoadingManager
  * Override CreateManagers() in the loading manager to create the GameStateManager prefab
  * Override OnLoad() in the loadin gmanager to have the GameStateManager transition to the initial state
* Create an empty Prefab and add the GameStateManager component to it
* Create a new MainMenuState script that overrides the Game GameState
* Create an empty Prefab and add the MainMenuState component to it
  * Set the Scene Name to main_menu
* Set the MainMenuState as the Initial Game State Prefab in the GameStateManager
  * Uncheck Make Scene Active

## UIManager Prefab

* Create a new UIManager script that overrides Core UIManager
* Create an empty Prefab and add the UIManager component to it
* Add the UIManager to the LoadingManager

# Splash Scene Setup

* Create and save a new scene (Scenes/splash.unity)
  * The only object in the scene should be a Main Camera
    * Clear Flags: Solid Color
    * Background: Opaque Black
    * Culling Mask: Nothing
    * Projection: Orthographic
    * Uncheck Occlusion Culling
    * Uncheck Allow HDR
    * Uncheck Allow MSAA
    * Leave the Audio Listener attached to the camera for audio to work
  * Add the AspectRatio component to the camera
  * Remove the Skybox Material
  * Environment Lighting Source: Color
  * Disable Realtime Global Illumination
  * Disable Baked Global Illumination
  * Disable Auto Generate lighting
* Add the scene to the Build Settings and ensure that it is Scene 0
* Add a new GameObject to the scene (SplashScreen) and add the SplashScreen component to it
* Attach the camera to the Camera field of the SplashScreen component
* Add whatever splash screen videos to the list of Splash Screens on the SplashScreen component
* Set the Main Scene Name to match whatever the name of your main scene is
  * The main scene should also have been added to the Build Settings along with any other required scenes

# Main Scene Setup

* Create and save a new scene (Scenes/main.unity)
  * The only object in the scene should be a camera
* Setup the camera in the scene
  * Clear Flags: Solid Color
  * Background: Opaque Black
  * Culling Mask: Nothing
  * Projection: Orthographic
  * Uncheck Occlusion Culling
  * Uncheck Allow HDR
  * Uncheck Allow MSAA
  * Leave the Audio Listener attached to the camera for audio to work
* Environment Lighting Source: Color
* Disable Realtime Global Illumination
* Disable Baked Global Illumination
* Disable Auto Generate lighting

## Loading Screen Setup

* Add a new Canvas object (LoadingScreen) to the scene
  * UI Scale Mode: Scale With Screen Size
  * Reference Resolution: 1280x720
  * Match Width Or Height: 0.5
  * Remove the Graphic Raycaster
  * Add the LoadingScreen component
  * Remove the EventSystem object that gets added (or turn it into a prefab if that hasn't been created yet)
* Add a Panel under the Canvas
  * Disable Raycast Target
  * Color: (255, 0, 255, 255)
* Add a TextMeshPro - Text (Name) under the Panel
  * Text: "Placeholder"
  * Center the text
  * Disable Raycast Target
* Add an Empty GameObject (Progress) under the Panel and add the ProgressBar component to it
  * Pos Y: -125
* Attach the ProgressBar component to the LoadingScreen component
* Add an Image under the Progress Bar (Background)
  * Move the image below the Name text
  * Color: (0, 0, 0, 255)
  * Size: (500, 25)
  * Source Image: Core Progress Image
  * Disable Raycast Target
* And an Image under the Background Image (Foreground)
  * Position: (0, 0, 0)
  * Size: (500, 25)
  * Source Image: Core Progress Image
  * Disable Raycast Target
  * Image Type: Filled
  * Fill Method: Horizontal
  * Fill Origin: Left
  * Fill Amount: 0.25
* Attach the images to the ProgressBar component
* Add a TextMeshPro - Text (Status) under the Progress Bar
  * Pos Y: -75
  * Text: "Loading..."
  * Center the text
  * Disable Raycast Target
* Attach the Text to the ProgressBar component

## Loader Setup

* Add an empty GameObject (Loader) and add the override LoadingManager component to it
* Attach the LoadingScreen to the Loader
* Attach the Manager prefabs to the Loader

# Main Menu Setup

* Create a new MainMenu script that overrides the Game MenuPanel
  * Add a public void OnPlay() method that does nothing
  * Add a public void OnCredits() method that does nothing
  * Add a public void OnQuitGame() method that calls Application.Quit()
* Create an empty Prefab and add the Game Menu component to it
  * Layer: UI
  * Render Mode: Screen Space - Overlay
  * UI Scale Mode: Scale With Screen Size
  * Reference Resolution: 1280x720
  * Match Width Or Height: 0.5
* Add a Panel under the Canvas (Main)
  * Remove the Image
  * Add a Vertical Layout Group
    * Spacing: 10
    * Alignment: Middle Center
    * Child Controls Width / Height
    * Force Expand nothing
  * Add the MainMenu script
    * Set Owner to the Menu object
    * Set the Main Panel on the Menu object to the Main panel
* Add a Button under the Main panel (Play)
  * Normal Color: (255, 0, 255, 255)
  * Highlight Color: (0, 255, 0, 255)
  * Add an On Click handler that calls the MainMenu OnPlay method
  * Add a Layout Element to the Button
    * Preferred Width: 200
    * Preferred Height: 50
  * Replace the Text under the Button with a TextMeshPro - Text
    * Text: "Play"
    * Center the text
    * Disable Raycast Target
  * Set the Main Menu Initial Selection to the Play Button
* Duplicate the Play Button (Credits)
  * Set the On Click handler to the MainMenu OnCredits method
  * Set the Text to "Credits"
* Duplicate the Credits Button (Quit)
  * Set the On Click handler to the MainMenu OnQuitGame method
  * Set the Text to "Quit"
* Add a field for the MainMenu to the MainMenuState and connect it on the prefab
  * OnEnter create the menu prefab with the UIManager
  * OnExit destroy the menu prefab
  * OnResume activate the menu prefab
  * OnPause deactivate the menu prefab

## Main Menu Scene Setup

* Do not add a Main Camera to these scenes
* Create and save a new scene (Scenes/main_menu.unity)
  * Remove all default objects from the scene
  * Environment Lighting Source: Color
  * Disable Realtime Global Illumination
  * Disable Baked Global Illumination
  * Disable Auto Generate lighting
  * Add a new Canvas object (TitleScreen) to the scene
    * UI Scale Mode: Scale With Screen Size
    * Reference Resolution: 1280x720
    * Match Width Or Height: 0.5
    * Remove the Graphic Raycaster
    * Remove the EventSystem object that gets added (or turn it into a prefab if that hasn't been created yet)
  * Add a Panel under the Canvas
    * Disable Raycast Target
    * Color: (255, 0, 0, 255)
  * Add a TextMeshPro - Text (Status) under the Progress Bar
    * Pos Y: 256
    * Text: "Placeholder"
    * Center the text
    * Disable Raycast Target
* Add the scene to the Build Settings
* The scene should now load when the main scene is run as long as the name of the scene matches what was set in the MainMenuState prefab

# Initial Game State Setup

## Game Data

* Create a new GameData script that overrides Game GameData and adds an Asset Menu item for it
* Create a new GameData data object
  * Set the World Layer to World
  * Create and attach a ServerSpectator prefab if desired
    * **TODO:** Configure this
    * **TODO:** Create a viewer prefab for it

## Player Data

* Create a new PlayerData script that overrides Game PlayerData and adds an Asset Menu item for it
* Create a new PlayerData data object
  * Set the Player Layer to Player
  * Set the Viewer Layer to Viewer
  * **TODO:** Create a viewer prefab for it
    * Create a new Viewer script that overrides a Core Viewer (? Core or Game?)
    * Create an empty Prefab and add the Viewer component to it
      * Add a camera under the prefab
        * Clear Mode: Sky
        * Background Color: Default
        * Projection: Depends on viewer needs
        * Remove the Audio Listener
        * Add a Post Process Layer component to the Camera object
        * Add an Aspect Ratio component to the Camera (UI) object
      * Add another camera under the prefab (UI)
        * Layer: UI
        * Clear Mode: None
        * Culling Mask: UI
        * Projection: Orthographic
        * Remove the AudioListener
        * Add an Aspect Ratio component to the Camera (UI) object
      * Add an empty GameObject under the prefab and add a Post Process Volume to it
      * Attach the Cameras and the Post Process Volume to the Viewer component
      * **Create the Post Process Layer (one per-viewer, Viewer{N}_PostProcess)**
* Create a new GameState subclass and attach it to a new empty Prefab
  * This state should probably get the ViewerManager and InputManager state setup
* Attach the new GameState prefab to the GameStateManager prefab
* **TODO:** More GameStates
* **TODO:** Pause / Pause Menu
* **TODO:** Create the PlayerManager script/prefab
  * This must be a prefab due to the abstract base class
* **TODO:** Create the Player script/prefab
* **TODO:** How to controls
* **TODO:** Creating Data
* **TODO:** Credits

# Performance Notes

* Mark all static objects as Static in their prefab editor
