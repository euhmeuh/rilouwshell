\ RilouwShell 0.1.0
\ Copyright (c) 2019 Jerome Martin
\ Released under the terms of the GNU GPL version 3
\ http://rilouw.eu/project/rilouwshell

require sdl.fs

require utils.fs
require tileset.fs
require draw.fs

require element.fs
require page.fs
require event.fs

require label.fs
require button.fs

\ --- Constants ---

512 constant WIDTH
384 constant HEIGHT

\ --- Global variables ---

create hello-label label% %allot drop
create ok-btn button% %allot drop
create big-btn button% %allot drop

\ --- Main program ---

create ok-button-text s" Okay" s,
create big-button-text s" Hello world!" s,
create hello-label-text s\" Bonjour J\xE9r\xF4me." s,

: init-page ( -- )
  3 hello-label label-x !
  1 hello-label label-y !
  10 hello-label label-w !
  hello-label-text hello-label label-text !
  hello-label init-label
  hello-label TYPE-LABEL add-to-page

  1 ok-btn button-x !
  3 ok-btn button-y !
  4 ok-btn button-w !
  true ok-btn button-primary !
  ok-button-text ok-btn button-text !
  ok-btn init-button
  ok-btn TYPE-BUTTON add-to-page

  6 big-btn button-x !
  3 big-btn button-y !
  9 big-btn button-w !
  big-button-text big-btn button-text !
  big-btn init-button
  big-btn TYPE-BUTTON add-to-page

;

: render ( -- )
  buffer-surface render-page
;

: start-main-loop ( -- )
  WIDTH HEIGHT init-draw
  init-buttons
  init-page

  begin
    quit? 0=
  while
    process-input
    render
    flip
    clean
    10 sdl-delay
  repeat

  shutdown
;

start-main-loop
