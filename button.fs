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
end-struct button%

: render-button  { surface btn -- }
  \ Base rectangle
  surface
  btn button-x @ tile/pos 1-
  btn button-y @ tile/pos 1-
  btn button-w @ tile/pos 2 +
  1 tile/pos 4 +
  Orange draw-rect

  \ Black inside
  surface
  btn button-x @ tile/pos
  btn button-y @ tile/pos
  btn button-w @ tile/pos
  1 tile/pos
  Black draw-rect

  \ Full corners
  surface
  btn button-x @ tile/pos
  btn button-y @ tile/pos
  top-left draw-corner

  surface
  btn button-x @ tile/pos
  btn button-y @ tile/pos 14 +
  bottom-left draw-corner

  surface
  btn button-x @ tile/pos
  btn button-w @ tile/pos + 2 -
  btn button-y @ tile/pos
  top-right draw-corner

  surface
  btn button-x @ tile/pos
  btn button-w @ tile/pos + 2 -
  btn button-y @ tile/pos 14 +
  bottom-right draw-corner

  \ Empty corners
  surface
  btn button-x @ tile/pos 1-
  btn button-y @ tile/pos 1-
  top-left inverted draw-corner

  surface
  btn button-x @ tile/pos 1-
  btn button-y @ tile/pos 17 +
  bottom-left inverted draw-corner

  surface
  btn button-x @ tile/pos
  btn button-w @ tile/pos + 1-
  btn button-y @ tile/pos 1-
  top-right inverted draw-corner

  surface
  btn button-x @ tile/pos
  btn button-w @ tile/pos + 1-
  btn button-y @ tile/pos 17 +
  bottom-right inverted draw-corner
;
