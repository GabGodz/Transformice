SetDefaultMouseSpeed, 0
SetKeyDelay, 1
F3::
Gui, Color, Black
Gui, Font, cWhite
Gui, Add, Text,, Digite o nome da sala:
Gui, Font, cBlack
Gui, Add, Edit, vUserInput w200
Gui, Font, cWhite
Gui, Add, Button, gSubmit Default, OK
Gui, Show,, FastRoom

return

Submit:
Gui, Submit
Gui, Destroy
if (UserInput = "")
{
    MsgBox, Você cancelou a entrada.
    return
}
CoordMode, Mouse, Screen
Click 815, 583
Sleep 300
Click 845, 455
Sleep 300
Click 706, 165
Sleep 100
Send, %UserInput%
Sleep 100
MouseMove 804, 250
Sleep 50
Click 3
Sleep 100
Send, 10
Sleep 100
Click 582, 294
Sleep 100
Click 647, 318
Sleep 100
Click 682, 335
Sleep 100
Click 602, 359
Sleep 100
Click 680, 510
return


