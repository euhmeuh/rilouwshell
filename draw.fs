\ RilouwShell 0.1.0
\ Copyright (c) 2019 Jerome Martin
\ Released under the terms of the GNU GPL version 3
\ http://rilouw.eu/project/rilouwos

0 value Black
0 value Orange

create tmp-rect sdl-rect% %allot drop

: tmp-rect! ( x y w h -- )
  tmp-rect sdl-rect-h w!
  tmp-rect sdl-rect-w w!
  tmp-rect sdl-rect-y w!
  tmp-rect sdl-rect-x w!
;

: draw-rect { surface x1 y1 x2 y2 color -- }
\ x  y  w       h
  x1 y1 x2 x1 - y2 y1 - tmp-rect!
  surface tmp-rect color sdl-fill-rect drop
;

: draw-hline ( surface x y w color -- )
  >r \ color
  ( x y w ) 1 tmp-rect!
  ( surface ) tmp-rect r> sdl-fill-rect drop
;

: draw-vline ( surface x y h color -- )
  >r \ color
  ( x y h ) 1 swap tmp-rect!
  ( surface ) tmp-rect r> sdl-fill-rect drop
;
