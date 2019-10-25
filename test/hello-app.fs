\ RilouwShell 0.1.0
\ Copyright (c) 2019 Jerome Martin
\ Released under the terms of the GNU GPL version 3
\ http://rilouw.eu/project/rilouwshell

require ../rilouwshell.fs

create okay-text s" Okay" s,
create exit-text s" Quitter" s,
create hello-text s\" Bonjour J\xE9r\xF4me." s,
create details-text s" Bienvenue dans RilouwOS 0.1.0 !" s,
create back-text s" Retour" s,

\ events
0 constant #EXIT
1 constant #DETAILS
2 constant #GOBACK

page
\ ---LABELS-----------------------------
\ x y w  text            primary
\ --------------------------------------
  3 1 10 hello-text      false    label,
\ --------------------------------------

\ ---BUTTONS-------------------------------------------------
\ x y w  text            primary  enabled  event
\ -----------------------------------------------------------
  1 3 6  exit-text       false    true     #EXIT      button,
  8 3 6  okay-text       true     true     #DETAILS   button,
\ -----------------------------------------------------------
end-page HELLO-PAGE

page
\ ---LABELS-----------------------------
\ x y w  text            primary
\ --------------------------------------
  3 1 20 details-text    false    label,

\ ---BUTTONS-------------------------------------------------
\ x y w  text            primary  enabled  event
\ -----------------------------------------------------------
  1 3 5  back-text       false    true     #GOBACK    button,
end-page DETAILS-PAGE

state-machine
  HELLO-PAGE state
    #EXIT    -> stop-machine         <-
    #DETAILS -> DETAILS-PAGE go-page <-
  end-state
  DETAILS-PAGE state
    #GOBACK  -> HELLO-PAGE go-page <-
  end-state
end-state-machine HELLO-APP

\ --- Main program ---

false HELLO-APP start-machine
