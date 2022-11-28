#NoEnv
#SingleInstance, Force
SendMode, Input
SetBatchLines, -1
SetWorkingDir, %A_ScriptDir%

#Include, gui/main.ahk
startGui()

#Persistent

#Include, workspace/state.ahk

return

#Include, workspace/functions.ahk
#Include, utils/functions.ahk

#Include, workspace/keybinds.ahk
