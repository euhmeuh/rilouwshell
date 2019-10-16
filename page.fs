\ RilouwShell 0.1.0
\ Copyright (c) 2019 Jerome Martin
\ Released under the terms of the GNU GPL version 3
\ http://rilouw.eu/project/rilouwos

0 value page-pointer
create PAGE 20 cells allot

: page@ ( -- current-page )  PAGE page-pointer cells + ;

: add-to-page ( element type -- )
  page@ !
  page@ cell+ !
  page-pointer 2 + to page-pointer
;

: render-page ( surface -- )
  page-pointer 0 do
    dup
    PAGE i    cells + @
    PAGE i 1+ cells + @
    swap render-element
  2 +loop
  drop
;
