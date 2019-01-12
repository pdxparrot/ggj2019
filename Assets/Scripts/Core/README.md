# Pre-Asset Setup

* Art/Core/pdxparrot.png
* Art/Core/progress.png
  * Texture Type: Sprite (2D and UI)
* Data/Audio/main.mixer
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
* Data/Physics/Frictionless.physicMaterial
  * Static Friction: 0
  * Dynamic Friction: 0
* Data/Physics/Frictionless 2D.physicsMaterial2D
  * Friction: 0

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
    * TODO: water?
    * Vfx -> Vfx
    * Viewer -> Weather
    * Viewer -> World
    * Player -> Weather
    * Player -> World
    * World -> Weather
* Physics 2D Settings
  * Set the Default Material to frictionless if desired
  * Only enable the minimum necessary collisions
    * TODO: water?
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
  * Active Input Handling: Both (at least until the old Input Manager is axed)
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

* Create Assets/Scripts/Core/Input directory
* Data/Input/Controls.inputactions
  * Generate C# Class
    * Code Path: Assets\Scripts\Core\Input\Controls.cs
    * Class Name: Controls
    * Namespace: pdxpartyparrot.Core.Input
    * Generate Events
    * Generate Interfaces
  * Add the Action Map and the desired controls
* Data/Prefabs/Input/EventSystem.prefab
  * Create using default EventSystem that gets added automatically when adding a UI object
* Scripts/Core/com.pdxpartyparrot.Core.asmdef
  * References: Unity.InputSystem, Unity.Postprocessing.Runtime, Unity.TextMeshPro, Kino.Postprocessing
* Scripts/Core/Editor/com.pdxpartyparrot.Core.Editor
  * Editor platform only
  * References: com.pdxpartyparrot.Core.asmdef
* Scripts/Game/com.pdxpartyparrot.Game.asmdef
  * References: nity.InputSystem, com.pdxpartyparrot.Core.asmdef
* Scripts/<project>/com.pdxpartyparrot.<projext>.asmdef
  * References: com.pdxpartyparrot.Core.asmdef, com.pdxpartyparrot.Game.asmdef

## Manager Prefabs Setup
* AudioManager
  * Create an empty Prefab and add the AudioManager component to it
  * Attach the main mixer to the prefab Mixer
  * Add 3 Audio Sources to the prefab
    * Disable Play on Awake
    * Set 2 of them to Loop
  * Attach the non-looping source to the One Shot Audio Source
  * Attach the looping sources to the Music Audio Sources
* CameraManager
  * Create a new Viewer script that overrides a Core Viewer
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
  * Create an empty Prefab and add the CameraManager component to it
* InputManager
  * Create an empty Prefab and add the InputManager component to it
  * Attach the Controls.inputasset asset
  * Attach the EventSystem prefab
* SceneManager
  * Create an empty Prefab and add the SceneManager component to it
  * Set the main scene name if necessary
* NetworkManager
  * Create an empty Prefab and add the NetworkManager component to it
    * Disable Don't Destroy on Load
    * Set the Network Info as desired
* GameStateManager
  * Create an empty Prefab and add the GameStateManager component to it

# Splash Screen Setup

* Create and save a new scene
  * The only object in the scene should be a Main Camera
  * Leave the Audio Listener attached to the camera for audio to work
  * Add the AspectRatio component to the camera
* Add the scene to the Build Settings and ensure that it is Scene 0
* Add a new GameObject to the scene and add the SplashScreen component to it
* Attach the camera to the Camera field of the SplashScreen component
* Add whatever splash screen videos to the list of Splash Screens on the SplashScreen component
* Set the Main Scene Name to match whatever the name of your main scene is
  * The main scene should also have been added to the Build Settings along with any other required scenes

# Main Scene Setup

* Create and save a new scene
  * The only object in the scene should be a camera
* Setup the camera in the scene
  * Clear Mode: Background Color
  * Background Color: Opaque Black
  * Culling Mask: Nothing
  * Projection: Perspective (**TODO:** would ortho make more sense for this?)
  * Turn off Clear Depth
  * Disable Occlusion Culling (**TODO:** is this right?)
  * Leave the Audio Listener attached to the camera for audio to work

## Loading Screen Setup

* Add a new Canvas object to the scene
* Configure the Canvas
  * Additional Shader Channels: Nothing
  * UI Scale Mode: Scale With Screen Size
  * Reference Resolution: 1280x720
  * Match: 0.5
  * Remove the Graphic Raycaster
  * Add the LoadingScreen component
* Remove the EventSystem object that gets added (or turn it into a prefab if that hasn't been created yet)
* Add a Panel under the Canvas
  * Disable Raycast Target
  * Color: (255, 0, 255, 255)
* Add a Text under the Panel
  * Text: "Placeholder"
  * Center the text
  * Disable Raycast Target
* Add an Empty GameObject under the Panel and add the ProgressBar component to it
* Attach the ProgressBar component to the LoadingScreen component
* Add an Image under the Progress Bar (Background) and another Image under that Image (Foreground)
  * Position: (0, 0, 0)
  * Size: (500, 25)
  * Source Image: Core Progress Image
  * Disable Raycast Target
* Configure the Foreground Image
  * Image Type: Filled
  * Fill Method: Horizontal
  * Fill Origin: Left
  * Fill Amount: 0.5
* Attach the images to the ProgressBar component
* Add a Text (this can go under the Progress Bar if desired)
  * Text: "Loading..."
  * Center the text
  * Disable Raycast Target
* Attach the Text to the ProgressBar component

## Loader Setup

* Create a new LoadingManager script that overrides Game LoadingManager
* Add an empty GameObject and add the override LoadingManager component to it
  * The Default Scene Name will need to be set once a default scene is created
* Attach the LoadingScreen to the Loader
* Attach the Manager prefabs to the Loader

# Game Scene Setup
* Do not add a Main Camera to these scenes

# Initial Game State Setup
* Create a new GameState subclass and attach it to a new empty Prefab
  * This state should probably get the ViewerManager and InputManager state setup
* Attach the new GameState prefab to the GameStateManager prefab
* **TODO:** Create the PlayerManager script/prefab
  * This must be a prefab due to the abstract base class
* **TODO:** Create the Player script/prefab

# Performance Notes

* Mark all static objects as Static in their prefab editor
