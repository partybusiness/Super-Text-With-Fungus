WHAT IS THIS?

Basically, this is something that lets you use Fungus and SuperTextMesh at the same time. This tiny part was written by Ryan Hill, who you can track down via Twitter @flinflonimation

Fungus is a Visual Novel engine for Unity, and SuperTextMesh is something that lets you do some fancy stuff the regular Unity text mesh doesn't do.

To use this, you need both Fungus and Super Text Mesh.

Fungus is free and is linked from here:
http://fungusgames.com/download/

This was tested on Fungus version 3.4.0

Super Text Mesh is available on the Unity Asset Store:
https://www.assetstore.unity3d.com/en/#!/content/57995

Import both of those into Unity, then import this unity package.

SETTING UP FUNGUS

If you've never used Fungus before, there are some good docs here:
http://snozbot.github.io/

The short version is, once you've imported the package, go to Tools -> Fungus -> Create -> Flowchart. 

Then you can select the created flowchart and click Open Flowchart Window. In the new window, click the visible Block, and you'll have access to a list of commands that are executed when that block is triggered.

You can click the + and it'll give you a drop-down where you can choose Narrative->Say and it'll add a Say Command. Type whatever you want people to say using this command.

By default, the Say Command uses the basic Unity text mesh, and what we want is to make it use the Super Text Mesh instead.

SETTING UP THE SAYDIALOG

Drag the SayDialog prefab from Assets/Clavian/SuperTextMesh/Fungus/ into the scene. This one has custom SuperTextMeshWriter and SuperSayDialog components in place of the standard Writer and SayDialog.

To use this SayDialog in the scene instead of the regular SayDialog, you'll need to set it using the Set Say Dialog attribute in the Say Command, or using the Set Say Dialog command.

SETTING UP CHARACTERS

Since the SuperText has some different options available than basic Fungus, you can also specify how those options change when a character is talking.

Look at the Voices list in your SuperText. When you use a Fungus character, it will check this list of Voices for a Voice of the same name as the Character's gameobject. If it finds a match, it will used that Voice.
Note this is the name of the character's GameObject, which might not be the same as the Name Text of the Character component.

USING TAGS

The one downside is you can't use the Fungus tags anymore, but on the plus side, Super Text Mesh has its own tags.

Here's a quick list of tags. There is more detail in the SuperTextMesh documentation:

<c=NAME/HEX/COLOR> = sets colour
</c> = returns to default color
<s=FLOAT> = set text size
</s> = Return to default size of this text mesh.
<d=INT/NAME> = set a delay time
<d> = Use the default delay. This can be changed in TextData.
<b> = bold text
<i> = italic
<w=NAME> Following text will have a wave effect. You can define this effect within TextData.
<w> Use default wave pattern. This can be changed in TextData.
</w> Cancel wave effect.
<j=NAME> Following text will have a jitter effect. You can define this effect within TextData.
<j> Use default jitter pattern. This can be changed in Text Data.
</j> Cancel jitter effect.
<f=NAME> Changes the font of the entire text mesh. Define within TextData.
<v=NAME> Following text will use different STM settings. Definable within TextData.






