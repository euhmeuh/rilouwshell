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

: render-button  ( surface button -- )
  dup button-x @ swap
  dup button-y @ swap
  button-w @
  Orange draw-hline
;
