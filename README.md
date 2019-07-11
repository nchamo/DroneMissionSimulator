# Drone Mission Simulator
Simulate drone flights and take images of your 3D objects.

## Description
This simulator was built on Unity. It was developed to flight over a [terrain](https://docs.unity3d.com/Manual//terrain-UsingTerrains.html) and take images of it, like a drone would. The project is based on [this](https://www.mapsmadeeasy.com/flight_planner) other flight planner. 

The project has two possible outputs:
1. The mission simulation, which outputs geotagged images and a XML with the simulation description.
2. You can also generate an elevation map of your terrain. It might be useful if you want to have a ground truth.

## Instalation
The project doesn't have a binary or something like that yet. We only provide the Unity project so you can run it by yourself. 
To run the simulator, you will have to:
1. Download [Unity Hub](https://store.unity.com/download?ref=personal)
2. Download a version of the Unity Editor (the project was build with the version 2019.1.5)
3. Clone or download this repo
4. Go to the folder `Assets > Scenes` and open the file `Scene.unity`.

### Important
The project has only been tested on Windows. In fact, for Geotagging, we use [exiftool](https://www.sno.phy.queensu.ca/~phil/exiftool/) and the project currently has a Windows pre-built binary for doing so. So it might require some extra work (but not too much) to run it in other platforms.
If you happen to do so, please create a Pull Request so we can merge it.

## Running
When openning the proyect, you might see some errors:
![Errors](https://user-images.githubusercontent.com/15222168/61068348-2e854b00-a3e0-11e9-935e-25b601edf45e.PNG)
That's ok, it's because some files were not added to the repo in order to reduce the size. Those files will be auto-generated, so don't worry.

### Simulation
To run the simulation, simply press the `Play` button. The simulation will start, and the georeferenced images will be generated in a folder named `Images` inside the repo (this can be modified of course). Once the simulation is over, and the images are generated and tagged, the simulation will stop by itself.
It might look like the simulation 'has lag'. This can happen if the flight speed and the images' resolution are too high. 

#### Flight Parameters
To modify the mission's parameters, you must first select the camera on the `Hierarchy` menu, and then you will be able to adjust your flight plan on the `Inspector` on the right.
![Steps](https://user-images.githubusercontent.com/15222168/61068352-30e7a500-a3e0-11e9-963f-241b11f5ab1f.PNG)

##### Debug
If you just want to check the mission plan, without actually generating the images, you can uncheck the parameter "Take Screeenshots". This will also remove any lag you might have.
Also, when the option "Draw Grid" is selected, you can go to the `Scene` view in the editor to see how the plan is going.

#### Light Baking
It might happen that at some point, the scene becomes a little darker. This is because Unity regenerated the high quality lights in the scene when it detects a change. You can know if this process is happening by checking at the bottom right corner.
If this process is ongoing, you can just wait a few seconds for the process to regenarate the lights.

### Elevation Map
To generate the elevation map, you can go to `Window > Elevation Map`.
![Window](https://user-images.githubusercontent.com/15222168/61068355-3349ff00-a3e0-11e9-8386-1ee891e5140a.PNG)

You will be able to determine the size and the quality of the map.

![Options](https://user-images.githubusercontent.com/15222168/61068359-37761c80-a3e0-11e9-975d-4c93ae77c7f3.PNG)

## Terrain
If you want to modify the terrain that already comes with the project, you can easily add/remove elevation, trees, grass, water, etc. Check the [documentation](https://docs.unity3d.com/Manual/script-Terrain.html) on how to do so.
The proyect already has some trees downloaded from the [Unity Asset Store](https://assetstore.unity.com/). You can download more if you like. To check the pre-downloaded models, navigate to `Assets > Models`.

## What's coming in the future?
There are a few improvement to be made, starting with:
1. Making the simulator cross platform.
2. Building the project so that it is not necessary to install Unity.
3. Adding the possibility to add Ground Control Points and export a file with their locations in each generated image. 

### Contributing 
If you happen to tackle any of the issue above, or any others, please send a PR our way and we will be more than happy to merge them!
