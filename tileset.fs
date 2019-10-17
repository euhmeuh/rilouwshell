\ RilouwShell 0.1.0
\ Copyright (c) 2019 Jerome Martin
\ Released under the terms of the GNU GPL version 3
\ http://rilouw.eu/project/rilouwshell

struct
  cell% field tileset-surface
  cell% field tileset-w
  cell% field tileset-tile-size
end-struct tileset%

: tileset->rect { tileset tile -- x y w h }
  tile tileset tileset-w @ /mod ( yi xi )
  tileset tileset-tile-size @ dup ( yi xi s s )
  -rot * -rot * swap ( x y )
  tileset tileset-tile-size @ dup
;
