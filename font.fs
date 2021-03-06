\ RilouwShell 0.1.0
\ Copyright (c) 2019 Jerome Martin
\ Released under the terms of the GNU GPL version 3
\ http://rilouw.eu/project/rilouwshell

create font-tileset tileset% %allot drop

16 constant FONT-W
11 constant CHAR-W
14 constant CHAR-H

: init-font ( -- )
  s" res/font.png0" load-image
  font-tileset tileset-surface !
  FONT-W font-tileset tileset-w !
  CHAR-W font-tileset tileset-tile-w !
  CHAR-H font-tileset tileset-tile-h !
;

: invert-font ( b -- )
  if InvertedPalette else Palette then
  font-tileset tileset-surface @ swap
  0 3 sdl-set-colors drop
;

: write-at { surface str x y -- }
  str c@ 0 do
    surface
    x i CHAR-W * + y
    font-tileset
    str 1+ i + c@ 32 -
    draw-tileset
  loop
;
