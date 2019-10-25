\ RilouwShell 0.1.0
\ Copyright (c) 2019 Jerome Martin
\ Released under the terms of the GNU GPL version 3
\ http://rilouw.eu/project/rilouwshell

struct
  ptr% field machine-state
  ptr% field machine-states
end-struct machine%

struct
  ptr% field state-page
  ptr% field state-transitions
end-struct state%

: state-machine ( -- #state ) 0 ;
: end-state-machine ( ...states #state <name> -- )
  create machine% %allot >r
  dup make-array
  ( ...states #state array )
  dup r@ machine-states !
      read-array
  \ set first state
  1 r@ machine-states array@
  r> machine-state !
;

: state ( -- #trans ) 0 ;
: end-state ( #state page ...trans #trans -- state #state )
  state% %allot >r
  dup make-array
  dup r@ state-transitions !
      read-array
  r@ state-page !
  1+ \ increment #state
  r> swap
;

: => ( -- xt colon-sys ) :noname ;
: end-trans ( #trans event xt colon-sys -- trans #trans )
  here -rot
  swap , , \ put event and xt here
  swap 1+ \ increment #trans
;
