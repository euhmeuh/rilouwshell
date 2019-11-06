\ RilouwShell 0.1.0
\ Copyright (c) 2019 Jerome Martin
\ Released under the terms of the GNU GPL version 3
\ http://rilouw.eu/project/rilouwshell

create button-tileset tileset% %allot drop

0 constant BUTTON-BORDER-LEFT
1 constant BUTTON-BORDER-RIGHT

0 value DRAW-CLICKED
0 value DRAW-INVERTED

: clicked-tile? DRAW-CLICKED if 2 + then ;
: inverted-tile? DRAW-INVERTED if 4 + then ;

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
  cell% field button-x
  cell% field button-y
  cell% field button-w
  ptr%  field button-text
  cell% field button-primary
  cell% field button-enabled
  cell% field button-event

  ptr% field button-surface-normal
  ptr% field button-surface-clicked
end-struct button%

: new-button-surface ( button -- surface )
  button-w @ tile/pos 2 +
  TILE 4 +
  new-surface
;

: init-buttons ( -- )
  BUTTON-SPRITE load-sprite
  button-tileset tileset-surface !
  8 button-tileset tileset-w !
  3 button-tileset tileset-tile-w !
  20 button-tileset tileset-tile-h !
;

: select-button-surface ( button -- surface )
  DRAW-CLICKED if
    button-surface-clicked @
  else
    button-surface-normal @
  then
;

: draw-button-borders ( button -- )
  >r

  r@ select-button-surface

  dup
  0 0
  button-tileset
  BUTTON-BORDER-LEFT
  inverted-tile? clicked-tile? draw-tileset

  ( surface )
  r> button-w @ tile/pos 1-
  0
  button-tileset
  BUTTON-BORDER-RIGHT
  inverted-tile? clicked-tile? draw-tileset
;

: draw-button-lines ( button -- )
  >r

  r@ select-button-surface

  \ higher line or rect
  dup
  3  DRAW-CLICKED if 2 else 0 then
  r@ button-w @ tile/pos 4 -
  r@ button-primary @ if
    TILE 1+ Orange draw-rect
  else
    Orange draw-hline
  then

  \ lower-line or rect
  ( surface )
  DRAW-CLICKED
  r@ button-primary @
  or if
    3  TILE 3 +
    r> button-w @ tile/pos 4 -
    Orange draw-hline
  else
    3  TILE 1+
    r> button-w @ tile/pos 4 -
    3
    Orange draw-rect
  then
;

: draw-button-text ( button -- )
  >r

  r@ button-primary @ invert-font

  r@ select-button-surface
  r> button-text @
  9
  DRAW-CLICKED if 5 else 3 then
  write-at
;

: init-button ( button -- )
  >r

  \ init surfaces
  r@ new-button-surface
  r@ button-surface-normal !
  r@ new-button-surface
  r@ button-surface-clicked !

  r@ button-primary @ 0= to DRAW-INVERTED

  \ normal button
  false to DRAW-CLICKED
  r@ draw-button-borders
  r@ draw-button-lines
  r@ draw-button-text

  \ clicked button
  true to DRAW-CLICKED
  r@ draw-button-borders
  r@ draw-button-lines
  r> draw-button-text
;

: no-surface? ( button -- b )
  button-surface-normal @ 0=
;

: get-button-surface ( button -- surface )
  dup no-surface? if
    dup init-button
  then

  dup clicked? if
    button-surface-clicked @
  else
    button-surface-normal @
  then
;

: (render-button) ( surface button -- )
  >r
  r@ get-button-surface
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

: button, ( #element x y w text primary enabled event -- element type #element )
  button% %allot >r
  r@ button-event !
  r@ button-enabled !
  r@ button-primary !
  r@ button-text !
  r@ button-w !
  r@ button-y !
  r@ button-x !
  1+ \ increment #element
  r> TYPE-BUTTON rot
;
