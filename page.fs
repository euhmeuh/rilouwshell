\ RilouwShell 0.1.0
\ Copyright (c) 2019 Jerome Martin
\ Released under the terms of the GNU GPL version 3
\ http://rilouw.eu/project/rilouwshell

0 value page-pointer
create PAGE 20 cells allot

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
: update-focus ( -- )
  reset-hover
  page-pointer 0 do
    PAGE i cells + 2@
    over -rot \ keep element
    get-element-rect
    mouse-x @ mouse-y @ point-in-rect? if
      ( element ) dup
      to focused-element
      to hover-element
      leave
    else drop then
  2 +loop
;

: page@ ( -- current-page )  PAGE page-pointer cells + ;

: add-to-page ( element type -- )
  page@ 2!
  page-pointer 2 + to page-pointer
;

: render-page ( surface -- )
  page-pointer 0 do
    dup \ keep for next loop

    PAGE i cells + 2@
    rot >r \ keep surface
    ( element type )
    over focused? if
      2dup r@ -rot ( element type surface element type )
      get-element-rect draw-focus-around
    then
    r> -rot render-element
  2 +loop
  drop
;
