Sln Converter
=============

A tool to convert Visual Studio Solution files (sln) into/out of xml, to aid in reconciling differences and merges.

I built this tool to enable safe and reliable diffing of complex VS solution files. The converter translates the 
solution file into a structured format - it is not just a map from SLN format to XML. 

Main features are:

* SLN -> XML / XML -> SLN bidrection lossless conversion
* Structured XML to clearly document the Solution's Folder structure
* Minimised or eliminated GUIDs where possible
* Provides a sample powershell script to automatically convert 2 files to XML, and pass them to Beyond Compare

NOTE: With a little configuration, it is possible to make the last point automatic within Beyond Compare. This can
be acheived by configuring a new file format for *.sln, and defining a conversion tool for loading and saving.

Loading:  "path\to\SlnCompare\Laan.SolutionConverter.exe" -i %s -o %t -m xml
Saving:   "path\to\SlnCompare\Laan.SolutionConverter.exe" -i %s -o %t -m sln

