## Settings 

### Uses the native settings management
* Android: SharedPreferences
* iOS: NSUserDefaults
* Windows Phone: IsolatedStorageSettings
* Windows Store: ApplicationDataContainer

### Setup & Usage
* Available on NuGet: https://www.nuget.org/packages/Xam.Plugins.Settings/
* Install into your PCL project and Client projects.
* Open up Helpers/Settings.cs in your PCL for directions on how to use.

### NuGet Creation & Packaging

I created this NuGet PCL package by creating an interface that I would implement on each platform. This lives inside of my .Abstractions.dll that will be installed on each platform. I then create a base PCL project that has a Settings.cs file with an internal IoC to new up a CrossSettings class. The key here is that this file is linked to all of my platform specific projects to new up the correct version.

You can find my nuspec here: https://github.com/jamesmontemagno/Xam.PCL.Plugins/blob/master/Settings/Common/Xam.Plugins.Settings.nuspec


#### Contributors
* [jamesmontemagno](https://github.com/jamesmontemagno)

Thanks!

#### License
Licensed under the [Apache License, Version 2.0](http://www.apache.org/licenses/LICENSE-2.0.html)
