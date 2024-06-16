# Make sure the RenameFiles.ps1 script is in the same directory or provide the correct path to it
$scriptPath = ".\RenameFiles.ps1"

# Invoke the RenameFiles.ps1 script with different arguments
& $scriptPath "ValheimMeshReplacerTemplate" "MyMod"
& $scriptPath "valheimmeshreplacertemplate" "mymod"
& $scriptPath "valheim-mesh-replacer-template" "my-mod"
