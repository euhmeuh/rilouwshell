\ RilouwShell 0.1.0
\ Copyright (c) 2019 Jerome Martin
\ Released under the terms of the GNU GPL version 3
\ http://rilouw.eu/project/rilouwshell

struct
  cell% field label-text
  cell% field label-primary
  cell% field label-x
  cell% field label-y
  cell% field label-w
end-struct label%

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
