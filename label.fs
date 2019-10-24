\ RilouwShell 0.1.0
\ Copyright (c) 2019 Jerome Martin
\ Released under the terms of the GNU GPL version 3
\ http://rilouw.eu/project/rilouwshell

struct
  cell% field label-x
  cell% field label-y
  cell% field label-w
  ptr%  field label-text
  cell% field label-primary

  ptr% field label-surface
end-struct label%

: init-label ( label -- )
  >r

  \ init surface
  r@ label-w @ tile/pos
  TILE
  new-surface
  r@ label-surface !

  \ text
  r@ label-primary @ invert-font
  r@ label-surface @
  r> label-text @
  0 3
  write-at
;

: no-surface? ( label -- b )
  label-surface @ 0=
;

: get-label-surface ( label -- surface )
  dup no-surface? if
    dup init-label
  then
  label-surface @
;

: (render-label) ( surface label -- )
  >r
  r@ get-label-surface
  swap
  r@ label-x @ tile/pos
  r> label-y @ tile/pos
  blit
;

' (render-label) is render-label

: (get-label-rect) ( label -- x y w h )
  >r
  r@ label-x @ tile/pos
  r@ label-y @ tile/pos
  r> label-w @ tile/pos
  TILE
;

' (get-label-rect) is get-label-rect

: label, ( #element x y w text primary -- element type #element )
  label% %allot >r
  r@ label-primary !
  r@ label-text !
  r@ label-w !
  r@ label-y !
  r@ label-x !
  1+ \ increment #element
  r> TYPE-LABEL rot
;
