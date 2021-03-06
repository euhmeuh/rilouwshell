\ RilouwShell 0.1.0
\ Copyright (c) 2019 Jerome Martin
\ Released under the terms of the GNU GPL version 3
\ http://rilouw.eu/project/rilouwshell

0 value quit?

create tmp-event sdl-event% %allot drop

: update-mouse ( -- )
  mouse-x mouse-y sdl-get-mouse-state mouse-buttons !
;

: update-clicked ( mouse-down? -- )
  if
    hovered-element to clicked-element
  else
    clicked-element hovered-element =
    if clicked-element click-element then
    reset-clicked
  then
;

: process-input ( machine -- )
  machine-state @ state-page @ >r
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

      SDL_MOUSEMOTION of
        update-mouse
        r@ update-focus
      endof

      SDL_MOUSEBUTTONUP   of update-mouse  false update-clicked endof
      SDL_MOUSEBUTTONDOWN of update-mouse  true  update-clicked endof
    endcase
  repeat

  r> drop
;
