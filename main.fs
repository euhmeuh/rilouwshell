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

require button.fs

\ --- Constants ---

512 constant WIDTH
384 constant HEIGHT

\ --- Global variables ---

create ok-btn button% %allot drop
create big-btn button% %allot drop

\ --- Utils ---

: load-image ( str len -- surface )
  2dup 2>r \ keep filename for error
  terminate-str sdl-load-image dup 0= if
    ." Unable to load image file: '" 2r> type ." '" cr
  else
    dup sdl-display-format
    swap sdl-free-surface
    2r> 2drop
  then
;

\ --- Main program ---

: init-page ( -- )
  1 ok-btn button-x !
  1 ok-btn button-y !
  4 ok-btn button-w !
  ok-btn init-button
  ok-btn TYPE-BUTTON add-to-page

  1 big-btn button-x !
  3 big-btn button-y !
  8 big-btn button-w !
  big-btn init-button
  big-btn TYPE-BUTTON add-to-page
;

: render ( -- )
  buffer-surface render-page
;

: start-main-loop ( -- )
  WIDTH HEIGHT init-draw
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
