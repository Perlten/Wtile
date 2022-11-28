#NoEnv
#SingleInstance, Force
SendMode, Input
SetBatchLines, -1
SetWorkingDir, %A_ScriptDir%

startGui(){
    Gui, +toolwindow +LabelTaskbar -caption +alwaysontop -dpiscale +Resize
    Gui, color , 000000

    Gui, Font, S10 Cffffff
    Gui, Add, Text, ,test

    Gui, show
    OnMessage(0x201,"WM_LBUTTONDOWN")
}

render(){
    text := ""
}

WM_LBUTTONDOWN(wParam,lParam,msg,hwnd){
    PostMessage, 0xA1, 2
}