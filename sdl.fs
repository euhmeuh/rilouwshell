c-library sdl_lib
s" SDL" add-lib
s" SDL_image" add-lib

\c #include <SDL/SDL.h>
\c #include <SDL/SDL_image.h>
\c #include <SDL/SDL_keysym.h>

\ -- General --

c-function sdl-init  SDL_Init  n -- n   ( flags -- error code )
c-function sdl-quit  SDL_Quit  -- void  ( -- )

\ -- Video --

c-function sdl-set-video-mode  SDL_SetVideoMode   n n n n -- a  ( width height bpp flags -- surface )
c-function sdl-wm-set-caption  SDL_WM_SetCaption  a a -- void   ( *title *icon -- )
c-function sdl-show-cursor     SDL_ShowCursor     n -- n        ( toggle -- status )

c-function sdl-display-format      SDL_DisplayFormat     a -- a                ( *surface -- *surface )
c-function sdl-create-rgb-surface  SDL_CreateRGBSurface  n n n n n n n n -- a  ( flags w h dp r g b a -- *surface )
c-function sdl-map-rgb             SDL_MapRGB            a n n n -- n          ( *format r g b -- pixel )
c-function sdl-blit-surface        SDL_BlitSurface       a a a a -- n          ( *src *srcrect *dst *dstrect -- )
c-function sdl-flip                SDL_Flip              a -- n                ( *surface -- )
c-function sdl-free-surface        SDL_FreeSurface       a -- void             ( *surface -- )
c-function sdl-fill-rect           SDL_FillRect          a a n -- n            ( *dst *dstrect color -- error )

\ -- Events -- 

c-function sdl-pump-events      SDL_PumpEvents     -- void   ( -- )
c-function sdl-poll-event       SDL_PollEvent      a -- n    ( *event -- any-event? )
c-function sdl-get-key-state    SDL_GetKeyState    a -- a    ( *numkeys -- key-array )
c-function sdl-get-mouse-state  SDL_GetMouseState  a a -- n  ( *x *y -- button-bitmask )

\ -- Time --

c-function sdl-get-ticks  SDL_GetTicks  -- n       ( -- ticks )
c-function sdl-delay      SDL_Delay     n -- void  ( ms -- )

\ -- OpenGL extension --

c-function sdl-gl-set-attribute  SDL_GL_SetAttribute  n n -- n  ( attr value -- error )
c-function sdl-gl-get-attribute  SDL_GL_GetAttribute  n a -- n  ( attr *value -- error )
c-function sdl-gl-swap-buffers   SDL_GL_SwapBuffers   -- void   ( -- )

\ -- Image extension --

c-function sdl-load-image  IMG_Load  a -- a

end-c-library

\ -- C wrapper utils --

0 constant NULL
4 4 2constant int%
2 2 2constant word%
1 1 2constant byte%
cell% 2constant ptr%

\ -- Video structs --

struct
  byte% field sdl-color-r
  byte% field sdl-color-g
  byte% field sdl-color-b
  byte% field sdl-color-unused
end-struct sdl-color%

struct
  int%  field sdl-palette-size
  ptr%  field sdl-palette-colors
end-struct sdl-palette%

struct
  word% field sdl-rect-x
  word% field sdl-rect-y
  word% field sdl-rect-w
  word% field sdl-rect-h
end-struct sdl-rect%

struct
  ptr%  field sdl-pixelformat-palette
  byte% field sdl-pixelformat-bitsperpixel
  byte% field sdl-pixelformat-bytesperpixel
  int%  field sdl-pixelformat-rmask
  int%  field sdl-pixelformat-gmask
  int%  field sdl-pixelformat-bmask
  int%  field sdl-pixelformat-amask
  byte% field sdl-pixelformat-rshift
  byte% field sdl-pixelformat-gshift
  byte% field sdl-pixelformat-bshift
  byte% field sdl-pixelformat-ashift
  byte% field sdl-pixelformat-rloss
  byte% field sdl-pixelformat-gloss
  byte% field sdl-pixelformat-bloss
  byte% field sdl-pixelformat-aloss
  int%  field sdl-pixelformat-colorkey
  byte% field sdl-pixelformat-alpha
end-struct sdl-pixelformat%

struct
  int%  field sdl-surface-flags
  ptr%  field sdl-surface-format
  int%  field sdl-surface-w
  int%  field sdl-surface-h
  word% field sdl-surface-pitch
  ptr%  field sdl-surface-pixels
  int%  field sdl-surface-offset
  ptr%  field sdl-surface-hwdata
  sdl-rect% field sdl-surface-cliprect
  int%  field sdl-surface-unused1
  int%  field sdl-surface-locked
  ptr%  field sdl-surface-map
  int%  field sdl-surface-formatversion
  int%  field sdl-surface-refcount
