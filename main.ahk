#NoEnv
#SingleInstance, Force
#Persistent
SendMode, Input
SetBatchLines, -1
SetWorkingDir, %A_ScriptDir%

#Include, gui/main.ahk
startGui()
SetTimer, guiTick, 16

#include, gui/state.ahk
#Include, workspace/state.ahk

return

#Include, workspace/functions.ahk
#Include, utils/functions.ahk

#Include, workspace/keybinds.ahk
#Include, gui/keybinds.ahk
