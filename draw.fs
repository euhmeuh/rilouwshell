\ RilouwShell 0.1.0
\ Copyright (c) 2019 Jerome Martin
\ Released under the terms of the GNU GPL version 3
\ http://rilouw.eu/project/rilouwshell

16 constant TILE

$0000 value Transparent
$0020 value Black
$EC06 value Orange

0 value screen-surface
0 value buffer-surface

\ -- Rectangle --

create src-rect sdl-rect% %allot drop
create dst-rect sdl-rect% %allot drop

: rect! ( x y w h rect -- )
  >r
  r@ sdl-rect-h w!
  r@ sdl-rect-w w!
  r@ sdl-rect-y w!
  r> sdl-rect-x w!
;

\ convert from tile to pixel coordinates
: tile/pos ( tile -- pixel ) TILE * ;
: rect/pos ( tx1 ty1 tx2 ty2 -- px1 py1 px2 py2 )
  swap tile/pos TILE + 1-
  swap tile/pos TILE + 1-
  2swap
  swap tile/pos
  swap tile/pos
  2swap
;

\ -- Drawing primitives --

: blit ( src dst x y -- )
  0 0 dst-rect rect! \ w and h are not used
  NULL swap dst-rect sdl-blit-surface drop
;

: draw-tileset ( surface x y tileset tile -- )
  over swap \ keep tileset
  tileset->rect src-rect rect!
  -rot 2 2 dst-rect rect!
  tileset-surface @ ( surface tileset-surface )
  src-rect rot dst-rect sdl-blit-surface drop
;

: draw-rect ( surface x y w h color -- )
  >r \ color
  ( x y w h ) src-rect rect!
  ( surface ) src-rect r> sdl-fill-rect drop
;

: draw-hline ( surface x y w color -- )
  >r \ color
  ( x y w ) 1 src-rect rect!
  ( surface ) src-rect r> sdl-fill-rect drop
;

: draw-vline ( surface x y h color -- )
  >r \ color
  ( x y h ) 1 swap src-rect rect!
  ( surface ) src-rect r> sdl-fill-rect drop
;

: new-surface ( width height -- surface )
  SDL_SWSURFACE -rot 16 0 0 0 0 sdl-create-rgb-surface
;

\ -- Load images --

: load-image ( str len -- surface )
  2dup 2>r \ keep filename for error
  terminate-str sdl-load-image dup 0= if
    ." Unable to load image file: '" 2r> type ." '" cr
  else
    dup sdl-display-format
    swap sdl-free-surface
    2r> 2drop
  then
;

\ -- Sprite definition --

: sprite ( -- ) here 0 0 ;
: end-sprite ( here w h <name> -- )
  align
  create rot , 2,
  does> dup @ swap cell+ 2@
;

: px, ( n -- )
  dup
  $FF and c,
  $FF00 and 8 rshift c,
;

: l: ( w h <line> -- w h )
  1+ \ increase h
  swap ( h w )
  parse-name dup -rot 2>r \ keep a copy of length on the stack
  ( h w len ) max \ increase w
  swap 2r> ( w h line len )
  0 do
    dup I + c@ case
      [char] . of Transparent px, endof
      [char] B of Black px, endof
      [char] O of Orange px, endof
      true abort" Wrong character! Expected a 'O', 'B' or '.'"
    endcase
  loop
  drop
;

: load-sprite ( sprite w h -- surface )
 over 2* \ pitch
 16 swap \ bpp
 0 0 0 0 sdl-create-rgb-surface-from
;

\ -- Focus indicator --

create focus-tileset tileset% %allot drop

sprite
l: ...OOOOOOOO...
l: ..O........O..
l: .O..........O.
l: O............O
l: O............O
l: O............O
l: O............O
l: O............O
l: O............O
l: O............O
l: O............O
l: .O..........O.
l: ..O........O..
l: ...OOOOOOOO...
end-sprite SPRITE-FOCUS

0 constant TOP-LEFT
1 constant TOP-RIGHT
2 constant BOTTOM-LEFT
3 constant BOTTOM-RIGHT

: init-focus ( -- )
  SPRITE-FOCUS load-sprite
  dup true Transparent sdl-set-color-key drop
  focus-tileset tileset-surface !
  2 focus-tileset tileset-w !
  7 focus-tileset tileset-tile-w !
  7 focus-tileset tileset-tile-h !
;

: draw-focus ( surface x y n -- )
  focus-tileset swap draw-tileset
;

: draw-focus-around { surface x y w h -- }
  surface x 3 -     y 3 -     TOP-LEFT     draw-focus
  surface x w + 4 - y 3 -     TOP-RIGHT    draw-focus
  surface x 3 -     y h + 2 - BOTTOM-LEFT  draw-focus
  surface x w + 4 - y h + 2 - BOTTOM-RIGHT draw-focus
;

\ -- Font --

require font.fs

\ -- Initializations --

: init-window ( width height -- )
  2>r

  SDL_INIT_VIDEO sdl-init
  0<> s" Unable to initialize SDL" ?error

  2r@ 2* swap 2* swap
  16 SDL_SWSURFACE sdl-set-video-mode
  dup 0< s" Unable to set video mode" ?error
  to screen-surface

  s" Rilouw Shell0" terminate-str NULL sdl-wm-set-caption

  2r> new-surface to buffer-surface
;

: init-draw ( width height -- )
  init-window
  init-focus
  init-font
;

: flip ( -- )
  buffer-surface NULL screen-surface NULL sdl-blit-surface drop
  screen-surface sdl-flip drop
;

: clean ( -- )
  buffer-surface NULL Black sdl-fill-rect drop
;

: shutdown ( -- )
  buffer-surface sdl-free-surface
  sdl-quit
;
