{space}::
ControlClick , x484 y509 , Transformice - Baffler's Standalone ,, Left , 1 , NA   
ControlClick , x1064 y509 , Transformice - Baffler's Standalone ,, Left , 1 , NA   
; Obtém a posição atual do mouse
MouseGetPos, xpos, ypos
; Subtrai 5 dos valores X e Y para mover o mouse 5 pixels para trás
new_xpos := xpos - 5
new_ypos := ypos - 5
; Move o mouse para a nova posição
MouseMove, new_xpos, new_ypos
; Move o mouse de volta para a posição original
MouseMove, xpos, ypos
return