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
0 value corners-surface

create tmp-rect sdl-rect% %allot drop
create corner-rect sdl-rect% %allot drop

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

: blit ( src dst x y -- )
  0 0 tmp-rect rect! \ w and h are not used
  NULL swap tmp-rect sdl-blit-surface drop
;

: top-left     ( -- )  0 0 2 2 corner-rect rect! ;
: top-right    ( -- )  2 0 2 2 corner-rect rect! ;
: bottom-left  ( -- )  0 2 2 2 corner-rect rect! ;
: bottom-right ( -- )  2 2 2 2 corner-rect rect! ;

: inverted  ( -- )
  corner-rect dup sdl-rect-x @ 4 + swap sdl-rect-x w!
;

: draw-corner ( surface x y -- )
  2 2 tmp-rect rect!
  >r \ surface
  corners-surface corner-rect r> tmp-rect sdl-blit-surface drop
;

: draw-rect ( surface x y w h color -- )
  >r \ color
  ( x y w h ) tmp-rect rect!
  ( surface ) tmp-rect r> sdl-fill-rect drop
;

: draw-hline ( surface x y w color -- )
  >r \ color
  ( x y w ) 1 tmp-rect rect!
  ( surface ) tmp-rect r> sdl-fill-rect drop
;

: draw-vline ( surface x y h color -- )
  >r \ color
  ( x y h ) 1 swap tmp-rect rect!
  ( surface ) tmp-rect r> sdl-fill-rect drop
;

: new-surface ( width height -- surface )
  SDL_SWSURFACE -rot 16 0 0 0 0 sdl-create-rgb-surface
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

\ -- Global sprites --

sprite
l: OOOOBBBB
l: O..OB..B
l: O..OB..B
l: OOOOBBBB
end-sprite SPRITE-CORNERS

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

: init-corners ( -- )
  SPRITE-CORNERS load-sprite to corners-surface
  corners-surface true Transparent sdl-set-color-key drop
;

: init-draw ( width height -- )
  init-window
  init-corners
;

: flip ( -- )
  buffer-surface NULL screen-surface NULL sdl-blit-surface drop
  screen-surface sdl-flip drop
;

: clean ( -- )
  screen-surface NULL Black sdl-fill-rect drop
;

: shutdown ( -- )
  buffer-surface sdl-free-surface
  sdl-quit
;
