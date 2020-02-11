\ RilouwOS 0.1.0
\ Copyright (c) 2020 Jerome Martin
\ Released under the terms of the GNU AGPL version 3
\ http://rilouw.eu/projects/rilouwos

c-library rilouwshell_wayland_client
s" wayland-client" add-lib

\c #include "client.h"

c-function rsh-client-start rsh_client_start  -- n  ( -- exit-code )

end-c-library
