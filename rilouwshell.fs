\ RilouwShell 0.1.0
\ Copyright (c) 2019 Jerome Martin
\ Released under the terms of the GNU GPL version 3
\ http://rilouw.eu/project/rilouwshell

require sdl.fs

require utils.fs
require machine.fs
require tileset.fs
require draw.fs

require element.fs
require page.fs
require event.fs

require label.fs
require button.fs

\ --- Global variables ---

512 value WIDTH
384 value HEIGHT
0 value current-machine

: current-page ( -- page )
  current-machine machine-state @
  state-page @
;

: render ( -- )
  buffer-surface current-page render-page
;

: start-machine ( init-xt machine -- )
  to current-machine

  WIDTH HEIGHT init-draw
  init-buttons

  \ give a way for the user to initialize elements
  ?dup if execute then

  begin
    quit? 0=
  while
    current-machine process-input
    render
    flip
    clean
    10 sdl-delay
  repeat

  shutdown
;

: stop-machine ( -- )
  true to quit?
;

: go-page ( page -- )
  drop
;
