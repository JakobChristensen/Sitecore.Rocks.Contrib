Sitecore.Rocks.Contrib
======================

Community contributions for Sitecore Rocks.

## Overview
This plugin contains a number of enhancements to Sitecore Rocks (Visual Studio and Windows). 

#### Upload Media Files command
Adds an Upload Media Files command to any item named "Media Library" in the Sitecore Explorer.

## Getting Started

1. Open the solution file in Visual Studio 2013.
2. Configure debugging as described below.
3. Press F5 to start.

### Debugging
1. Open the Properties Pane for the Sitecore.Rocks.Contrib project.
2. Set Start Action to "Start External Program". 
3. For Sitecore Rocks Windows, browse to "Sitecore.Rocks.Windows.exe", e.g.
   "C:\Sitecore\Sitecore.Rocks\Sitecore.Rocks.Windows.exe"
   or for Visual Studio, browse to "devenv.exe", e.g.:
   "C:\Program Files (x86)\Microsoft Visual Studio 12.0\Common7\IDE\devenv.exe"
4. Set Command Line Arguments to the output folder of the Sitecore.Rocks.Contrib project, e.g.:
   "-rocks:plugins c:\Sitecore\Sitecore.Rocks.Contrib\Sitecore.Rocks.Contrib\bin\Debug"

## Development links

[Sitecore Rocks on sdn.sitecore.net](http://sdn.sitecore.net/Products/Sitecore%20Rocks.aspx) 

[Sitecore Rocks wiki](http://vsplugins.sitecore.net)

[Sitecore Rocks - Creating Visual Studio projects](http://vsplugins.sitecore.net/Creating-Visual-Studio-Projects.ashx) 

[Sitecore Rocks - How To](http://vsplugins.sitecore.net/How-To.ashx)

[Sitecore Rocks - NuGet repository](https://www.myget.org/gallery/sitecorerocks) 
