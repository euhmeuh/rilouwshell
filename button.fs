\ RilouwShell 0.1.0
\ Copyright (c) 2019 Jerome Martin
\ Released under the terms of the GNU GPL version 3
\ http://rilouw.eu/project/rilouwshell

create button-tileset tileset% %allot drop

0 constant BORDER-LEFT
1 constant BORDER-RIGHT

: CLICKED   2 + ;
: INVERTED  4 + ;

sprite
l: ..OO..........OO........
l: .OOOO........OOOO.......
l: OOOOOO..OO..OOBBOO..OO..
l: OOOOOO.OOOO.OBBBBO.OOOO.
l: OOOOOOOOOOOOOBBBBOOOBBOO
l: OOOOOOOOOOOOOBBBBOOBBBBO
l: OOOOOOOOOOOOOBBBBOOBBBBO
l: OOOOOOOOOOOOOBBBBOOBBBBO
l: OOOOOOOOOOOOOBBBBOOBBBBO
l: OOOOOOOOOOOOOBBBBOOBBBBO
l: OOOOOOOOOOOOOBBBBOOBBBBO
l: OOOOOOOOOOOOOBBBBOOBBBBO
l: OOOOOOOOOOOOOBBBBOOBBBBO
l: OOOOOOOOOOOOOBBBBOOBBBBO
l: OOOOOOOOOOOOOBBBBOOBBBBO
l: OBOOBOOOOOOOOOBBOOOBBBBO
l: OBBBBOOOOOOOOOOOOOOBBBBO
l: OBBBBOOOOOOOOOOOOOOOBBOO
l: .OBBO..OOOO..OOOO..OOOO.
l: ..OO....OO....OO....OO..
end-sprite BUTTON-SPRITE

struct
  cell% field button-text
  cell% field button-primary
  cell% field button-enabled
  cell% field button-x
  cell% field button-y
  cell% field button-w

  ptr% field button-surface-normal
  ptr% field button-surface-clicked
end-struct button%

: new-button-surface ( button -- surface )
  button-w @ tile/pos 6 +
  TILE 9 +
  new-surface
;

: init-buttons ( -- )
  BUTTON-SPRITE load-sprite
  button-tileset tileset-surface !
  8 button-tileset tileset-w !
  3 button-tileset tileset-tile-w !
  20 button-tileset tileset-tile-h !
;

: init-button ( button -- )
  >r

  \ init surfaces
  r@ new-button-surface
  r@ button-surface-normal !
  r@ new-button-surface
  r@ button-surface-clicked !

  \ normal borders
  r@ button-surface-normal @
  2 2 button-tileset
  BORDER-LEFT INVERTED draw-tileset

  r@ button-surface-normal @
  r@ button-w @ tile/pos 1+
  2
  button-tileset
  BORDER-RIGHT INVERTED draw-tileset

  \ normal lines
  r@ button-surface-normal @
  5 2
  r@ button-w @ tile/pos 4 -
  Orange draw-hline

  r@ button-surface-normal @
  5
  TILE 3 +
  r@ button-w @ tile/pos 4 -
  3
  Orange draw-rect

  \ clicked borders
  r@ button-surface-clicked @
  2 2 button-tileset
  BORDER-LEFT INVERTED CLICKED draw-tileset

  r@ button-surface-clicked @
  r@ button-w @ tile/pos 1+
  2
  button-tileset
  BORDER-RIGHT INVERTED CLICKED draw-tileset

  \ clicked lines
  r@ button-surface-clicked @
  5 4
  r@ button-w @ tile/pos 4 -
  Orange draw-hline

  r@ button-surface-clicked @
  5
  TILE 5 +
  r@ button-w @ tile/pos 4 -
  Orange draw-hline

  r> drop
;

: get-current-button-surface ( button -- surface )
  dup focused? if
    button-surface-clicked @
  else
    button-surface-normal @
  then
;

: (render-button) ( surface button -- )
  >r
  r@ get-current-button-surface
  swap
  r@ button-x @ tile/pos 3 -
  r> button-y @ tile/pos 3 -
  blit
;

' (render-button) is render-button

: (get-button-rect) ( button -- x y w h )
  >r
  r@ button-x @ tile/pos
  r@ button-y @ tile/pos
  r> button-w @ tile/pos
  TILE
;

' (get-button-rect) is get-button-rect
