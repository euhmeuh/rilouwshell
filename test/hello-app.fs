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

(

\ events
0 constant #EXIT
1 constant #DETAILS
2 constant #GOBACK

page
\ x y w  text            primary
  3 1 10 hello-text      false    <label>

\ x y w  text            primary  enabled  on-clicked
  1 3 6  exit-text       false    true     #EXIT      <button>
  8 3 6  okay-text       true     true     #DETAILS   <button>
end-page HELLO-PAGE

page
\ x y w  text            primary
  3 1 15 details-text    false    <label>

\ x y w  text            primary  enabled  on-clicked
  1 3 5  back-text       false    true     #GOBACK    <button>
end-page DETAILS-PAGE

state-machine
  HELLO-PAGE starts
  #EXIT     will: stop-machine ;
  #DETAILS  will: DETAILS-PAGE go-page ;
  #GOBACK   will: HELLO-PAGE go-page ;
end-state-machine HELLO-APP

HELLO-APP start-machine

)

create hello-label label% %allot drop
create exit-btn button% %allot drop
create okay-btn button% %allot drop

create hello-page page% %allot drop
create hello-page-content
20 cells allot

create hello-app machine% %allot drop

\ --- Main program ---

: init-app ( -- )
  hello-page hello-app machine-page !

  hello-page-content hello-page page-content !
  0 hello-page page-cursor !

   3 hello-label label-x !
   1 hello-label label-y !
  10 hello-label label-w !
  hello-text hello-label label-text !
  hello-label init-label
  hello-label TYPE-LABEL hello-page add-to-page

  1 exit-btn button-x !
  3 exit-btn button-y !
  6 exit-btn button-w !
  exit-text exit-btn button-text !
  exit-btn init-button
  exit-btn TYPE-BUTTON hello-page add-to-page

  8 okay-btn button-x !
  3 okay-btn button-y !
  6 okay-btn button-w !
  true okay-btn button-primary !
  okay-text okay-btn button-text !
  okay-btn init-button
  okay-btn TYPE-BUTTON hello-page add-to-page
;

' init-app hello-app start-machine
