# Project Setup

## How to Install Unity Hub
Download and install the latest version of Unity Hub from the [Official Unity Website](https://unity.com/download).
> Scroll down to select the download link for your Operating system (Windows, Linux, Mac)

## How to Clone the Repository
Clone this repository to your local machine.
by either running this command within a Command prompt with git installed

`git clone https://github.com/DakotaIrsik/NeonLadderURP.git `

or by clicking the "Code -> Download" button and extracting the repository. (Note the location you download / clone the repository to)

## Open the Project in Unity Hub
1. Open Unity Hub.
2. Create your Unity Account (Single-Sign-On is easiest, IE Google)
3. Click on `Add` within the Unity Hub and select the directory which contains the repository

## How to Install Unity Editor
1. Unity Hub will prompt you to install the necessary editor if not installed (2022.3.31f1).
2. Uncheck Visual Studio from installed components list (unless you want it)
3. Scroll down and Select the build platform for the operating system you're running on IE if you're on Windows choose "Windows Build Support (IL2CPP)"
> During the installation, UAC may prompt fo the installation, choose yes.

## How to Open the Project
1. Select Projects from Unity Hub
2. Select The cloned folder (NeonLadderURP if you cloned via a shell, or NeonLadderURP-main if you downloaded and extracted)
3. Wait for the project to load. 
4. If prompted "This project is using the new input system package but the native platform backends for the new input system are not enabled in the player settings. This meants that no input fro native devices will come through, Do you want to enable the backends? Doing so will "RESTART" the editor. Click - YES

Once loaded, open the `SampleScene`:
1. Locate the `Project` tab at the bottom of the Unity Editor (should be preselected)
2. Naviagate to `Scenes/`.
3. Double-click on `SampleScene` to open it.

## Play the Scene
1. Click the `Play` button at the top of the Unity Editor.
2. Notice that this scene appears empty except for lighting.
3. Click the `Play` button again to stop the scene.

## Download Additional Assets
1. In the Project tab's search bos, search for `DownloadInstructions`.
2. Locate the DownloadInstructions.txt file which exists in the SURIYUN folder (you can see the full path of the DownloadInstructions at the bottom of the Editor, inicating the location of the path).
3. Follow the instructions provided in the `DownloadInstructions` files to download the asset.

## Relaunch Unity Editor
Exit the Unity Editor application by clicking the X at the top right of the Unity Editor application.
> **Note:** You MUST RESTART the Unity Editor so that asset metadata refreshes the relationship between the source controlled animation controllers and the package-driven animations themselves, if you don't restart the game will NOT function as intended.
Relaunch the game project from Unity Hub
1. Location the Unity Hub window (this should still be open, as we only closed the Unity Editor) if it was closed, relaunch Unity Hub
2. Select Projects from Unity Hub
3. Select project name (NeonLadderURP if you cloned via a shell, or NeonLadderURP-main if you downloaded and extracted)
 Wait for the project to load.

## Play the Scene (again)
You should now be ready to click the "Play" button once again within the Unity Editor and see the main protagonists idle animations and be able to move left and right with the A/D or <-/-> (arrow keys)

## Conclusion
Repeat Download Additional Assets Steps for all remaining packages. making sure to restart before attempting to "Play" the game
> **Note:** The import process for large packages, like the `LeartesStudios` package (~30 GB), may take HOURS depending on your PC specs.

You should now experience the game as if it were deployed to a specified build platform.