end-struct sdl-surface%

\ -- Event structs --

struct
  byte% field sdl-joy-axis-event-type
  byte% field sdl-joy-axis-event-which
  byte% field sdl-joy-axis-event-axis
  word% field sdl-joy-axis-event-value
end-struct sdl-joy-axis-event%

struct
  byte% field sdl-joy-ball-event-type
  byte% field sdl-joy-ball-event-which
  byte% field sdl-joy-ball-event-ball
  word% field sdl-joy-ball-event-xrel
  word% field sdl-joy-ball-event-yrel
end-struct sdl-joy-ball-event%

struct
  byte% field sdl-joy-hat-event-type
  byte% field sdl-joy-hat-event-which
  byte% field sdl-joy-hat-event-hat
  byte% field sdl-joy-hat-event-value
end-struct sdl-joy-hat-event%

struct
  byte% field sdl-joy-button-event-type
  byte% field sdl-joy-button-event-which
  byte% field sdl-joy-button-event-button
  byte% field sdl-joy-button-event-state
end-struct sdl-joy-button-event%

struct
  char% field sdl-keysym-scancode
  int%  field sdl-keysym-sym
  int%  field sdl-keysym-mod
  word% field sdl-keysym-unicode
end-struct sdl-keysym%

struct
  byte% field sdl-active-event-type
  byte% field sdl-active-event-gain
  byte% field sdl-active-event-state
end-struct sdl-active-event%

struct
  byte% field sdl-keyboard-event-type
  byte% field sdl-keyboard-event-which
  byte% field sdl-keyboard-event-state
  sdl-keysym% field sdl-keyboard-event-keysym
end-struct sdl-keyboard-event%

struct
  byte% field sdl-mouse-motion-event-type
  byte% field sdl-mouse-motion-event-which
  byte% field sdl-mouse-motion-event-state
  word% field sdl-mouse-motion-event-x
  word% field sdl-mouse-motion-event-y
  word% field sdl-mouse-motion-event-xrel
  word% field sdl-mouse-motion-event-yrel
end-struct sdl-mouse-motion-event%

struct
  byte% field sdl-mouse-button-event-type
  byte% field sdl-mouse-button-event-which
  byte% field sdl-mouse-button-event-button
  byte% field sdl-mouse-button-event-state
  word% field sdl-mouse-button-event-x
  word% field sdl-mouse-button-event-y
end-struct sdl-mouse-button-event%

struct
  byte% field sdl-resize-event-type
  int%  field sdl-resize-event-width
  int%  field sdl-resize-event-height
end-struct sdl-resize-event%

struct
  byte% field sdl-expose-event-type
end-struct sdl-expose-event%

struct
  byte% field sdl-quit-event-type
end-struct sdl-quit-event%

struct
  byte% field sdl-user-event-type
  int%  field sdl-user-event-code
  ptr%  field sdl-user-event-data1
  ptr%  field sdl-user-event-data2
end-struct sdl-user-event%

struct
  byte% field sdl-sys-wm-event-type
  ptr%  field sdl-sys-wm-event-msg
end-struct sdl-sys-wm-event%

struct
  byte%                   field sdl-event-type
  sdl-active-event%       field sdl-event-active
  sdl-keyboard-event%     field sdl-event-key
  sdl-mouse-motion-event% field sdl-event-motion
  sdl-mouse-button-event% field sdl-event-button
  sdl-joy-axis-event%     field sdl-event-jaxis
  sdl-joy-ball-event%     field sdl-event-jball
  sdl-joy-hat-event%      field sdl-event-jhat
  sdl-joy-button-event%   field sdl-event-jbutton
  sdl-resize-event%       field sdl-event-resize
  sdl-expose-event%       field sdl-event-expose
  sdl-quit-event%         field sdl-event-quit
  sdl-user-event%         field sdl-event-user
  sdl-sys-wm-event%       field sdl-event-syswm
end-struct sdl-event%

\ -- General constants --

$00000001 constant SDL_INIT_TIMER
$00000010 constant SDL_INIT_AUDIO
$00000020 constant SDL_INIT_VIDEO
$00000100 constant SDL_INIT_CDROM
$00000200 constant SDL_INIT_JOYSTICK
$00100000 constant SDL_INIT_NOPARACHUTE
$01000000 constant SDL_INIT_EVENTTHREAD
$00000000 constant SDL_INIT_EVERYTHING

