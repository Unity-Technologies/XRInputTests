Welcome to this Test Suite!

########################################
### Running an Exported Test Project ###
########################################
If you are receiving this project as a bug reproduction example, this is probably you!

If you are in a slimmed down export project then everything you need is 
contained in the scene located in the Assets folder.
Just load the scene and compile or hit play!

####################################
### Running the Complete Project ###
####################################
If you are in the complete project, start by opening the XR Tests Editor Window.  
This can be found from the menu bar at Window->XR Tests.
From this menu you can use the provided buttons to set up a test to run, edit, or export.
It is suggested you use these buttons to setup unless you really know what you are doing!

######################
### Add a New Test ###
######################
To add a test scene within the complete project
- Copy the Assets/Tests/Template to Assets/Tests/YourNewTestName
- Rename Assets/Tests/YourNewTestName/Template.unity to Assets/Tests/YourNewTestName/YourNewTestName.unity
- Add your test-specific GameObjects.  Only add new assets to Assets/Tests/YourNewTestName/
- Add your test-specific instructions to the TestInstructions GameObject in Assets/Tests/YourNewTestName/YourNewTestName.unity
- Add your test to the build settings by navigating to Menu Bar -> Window -> XR Test Config and pressing the Update Build Settings button.
- You can refresh the XR Tests window using the "Refresh" button located at the top of the window.
- Before committing the new test in Mercurial be sure to verify it by:
--- Running the test in playmode - no consolde debug logs, warnings, or errors!
--- Running the test in a build
--- Export the test, make sure it works in playmode and as a build - no consolde debug logs, warnings, or errors!

######################
### Best Practices ###
######################
- DON'T USE PREFABS!  This can break when backporting!
- Use (Menu Bar -> Window -> XR Test Config) and (Menu Bar -> Window -> XR Tests)!  Proper setup of these tests requires correctly activated scenes.  The buttons do this for you!
- Test and double test before committing new changes! This keeps the project nice for everyone!
- Please only add new assets to Assets/Tests/YourNewTestName/
