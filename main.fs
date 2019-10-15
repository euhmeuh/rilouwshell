\ RilouwShell 0.1.0
\ Copyright (c) 2019 Jerome Martin
\ Released under the terms of the GNU GPL version 3
\ http://rilouw.eu/project/rilouwos

require sdl.fs

require utils.fs
require draw.fs
require button.fs

\ --- Constants ---

512 constant WIDTH
384 constant HEIGHT

\ --- Global variables ---
0 value quit?

create tmp-event sdl-event% %allot drop

create ok-btn button% %allot drop

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
;

: process-input ( -- )
  begin
    tmp-event sdl-poll-event \ while there is an event
  while
    tmp-event sdl-event-type c@
    case
      SDL_KEYDOWN of 
        tmp-event sdl-event-key sdl-keysym-sym uw@
        case
          SDLK_ESCAPE of true to quit? endof
          SDLK_q      of true to quit? endof
        endcase
      endof

      SDL_MOUSEMOTION of endof
    endcase
  repeat
;

: render ( -- )
  buffer-surface ok-btn render-button
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
