#SingleInstance, Force
F3::suspend


eCount := 0

$*f::
    eCount++
    
    if (eCount = 1) {
        SendInput 3xxxx^{WheelDown}
    }
    else if (eCount = 2) {
        SendInput 3xxxx^{WheelDown}^{WheelDown}
    }
    else if (eCount = 3) {
        SendInput 3xxxx^{WheelDown}^{WheelDown}^{WheelDown}
        eCount := 0
    }
return

+$*g::
    eCount++
    
    if (eCount = 1) {
        SendInput 2xxxx^{WheelUp}
    }
    else if (eCount = 2) {
        SendInput 2xxxx^{WheelUp}^{WheelUp}
    }
    else if (eCount = 3) {
        SendInput 2xxxx^{WheelUp}^{WheelUp}^{WheelUp}
        eCount := 0
    }
return

v::sendinput 3xxxxxx

eCount := 0

$*3::
    eCount++
    
    if (eCount = 1) {
        SendInput 3^{Wheelup}
    }
    else if (eCount = 2) {
        SendInput 3^{Wheelup}^{Wheelup}
    }
    else if (eCount = 3) {
        SendInput 3^{Wheelup}^{Wheelup}^{Wheelup}
        eCount := 0
    }
return

$*2::
    eCount++
    
    if (eCount = 1) {
        SendInput 2^{WheelDown}
    }
    else if (eCount = 2) {
        SendInput 2^{WheelDown}^{WheelDown}
    }
    else if (eCount = 3) {
        SendInput 2^{WheelDown}^{WheelDown}^{WheelDown}
        eCount := 0
    }
return