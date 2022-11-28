startGui(){
    Gui, +toolwindow +LabelTaskbar -caption +alwaysontop -dpiscale +Resize
    Gui, color , 000000

    Gui, Font, S10 Cffffff
    addLabel("Main", "")

    startY := A_ScreenHeight - 50
    Gui, show, x0 y%startY%, WtileGui
    WinSet, AlwaysOnTop, On, WtileGui ahk_class AutoHotkeyGUI
    OnMessage(0x201,"WM_LBUTTONDOWN")
}

renderGui(){
    workspace := getCurrentWorkspace()
    windowList := workspace.windowList

    titleList := []
    for k, v in windowList{
        if (v == "") {
            Continue
        }
        Winget, Title, ProcessName, ahk_id %v%
        Title := StrSplit(Title, ".")[1]
        titleList.Push(Title)
    }
    windowStr := ""
    for k, v in titleList {
        if (v == "") {
            Continue
        }
        windowStr := windowStr . k ": " v " "
    }

    currentWorkspaceIndex := getWorkspaceIndex()
    currentWindowIndex := workspace.currentWindowIndex

    str = Workspace: %currentWorkspaceIndex% | Window: %currentWindowIndex% | %windowStr%
    GuiControl, Text, Main, %str%
    strWidth := wCal(str)
    GuiControl, move, Main, %strWidth%
}

addLabel(labelID, labelText = ""){
    global
    labelID := "v" . labelID
    Gui, Add, Text, w600 %labelID%, %labelText%
}

WM_LBUTTONDOWN(wParam,lParam,msg,hwnd){
    PostMessage, 0xA1, 2
}

wCal(title,fSize=10) {											; use title/font size
    t := StrSplit(title,A_Space)							; get number of space characters
    Loop, Parse, title										; get number of characters
        width := (fSize/1.3*A_Index) + fSize*1.3-t.Length()	; doing some "math" (far from being exact, just t&e)
    width := width * (A_ScreenDPI / 100) ; needs to scale with dpi
    Return "w"width
}

guiTick(){
    global guiHidden
    if(hideGui) {
        Gui, Hide
        guiHidden := true
    }else if(guiHidden) {
        Gui, Show
        guiHidden := false
    }
    WinSet, AlwaysOnTop, On, WtileGui ahk_class AutoHotkeyGUI
}