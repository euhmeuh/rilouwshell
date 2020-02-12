/* RilouwOS 0.1.0
 * Copyright (c) 2020 Jerome Martin
 * Released under the terms of the GNU AGPL version 3
 * http://rilouw.eu/projects/rilouwos
 */

#ifndef RILOUWSHELL_WAYLAND_CLIENT_H
#define RILOUWSHELL_WAYLAND_CLIENT_H

#include <stdbool.h>
#include <stdint.h>

struct rsh_client_state {
  struct wl_compositor *compositor;
  struct wl_seat *seat;
  struct wl_surface *surface;
  struct wl_shm *shm;
  struct wl_buffer *buffer;

  uint32_t width;
  uint32_t height;

  uint32_t *shm_data; // raw pixel data

  struct zwlr_layer_shell_v1 *shell;
  struct zwlr_layer_surface_v1 *layer_surface;

  bool running;
};

int
rsh_client_start();

#endif
