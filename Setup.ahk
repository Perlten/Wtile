#NoEnv
#SingleInstance, Force
SendMode, Input
SetBatchLines, -1
SetWorkingDir, %A_ScriptDir%

downloadFile(url, dir := ".", filename := "") {
    whr := ComObjCreate("WinHttp.WinHttpRequest.5.1")
    whr.Open("GET", url, true)
    whr.Send()
    whr.WaitForResponse()

    if !FileExist(dir)
        FileCreateDir, %dir%

    if filename is space
    {
        try
        RegExMatch( whr.GetResponseHeader("Content-Disposition"), "filename=""\K[^""]+", filename )
        catch
            RegExMatch( whr.Option(1), "[^/]+$", filename ) ; 1 = WinHttpRequestOption_URL
    }

    body := whr.ResponseBody
    data := NumGet(ComObjValue(body) + 8 + A_PtrSize, "UInt")
    size := body.MaxIndex() + 1
    FileOpen(dir "\" filename, "w").RawWrite(data + 0, size)
}

version := "v1.3"

EnvGet, homedrive, homedrive
EnvGet, homepath, homepath

url := "https://github.com/Perlten/Wtile/releases/download/" version "/wtile.exe"
location := homedrive homepath "\AppData\Local\wtile"
downloadFile(url, location, "wtile.exe")

wtileLocation := homedrive homepath "\AppData\Local\wtile\wtile.exe"
programLocation := homedrive homepath "\AppData\Roaming\Microsoft\Windows\Start Menu\Programs\wtile.lnk"
FileCreateShortcut, %wtileLocation%, %programLocation%

startLocation := homedrive homepath "\AppData\Roaming\Microsoft\Windows\Start Menu\Programs\Startup\wtile.lnk"
FileCreateShortcut, %wtileLocation%, %startLocation%