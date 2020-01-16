\ RilouwShell 0.1.0
\ Copyright (c) 2019 Jerome Martin
\ Released under the terms of the GNU GPL version 3
\ http://rilouw.eu/project/rilouwshell

(
struct
  cell% field input-label
  cell% field input-text
  cell% field input-x
  cell% field input-y
  cell% field input-w
end-struct input%

struct
  cell% field area-content
  cell% field area-x
  cell% field area-y
  cell% field area-w
  cell% field area-h
end-struct area%

struct
  cell% field toggle-on-label
  cell% field toggle-off-label
  cell% field toggle-x
  cell% field toggle-y
  cell% field toggle-w
end-struct toggle%
)

0 constant TYPE-LABEL
1 constant TYPE-BUTTON
2 constant TYPE-INPUT
3 constant TYPE-AREA
4 constant TYPE-TOGGLE

defer render-label  ( surface label -- )
defer render-button ( surface button -- )
defer render-input  ( surface input -- )
defer render-area   ( surface area -- )
defer render-toggle ( surface toggle -- )

defer get-label-rect  ( label -- x y w h )
defer get-button-rect ( button -- x y w h  )
defer get-input-rect  ( input -- x y w h )
defer get-area-rect   ( area -- x y w h )
defer get-toggle-rect ( toggle -- x y w h )

: render-element ( surface element type -- )
  case
    TYPE-LABEL  of render-label endof
    TYPE-BUTTON of render-button endof
    TYPE-INPUT  of render-input endof
    TYPE-AREA   of render-area endof
    TYPE-TOGGLE of render-toggle endof
    2drop
  endcase
;

: click-element ( element -- )
  dup if ." clicked" then
  drop
;

: get-element-rect ( element type -- )
  case
    TYPE-LABEL  of get-label-rect endof
    TYPE-BUTTON of get-button-rect endof
    TYPE-INPUT  of get-input-rect endof
    TYPE-AREA   of get-area-rect endof
    TYPE-TOGGLE of get-toggle-rect endof
    drop
  endcase
;
