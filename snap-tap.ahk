; Scan Code Mappings:
; W   SC011
; A   SC01E
; S   SC01F
; D   SC020

#Requires AutoHotkey v2
#SingleInstance force
#MaxThreads 1
#MaxThreadsBuffer 1
Persistent true
ListLines False
KeyHistory 0
ProcessSetPriority "High"
A_MaxHotkeysPerInterval := 99000000
A_HotkeyInterval := 0

global a_held := 0  ; Variable that stores the actual keyboard state of the A key
global d_held := 0  ; Variable that stores the actual keyboard state of the D key
global a_scrip := 0 ; Variable that stores the state of the A key output from the script
global d_scrip := 0 ; Variable that stores the state of the D key output from the script

global w_held := 0
global s_held := 0
global w_scrip := 0
global s_scrip := 0

/* Un-comment this section if you want the end button to open a dialog that asks to close the script
ToolTip "Script Enabled",A_ScreenWidth/2,A_ScreenHeight/2
SetTimer () => ToolTip(), -1000

End:: ; <--- this button exits the script
{ 
    Result := MsgBox("Are you sure you want to exit?",, 4)
    if Result = "Yes" 
    {
        ExitApp
    }
}
*/

*$SC01E:: ; *$a:: ; Every time the a key is pressed, * to include occurences with modifiers (shift, control, alt, etc)
{   
    global

    a_held := 1  ; Track the actual state of the A key
    
    if d_scrip
    { 
        d_scrip := 0
        SendInput "{Blind}{SC020 up}" ; Release the D key if it's held down, {Blind} so it includes any key modifiers (shift primarily)
    }
    
    a_scrip := 1
    SendInput "{Blind}{SC01E down}" ; A down key
}

*$SC01E up:: ; *$a up:: ; Every time the a key is released
{    
    global

    a_held := 0
    
    if a_scrip
    {
        a_scrip := 0
        SendInput "{Blind}{SC01E up}"  ; A up key
    }
        
    if d_held && !d_scrip
    {
        d_scrip := 1
        SendInput "{Blind}{SC020 down}"  ; D down key if it's held
    }
}

*$SC020:: ; *$d::
{    
    global

    d_held := 1
    
    if a_scrip
    {
        a_scrip := 0
        SendInput "{Blind}{SC01E up}"  ; Release the A key if it's held down
    }
    
    d_scrip := 1
    SendInput "{Blind}{SC020 down}"  ; D down key
}

*$SC020 up:: ; *$d up::
{    
    global

    d_held := 0
    
    if d_scrip
    {
        d_scrip := 0
        SendInput "{Blind}{SC020 up}"  ; D up key
    }
    
    if a_held && !a_scrip
    {
        a_scrip := 1
        SendInput "{Blind}{SC01E down}"  ; A down key if it's held
    }
}

*$SC011:: ; *$w::
{    
    global

    w_held := 1

    if s_scrip 
    {
        s_scrip := 0
        SendInput "{Blind}{SC01F up}"
    }

    w_scrip := 1
    SendInput "{Blind}{SC011 down}"
}

*$SC011 up:: ; *$w up::
{    
    global

    w_held := 0

    if w_scrip
    {
        w_scrip := 0
        SendInput "{Blind}{SC011 up}"
    }

    if s_held && !s_scrip 
    {
        s_scrip := 1
        SendInput "{Blind}{SC01F down}"
    }
}

*$SC01F:: ; *$s::
{    
    global

    s_held := 1

    if w_scrip 
    {
        w_scrip := 0
        SendInput "{Blind}{SC011 up}"
    }

    s_scrip := 1
    SendInput "{Blind}{SC01F down}"
}

*$SC01F up:: ; *$s up::
{    
    global

    s_held := 0

    if s_scrip 
    {
        s_scrip := 0
        SendInput "{Blind}{SC01F up}"
    }

    if w_held && !w_scrip 
    {
        w_scrip := 1
        SendInput "{Blind}{SC011 down}"
    }
}
