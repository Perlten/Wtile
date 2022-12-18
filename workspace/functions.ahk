moveToWorkspace(toWorkspace){
    global lastWorkspace
    global currentWorkspace

    if (currentWorkspace == toWorkspace) {
        activateCurrentWindow()
        return
    }

    lastWorkspace := currentWorkspace

    currentWorkspace := toWorkspace

    workspace := getCurrentWorkspace()

    ; if the current window is not longer valid, change to the first valid one
    currentWindowID := workspace["windowList"][workspace["currentWindowIndex"]]
    if (!isValidWindowID(currentWindowID)) {
        changeCurrentWindowToFirstAvailable(workspace)
    }

    switchIcon(toWorkspace)
    activateCurrentWindow()
    renderGui()
}

activateCurrentWindow(){
    workspace := getCurrentWorkspace()
    windowList := workspace["windowList"]
    currentWindowIndex := workspace["currentWindowIndex"]
    windowIdToFocus := windowList[currentWindowIndex]
    activateWindow(windowIdToFocus)
}

getCurrentWorkspace(){
    global workspaceList
    global currentWorkspace

    workspace := workspaceList[currentWorkspace]
    return workspace
}

isValidWindowID(windowID){
    return WinExist("ahk_id" windowID)
}

changeCurrentWindowToFirstAvailable(ByRef workspace){
    windowList := workspace["windowList"]

    for index, windowID in windowList {
        if (isValidWindowID(windowID)) {
            workspace["currentWindowIndex"] := index
            return windowList[index]
        }
    }
    renderGui()
}
windowAlteration(wParam, lParam){
    global currentWorkspace

    if (wParam == 4 || wParam == 32772) { ; On active window change
        global lastWorkspace
        global workspaceList
    }

    if (wParam == 1) { ; 1 == Window created
        workspace := getCurrentWorkspace()
        currentWindowID := workspace["windowList", workspace["currentWindowIndex"]]

        if (isWindowFullscreen(currentWindowID)) {
            return
        }

        addWindowToWorkspace(currentWorkspace, lParam)
        renderGui()
    }

    if (wParam == 2) { ; 2 == window deleted
        workspace := getCurrentWorkspace()
        currentWindowID := workspace["windowList", workspace["currentWindowIndex"]]

        ; Only change window if closed window is current window (Might want to remove this part)
        if (lParam == currentWindowID){
            changeCurrentWindowToFirstAvailable(workspace)
        }
        renderGui()
    }

}

addWindowToWorkspace(workspaceIndex, windowId := ""){
    global workspaceList
    global currentWorkspace

    if (!windowId){
        windowId := getCurrentWindow()
    }

    ; removes windowid if already in workspace
    for i, workspace in workspaceList {
        windowList := workspace["windowList"]
        for j, currentWindowId in windowList {
            if (currentWindowId == windowId){
                windowList.RemoveAt(j)
                newWindowID := changeCurrentWindowToFirstAvailable(workspace)
                activateWindow(newWindowID)
            }
        }
    }

    workspace := workspaceList[workspaceIndex]

    ; adds window to workspace and overrides the first non valid window if exists
    windowList := workspace["windowList"]
    foundInvalidWindow := false
    for i, currentWindowId in windowList {
        if (!WinExist("ahk_id" currentWindowId)) {
            windowList[i] := windowId
            foundInvalidWindow := true
            ; set current window to be the new window just created
            workspace["currentWindowIndex"] := i
            break
        }
    }

    if (!foundInvalidWindow) {
        workspace["windowList"].Push(windowId)
        ; set current window to be the new window just created
        workspace["currentWindowIndex"] := workspace["windowList"].MaxIndex()
    }
    renderGui()
}

activateWindow(windowID){
    WinActivate, ahk_id %windowID%
    centerMouseOnActiveWindow()
    renderGui()
}

getCurrentWindow() {
    WinGet, currentWindowId ,, A
    return currentWindowId
}

switchToWindow(index){
    workspace := getCurrentWorkspace()

    if (workspace["currentWindowIndex"] == index) {
        activateCurrentWindow()
        renderGui()
        return
    }

    windowList := workspace["windowList"]
    toSwitchId := windowList[index]

    if (!WinExist("ahk_id" toSwitchId)){
        return
    }

    currentWindowIndex := workspace["currentWindowIndex"]
    workspace["lastWindow"] := currentWindowIndex

    activateWindow(toSwitchId)
    workspace["currentWindowIndex"] := index
    renderGui()

    return
}

changeWindowOrder(newIndex) {
    workspace := getCurrentWorkspace()
    currentWindowId := getCurrentWindow()

    windowList := workspace["windowList"]

    index := getIndexInArray(windowList, currentWindowId)
    switchPlaceInArray(windowList, index, newIndex)
    renderGui()
}

removeWindowFromWorkspace() {
    workspace := getCurrentWorkspace()
    workspace["windowList"].RemoveAt(workspace["currentWindowIndex"])
    renderGui()
}

switchToLastWindow() {
    workspace := getCurrentWorkspace()
    switchToWindow(workspace["lastWindow"])
    renderGui()
}

getWorkspaceIndex(){
    global currentWorkspace
    return currentWorkspace
}