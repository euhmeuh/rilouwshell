\ RilouwShell 0.1.0
\ Copyright (c) 2019 Jerome Martin
\ Released under the terms of the GNU GPL version 3
\ http://rilouw.eu/project/rilouwshell

: 3dup ( a b c -- a b c a b c )
  2 pick 2 pick 2 pick
;

: log ( x -- x ) dup . cr ;
: terminate-str ( str len -- str ) over + 1- 0 swap c! ;
: ?error ( f str len -- )  rot if type cr bye else 2drop then ;

: point-in-rect? { xr yr wr hr x y -- b }
  x xr >
  x xr wr + < and
  y yr >
  y yr hr + < and
  and
;

: array-len ( array -- len ) @ ;
: array-addr ( array -- addr ) cell+ ;
: array@ ( n array -- element )
  2dup array-len >= abort" array overflow"
  array-addr swap cells + @
;

: make-array ( len -- array )
  dup 1+ cells here swap allot
      tuck ! \ save length
;

: array-limits ( array -- limit start )
  dup array-len swap array-addr ( len addr )
  tuck swap cells + swap
;

: read-array ( ...element len array -- )
  cell+ tuck swap cells + swap
  do i ! cell +loop
;
