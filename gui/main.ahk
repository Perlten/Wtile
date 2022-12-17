startGui(){
    global fontSize, startX, startY, startW, startH
    Gui, +toolwindow +LabelTaskbar -caption +alwaysontop -dpiscale +Resize
    Gui, color , 000000

    Gui, Font, S%fontSize% Cffffff
    addLabel("LMain", "hsjdsad")
    addLabel("RMain", "hjejk med asgi")

    startW := startW - 16 ; For some reason it adds 16 every time the programs starts
    startH := startH - 16

    Gui, show, w%startW% h%startH% x%startX% y%startY%, WtileGui
    WinSet, AlwaysOnTop, On, WtileGui ahk_class AutoHotkeyGUI
    OnMessage(0x201,"WM_LBUTTONDOWN")
    renderGui()
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
        windowStr := windowStr . k ": " v " / "
    }

    currentWorkspaceIndex := getWorkspaceIndex()
    currentWindowIndex := workspace.currentWindowIndex

    leftString = Workspace: %currentWorkspaceIndex% | Window: %currentWindowIndex% | %windowStr%

    timeString = %A_DDD% - %A_DD%/%A_MM%/%A_YYYY% - %A_Hour%:%A_Min%:%A_Sec%
    rightString = %timeString%

    updateTextGui(leftString, "LMain", "left")
    updateTextGui(rightString, "RMain", "right")
}

updateTextGui(str, guiID, align="left"){
    GuiControl, Text, %guiID%, %str%
    if (align == "left") {
        strWidth := wCal(str, false)
        GuiControl, move, %guiID%, %strWidth%
    }else if (align == "right") {
        strWidth := wCal(str, true)
        Gui, +LastFound
        WinGetPos,x,y,w,h
        offset := w - strWidth + 30
        GuiControl, move, %guiID%, w%strWidth% x%offset%
    }
}

addLabel(labelID, labelText = ""){
    global
    labelID := "v" . labelID
    Gui, Add, Text, y8 %labelID%, %labelText%
}

WM_LBUTTONDOWN(wParam,lParam,msg,hwnd){
    PostMessage, 0xA1, 2
}

wCal(title, asNumber) {											; use title/font size
    global fontSize
    t := StrSplit(title,A_Space)							; get number of space characters
    Loop, Parse, title										; get number of characters
        width := (fontSize/1.3*A_Index) + fontSize*1.3-t.Length()	; doing some "math" (far from being exact, just t&e)
    width := width * (A_ScreenDPI / 100) ; needs to scale with dpi
    if (asNumber == true) {
        return width
    }
    Return "w"width
}

guiTick(){
    global guiHidden, fontSize
    if(hideGui) {
        Gui, Hide
        guiHidden := true
    }else if(guiHidden) {
        Gui, Show
        guiHidden := false
    }

    if (not guiHidden) {
        WinSet, AlwaysOnTop, On, WtileGui ahk_class AutoHotkeyGUI
        Gui, Font, s%fontSize%
        GuiControl, Font, LMain
        GuiControl, Font, RMain

        renderGui()
    }
}

loadGuiSettings() {
    global fontSize, startX, startY, startH, startW

    EnvGet, homedrive, homedrive
    EnvGet, homepath, homepath

    path := homedrive homepath "\.wtile\settings.json"
    doesFileExist := FileExist(path)
    if (!doesFileExist) {
        fontSize := 10
        startX := 0
        startY := 0
        startW := 100
        startH := 30
    } else {
        jf := new JSONFile(path)
        fontSize := jf.fontSize
        startX := jf.startX
        startY := jf.startY
        startW := jf.startW
        startH := jf.startH
    }
}

saveGuiSettings() {
    global fontSize

    EnvGet, homedrive, homedrive
    EnvGet, homepath, homepath

    if (!doesFileExist) {
        folder := homedrive homepath "\.wtile"
        FileCreateDir, %folder%
    }

    Gui,+LastFound
    WinGetPos,x,y,w,h

    path := homedrive homepath "\.wtile\settings.json"
    jf := new JSONFile(path)
    jf.fontSize := fontSize
    jf.startX := x
    jf.startY := y
    jf.startH := h
    jf.startW := w
    jf.Save(Prettify := true)
}
