#h::
    global hideGui
    hideGui := !hideGui
return

#^2::
    global fontSize
    fontSize := fontSize + 1
return

#^1::
    global fontSize
    fontSize := fontSize - 1
return

#^3::
    global resizeEnable
    resizeEnable := !resizeEnable
    if resizeEnable {
        Gui +Resize
    }else {
        Gui -Resize
    }
return

#^0::
    saveGuiSettings()
return