# MotionBlurDisable

A [MelonLoader](https://melonwiki.xyz/) mod for [Neos VR](https://neos.com/) that allows exporting items to 7zbson. This allows items to be backed up locally.

## Installation
1. Install [MelonLoader](https://melonwiki.xyz/) version 0.4.4 or higher. 
1. Place [ExportNeosToJson.dll](https://github.com/zkxs/ExportNeosToJson/releases/latest/download/ExportNeosToJson.dll) into your `Mods` folder. This folder should be at `C:\Program Files (x86)\Steam\steamapps\common\NeosVR\Mods` for a default install. You can create it if it's missing, or if you launch the game once with MelonLoader installed it will create the folder for you.
1. Start the game. If you want to verify that the mod is working you can check the logs in MelonLoader's console.

## FAQ
### What does this actually do?
It injects an additional 7zbson option into the export dialog.

### Why isn't this a Neos plugin?
1. Neos plugins are intended for adding new components, and in fact a component is required to be used as the entry point for your plugin's code. It's not possible to make a plugin that has zero components. (If it is possible please tell me how). I *do not* want my installation process to include "put this empty do-nothing component somewhere in your Local Home"
2. Neos plugins load after the engine has already initialized. This means there are certain things that happen too early for a Neos plugin to alter.

### Why MelonLoader over some other mod loader?
1. Because I'm familiar with MelonLoader from my VRChat days.
1. If Neos ever starts using IL2CPP MelonLoader already has support.

### Does this violate the EULA?
[Probably yes](https://store.steampowered.com//eula/740250_eula_0), but honestly I could do the same thing in a Neos plugin in a less user-friendly way (assuming a .NET 4.7.2 plugin will actually compile and load)
