Print(string){
    ToolTip `n%string%
}

isWindowFullscreen(WinID) {
    ;https://autohotkey.com/board/topic/38882-detect-fullscreen-application/
    ;checks if the specified window is full screen
    ;code from NiftyWindows source
    ;(with only slight modification)

    ;use WinExist of another means to get the Unique ID (HWND) of the desired window

    if ( !WinID )
        return false

    WinGet, WinMinMax, MinMax, ahk_id %WinID%
    WinGetPos, WinX, WinY, WinW, WinH, ahk_id %WinID%

    if (WinMinMax = 0) && (WinX = 0) && (WinY = 0) && (WinW = A_ScreenWidth) && (WinH = A_ScreenHeight)
    {
        WinGetClass, WinClass, ahk_id %WinID%
        WinGet, WinProcessName, ProcessName, ahk_id %WinID%
        SplitPath, WinProcessName, , , WinProcessExt

        if (WinClass != "Progman") && (WinProcessExt != "scr")
        {
            ;program is full-screen
            return true
        }
    }
    return false
}

centerMouseOnActiveWindow(){
    CoordMode,Mouse,Screen
    WinGetPos, winTopL_x, winTopL_y, width, height, A
    winCenter_x := winTopL_x + width/2
    winCenter_y := winTopL_y + height/2
    DllCall("SetCursorPos", int, winCenter_x, int, winCenter_y)
    return
}

getIndexInArray(array, item) {
    for index, element in array{
        if (element == item) {
            return index
        }
    }
    return -1
}

switchPlaceInArray(array, indexA, indexB){
    temp := array[indexA]
    array[indexA] := array[indexB]
    array[indexB] := temp
}

switchToLastWorkspace(){
    global lastWorkspace
    moveToWorkspace(lastWorkspace)
}

switchIcon(index){
    pathToIcon := A_ScriptDir "\icons\icon" index ".ico"
    IfExist, %pathToIcon%
        Menu, Tray, Icon, %pathToIcon%
}

ReadInteger( p_address, p_offset, p_size, p_hex=true ) {
    value = 0
    old_FormatInteger := a_FormatInteger
    if ( p_hex )
        SetFormat, integer, hex
    else
        SetFormat, integer, dec
    loop, %p_size%
        value := value+( *( ( p_address+p_offset )+( a_Index-1 ) ) << ( 8* ( a_Index-1 ) ) )
    SetFormat, integer, %old_FormatInteger%
    return, value
}