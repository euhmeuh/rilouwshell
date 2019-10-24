\ RilouwShell 0.1.0
\ Copyright (c) 2019 Jerome Martin
\ Released under the terms of the GNU GPL version 3
\ http://rilouw.eu/project/rilouwshell

struct
  ptr% field page-name
  ptr% field page-content
  cell% field page-cursor
end-struct page%

variable mouse-x
variable mouse-y
variable mouse-buttons

: mouse-left? ( -- b )  mouse-buttons @ 1 and ;
: mouse-right? ( -- b )  mouse-buttons @ 4 and ;

0 value focused-element
0 value hover-element

: focused? ( element -- )  focused-element = ;
: hover? ( element -- )  hover-element = ;
: reset-focus ( -- )  0 to focused-element ;
: reset-hover ( -- )  0 to hover-element ;

: page-limits ( page -- limit start )
  >r
  r@ page-content @
  r@ page-cursor @ cells +
  r> page-content @
;

: update-focus ( page -- )
  reset-hover
  page-limits do
    i 2@
    ( element type )
    dup TYPE-LABEL <> if \ labels don't get focus
      over -rot \ keep element
      get-element-rect
      mouse-x @ mouse-y @ point-in-rect? if
        ( element ) dup
        to focused-element
        to hover-element
        leave
      else drop then
    else 2drop then
  2 cells +loop
;

: next-element ( page -- next-element-addr )
  dup page-content @ swap
  page-cursor @ cells +
;

: add-to-page ( element type page -- )
  >r
  r@ next-element 2!
  r@ page-cursor @ 2 +
  r> page-cursor !
;

: render-page ( surface page -- )
  page-limits do
    dup \ keep surface for next loop
    i 2@
    ( surface surface element type )
    over focused? if
      3dup get-element-rect draw-focus-around
    then
    render-element
  2 cells +loop
  drop \ surface
;

: page ( -- #element ) 0 ;
: end-page ( ...element&type #element <name> -- )
  create page% %allot >r \ keep page
  ( ...element&type #element )

  \ allot content
  dup here swap 2* cells allot r@ page-content !

  r> swap
  \ consume each element
  0 do
    dup >r add-to-page
    r>
  loop
  drop
;
