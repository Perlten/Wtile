#NoEnv
#SingleInstance, Force
#Persistent
SendMode, Input
SetBatchLines, -1
SetWorkingDir, %A_ScriptDir%

; Libs
#include, libs/json.ahk
#include, libs/jsonFile.ahk

; State
#include, gui/state.ahk
#Include, workspace/state.ahk

; Mains
#Include, gui/main.ahk
loadGuiSettings()
startGui()
SetTimer, guiTick, 500
SetTimer, updateSystemInformation, 5000

return

; Functions
#Include, workspace/functions.ahk
#Include, utils/functions.ahk

; Keybinds
#Include, workspace/keybinds.ahk
#Include, gui/keybinds.ahk
