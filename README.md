# XR Input Test Suite

## Running the Complete Project in the Editor
If you are in the complete project, start by opening the XR Tests Editor Window.  This can be found from the menu bar at Window->XR Tests.  From this menu you can use the provided buttons to set up a test to run, edit, or export.
- "Run" loads the scene for a test and enters playmode
- "Edit" loads the scene for a test without entering playmode
- "Export" creates a single-scene project ready to zip up and attach to an issue as a minimum reproduction.

## Running the Complete Project on a Device
Simply compile the project and deploy it to your platform of choice.  Each scene has a pair of arrows.  Gaze at an arrow for a few seconds to transition to the next scene.

If you would like to reset the build setting scenes to include all test scenes, you can do so from the menu bar at Window->XR Tests.  The top bar of the XR Tests panel includes a button that resets the scene list in build settings.

## Add a New Test
To add a test scene within the complete project
- Copy the Assets/Tests/Template to Assets/Tests/YourNewTestName
- Rename Assets/Tests/YourNewTestName/Template.unity to Assets/Tests/YourNewTestName/YourNewTestName.unity
- Add your test-specific GameObjects.  Only add new assets to Assets/Tests/YourNewTestName/
- Add your test-specific instructions to the TestInstructions GameObject in Assets/Tests/YourNewTestName/YourNewTestName.unity
- Add your test to the build settings by navigating to Menu Bar -> Window -> XR Test and pressing the Update Build Settings button.
- You can refresh the XR Tests window using the "Refresh" button located at the top of the window.
- Before committing the new test to source control be sure to verify it by:
  - Running the test in playmode - no consolde debug logs, warnings, or errors! Launch the test by pressing the "Run" button under Window->XR Tests panel -> YourNewTestName
  - Running the test in a build
  - Export the test, make sure it works in playmode and as a build - no consolde debug logs, warnings, or errors!  You should be able to navigate between scenes with the arrows forward and backward from the new test.

## Best Practices
- If you are adding a completely new test, add it to the earliest editor branch that supports the feature and graft it forward version branch by version branch.
- Test and double test before committing new changes! This keeps the project nice for everyone!
- Please only add new assets to Assets/Tests/YourNewTestName/. All gameobjects in the template scene are expected by the Test Driver.
