\ RilouwShell 0.1.0
\ Copyright (c) 2019 Jerome Martin
\ Released under the terms of the GNU GPL version 3
\ http://rilouw.eu/project/rilouwshell

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
  button-w @ tile/pos 2 +
  TILE 6 +
  new-surface
;

: init-normal-surface  ( button -- )
  >r

  r@ new-button-surface
  r@ button-surface-normal !

  \ Base rectangle
  r@ button-surface-normal @
  0 0
  r@ button-w @ tile/pos 2 +
  TILE 4 +
  Orange draw-rect

  \ Black inside
  r@ button-surface-normal @
  1 1
  r@ button-w @ tile/pos
  TILE
  Black draw-rect

  \ Full corners
  r@ button-surface-normal @
  1 1 CORNER-TL draw-corner

  r@ button-surface-normal @
  1
  TILE 1-
  CORNER-BL draw-corner

  r@ button-surface-normal @
  r@ button-w @ tile/pos 1 -
  1
  CORNER-TR draw-corner

  r@ button-surface-normal @
  r@ button-w @ tile/pos 1 -
  TILE 1-
  CORNER-BR draw-corner

  \ Empty corners
  r@ button-surface-normal @
  0 0 CORNER-TL INVERTED draw-corner

  r@ button-surface-normal @
  0
  TILE 2 +
  CORNER-BL INVERTED draw-corner

  r@ button-surface-normal @
  r@ button-w @ tile/pos
  0
  CORNER-TR INVERTED draw-corner

  r@ button-surface-normal @
  r@ button-w @ tile/pos
  TILE 2 +
  CORNER-BR INVERTED draw-corner

  r> drop
;

: init-clicked-surface ( button -- )
  >r

  r@ new-button-surface r@ button-surface-clicked !

  \ copy normal button
  r@ button-surface-normal @
  r@ button-surface-clicked @
  0 2 blit

  \ erase bottom corners
  r@ button-surface-clicked @
  0
  TILE 2 +
  CORNER-BL INVERTED draw-corner

  r@ button-surface-clicked @
  r@ button-w @ tile/pos
  TILE 2 +
  CORNER-BR INVERTED draw-corner

  \ erase bottom lines
  r@ button-surface-clicked @
  1 20
  r@ button-w @ tile/pos
  Black draw-hline

  r@ button-surface-clicked @
  2 21
  r@ button-w @ tile/pos 2 -
  Black draw-hline

  r> drop
;

: init-button ( button -- )
  dup init-normal-surface
  init-clicked-surface
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
  r@ button-x @ tile/pos 1-
  r> button-y @ tile/pos 1-
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
