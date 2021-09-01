# ExportNeosToJson

A [NeosModLoader](https://github.com/zkxs/NeosModLoader) mod for [Neos VR](https://neos.com/) that allows exporting items as json, Bson, 7zbson and lz4bson files. This allows items to be backed up locally, as well as letting you edit normally inaccessible internals, such as arrays. Note that assets behave in weird ways and will (probably?) only be linked to. Json, Bson, 7zbson and lz4bson files can be reimported into the game easily by anyone, without needing a mod.

## Installation
1. Install [NeosModLoader](https://github.com/zkxs/NeosModLoader).
1. Place [ExportNeosToJson.dll](https://github.com/zkxs/ExportNeosToJson/releases/latest/download/ExportNeosToJson.dll) into your `nml_mods` folder. This folder should be at `C:\Program Files (x86)\Steam\steamapps\common\NeosVR\nml_mods` for a default install. You can create it if it's missing, or if you launch the game once with NeosModLoader installed it will create the folder for you.
1. Start the game. If you want to verify that the mod is working you can check your Neos logs.

## FAQ
### What does this actually do?
It injects additional json, Bson, 7zbson and lz4bson options into the export dialog.

### Is this against guidelines?
Maybe. [Read more](https://github.com/zkxs/NeosModLoader/blob/master/doc/neos_guidelines.md).
