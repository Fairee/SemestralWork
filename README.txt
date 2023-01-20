This is just a basic aplication to test the posibilities of GeoSpatial API.
Also it includes scripts to parse .shp file with polylineZ data.
On top of that it can create meshes of thick lines based on the data.
There is an object Called EDITOR-PipeGenerator which is the holder of the Pipe Creator script. To generate the pipes provided the whole path to the file .shp in the Filename. There has to be the .shp file and .dbf file both with the same name just different ending. Also there is the IS WGS checkbox. Check that it is WGS if and only if the .shp file is in the WGS geo-coordinates. If the file is in krovak plane coordinates uncheck it.

As for the AR part-
The most necessery thing to note is the necesity to provide a key while building the application. There is the key file include which is called key.keystore. The password is 123456 and the same password for the only key inside of it. Without it Unity will not be able to build the app. If you would create different keystore, its SHA1 need to be registred on the https://console.cloud.google.com/

ALSO if you build the app and the camera isn't working. You have to change something in the Unity scene ... anything really... move the EventSystem by one on X axis for example and build it again. This time it will work. I have no idea why it acts like this, but it does. If you would just change something in a script and compile it you would get a non working app.