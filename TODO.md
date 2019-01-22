* Kill all UnityEngine.Input usage
    * Try setting to just Input System, does it work now? - no, UI is still broken
* Unregister DI managers
* Text
    * Requires a TMP Text
    * Sets the TMP Text text from a string table data object
* Button
    * Requires a UI.Button and a ButtonAudioSource (Hover / Play hooks)
    * Has a hook for a Text
    * Sets up the button audio hooks
    * Sets the button text
* Player UI
* Test running around a level
* Setup / test SceneTester
* 2D support
    * 2D static camera
    * 2D follow camera
* Fighter camera (ie, zoom in/out to keep N objects in view on a level
