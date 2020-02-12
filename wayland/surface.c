/* RilouwOS 0.1.0
 * Copyright (c) 2020 Jerome Martin
 * Released under the terms of the GNU AGPL version 3
 * http://rilouw.eu/projects/rilouwos
 */

#include <stdlib.h>
#include <stdio.h>
#include <wayland-client.h>
#include "wlr-layer-shell-unstable-v1-protocol.h"
#include "client.h"
#include "buffer.h"

void
rsh_paint_pixels(struct rsh_client_state *state) {
  int n;
  uint32_t *pixels = state->shm_data;
  int size = state->width * state->height;
  printf("Painting pixels\n");
  for (n = 0; n < size; n++) {
    *pixels++ = n%7 == 0 ? 0x00 : 0xE88030;
  }
}

static void
wl_surface_enter(
  void *data,
  struct wl_surface *wl_surface,
  struct wl_output *wl_output
) {
  printf("Surface entered\n");
}

static void
wl_surface_leave(
  void *data,
  struct wl_surface *wl_surface,
  struct wl_output *wl_output
) {
  printf("Surface leaved\n");
  // Nope
}

static const struct wl_surface_listener wl_surface_listener = {
  .enter = wl_surface_enter,
  .leave = wl_surface_leave,
};

static void
layer_surface_configure(
  void *data,
  struct zwlr_layer_surface_v1 *zwlr_layer_surface_v1,
  uint32_t serial,
  uint32_t width,
  uint32_t height
) {
  printf("Configuring layer surface (w: %i, h: %i)\n", width, height);
  struct rsh_client_state *state = data;
  state->width = width;
  state->height = height;
  zwlr_layer_surface_v1_ack_configure(state->layer_surface, serial);
  rsh_init_buffer(state);
  rsh_paint_pixels(state);
  wl_surface_attach(state->surface, state->buffer, 0, 0);
  wl_surface_commit(state->surface);
}

static void
layer_surface_closed(
  void *data,
  struct zwlr_layer_surface_v1 *zwlr_layer_surface_v1
) {
  printf("Closing layer surface\n");
  // No thanks
}

static const struct zwlr_layer_surface_v1_listener layer_surface_listener = {
  .configure = layer_surface_configure,
  .closed = layer_surface_closed,
};

void
rsh_init_surface(struct rsh_client_state *state) {
  state->surface = wl_compositor_create_surface(state->compositor);
  if (state->surface == NULL) {
    fprintf(stderr, "Can't create surface\n");
    exit(EXIT_FAILURE);
  }
  wl_surface_add_listener(state->surface, &wl_surface_listener, state);

  state->layer_surface = zwlr_layer_shell_v1_get_layer_surface(
    state->shell,
    state->surface,
    NULL,
    ZWLR_LAYER_SHELL_V1_LAYER_OVERLAY,
    "desktop"
  );

  if (state->layer_surface == NULL) {
    fprintf(stderr, "Can't create shell surface\n");
    exit(EXIT_FAILURE);
  }

  zwlr_layer_surface_v1_set_anchor(
    state->layer_surface,
    ZWLR_LAYER_SURFACE_V1_ANCHOR_TOP |
    ZWLR_LAYER_SURFACE_V1_ANCHOR_BOTTOM |
    ZWLR_LAYER_SURFACE_V1_ANCHOR_LEFT |
    ZWLR_LAYER_SURFACE_V1_ANCHOR_RIGHT
  );

  zwlr_layer_surface_v1_add_listener(state->layer_surface, &layer_surface_listener, state);
  wl_surface_commit(state->surface);
}
