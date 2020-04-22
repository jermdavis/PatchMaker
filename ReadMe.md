<img src="./Notes/Icons/ConfigurationEditor_128x.png" align="right"  />

# PatchMaker

An experiment to see if it's possible to build a helpful UI for creating and testing Sitecore config patches.

Since it's an experiment, there are probably some rough edges in this code. Don't judge it too harshly please -
but [write me a bug report](https://github.com/jermdavis/PatchMaker/issues) if you find anything that doesn't work.

## Projects

* **PatchMaker**: The core classes for describing patch operations, and generating patch files.
   * `PatchAttribute.cs` / `PatchDelete.cs` / `PatchInsert.cs` / `PatchInstead.cs` / `SetAttribute.cs` are objects which describe the different types of patch that can be applied.
   * `PatchGenerator.cs` turns a set of patch objects into an XML patch file
* **PatchMaker.App**: Experimental UI for using the core classes to generate and preview new patch files. 
   * `PatchPlanningForm.cs` is the main window, which shows on startup.
   * The `PatchForms` folder contains the UI for the individual patch types
   * `PatchGenerationForm.cs` deals with showing and saving the resulting patch file.
   * And `PatchPreviewForm.cs` is the UI for seeing the effect of your patch file on the original XML file.
* **PatchMaker.Sitecore**: A wrapper for Sitecore's internal patch processing logic - used to generate previews of what a patch file will do in tests and the UI.
   * `SitecorePatcher.cs` provides a static class / method that wraps Sitecore's logic.
* **PatchMaker.Tests**: A set of unit tests for the core classes.

## Running it

You can download a pre-built copy from the [Release page](https://github.com/jermdavis/PatchMaker/issues).

_**Note**_: For licensing reasons, these builds do not include the Sitecore DLL used for previewing the effect of your patches.
The app will run happily without it, but the "preview" button in the patch generation UI will be disabled. If you use a pre-built
release you can drop your own copy of `Sitecore.Kernel.dll` into the folder you run `PatchMaker.App.exe`
from. The code is built against Sitecore V8.2 - so if you don't use this release yourself you will need a version
redirect in the `app.config`.

Take a look at [the help included](PatchMaker.App/PatchMaker.App.Help.html) with the app for details of how to
use it.

## Building it

Pretty simple, hopefully:

* Clone the source.
* Open it in Visual Studio. It was written using VS 2019. You need to have the SDK for .Net 4.7.2 installed. And you need the .Net Desktop Development workload.
* Make sure your NuGet configuration includes the Sitecore feed. The solution includes a [nuget.config](nuget.config) - but you can also [set this up in the Visual Studio config](https://docs.microsoft.com/en-us/azure/devops/artifacts/nuget/consume?view=azure-devops#windows-add-the-feed-to-your-nuget-configuration) directly.
* Hit build.
* You can make sure the tests pass.
* And run PatchMaker.App.exe to experiment.