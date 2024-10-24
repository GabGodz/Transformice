F7::suspend
Toggle := false

F6::
Toggle := !Toggle

if (Toggle) {
    Hotkey, WheelUp, ScrollUp
    Hotkey, WheelDown, ScrollDown
} else {
    Hotkey, WheelUp, Off
    Hotkey, WheelDown, Off
}
return

ScrollUp:
Send, ^{WheelUp}
return

ScrollDown:
Send, ^{WheelDown}
return