\ -- Video constants --

$00000000   constant SDL_SWSURFACE
$00000001   constant SDL_HWSURFACE
$00000004   constant SDL_ASYNCBLIT
$10000000   constant SDL_ANYFORMAT
$20000000   constant SDL_HWPALETTE
$40000000   constant SDL_DOUBLEBUF
$80000000   constant SDL_FULLSCREEN
$00000002   constant SDL_OPENGL
$0000000    constant SDL_OPENGLBLIT
$00000010   constant SDL_RESIZABLE
$00000020   constant SDL_NOFRAME

\ -- Event constants --

 0 constant SDL_NOEVENT
 1 constant SDL_ACTIVEEVENT
 2 constant SDL_KEYDOWN
 3 constant SDL_KEYUP
 4 constant SDL_MOUSEMOTION
 5 constant SDL_MOUSEBUTTONDOWN
 6 constant SDL_MOUSEBUTTONUP
 7 constant SDL_JOYAXISMOTION
 8 constant SDL_JOYBALLMOTION
 9 constant SDL_JOYHATMOTION
10 constant SDL_JOYBUTTONDOWN
11 constant SDL_JOYBUTTONUP
12 constant SDL_QUIT
13 constant SDL_SYSWMEVENT
14 constant SDL_EVENT_RESERVEDA
15 constant SDL_EVENT_RESERVEDB
16 constant SDL_VIDEORESIZE
17 constant SDL_VIDEOEXPOSE
18 constant SDL_EVENT_RESERVED2
19 constant SDL_EVENT_RESERVED3
20 constant SDL_EVENT_RESERVED4
21 constant SDL_EVENT_RESERVED5
22 constant SDL_EVENT_RESERVED6
23 constant SDL_EVENT_RESERVED7

\ -- Keys constants --

  0 constant SDLK_UNKNOWN
  0 constant SDLK_FIRST
  8 constant SDLK_BACKSPACE
  9 constant SDLK_TAB
 12 constant SDLK_CLEAR
 13 constant SDLK_RETURN
 19 constant SDLK_PAUSE
 27 constant SDLK_ESCAPE
 32 constant SDLK_SPACE
 33 constant SDLK_EXCLAIM
 34 constant SDLK_QUOTEDBL
 35 constant SDLK_HASH
 36 constant SDLK_DOLLAR
 38 constant SDLK_AMPERSAND
 39 constant SDLK_QUOTE
 40 constant SDLK_LEFTPAREN
 41 constant SDLK_RIGHTPAREN
 42 constant SDLK_ASTERISK
 43 constant SDLK_PLUS
 44 constant SDLK_COMMA
 45 constant SDLK_MINUS
 46 constant SDLK_PERIOD
 47 constant SDLK_SLASH
 48 constant SDLK_0
 49 constant SDLK_1
 50 constant SDLK_2
 51 constant SDLK_3
 52 constant SDLK_4
 53 constant SDLK_5
 54 constant SDLK_6
 55 constant SDLK_7
 56 constant SDLK_8
 57 constant SDLK_9
 58 constant SDLK_COLON
 59 constant SDLK_SEMICOLON
 60 constant SDLK_LESS
 61 constant SDLK_EQUALS
 62 constant SDLK_GREATER
 63 constant SDLK_QUESTION
 64 constant SDLK_AT
 91 constant SDLK_LEFTBRACKET
 92 constant SDLK_BACKSLASH
 93 constant SDLK_RIGHTBRACKET
 94 constant SDLK_CARET
 95 constant SDLK_UNDERSCORE
 96 constant SDLK_BACKQUOTE
 97 constant SDLK_a
 98 constant SDLK_b
 99 constant SDLK_c
100 constant SDLK_d
101 constant SDLK_e
102 constant SDLK_f
103 constant SDLK_g
104 constant SDLK_h
105 constant SDLK_i
106 constant SDLK_j
107 constant SDLK_k
108 constant SDLK_l
109 constant SDLK_m
110 constant SDLK_n
111 constant SDLK_o
112 constant SDLK_p
113 constant SDLK_q
114 constant SDLK_r
115 constant SDLK_s
116 constant SDLK_t
117 constant SDLK_u
118 constant SDLK_v
119 constant SDLK_w
120 constant SDLK_x
121 constant SDLK_y
121 constant SDLK_z
127 constant SDLK_DELETE
