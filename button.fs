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

: init-button  ( button -- )
  >r

  r@ button-w @ tile/pos 2 +
  TILE 4 +
  new-surface r@ button-surface-normal !

  \ Base rectangle
  r@ button-surface-normal @ NULL Orange sdl-fill-rect drop

  \ Black inside
  r@ button-surface-normal @
  1 1
  r@ button-w @ tile/pos
  TILE
  Black draw-rect

  \ Full corners
  r@ button-surface-normal @
  1 1 top-left draw-corner

  r@ button-surface-normal @
  1
  TILE 1-
  bottom-left draw-corner

  r@ button-surface-normal @
  r@ button-w @ tile/pos 1 -
  1
  top-right draw-corner

  r@ button-surface-normal @
  r@ button-w @ tile/pos 1 -
  TILE 1-
  bottom-right draw-corner

  \ Empty corners
  r@ button-surface-normal @
  0 0 top-left inverted draw-corner

  r@ button-surface-normal @
  0
  TILE 2 +
  bottom-left inverted draw-corner

  r@ button-surface-normal @
  r@ button-w @ tile/pos
  0
  top-right inverted draw-corner

  r@ button-surface-normal @
  r@ button-w @ tile/pos
  TILE 2 +
  bottom-right inverted draw-corner

  r> drop
;

: render-button { surface button -- }
  button button-surface-normal @
  surface
  button button-x @ tile/pos 1-
  button button-y @ tile/pos 1-
  blit
;
