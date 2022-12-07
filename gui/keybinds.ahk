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
    saveGuiSettings()
return