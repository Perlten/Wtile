#NoEnv
#SingleInstance, Force
SendMode, Input
SetBatchLines, -1
SetWorkingDir, %A_ScriptDir%

workspaceList := { 1: {lastWindow: 0, currentWindowIndex: 1, windowList: []}
,2: {lastWindow: 0, currentWindowIndex: 1, windowList: []}
,3: {lastWindow: 0, currentWindowIndex: 1, windowList: []}
,4: {lastWindow: 0, currentWindowIndex: 1, windowList: []}
,5: {lastWindow: 0, currentWindowIndex: 1, windowList: []}
,6: {lastWindow: 0, currentWindowIndex: 1, windowList: []}
,7: {lastWindow: 0, currentWindowIndex: 1, windowList: []}
,8: {lastWindow: 0, currentWindowIndex: 1, windowList: []}
,9: {lastWindow: 0, currentWindowIndex: 1, windowList: []}}

lastWorkspace := 1
currentWorkspace := 1

Gui, +LastFound
DllCall("RegisterShellHookWindow", UInt,WinExist())
OnMessage(DllCall("RegisterWindowMessage", Str,"SHELLHOOK"), "windowAlteration")