# General

* Kill all UnityEngine.Input usage
    * Try setting to just Input System, does it work now? - no, UI is still broken
* Fighter camera (ie, zoom in/out to keep N objects in view on a level
* Setting sprite layers and stuff from data / code might be useful
* Move game data into scriptable objects
* The pollen follow code could be a component of its own

# Actors

* Split up actor components and reduce subclassing
    * The player junk in Game is probably just stuff that should merge into Actor components
* No more ActorManager subclassing, use Actor tags instead like we're doing with SpawnPoints
* Have Actor stuff just be components that can be compounded
* ActorID should be a GUID
* Actors should have their own OnUpdate/OnFixedUpdate/OnLateUpdate stuff so that we can stop having if(IsPaused) everywhere
    * ActorManager would be responsible for calling these when the game isn't paused

# Spine

* Move the Spine related stuff into core tools and #if USE_SPINE it
* Score bonus / penaly for kills / deaths / pollen
    * This is in but not used
* Would camera shake on game end work?
* Losing a flower shoul dhave a score modifier
