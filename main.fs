require sdl.fs

\ --- Constants ---

512 constant WIDTH
384 constant HEIGHT
 16 constant TILE

\ --- Colors ---
0 value Black
0 value Orange

\ --- Global variables ---
0 value quit?
0 value screen-surface
0 value buffer-surface

create tmp-event sdl-event% %allot drop
create tmp-rect sdl-rect% %allot drop

\ --- Utils ---

: log ( x -- x ) dup . cr ;
: terminate-str ( str len -- str ) over + 1- 0 swap c! ;
: ?error ( f str len -- )  rot if type cr bye else 2drop then ;

\ convert from tile to pixel coordinates
: tile/pos ( tile -- pixel ) TILE * ;
: rect/pos ( tx1 ty1 tx2 ty2 -- px1 py1 px2 py2 )
  swap tile/pos TILE + 1-
  swap tile/pos TILE + 1-
  2swap
  swap tile/pos
  swap tile/pos
  2swap
;

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

: draw-rect { surface x1 y1 x2 y2 color -- }
  x1 tmp-rect sdl-rect-x w!
  y1 tmp-rect sdl-rect-y w!
  x2 x1 - tmp-rect sdl-rect-w w!
  y2 y1 - tmp-rect sdl-rect-h w!
  surface tmp-rect color sdl-fill-rect drop
;

\ --- Main program ---

: init-colors
  screen-surface sdl-surface-format @
  dup   0   0   0 sdl-map-rgb to Black
      233 129  50 sdl-map-rgb to Orange
;

: init
  SDL_INIT_VIDEO sdl-init
  0<> s" Unable to initialize SDL" ?error

  1024 768 16 SDL_SWSURFACE sdl-set-video-mode
  dup 0< s" Unable to set video mode" ?error
  to screen-surface

  s" Rilouw Desktop0" terminate-str NULL sdl-wm-set-caption

  s" mockups/ui.png0" load-image to buffer-surface
  \ SDL_SWSURFACE 512 384 16 0 0 0 0 sdl-create-rgb-surface to buffer-surface
;

: process-input
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

: render
  \ buffer-surface 2 2 30 23 rect/pos Orange draw-rect
;

: flip
  buffer-surface NULL screen-surface NULL sdl-blit-surface drop
  screen-surface sdl-flip drop
;

: clean
  screen-surface 0 Black sdl-fill-rect drop
;

: shutdown
  buffer-surface sdl-free-surface 
  sdl-quit
;

: start-main-loop ( -- )
  init
  init-colors

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
