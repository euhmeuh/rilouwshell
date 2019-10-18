\ RilouwShell 0.1.0
\ Copyright (c) 2019 Jerome Martin
\ Released under the terms of the GNU GPL version 3
\ http://rilouw.eu/project/rilouwshell

struct
  cell% field tileset-surface
  cell% field tileset-w
  cell% field tileset-tile-w
  cell% field tileset-tile-h
end-struct tileset%

: tileset->rect { tileset tile -- x y w h }
  tile tileset tileset-w @ /mod ( xi yi )
  tileset tileset-tile-h @ * swap
  tileset tileset-tile-w @ * swap
  tileset tileset-tile-w @
  tileset tileset-tile-h @
;
